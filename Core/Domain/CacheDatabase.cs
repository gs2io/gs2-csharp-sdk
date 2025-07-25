using System;
using System.Collections.Generic;
using System.Linq;
using Core.Util;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#endif

namespace Gs2.Core.Domain
{
    public class CacheDatabase
    {
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>> _cache = new Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>>> _cacheUpdateCallback = new Dictionary<Type, Dictionary<string, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>>>();
        private readonly Dictionary<Type, HashSet<string>> _listCached = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>> _listCacheUpdateCallback = new Dictionary<Type, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>>();
        private readonly Dictionary<Type, HashSet<string>> _listCacheUpdateRequired = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, object>> _listCacheContexts = new Dictionary<Type, Dictionary<string, object>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, AsyncLock>>> _lockObjects = new Dictionary<Type, Dictionary<string, Dictionary<string, AsyncLock>>>();

        public void Clear()
        {
            this._cache.Clear();
            this._listCached.Clear();
            this._listCacheContexts.Clear();
            this._listCacheUpdateRequired.Clear();
            foreach (var pair in this._cacheUpdateCallback.ToArray()) {
                var callbacksContainers = pair.Value;
                foreach (var callbacksContainer in callbacksContainers.ToArray()) {
                    var callbacks = callbacksContainer.Value;
                    foreach (var callback in callbacks.ToArray()) {
                        foreach (var c in callback.Value.ToArray()) {
                            c.Value.Item2.Invoke();
                        }
                    }
                }
            }
        }

        public void ClearAndAllUnsubscribe()
        {
            this._cache.Clear();
            this._listCached.Clear();
            this._listCacheContexts.Clear();
            this._listCacheUpdateRequired.Clear();
            this._cacheUpdateCallback.Clear();
            this._listCacheUpdateCallback.Clear();
        }

        public void SetListCached<TKind>(string parentKey, object listCacheContext = null)
        {
            this._listCached.Ensure(typeof(TKind)).Add(parentKey);
            this._listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
            if (listCacheContext != null)
            {
                this._listCacheContexts.Ensure(typeof(TKind))[parentKey] = listCacheContext;
            }
            foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).ToArray()) {
                (callback.Value.Item1 as Action<TKind[]>)?.Invoke(List<TKind>(parentKey));
            }
        }

        public void ClearListCache<TKind>(string parentKey)
        {
            this._cache.Get(typeof(TKind))?.Get(parentKey)?.Clear();
            this._listCached.Get(typeof(TKind))?.Remove(parentKey);
            this._listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
            this._listCacheContexts.Get(typeof(TKind))?.Remove(parentKey);
            foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).ToArray()) {
                callback.Value.Item2?.Invoke();
            }
        }

        public void RequireListCacheUpdate<TKind>(string parentKey)
        {
            this._listCacheUpdateRequired.Ensure(typeof(TKind)).Add(parentKey);
            if (this._listCacheContexts.Get(typeof(TKind))?.Ensure(parentKey) == null) {
                ClearListCache<TKind>(parentKey);
            }
            else {
                foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).ToArray()) {
                    callback.Value.Item2?.Invoke();
                }
            }
        }

        public bool IsListCached<TKind>(string parentKey)
        {
            return this._listCached.Get(typeof(TKind))?.Contains(parentKey) == true;
        }

        private bool IsListCacheUpdateRequired<TKind>(string parentKey)
        {
            return this._listCacheUpdateRequired.Get(typeof(TKind))?.Contains(parentKey) == true;
        }

        public void Put<TKind>(string parentKey, string key, TKind obj, long ttl)
        {
            if (ttl == 0)
            {
                ttl = UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.DefaultCacheMinutes;
            }
            var parent = this._cache.Ensure(typeof(TKind)).Ensure(parentKey);
            var exists = parent.ContainsKey(key);
            var changed = !exists || parent[key].Item1 == null || !parent[key].Item1.Equals(obj);
            parent[key] = new Tuple<object, long>(obj, ttl);
            if (changed) {
                foreach (var callback in this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey)
                             .Ensure(key).ToArray()) {
                    (callback.Value?.Item1 as Action<TKind>)?.Invoke(obj);
                }
                foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey)
                             .ToArray()) {
                    (callback.Value?.Item1 as Action<TKind[]>)?.Invoke(List<TKind>(parentKey));
                }
            }
        }

        public void Delete<TKind>(string parentKey, string key)
        {
            this._cache.Get(typeof(TKind))?.Get(parentKey)?.Remove(key);
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
            async UniTaskVoid Invoke() {
                await UniTask.SwitchToMainThread();
#endif
                foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).ToArray()) {
                    (callback.Value?.Item1 as Action<TKind[]>)?.Invoke(List<TKind>(parentKey));
                }
                foreach (var callback in this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Ensure(key).Values.ToArray()) {
                    callback?.Item2?.Invoke();
                }
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
            }
            Invoke().Forget();
#endif
        }

        private static ulong _callbackId = 1;

        public ulong Subscribe<TKind>(string parentKey, string key, Action<TKind> subscribe, Action reFetch)
        {
            lock(this) {
                this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Ensure(key).Add(_callbackId, new Tuple<object, Action>(subscribe, reFetch));
                return _callbackId++;
            }
        }

        public void Unsubscribe<TKind>(string parentKey, string key, ulong callbackId)
        {
            lock (this) {
                this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Ensure(key).Remove(callbackId);
            }
        }

        public ulong ListSubscribe<TKind>(string parentKey, Action<TKind[]> subscribe, Action reFetch)
        {
            lock(this) {
                this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Add(_callbackId, new Tuple<object, Action>(subscribe, reFetch));
                return _callbackId++;
            }
        }

        public void ListUnsubscribe<TKind>(string parentKey, ulong callbackId)
        {
            lock (this) {
                this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Remove(callbackId);
            }
        }

        public Tuple<TKind, bool> Get<TKind>(string parentKey, string key)
        {
            var cache = this._cache.Get(typeof(TKind))?.Get(parentKey);
            if (cache != null && cache.TryGetValue(key, out var cachedValue))
            {
                var obj = cachedValue.Item1;
                var ttl = cachedValue.Item2;
                if (ttl >= UnixTime.ToUnixTime(DateTime.Now))
                {
                    return new Tuple<TKind, bool>((TKind) obj, true);;
                }
                else
                {
                    ClearListCache<TKind>(parentKey);
                    Delete<TKind>(
                        parentKey,
                        key
                    );
                }
            }

            return new Tuple<TKind, bool>(default, false);
        }

        public AsyncLock GetLockObject<TKind>(string parentKey, string key)
        {
            lock (this) {
                var asyncLocks = this._lockObjects.Get(typeof(TKind))?.Get(parentKey);
                if (asyncLocks != null && asyncLocks.TryGetValue(key, out var asyncLock)) {
                    return asyncLock;
                }
                return this._lockObjects.Ensure(typeof(TKind)).Ensure(parentKey)[key] = new AsyncLock();
            }
        }

        public TKind[] List<TKind>(string parentKey)
        {
            return TryGetList<TKind>(parentKey, out var list, out var listCacheContext) ? list : Array.Empty<TKind>();
        }

        public bool TryGetList<TKind>(string parentKey, out TKind[] list)
        {
            if (!IsListCached<TKind>(parentKey))
            {
                list = null;
                return false;
            }
            return TryGetListForce(parentKey, out list);
        }

        // listCacheContext は RequireListCacheUpdate() が呼ばれたあとのみ値が代入されます
        public bool TryGetList<TKind>(string parentKey, out TKind[] list, out object listCacheContext)
        {
            if (TryGetList(parentKey, out list))
            {
                listCacheContext = IsListCacheUpdateRequired<TKind>(parentKey)
                    ? this._listCacheContexts.Get(typeof(TKind))?.Get(parentKey)
                    : null;
                return true;
            }
            else
            {
                listCacheContext = null;
                return false;
            }
        }
        
        public TKind[] ListForce<TKind>(string parentKey)
        {
            return TryGetListForce<TKind>(parentKey, out var list) ? list : Array.Empty<TKind>();
        }

        public bool TryGetListForce<TKind>(string parentKey, out TKind[] list)
        {
            var now = UnixTime.ToUnixTime(DateTime.Now);
            var values = this._cache.Ensure(typeof(TKind)).Ensure(parentKey).Values;
            if (values.Any(value => value.Item2 < now))
            {
                ClearListCache<TKind>(parentKey);
                list = null;
                return false;
            }

            list = values
                .Where(pair => pair.Item2 >= now)
                .Select(pair => (TKind)pair.Item1).Where(v => v != null)
                .ToArray();
            return true;
        }

    }
    
    static class ExtensionMethods
    {
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            return dictionary != null && dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue Ensure<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            return dictionary.TryGetValue(key, out var value) ? value : dictionary[key] = new TValue();
        }
    }
}