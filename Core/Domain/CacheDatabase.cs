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

        public void Clear()
        {
            _cache.Clear();
            _listCached.Clear();
        }
        
        public bool IsListCached<TKind>(string parentKey) {
            return _listCached.Get(typeof(TKind))?.Contains(parentKey) == true;
        }

        public void ListCached<TKind>(string parentKey) {
            _listCached.Ensure(typeof(TKind)).Add(parentKey);
        }

        public void ListCacheClear<TKind>(string parentKey) {
            _cache.Get(typeof(TKind))?.Get(parentKey)?.Clear();
            _listCached.Get(typeof(TKind))?.Remove(parentKey);
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
                    ListCacheClear<TKind>(parentKey);
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
            var values = _cache.Ensure(typeof(TKind)).Ensure(parentKey).Values;
            if (values.Count(pair => pair.Item2 < UnixTime.ToUnixTime(DateTime.Now)) > 0)
            {
                ListCacheClear<TKind>(parentKey);
            }
            return values
                .Where(pair => pair.Item2 >= UnixTime.ToUnixTime(DateTime.Now))
                .Select(pair => (TKind)pair.Item1).Where(v => v != null)
                .ToArray();
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