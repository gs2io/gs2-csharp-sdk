using System;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Grade.Model;
using Gs2.Gs2Grade.Model.Cache;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

#pragma warning disable 1998

namespace Gs2.Gs2Grade.Domain.SpeculativeExecutor
{
    internal sealed class StatusSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _gradeName;
        private readonly string _propertyId;
        private readonly int? _timeOffset;
        private readonly string _expectedStatusId;
        private readonly Func<Status, Status> _transform;

        internal StatusSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string gradeName,
            string propertyId,
            int? timeOffset,
            string expectedStatusId,
            Func<Status, Status> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _gradeName = gradeName;
            _propertyId = propertyId;
            _timeOffset = timeOffset;
            _expectedStatusId = expectedStatusId;
            _transform = transform;
        }

        public string CompositionKey => string.Join(
            ":",
            "grade",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "Status",
            _gradeName,
            _propertyId
        );

        private bool IsExpectedStatus(Status item) {
            return item != null &&
                   item.StatusId == _expectedStatusId &&
                   item.UserId == _userId &&
                   item.GradeName == _gradeName &&
                   item.PropertyId == _propertyId;
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                Status source;
                if (hasCurrent) {
                    if (current is not Status currentStatus ||
                        !IsExpectedStatus(currentStatus)) {
                        next = null;
                        return false;
                    }
                    source = currentStatus;
                }
                else {
                    var (cachedItem, find) = ((Status)null).GetCache(
                        _cache,
                        _namespaceName,
                        _userId,
                        _gradeName,
                        _propertyId,
                        _timeOffset
                    );
                    if (!find) {
                        next = null;
                        return false;
                    }
                    if (!IsExpectedStatus(cachedItem)) {
                        next = null;
                        return false;
                    }
                    source = cachedItem;
                }
                next = _transform(source);
                return IsExpectedStatus(next as Status);
            }
            catch (Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is Status item && IsExpectedStatus(item)) {
                item.PutCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _gradeName,
                    _propertyId,
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

    internal static class StatusSpeculativeExecutor
    {
        private static bool HasMatchingUser(
            AccessToken accessToken,
            string requestUserId
        ) {
            return !string.IsNullOrEmpty(accessToken?.UserId) &&
                   accessToken.UserId == requestUserId;
        }

#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> PrepareAsync(
#else
        internal static async Task<Func<object>> PrepareAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            string namespaceName,
            string requestUserId,
            string gradeName,
            string propertyId,
            Func<Status, Status> transform
        ) {
            if (domain?.RestSession == null ||
                !HasMatchingUser(accessToken, requestUserId) ||
                string.IsNullOrEmpty(namespaceName) ||
                string.IsNullOrEmpty(gradeName) ||
                string.IsNullOrEmpty(propertyId) ||
                transform == null) {
                return null;
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            var namespaceDomain = domain.Grade.Namespace(namespaceName);
            var statusDomain = namespaceDomain.AccessToken(
                accessToken
            ).Status(
                gradeName,
                propertyId
            );
            propertyId = statusDomain.PropertyId;
            if (string.IsNullOrEmpty(propertyId)) {
                return null;
            }

            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            var expectedStatusId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "grade",
                namespaceName,
                "user",
                userId,
                "gradeModel",
                gradeName,
                "property",
                propertyId
            );
            var (item, statusFound) = ((Status)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                gradeName,
                propertyId,
                timeOffset
            );
            bool IsExpectedStatus(Status value) {
                return value != null &&
                       value.StatusId == expectedStatusId &&
                       value.UserId == userId &&
                       value.GradeName == gradeName &&
                       value.PropertyId == propertyId;
            }
            if (!statusFound || !IsExpectedStatus(item)) {
                return null;
            }

            var speculativeCommit = new StatusSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                gradeName,
                propertyId,
                timeOffset,
                expectedStatusId,
                transform
            );
            return speculativeCommit.Invoke;
        }
    }
}
