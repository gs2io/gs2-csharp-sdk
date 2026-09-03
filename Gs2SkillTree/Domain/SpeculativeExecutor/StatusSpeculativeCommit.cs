using System;
using Gs2.Core.Domain;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Util;
using Gs2.Gs2SkillTree.Model;
using Gs2.Gs2SkillTree.Model.Cache;

namespace Gs2.Gs2SkillTree.Domain.SpeculativeExecutor
{
    internal sealed class StatusSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _propertyId;
        private readonly int? _timeOffset;
        private readonly string _expectedId;
        private readonly long? _preparedRevision;
        private readonly bool _preserveAuthoritativeReplacement;
        private readonly Func<Status, Status> _transform;

        internal StatusSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
            int? timeOffset,
            string expectedId,
            Func<Status, Status> transform,
            bool preserveAuthoritativeReplacement = false
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _propertyId = propertyId;
            _timeOffset = timeOffset;
            _expectedId = expectedId;
            _transform = transform;
            _preserveAuthoritativeReplacement =
                preserveAuthoritativeReplacement;
            var cached = ((Status)null).GetCache(
                _cache,
                _namespaceName,
                _userId,
                _propertyId,
                _timeOffset
            );
            _preparedRevision = cached.Item2
                ? cached.Item1?.Revision
                : null;
        }

        public string CompositionKey => string.Join(
            ":",
            "skillTree",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "Status",
            _propertyId
        );

        private bool IsExpected(Status item) {
            return item != null &&
                   item.StatusId == _expectedId &&
                   item.UserId == _userId &&
                   item.PropertyId == _propertyId;
        }

        internal bool CanPrepare() {
            var cached = ((Status)null).GetCache(
                _cache,
                _namespaceName,
                _userId,
                _propertyId,
                _timeOffset
            );
            return cached.Item2 && IsExpected(cached.Item1) &&
                   (!_preserveAuthoritativeReplacement ||
                    cached.Item1.Revision <= 0 ||
                    cached.Item1.Revision == _preparedRevision);
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                Status source;
                if (hasCurrent) {
                    source = current as Status;
                }
                else {
                    var cached = ((Status)null).GetCache(
                        _cache,
                        _namespaceName,
                        _userId,
                        _propertyId,
                        _timeOffset
                    );
                    source = cached.Item2 ? cached.Item1 : null;
                    if (_preserveAuthoritativeReplacement &&
                        source?.Revision > 0 &&
                        source.Revision != _preparedRevision) {
                        next = null;
                        return false;
                    }
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
            if (value is not Status item || !IsExpected(item)) {
                return null;
            }
            var current = ((Status)null).GetCache(
                _cache,
                _namespaceName,
                _userId,
                _propertyId,
                _timeOffset
            );
            if (!current.Item2 || !IsExpected(current.Item1)) {
                return null;
            }
            if (_preserveAuthoritativeReplacement &&
                current.Item1.Revision > 0 &&
                current.Item1.Revision != _preparedRevision) {
                return null;
            }
            _cache.Put(
                ((Status)null).CacheParentKey(
                    _namespaceName,
                    _userId,
                    _timeOffset
                ),
                ((Status)null).CacheKey(_propertyId),
                item,
                UnixTime.ToUnixTime(DateTime.Now) +
                1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }
}
