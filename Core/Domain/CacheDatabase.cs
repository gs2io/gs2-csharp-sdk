using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly object _syncRoot = new object();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>> _cache = new Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>>> _cacheUpdateCallback = new Dictionary<Type, Dictionary<string, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>>>();
        private readonly Dictionary<Type, HashSet<string>> _listCached = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>> _listCacheUpdateCallback = new Dictionary<Type, Dictionary<string, Dictionary<ulong, Tuple<object, Action>>>>();
        private readonly Dictionary<Type, HashSet<string>> _listCacheUpdateRequired = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, object>> _listCacheContexts = new Dictionary<Type, Dictionary<string, object>>();
        private sealed class CacheLockEntry
        {
            internal readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
            internal int ReferenceCount;
        }

        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, CacheLockEntry>>> _lockObjects = new Dictionary<Type, Dictionary<string, Dictionary<string, CacheLockEntry>>>();
        internal int LockObjectCount
        {
            get
            {
                lock (_syncRoot)
                {
                    return _lockObjects.Values
                        .SelectMany(parents => parents.Values)
                        .Sum(entries => entries.Count);
                }
            }
        }

        public void Clear()
        {
            Action[] callbacks;
            lock (_syncRoot)
            {
                this._cache.Clear();
                this._listCached.Clear();
                this._listCacheContexts.Clear();
                this._listCacheUpdateRequired.Clear();
                callbacks = this._cacheUpdateCallback.Values
                    .SelectMany(parentCallbacks => parentCallbacks.Values)
                    .SelectMany(itemCallbacks => itemCallbacks.Values)
                    .SelectMany(callbacksById => callbacksById.Values)
                    .Select(callback => callback.Item2)
                    .Concat(
                        this._listCacheUpdateCallback.Values
                            .SelectMany(parentCallbacks => parentCallbacks.Values)
                            .SelectMany(callbacksById => callbacksById.Values)
                            .Select(callback => callback.Item2)
                    )
                    .Where(callback => callback != null)
                    .ToArray();
            }
            InvokeCallbacks(callbacks);
        }

        public void ClearAndAllUnsubscribe()
        {
            lock (_syncRoot)
            {
                this._cache.Clear();
                this._listCached.Clear();
                this._listCacheContexts.Clear();
                this._listCacheUpdateRequired.Clear();
                this._cacheUpdateCallback.Clear();
                this._listCacheUpdateCallback.Clear();
            }
        }

        public void SetListCached<TKind>(string parentKey, object listCacheContext = null)
        {
            Action<TKind[]>[] callbacks;
            lock (_syncRoot)
            {
                this._listCached.Ensure(typeof(TKind)).Add(parentKey);
                this._listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
                if (listCacheContext != null)
                {
                    this._listCacheContexts.Ensure(typeof(TKind))[parentKey] = listCacheContext;
                }
                callbacks = GetListUpdateCallbacksLocked<TKind>(parentKey);
            }
            var list = callbacks.Length == 0 ? null : List<TKind>(parentKey);
            InvokeCallbacks(callbacks, list);
        }

        public void ClearListCache<TKind>(string parentKey)
        {
            Action[] callbacks;
            lock (_syncRoot)
            {
                callbacks = ClearListCacheLocked<TKind>(parentKey);
            }
            InvokeCallbacks(callbacks);
        }

        public void RequireListCacheUpdate<TKind>(string parentKey)
        {
            Action[] callbacks;
            lock (_syncRoot)
            {
                this._listCacheUpdateRequired.Ensure(typeof(TKind)).Add(parentKey);
                callbacks = this._listCacheContexts.Get(typeof(TKind))?.Get(parentKey) == null
                    ? ClearListCacheLocked<TKind>(parentKey)
                    : GetListRefetchCallbacksLocked<TKind>(parentKey);
            }
            InvokeCallbacks(callbacks);
        }

        private Action[] ClearListCacheLocked<TKind>(string parentKey)
        {
            this._cache.Get(typeof(TKind))?.Get(parentKey)?.Clear();
            PruneCacheLocked(typeof(TKind), parentKey);
            this._listCached.Get(typeof(TKind))?.Remove(parentKey);
            this._listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
            this._listCacheContexts.Get(typeof(TKind))?.Remove(parentKey);
            return GetListRefetchCallbacksLocked<TKind>(parentKey);
        }

        private Action[] GetListRefetchCallbacksLocked<TKind>(string parentKey)
        {
            return this._listCacheUpdateCallback.Get(typeof(TKind))?.Get(parentKey)?.Values
                       .Select(callback => callback.Item2)
                       .Where(callback => callback != null)
                       .ToArray()
                   ?? Array.Empty<Action>();
        }

        private Action<TKind[]>[] GetListUpdateCallbacksLocked<TKind>(string parentKey)
        {
            return this._listCacheUpdateCallback.Get(typeof(TKind))?.Get(parentKey)?.Values
                       .Select(callback => callback.Item1 as Action<TKind[]>)
                       .Where(callback => callback != null)
                       .ToArray()
                   ?? Array.Empty<Action<TKind[]>>();
        }

        private Action<TKind>[] GetItemUpdateCallbacksLocked<TKind>(string parentKey, string key)
        {
            return this._cacheUpdateCallback.Get(typeof(TKind))?.Get(parentKey)?.Get(key)?.Values
                       .Select(callback => callback.Item1 as Action<TKind>)
                       .Where(callback => callback != null)
                       .ToArray()
                   ?? Array.Empty<Action<TKind>>();
        }

        private Action[] GetItemRefetchCallbacksLocked<TKind>(string parentKey, string key)
        {
            return this._cacheUpdateCallback.Get(typeof(TKind))?.Get(parentKey)?.Get(key)?.Values
                       .Select(callback => callback.Item2)
                       .Where(callback => callback != null)
                       .ToArray()
                   ?? Array.Empty<Action>();
        }

        public bool IsListCached<TKind>(string parentKey)
        {
            lock (_syncRoot)
            {
                return this._listCached.Get(typeof(TKind))?.Contains(parentKey) == true;
            }
        }

        private bool IsListCacheUpdateRequiredLocked<TKind>(string parentKey)
        {
            return this._listCacheUpdateRequired.Get(typeof(TKind))?.Contains(parentKey) == true;
        }

        public void Put<TKind>(string parentKey, string key, TKind obj, long ttl)
        {
            if (ttl == 0)
            {
                ttl = UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.DefaultCacheMinutes;
            }

            Action<TKind>[] itemCallbacks = Array.Empty<Action<TKind>>();
            Action<TKind[]>[] listCallbacks = Array.Empty<Action<TKind[]>>();
            lock (_syncRoot)
            {
                var parent = this._cache.Ensure(typeof(TKind)).Ensure(parentKey);
                var exists = parent.ContainsKey(key);
                var changed = !exists || parent[key].Item1 == null || !parent[key].Item1.Equals(obj);
                parent[key] = new Tuple<object, long>(obj, ttl);
                if (changed)
                {
                    itemCallbacks = GetItemUpdateCallbacksLocked<TKind>(parentKey, key);
                    listCallbacks = GetListUpdateCallbacksLocked<TKind>(parentKey);
                }
            }
            InvokeCallbacks(itemCallbacks, obj);
            var list = listCallbacks.Length == 0 ? null : List<TKind>(parentKey);
            InvokeCallbacks(listCallbacks, list);
        }

        public void Delete<TKind>(string parentKey, string key)
        {
            lock (_syncRoot)
            {
                this._cache.Get(typeof(TKind))?.Get(parentKey)?.Remove(key);
                PruneCacheLocked(typeof(TKind), parentKey);
            }
            NotifyDeleteCallbacks<TKind>(parentKey, key);
        }

        private void PruneCacheLocked(Type kind, string parentKey)
        {
            if (!this._cache.TryGetValue(kind, out var parents) ||
                !parents.TryGetValue(parentKey, out var entries) ||
                entries.Count != 0)
            {
                return;
            }
            parents.Remove(parentKey);
            if (parents.Count == 0)
            {
                this._cache.Remove(kind);
            }
        }

        private void NotifyDeleteCallbacks<TKind>(string parentKey, string key)
        {
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
            async UniTaskVoid Invoke() {
                await UniTask.SwitchToMainThread();
                InvokeDeleteCallbacks<TKind>(parentKey, key);
            }
            Invoke().Forget();
#else
            InvokeDeleteCallbacks<TKind>(parentKey, key);
#endif
        }

        private void InvokeDeleteCallbacks<TKind>(string parentKey, string key)
        {
            Action<TKind[]>[] listCallbacks;
            Action[] itemCallbacks;
            lock (_syncRoot)
            {
                listCallbacks = GetListUpdateCallbacksLocked<TKind>(parentKey);
                itemCallbacks = GetItemRefetchCallbacksLocked<TKind>(parentKey, key);
            }

            var list = listCallbacks.Length == 0 ? null : List<TKind>(parentKey);
            InvokeCallbacks(listCallbacks, list);
            InvokeCallbacks(itemCallbacks);
        }

        private static long _callbackId;

        public ulong Subscribe<TKind>(string parentKey, string key, Action<TKind> subscribe, Action reFetch)
        {
            var callbackId = (ulong)Interlocked.Increment(ref _callbackId);
            lock (_syncRoot) {
                this._cacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Ensure(key).Add(callbackId, new Tuple<object, Action>(subscribe, reFetch));
                return callbackId;
            }
        }

        public void Unsubscribe<TKind>(string parentKey, string key, ulong callbackId)
        {
            lock (_syncRoot) {
                var kind = typeof(TKind);
                if (!this._cacheUpdateCallback.TryGetValue(kind, out var parents) ||
                    !parents.TryGetValue(parentKey, out var entries) ||
                    !entries.TryGetValue(key, out var callbacks))
                {
                    return;
                }
                callbacks.Remove(callbackId);
                if (callbacks.Count == 0)
                {
                    entries.Remove(key);
                }
                if (entries.Count == 0)
                {
                    parents.Remove(parentKey);
                }
                if (parents.Count == 0)
                {
                    this._cacheUpdateCallback.Remove(kind);
                }
            }
        }

        public ulong ListSubscribe<TKind>(string parentKey, Action<TKind[]> subscribe, Action reFetch)
        {
            var callbackId = (ulong)Interlocked.Increment(ref _callbackId);
            lock (_syncRoot) {
                this._listCacheUpdateCallback.Ensure(typeof(TKind)).Ensure(parentKey).Add(callbackId, new Tuple<object, Action>(subscribe, reFetch));
                return callbackId;
            }
        }

        public void ListUnsubscribe<TKind>(string parentKey, ulong callbackId)
        {
            lock (_syncRoot) {
                var kind = typeof(TKind);
                if (!this._listCacheUpdateCallback.TryGetValue(kind, out var parents) ||
                    !parents.TryGetValue(parentKey, out var callbacks))
                {
                    return;
                }
                callbacks.Remove(callbackId);
                if (callbacks.Count == 0)
                {
                    parents.Remove(parentKey);
                }
                if (parents.Count == 0)
                {
                    this._listCacheUpdateCallback.Remove(kind);
                }
            }
        }

        public Tuple<TKind, bool> Get<TKind>(string parentKey, string key)
        {
            var expired = false;
            var listRefetchCallbacks = Array.Empty<Action>();
            lock (_syncRoot)
            {
                var cache = this._cache.Get(typeof(TKind))?.Get(parentKey);
                if (cache != null && cache.TryGetValue(key, out var cachedValue))
                {
                    var obj = cachedValue.Item1;
                    var ttl = cachedValue.Item2;
                    if (ttl >= UnixTime.ToUnixTime(DateTime.Now))
                    {
                        return new Tuple<TKind, bool>((TKind) obj, true);
                    }

                    expired = true;
                    listRefetchCallbacks = ClearListCacheLocked<TKind>(parentKey);
                }
            }

            if (expired)
            {
                InvokeCallbacks(listRefetchCallbacks);
                NotifyDeleteCallbacks<TKind>(parentKey, key);
            }

            return new Tuple<TKind, bool>(default, false);
        }

        public AsyncLock GetLockObject<TKind>(string parentKey, string key)
        {
            var kind = typeof(TKind);
            var entry = AcquireLockObject(kind, parentKey, key);
            return new AsyncLock(
                entry.Semaphore,
                () => ReleaseLockObject(kind, parentKey, key, entry),
                () =>
                {
                    var nextEntry = AcquireLockObject(kind, parentKey, key);
                    return Tuple.Create(
                        nextEntry.Semaphore,
                        (Action)(() => ReleaseLockObject(kind, parentKey, key, nextEntry))
                    );
                }
            );
        }

        private CacheLockEntry AcquireLockObject(Type kind, string parentKey, string key)
        {
            lock (_syncRoot)
            {
                var entries = this._lockObjects.Ensure(kind).Ensure(parentKey);
                CacheLockEntry entry;
                if (!entries.TryGetValue(key, out entry))
                {
                    entry = entries[key] = new CacheLockEntry();
                }
                ++entry.ReferenceCount;
                return entry;
            }
        }

        private void ReleaseLockObject(Type kind, string parentKey, string key, CacheLockEntry entry)
        {
            lock (_syncRoot)
            {
                if (--entry.ReferenceCount != 0 ||
                    !this._lockObjects.TryGetValue(kind, out var parents) ||
                    !parents.TryGetValue(parentKey, out var entries) ||
                    !entries.TryGetValue(key, out var currentEntry) ||
                    !ReferenceEquals(currentEntry, entry))
                {
                    return;
                }
                entries.Remove(key);
                if (entries.Count == 0)
                {
                    parents.Remove(parentKey);
                }
                if (parents.Count == 0)
                {
                    this._lockObjects.Remove(kind);
                }
            }
        }

        public TKind[] List<TKind>(string parentKey)
        {
            return TryGetList<TKind>(parentKey, out var list, out var listCacheContext) ? list : Array.Empty<TKind>();
        }

        public bool TryGetList<TKind>(string parentKey, out TKind[] list)
        {
            Action[] callbacks = Array.Empty<Action>();
            bool result;
            lock (_syncRoot)
            {
                if (this._listCached.Get(typeof(TKind))?.Contains(parentKey) != true)
                {
                    list = null;
                    return false;
                }
                result = TryGetListForceLocked(parentKey, out list, out callbacks);
            }
            InvokeCallbacks(callbacks);
            return result;
        }

        // listCacheContext は RequireListCacheUpdate() が呼ばれたあとのみ値が代入されます
        public bool TryGetList<TKind>(string parentKey, out TKind[] list, out object listCacheContext)
        {
            Action[] callbacks = Array.Empty<Action>();
            bool result;
            lock (_syncRoot)
            {
                if (this._listCached.Get(typeof(TKind))?.Contains(parentKey) != true)
                {
                    list = null;
                    listCacheContext = null;
                    return false;
                }

                result = TryGetListForceLocked(parentKey, out list, out callbacks);
                listCacheContext = result && IsListCacheUpdateRequiredLocked<TKind>(parentKey)
                    ? this._listCacheContexts.Get(typeof(TKind))?.Get(parentKey)
                    : null;
            }
            InvokeCallbacks(callbacks);
            return result;
        }
        
        public TKind[] ListForce<TKind>(string parentKey)
        {
            return TryGetListForce<TKind>(parentKey, out var list) ? list : Array.Empty<TKind>();
        }

        public bool TryGetListForce<TKind>(string parentKey, out TKind[] list)
        {
            Action[] callbacks;
            bool result;
            lock (_syncRoot)
            {
                result = TryGetListForceLocked(parentKey, out list, out callbacks);
            }
            InvokeCallbacks(callbacks);
            return result;
        }

        private static void InvokeCallbacks(IEnumerable<Action> callbacks)
        {
            foreach (var callback in callbacks)
            {
                try
                {
                    callback.Invoke();
                }
                catch (System.Exception)
                {
                    // Cache observers are independent and must not change the result of a completed update.
                }
            }
        }

        private static void InvokeCallbacks<T>(IEnumerable<Action<T>> callbacks, T value)
        {
            foreach (var callback in callbacks)
            {
                try
                {
                    callback.Invoke(value);
                }
                catch (System.Exception)
                {
                    // Cache observers are independent and must not change the result of a completed update.
                }
            }
        }

        private bool TryGetListForceLocked<TKind>(string parentKey, out TKind[] list, out Action[] callbacks)
        {
            var now = UnixTime.ToUnixTime(DateTime.Now);
            if (!this._cache.TryGetValue(typeof(TKind), out var parents) ||
                !parents.TryGetValue(parentKey, out var entries))
            {
                callbacks = Array.Empty<Action>();
                list = Array.Empty<TKind>();
                return true;
            }
            var values = entries.Values;
            if (values.Any(value => value.Item2 < now))
            {
                callbacks = ClearListCacheLocked<TKind>(parentKey);
                list = null;
                return false;
            }

            callbacks = Array.Empty<Action>();
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
