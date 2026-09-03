using System;
using Gs2.Core.Domain;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Util;
using Gs2.Gs2LoginReward.Model;
using Gs2.Gs2LoginReward.Model.Cache;

namespace Gs2.Gs2LoginReward.Domain.SpeculativeExecutor
{
    internal sealed class ReceiveStatusSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _bonusModelName;
        private readonly int? _timeOffset;
        private readonly string _expectedId;
        private readonly Func<ReceiveStatus, ReceiveStatus> _transform;

        internal ReceiveStatusSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string bonusModelName,
            int? timeOffset,
            string expectedId,
            Func<ReceiveStatus, ReceiveStatus> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _bonusModelName = bonusModelName;
            _timeOffset = timeOffset;
            _expectedId = expectedId;
            _transform = transform;
        }

        public string CompositionKey => string.Join(
            ":",
            "loginReward",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "ReceiveStatus",
            _bonusModelName
        );

        private bool IsExpected(ReceiveStatus item) {
            return item != null &&
                   item.ReceiveStatusId == _expectedId &&
                   item.UserId == _userId &&
                   item.BonusModelName == _bonusModelName;
        }

        internal bool CanPrepare() {
            var cached = ((ReceiveStatus)null).GetCache(
                _cache,
                _namespaceName,
                _userId,
                _bonusModelName,
                _timeOffset
            );
            return cached.Item2 && IsExpected(cached.Item1);
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                ReceiveStatus source;
                if (hasCurrent) {
                    source = current as ReceiveStatus;
                }
                else {
                    var cached = ((ReceiveStatus)null).GetCache(
                        _cache,
                        _namespaceName,
                        _userId,
                        _bonusModelName,
                        _timeOffset
                    );
                    source = cached.Item2 ? cached.Item1 : null;
                }
                if (!IsExpected(source)) {
                    next = null;
                    return false;
                }
                var changed = _transform(source);
                if (!IsExpected(changed)) {
                    next = null;
                    return false;
                }
                next = changed;
                return true;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is ReceiveStatus item && IsExpected(item)) {
                var current = ((ReceiveStatus)null).GetCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _bonusModelName,
                    _timeOffset
                );
                if (!current.Item2 || !IsExpected(current.Item1)) {
                    return null;
                }
                if (
                    current.Item1.Revision == 0 && item.Revision == 0 &&
                    current.Item1.ToJson().ToJson() != item.ToJson().ToJson()) {
                    _cache.Put(
                        ((ReceiveStatus)null).CacheParentKey(
                            _namespaceName,
                            _userId,
                            _timeOffset
                        ),
                        ((ReceiveStatus)null).CacheKey(_bonusModelName),
                        item,
                        UnixTime.ToUnixTime(DateTime.Now) +
                        1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    return null;
                }
                item.PutCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _bonusModelName,
                    _timeOffset
                );
            }
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }
}
