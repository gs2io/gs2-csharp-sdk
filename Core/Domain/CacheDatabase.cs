using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Util;

namespace Gs2.Core.Domain
{
    public class CacheDatabase
    {
        private readonly Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>> _cache = new Dictionary<Type, Dictionary<string, Dictionary<string, Tuple<object, long>>>>();
        private readonly Dictionary<Type, Dictionary<string, bool>> _listCached = new Dictionary<Type, Dictionary<string, bool>>();

        public void Clear()
        {
            _cache.Clear();
            _listCached.Clear();
        }
        
        public bool IsListCached<TKind>(string parentKey) {
            if (!_listCached.ContainsKey(typeof(TKind)))
            {
                _listCached[typeof(TKind)] = new Dictionary<string, bool>();
            }
            return _listCached[typeof(TKind)].ContainsKey(parentKey);
        }

        public void ListCached<TKind>(string parentKey) {
            if (!_listCached.ContainsKey(typeof(TKind)))
            {
                _listCached[typeof(TKind)] = new Dictionary<string, bool>();
            }
            _listCached[typeof(TKind)][parentKey] = true;
        }

        public void ListCacheClear<TKind>(string parentKey) {
            if (!_cache.ContainsKey(typeof(TKind)))
            {
                _cache[typeof(TKind)] = new Dictionary<string, Dictionary<string, Tuple<object, long>>>();
            }
            if (!_cache[typeof(TKind)].ContainsKey(parentKey))
            {
                _cache[typeof(TKind)][parentKey] = new Dictionary<string, Tuple<object, long>>();
            }
            _cache[typeof(TKind)][parentKey].Clear();
            if (!_listCached.ContainsKey(typeof(TKind)))
            {
                _listCached[typeof(TKind)] = new Dictionary<string, bool>();
            }
            _listCached[typeof(TKind)].Remove(parentKey);
        }

        public void Put<TKind>(string parentKey, string key, TKind obj, long ttl)
        {
            if (!_cache.ContainsKey(typeof(TKind)))
            {
                _cache[typeof(TKind)] = new Dictionary<string, Dictionary<string, Tuple<object, long>>>();
            }
            if (!_cache[typeof(TKind)].ContainsKey(parentKey))
            {
                _cache[typeof(TKind)][parentKey] = new Dictionary<string, Tuple<object, long>>();
            }

            if (ttl == 0)
            {
                ttl = UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.DefaultCacheMinutes;
            }
            _cache[typeof(TKind)][parentKey][key] = new Tuple<object, long>(obj, ttl);
        }

        public void Delete<TKind>(string parentKey, string key)
        {
            if (!_cache.ContainsKey(typeof(TKind)))
            {
                _cache[typeof(TKind)] = new Dictionary<string, Dictionary<string, Tuple<object, long>>>();
            }
            if (!_cache[typeof(TKind)].ContainsKey(parentKey))
            {
                _cache[typeof(TKind)][parentKey] = new Dictionary<string, Tuple<object, long>>();
            }
            _cache[typeof(TKind)][parentKey].Remove(key);
        }

        public TKind Get<TKind>(string parentKey, string key)
        {
            if (!_cache.ContainsKey(typeof(TKind)))
            {
                _cache[typeof(TKind)] = new Dictionary<string, Dictionary<string, Tuple<object, long>>>();
            }
            if (!_cache[typeof(TKind)].ContainsKey(parentKey))
            {
                _cache[typeof(TKind)][parentKey] = new Dictionary<string, Tuple<object, long>>();
            }
            if (_cache[typeof(TKind)][parentKey].ContainsKey(key))
            {
                var pair = _cache[typeof(TKind)][parentKey][key];
                var obj = pair.Item1;
                var ttl = pair.Item2;
                if (ttl < UnixTime.ToUnixTime(DateTime.Now))
                {
                    Delete<TKind>(
                        parentKey,
                        key
                    );
                    return default;
                }

                return (TKind) obj;
            }
            else
            {
                return default;
            }
        }

        public TKind[] List<TKind>(string parentKey)
        {
            if (!_cache.ContainsKey(typeof(TKind)))
            {
                _cache[typeof(TKind)] = new Dictionary<string, Dictionary<string, Tuple<object, long>>>();
            }
            if (!_cache[typeof(TKind)].ContainsKey(parentKey))
            {
                _cache[typeof(TKind)][parentKey] = new Dictionary<string, Tuple<object, long>>();
            }
            return _cache[typeof(TKind)][parentKey].Values.Where(pair =>
            {
                return pair.Item2 >= UnixTime.ToUnixTime(DateTime.Now);
            }).Select(pair => (TKind)pair.Item1).ToArray();
        }
    }
}