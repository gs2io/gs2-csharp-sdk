using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Util;

namespace Gs2.Core.Domain
{
    public class CacheDatabase
    {
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>> _cache = new Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>>();
        private readonly Dictionary<Type, HashSet<string>> _listCached = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, HashSet<string>> _listCacheUpdateRequired = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, object>> _listCacheContexts = new Dictionary<Type, Dictionary<string, object>>();

        public void Clear()
        {
            _cache.Clear();
            _listCached.Clear();
            _listCacheContexts.Clear();
            _listCacheUpdateRequired.Clear();
        }

        public void SetListCached<TKind>(string parentKey, object listCacheContext = null)
        {
            _listCached.Ensure(typeof(TKind)).Add(parentKey);
            _listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
            if (listCacheContext != null)
            {
                _listCacheContexts.Ensure(typeof(TKind))[parentKey] = listCacheContext;
            }
        }

        public void ClearListCache<TKind>(string parentKey)
        {
            _cache.Get(typeof(TKind))?.Get(parentKey)?.Clear();
            _listCached.Get(typeof(TKind))?.Remove(parentKey);
            _listCacheUpdateRequired.Get(typeof(TKind))?.Remove(parentKey);
            _listCacheContexts.Get(typeof(TKind))?.Remove(parentKey);
        }

        public void RequireListCacheUpdate<TKind>(string parentKey)
        {
            _listCacheUpdateRequired.Ensure(typeof(TKind)).Add(parentKey);
        }

        private bool IsListCacheUpdateRequired<TKind>(string parentKey)
        {
            return _listCacheUpdateRequired.Get(typeof(TKind))?.Contains(parentKey) == true;
        }

        public void Put<TKind>(string parentKey, string key, TKind obj, long ttl)
        {
            if (ttl == 0)
            {
                ttl = UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.DefaultCacheMinutes;
            }
            _cache.Ensure(typeof(TKind)).Ensure(parentKey)[key] = new Tuple<object, long>(obj, ttl);
        }

        public void Delete<TKind>(string parentKey, string key)
        {
            _cache.Get(typeof(TKind))?.Get(parentKey)?.Remove(key);
        }

        public Tuple<TKind, bool> Get<TKind>(string parentKey, string key)
        {
            var cache = _cache.Get(typeof(TKind))?.Get(parentKey);
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

        public TKind[] List<TKind>(string parentKey)
        {
            return TryGetList<TKind>(parentKey, out var list, out var listCacheContext) ? list : Array.Empty<TKind>();
        }

        public bool TryGetList<TKind>(string parentKey, out TKind[] list)
        {
            if (_listCached.Get(typeof(TKind))?.Contains(parentKey) != true)
            {
                list = null;
                return false;
            }

            var now = UnixTime.ToUnixTime(DateTime.Now);
            var values = _cache.Ensure(typeof(TKind)).Ensure(parentKey).Values;
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
                    ? _listCacheContexts.Get(typeof(TKind))?.Get(parentKey)
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