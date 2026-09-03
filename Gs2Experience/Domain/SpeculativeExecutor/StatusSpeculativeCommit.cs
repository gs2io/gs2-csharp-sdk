using System;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Experience.Model;
using Gs2.Gs2Experience.Model.Cache;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

#pragma warning disable 1998

namespace Gs2.Gs2Experience.Domain.SpeculativeExecutor
{
    internal sealed class StatusSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _experienceName;
        private readonly string _propertyId;
        private readonly int? _timeOffset;
        private readonly string _expectedStatusId;
        private readonly string _expectedModelId;
        private readonly Func<Status, ExperienceModel, Status> _fullTransform;
        private readonly Func<Status, Status> _partialTransform;

        internal StatusSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string experienceName,
            string propertyId,
            int? timeOffset,
            string expectedStatusId,
            string expectedModelId,
            Func<Status, ExperienceModel, Status> fullTransform,
            Func<Status, Status> partialTransform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _experienceName = experienceName;
            _propertyId = propertyId;
            _timeOffset = timeOffset;
            _expectedStatusId = expectedStatusId;
            _expectedModelId = expectedModelId;
            _fullTransform = fullTransform;
            _partialTransform = partialTransform;
        }

        public string CompositionKey => string.Join(
            ":",
            "experience",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "Status",
            _experienceName,
            _propertyId
        );

        private bool IsExpectedStatus(Status item) {
            return item != null &&
                   item.StatusId == _expectedStatusId &&
                   item.UserId == _userId &&
                   item.ExperienceName == _experienceName &&
                   item.PropertyId == _propertyId;
        }

        private ExperienceModel GetLiveModel() {
            var (model, find) = ((ExperienceModel)null).GetCache(
                _cache,
                _namespaceName,
                _experienceName,
                null
            );
            return find && model != null &&
                   model.ExperienceModelId == _expectedModelId &&
                   model.Name == _experienceName
                ? model
                : null;
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                var model = GetLiveModel();
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
                        _experienceName,
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
                next = model != null
                    ? _fullTransform(source, model)
                    : _partialTransform(source);
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
                    _experienceName,
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
        private static bool IsExpectedModel(
            ExperienceModel model,
            string expectedModelId,
            string experienceName
        ) {
            return model != null &&
                   model.ExperienceModelId == expectedModelId &&
                   model.Name == experienceName;
        }

        internal static Status WithExperience(
            Status source,
            long value
        ) {
            if (source?.Clone() is not Status clone) {
                throw new NullReferenceException();
            }
            clone.ExperienceValue = value;
            clone.Revision = 0;
            return clone;
        }

        internal static Status WithExperienceDelta(
            Status source,
            long delta
        ) {
            if (source?.ExperienceValue == null) {
                throw new NullReferenceException();
            }
            return WithExperience(
                source,
                checked(source.ExperienceValue.Value + delta)
            );
        }

        internal static Status WithSubtractedExperience(
            Status source,
            long value
        ) {
            if (source?.ExperienceValue == null) {
                throw new NullReferenceException();
            }
            var changed = checked(source.ExperienceValue.Value - value);
            if (changed < 0) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            return WithExperience(source, changed);
        }

        internal static Status WithAddedExperience(
            Status source,
            long value,
            bool truncateExperienceWhenRankUp
        ) {
            if (truncateExperienceWhenRankUp) {
                if (source?.ExperienceValue == null ||
                    source.NextRankUpExperienceValue == null) {
                    throw new NullReferenceException();
                }
                var remaining = checked(
                    source.NextRankUpExperienceValue.Value -
                    source.ExperienceValue.Value
                );
                if (value > remaining) {
                    value = remaining;
                }
            }
            return WithExperienceDelta(source, value);
        }

        internal static Status WithAddedExperience(
            Status source,
            ExperienceModel model,
            long value,
            bool truncateExperienceWhenRankUp
        ) {
            if (source?.RankCapValue == null || model == null) {
                throw new NullReferenceException();
            }
            var changed = WithAddedExperience(
                source,
                value,
                truncateExperienceWhenRankUp
            );
            return model.RecalculateStatus(
                source,
                changed.ExperienceValue.Value,
                source.RankCapValue.Value
            );
        }

        internal static Status WithRankCap(
            Status source,
            long value
        ) {
            if (source?.Clone() is not Status clone) {
                throw new NullReferenceException();
            }
            clone.RankCapValue = value;
            if (value <= 1) {
                clone.ExperienceValue = 0;
                clone.RankValue = 1;
                clone.NextRankUpExperienceValue = 0;
            }
            clone.Revision = 0;
            return clone;
        }

        internal static Status WithRankCapDelta(
            Status source,
            long delta
        ) {
            if (source?.RankCapValue == null) {
                throw new NullReferenceException();
            }
            return WithRankCap(
                source,
                checked(source.RankCapValue.Value + delta)
            );
        }

        internal static Status WithSubtractedRankCap(
            Status source,
            long value
        ) {
            if (source?.RankCapValue == null) {
                throw new NullReferenceException();
            }
            var changed = checked(source.RankCapValue.Value - value);
            if (changed <= 0) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            return WithRankCap(source, changed);
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
            string experienceName,
            string propertyId,
            Func<Status, ExperienceModel, Status> fullTransform,
            Func<Status, Status> partialTransform
        ) {
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(accessToken?.UserId) ||
                accessToken.UserId != requestUserId ||
                propertyId == null ||
                fullTransform == null || partialTransform == null) {
                return null;
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            propertyId = propertyId
                .Replace("{region}", domain.RestSession.Region.DisplayName())
                .Replace("{ownerId}", domain.RestSession.OwnerId ?? "")
                .Replace("{userId}", userId);

            var expectedStatusId = string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "experience",
                namespaceName,
                "user",
                userId,
                "experienceModel",
                experienceName,
                "property",
                propertyId
            );
            var expectedModelId = string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "experience",
                namespaceName,
                "model",
                experienceName
            );

            var (model, modelFound) = ((ExperienceModel)null).GetCache(
                domain.Cache,
                namespaceName,
                experienceName,
                null
            );
            if (!modelFound ||
                !IsExpectedModel(model, expectedModelId, experienceName)) {
                model = null;
            }

            var (item, statusFound) = ((Status)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                experienceName,
                propertyId,
                timeOffset
            );
            if (!statusFound) {
                return null;
            }

            bool IsExpectedStatus(Status value) {
                return value != null &&
                       value.StatusId == expectedStatusId &&
                       value.UserId == userId &&
                       value.ExperienceName == experienceName &&
                       value.PropertyId == propertyId;
            }
            if (!IsExpectedStatus(item)) {
                return null;
            }

            var speculativeCommit = new StatusSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                experienceName,
                propertyId,
                timeOffset,
                expectedStatusId,
                expectedModelId,
                fullTransform,
                partialTransform
            );
            return speculativeCommit.Invoke;
        }
    }
}
