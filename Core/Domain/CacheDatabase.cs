using System;
using System.Collections.Generic;
using System.Linq;
using Core.Util;
using Gs2.Core.Util;

namespace Gs2.Core.Domain
{
    public class CacheDatabase
    {
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>> _cache = new Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Dictionary<ulong, object>>>> _cacheUpdateCallback = new Dictionary<Type, Dictionary<string, Dictionary<string, Dictionary<ulong, object>>>>();
        private readonly Dictionary<Type, HashSet<string>> _listCached = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<ulong, object>>> _listCacheUpdateCallback = new Dictionary<Type, Dictionary<string, Dictionary<ulong, object>>>();
        private readonly Dictionary<Type, HashSet<string>> _listCacheUpdateRequired = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, object>> _listCacheContexts = new Dictionary<Type, Dictionary<string, object>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, AsyncLock>>> _lockObjects = new Dictionary<Type, Dictionary<string, Dictionary<string, AsyncLock>>>();

        public void Clear()
        {
            this._cache.Clear();
            this._listCached.Clear();
            this._cacheUpdateCallback.Clear();
            this._listCacheContexts.Clear();
            this._listCacheUpdateCallback.Clear();
            this._listCacheUpdateRequired.Clear();
        }

        public void SetListCached<TKind>(string parentKey, object listCacheContext = null)
        {
            this._listCached.Ensure(typeof(TKind)).Add(parentKey);
            this._listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
            if (listCacheContext != null)
            {
                this._listCacheContexts.Ensure(typeof(TKind))[parentKey] = listCacheContext;
            }
            foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey)) {
                (callback.Value as Action)?.Invoke();
            }
        }

        public void ClearListCache<TKind>(string parentKey)
        {
            this._cache.Get(typeof(TKind))?.Get(parentKey)?.Clear();
            this._listCached.Get(typeof(TKind))?.Remove(parentKey);
            this._listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
            this._listCacheContexts.Get(typeof(TKind))?.Remove(parentKey);
        }

        public void RequireListCacheUpdate<TKind>(string parentKey)
        {
            this._listCacheUpdateRequired.Ensure(typeof(TKind)).Add(parentKey);
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
            this._cache.Ensure(typeof(TKind)).Ensure(parentKey)[key] = new Tuple<object, long>(obj, ttl);
            foreach (var callback in this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Ensure(key)) {
                (callback.Value as Action<TKind>)?.Invoke(obj);
            }
            foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey)) {
                (callback.Value as Action)?.Invoke();
            }
        }

        public void Delete<TKind>(string parentKey, string key)
        {
            this._cache.Get(typeof(TKind))?.Get(parentKey)?.Remove(key);
            foreach (var callback in this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey)) {
                (callback.Value as Action)?.Invoke();
            }
        }

        private static ulong _callbackId = 1;

        public ulong Subscribe<TKind>(string parentKey, string key, Action<TKind> subscribe)
        {
            this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Ensure(key).Add(_callbackId, subscribe);
            return _callbackId++;
        }

        public void Unsubscribe<TKind>(string parentKey, string key, ulong callbackId)
        {
            this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Ensure(key).Remove(callbackId);
        }

        public ulong ListSubscribe<TKind>(string parentKey, Action subscribe)
        {
            this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Add(_callbackId, subscribe);
            return _callbackId++;
        }

        public void ListUnsubscribe<TKind>(string parentKey, ulong callbackId)
        {
            this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Remove(callbackId);
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
            var asyncLocks = this._lockObjects.Get(typeof(TKind))?.Get(parentKey);
            if (asyncLocks != null && asyncLocks.TryGetValue(key, out var asyncLock))
            {
                return asyncLock;
            }

            return this._lockObjects.Ensure(typeof(TKind)).Ensure(parentKey)[key] = new AsyncLock();
        }

        public TKind[] List<TKind>(string parentKey)
        {
            return TryGetList<TKind>(parentKey, out var list, out var listCacheContext) ? list : Array.Empty<TKind>();
        }

        public bool TryGetList<TKind>(string parentKey, out TKind[] list)
        {
            if (this._listCached.Get(typeof(TKind))?.Contains(parentKey) != true)
            {
                list = null;
                return false;
            }

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