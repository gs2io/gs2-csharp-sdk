using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Mission.Model;
using Gs2.Gs2Mission.Model.Cache;
using Gs2.Gs2Mission.Model.Transaction;
using Gs2.Gs2Mission.Request;
using VerifyAction = Gs2.Core.Model.VerifyAction;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Mission.Domain.SpeculativeExecutor
{
    internal sealed class CompleteSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private sealed class CompleteCompositionState
        {
            internal Complete Item { get; }
            internal string[] ReceiveMissionTaskNames { get; }
            internal string RevertMissionTaskName { get; }

            internal CompleteCompositionState(
                Complete item,
                string[] receiveMissionTaskNames,
                string revertMissionTaskName
            ) {
                Item = item;
                ReceiveMissionTaskNames = receiveMissionTaskNames;
                RevertMissionTaskName = revertMissionTaskName;
            }
        }

        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _missionGroupName;
        private readonly int? _timeOffset;
        private readonly string _expectedCompleteId;
        private readonly Func<Complete, Complete> _transform;
        private readonly Func<Complete, IReadOnlyCollection<string>, Complete>
            _composeTransform;
        private readonly string[] _receiveMissionTaskNames;
        private readonly string _revertMissionTaskName;

        internal CompleteSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string missionGroupName,
            int? timeOffset,
            string expectedCompleteId,
            Func<Complete, Complete> transform,
            Func<Complete, IReadOnlyCollection<string>, Complete> composeTransform,
            string[] receiveMissionTaskNames,
            string revertMissionTaskName
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _missionGroupName = missionGroupName;
            _timeOffset = timeOffset;
            _expectedCompleteId = expectedCompleteId;
            _transform = transform;
            _composeTransform = composeTransform;
            _receiveMissionTaskNames = receiveMissionTaskNames;
            _revertMissionTaskName = revertMissionTaskName;
        }

        public string CompositionKey => string.Join(
            ":",
            "mission",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "Complete",
            _missionGroupName
        );

        private bool IsExpectedComplete(Complete item) {
            return item != null &&
                   item.CompleteId == _expectedCompleteId &&
                   item.UserId == _userId &&
                   item.MissionGroupName == _missionGroupName;
        }

        private bool TryMergeFoldState(
            CompleteCompositionState current,
            out string[] receiveMissionTaskNames,
            out string revertMissionTaskName,
            out bool duplicateRevert
        ) {
            var receive = current.ReceiveMissionTaskNames.ToList();
            var seen = new HashSet<string>(receive);
            foreach (var missionTaskName in
                     _receiveMissionTaskNames ?? Array.Empty<string>()) {
                if (seen.Add(missionTaskName)) {
                    receive.Add(missionTaskName);
                }
            }
            receiveMissionTaskNames = receive.ToArray();
            revertMissionTaskName = current.RevertMissionTaskName;
            duplicateRevert = false;
            if (_revertMissionTaskName == null) {
                return true;
            }
            if (revertMissionTaskName == null) {
                revertMissionTaskName = _revertMissionTaskName;
                return true;
            }
            if (revertMissionTaskName == _revertMissionTaskName) {
                duplicateRevert = true;
                return true;
            }
            return false;
        }

        private CompleteCompositionState InitialState(Complete item) {
            return new CompleteCompositionState(
                item,
                _receiveMissionTaskNames ?? Array.Empty<string>(),
                _revertMissionTaskName
            );
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
                out object next
        ) {
            try {
                if (hasCurrent) {
                    if (current is not CompleteCompositionState state ||
                        !IsExpectedComplete(state.Item) ||
                        !TryMergeFoldState(
                            state,
                            out var receiveMissionTaskNames,
                            out var revertMissionTaskName,
                            out var duplicateRevert
                        )) {
                        next = null;
                        return false;
                    }
                    var composedItem = duplicateRevert
                        ? state.Item
                        : _composeTransform(
                            state.Item,
                            state.ReceiveMissionTaskNames
                        );
                    if (!IsExpectedComplete(composedItem)) {
                        next = null;
                        return false;
                    }
                    next = new CompleteCompositionState(
                        composedItem,
                        receiveMissionTaskNames,
                        revertMissionTaskName
                    );
                    return true;
                }

                var (cachedItem, find) = ((Complete)null).GetCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _missionGroupName,
                    _timeOffset
                );
                if (!find || !IsExpectedComplete(cachedItem)) {
                    next = null;
                    return false;
                }
                var changed = _transform(cachedItem);
                next = InitialState(changed);
                return IsExpectedComplete(changed);
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is CompleteCompositionState state &&
                IsExpectedComplete(state.Item)) {
                state.Item.PutCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _missionGroupName,
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

    internal static class CompleteSpeculativeExecutor
    {
        private static string CompleteId(
            Gs2.Core.Domain.Gs2 domain,
            string namespaceName,
            string userId,
            string missionGroupName
        ) {
            return string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "mission",
                namespaceName,
                "user",
                userId,
                "group",
                missionGroupName,
                "complete"
            );
        }

        private static string MissionTaskId(
            Gs2.Core.Domain.Gs2 domain,
            string namespaceName,
            string missionGroupName,
            string missionTaskName
        ) {
            return string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "mission",
                namespaceName,
                "group",
                missionGroupName,
                "missionTaskModel",
                missionTaskName
            );
        }

        private static bool IsExpectedComplete(
            Complete item,
            string expectedCompleteId,
            string userId,
            string missionGroupName
        ) {
            return item != null &&
                   item.CompleteId == expectedCompleteId &&
                   item.UserId == userId &&
                   item.MissionGroupName == missionGroupName;
        }

        private static bool IsExpectedMissionTask(
            MissionTaskModel model,
            string expectedMissionTaskId,
            string missionTaskName
        ) {
            return model != null &&
                   model.MissionTaskId == expectedMissionTaskId &&
                   model.Name == missionTaskName;
        }

        private static bool RequiresCompleted(MissionTaskModel model) {
            return model.VerifyCompleteType != "consumeActions" &&
                   model.VerifyCompleteType != "verifyActions";
        }

        private static void ValidateReceiveRules(
            Complete source,
            IReadOnlyCollection<MissionTaskModel> missionTasks
        ) {
            if (source == null ||
                missionTasks.Any(RequiresCompleted) &&
                source.CompletedMissionTaskNames == null) {
                throw new NullReferenceException();
            }
            foreach (var missionTask in missionTasks) {
                if (RequiresCompleted(missionTask) &&
                    !source.CompletedMissionTaskNames.Contains(
                        missionTask.Name
                    )) {
                    throw new InvalidOperationException(
                        "mission task is not completed"
                    );
                }
            }
        }

        private static MissionTaskModel[] GetCachedMissionTasks(
            Gs2.Core.Domain.Gs2 domain,
            string namespaceName,
            string missionGroupName,
            IReadOnlyCollection<string> missionTaskNames
        ) {
            var result = new List<MissionTaskModel>(missionTaskNames.Count);
            foreach (var missionTaskName in missionTaskNames) {
                var (model, found) = ((MissionTaskModel)null).GetCache(
                    domain.Cache,
                    namespaceName,
                    missionGroupName,
                    missionTaskName,
                    null
                );
                if (!found || !IsExpectedMissionTask(
                        model,
                        MissionTaskId(
                            domain,
                            namespaceName,
                            missionGroupName,
                            missionTaskName
                        ),
                        missionTaskName
                    )) {
                    continue;
                }
                result.Add(model);
            }
            return result.ToArray();
        }

#if GS2_ENABLE_UNITASK
        internal static async UniTask<(bool KnownFalse, Func<object> Commit)>
            PrepareVerifyCompleteConsumeActionsAsync(
#else
        internal static async Task<(bool KnownFalse, Func<object> Commit)>
            PrepareVerifyCompleteConsumeActionsAsync(
#endif
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                IEnumerable<VerifyAction> actions,
                IReadOnlyCollection<Config> config
            ) {
            var commits = new List<Func<object>>();
            foreach (var action in actions ?? Array.Empty<VerifyAction>()) {
                VerifyAction snapshot;
                try {
                    snapshot = action?.Clone() as VerifyAction;
                    foreach (var entry in config ?? Array.Empty<Config>()) {
                        if (entry != null) {
                            snapshot = snapshot?.ApplyConfig(
                                entry.Key,
                                entry.Value
                            );
                        }
                    }
                }
                catch (System.Exception) {
                    continue;
                }
                if (snapshot == null) continue;

                Func<object> commit = null;
                try {
                    commit = await Gs2.Core.SpeculativeExecutor
                        .VerifyActionSpeculativeExecutor.ExecuteAsync(
                            domain,
                            accessToken,
                            snapshot,
                            BigInteger.One
                        );
                }
                catch (System.Exception) {
                    // Unknown locally. The inverse path may still prove false.
                }
                if (commit?.Target is IPreparedSpeculativeVerification) {
                    commits.Add(commit);
                    continue;
                }

                try {
                    commit = await Gs2.Core.SpeculativeExecutor
                        .VerifyActionSpeculativeExecutor.ExecuteInverseAsync(
                            domain,
                            accessToken,
                            snapshot,
                            BigInteger.One
                        );
                }
                catch (System.Exception) {
                    continue;
                }
                if (commit?.Target is IPreparedSpeculativeVerification) {
                    return (true, null);
                }
            }
            return (
                false,
                commits.Count == 0
                    ? null
                    : Gs2.Core.SpeculativeExecutor.SpeculativeExecutor
                        .BuildAtomicVerificationCommit(
                            commits,
                            commits.Count
                        )
            );
        }

#if GS2_ENABLE_UNITASK
        private static UniTask<Func<object>> Completed(Func<object> value) {
            return UniTask.FromResult(value);
        }

        private static UniTask<Func<object>> PrepareAsync(
#else
        private static Task<Func<object>> Completed(Func<object> value) {
            return Task.FromResult(value);
        }

        private static Task<Func<object>> PrepareAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            string namespaceName,
            string requestUserId,
            string missionGroupName,
            IReadOnlyCollection<string> missionTaskNames,
            Func<Complete, IReadOnlyCollection<MissionTaskModel>, Complete> transform,
            Func<Complete, IReadOnlyCollection<MissionTaskModel>, IReadOnlyCollection<string>, Complete> composeTransform,
            string[] receiveMissionTaskNames,
            string revertMissionTaskName
        ) {
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(accessToken?.UserId) ||
                accessToken.UserId != requestUserId ||
                string.IsNullOrEmpty(namespaceName) ||
                string.IsNullOrEmpty(missionGroupName) ||
                missionTaskNames == null) {
                return Completed(null);
            }
            var requestedMissionTaskNames = missionTaskNames
                .Where(value => !string.IsNullOrEmpty(value))
                .Distinct()
                .ToArray();
            if (requestedMissionTaskNames.Length == 0) {
                return Completed(null);
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            var expectedCompleteId = CompleteId(
                domain,
                namespaceName,
                userId,
                missionGroupName
            );

            var missionTasks = GetCachedMissionTasks(
                domain,
                namespaceName,
                missionGroupName,
                requestedMissionTaskNames
            );
            if (missionTasks.Length == 0) {
                return Completed(null);
            }
            var predictableMissionTaskNames = missionTasks
                .Select(model => model.Name)
                .ToArray();

            var (item, found) = ((Complete)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                missionGroupName,
                timeOffset
            );
            if (!found || !IsExpectedComplete(
                    item,
                    expectedCompleteId,
                    userId,
                    missionGroupName
                )) {
                return Completed(null);
            }

            Complete Transform(Complete source) {
                return transform(source, missionTasks);
            }

            Complete ComposeTransform(
                Complete source,
                IReadOnlyCollection<string> stagedReceiveMissionTaskNames
            ) {
                return composeTransform(
                    source,
                    missionTasks,
                    stagedReceiveMissionTaskNames
                );
            }

            var speculativeCommit = new CompleteSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                missionGroupName,
                timeOffset,
                expectedCompleteId,
                Transform,
                ComposeTransform,
                receiveMissionTaskNames == null
                    ? null
                    : predictableMissionTaskNames,
                revertMissionTaskName
            );
            return Completed(speculativeCommit.Invoke);
        }

#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> PrepareReceiveAsync(
#else
        internal static async Task<Func<object>> PrepareReceiveAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReceiveByUserIdRequest request
        ) {
            var preparedRequest = request == null
                ? null
                : new ReceiveByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithMissionGroupName(request.MissionGroupName)
                    .WithMissionTaskName(request.MissionTaskName)
                    .WithUserId(request.UserId);
            var preparedAccessToken = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            var logicalTimeMillis = Gs2.Core.Util.UnixTime.ToUnixTime(
                DateTime.Now
            ) + (long)(preparedAccessToken?.TimeOffset ?? 0) * 1000L;
            var missionTaskNames = new[] {
                preparedRequest?.MissionTaskName,
            };
            return await PrepareAsync(
                domain,
                preparedAccessToken,
                preparedRequest?.NamespaceName,
                preparedRequest?.UserId,
                preparedRequest?.MissionGroupName,
                missionTaskNames,
                (source, missionTasks) => {
                    ValidateReceiveRules(source, missionTasks);
                    var changed = source.SpeculativeExecution(preparedRequest);
                    changed.UpdatedAt = logicalTimeMillis;
                    return changed;
                },
                (source, missionTasks, stagedMissionTaskNames) => {
                    ValidateReceiveRules(source, missionTasks);
                    if (stagedMissionTaskNames.Contains(
                            preparedRequest.MissionTaskName
                        )) {
                        return source;
                    }
                    var changed = source.SpeculativeExecution(preparedRequest);
                    changed.UpdatedAt = logicalTimeMillis;
                    return changed;
                },
                missionTaskNames,
                null
            );
        }

#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> PrepareBatchReceiveAsync(
#else
        internal static async Task<Func<object>> PrepareBatchReceiveAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BatchReceiveByUserIdRequest request
        ) {
            var preparedRequest = request == null
                ? null
                : new BatchReceiveByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithMissionGroupName(request.MissionGroupName)
                    .WithMissionTaskNames(request.MissionTaskNames?.ToArray())
                    .WithUserId(request.UserId);
            var preparedAccessToken = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            var seen = new HashSet<string>();
            var missionTaskNames = (preparedRequest?.MissionTaskNames ??
                                    Array.Empty<string>())
                .Where(value =>
                    !string.IsNullOrEmpty(value) && seen.Add(value)
                )
                .ToArray();
            var logicalTimeMillis = Gs2.Core.Util.UnixTime.ToUnixTime(
                DateTime.Now
            ) + (long)(preparedAccessToken?.TimeOffset ?? 0) * 1000L;
            return await PrepareAsync(
                domain,
                preparedAccessToken,
                preparedRequest?.NamespaceName,
                preparedRequest?.UserId,
                preparedRequest?.MissionGroupName,
                missionTaskNames,
                (source, missionTasks) => {
                    ValidateReceiveRules(source, missionTasks);
                    var predictableRequest = new BatchReceiveByUserIdRequest()
                        .WithMissionTaskNames(
                            missionTasks.Select(model => model.Name).ToArray()
                        );
                    var changed = source.SpeculativeExecution(
                        predictableRequest
                    );
                    changed.UpdatedAt = logicalTimeMillis;
                    return changed;
                },
                (source, missionTasks, stagedMissionTaskNames) => {
                    ValidateReceiveRules(source, missionTasks);
                    if (source.ReceivedMissionTaskNames == null) {
                        throw new NullReferenceException();
                    }
                    var remaining = missionTasks
                        .Select(model => model.Name)
                        .Where(
                        value => !stagedMissionTaskNames.Contains(value)
                    ).ToArray();
                    if (remaining.Length == 0) {
                        return source;
                    }
                    var composeRequest = new BatchReceiveByUserIdRequest()
                        .WithMissionTaskNames(remaining);
                    var changed = source.SpeculativeExecution(composeRequest);
                    changed.UpdatedAt = logicalTimeMillis;
                    return changed;
                },
                missionTaskNames,
                null
            );
        }

#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> PrepareRevertReceiveAsync(
#else
        internal static async Task<Func<object>> PrepareRevertReceiveAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RevertReceiveByUserIdRequest request
        ) {
            var preparedRequest = request == null
                ? null
                : new RevertReceiveByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithMissionGroupName(request.MissionGroupName)
                    .WithMissionTaskName(request.MissionTaskName)
                    .WithUserId(request.UserId);
            var preparedAccessToken = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            var missionTaskNames = new[] {
                preparedRequest?.MissionTaskName,
            };
            return await PrepareAsync(
                domain,
                preparedAccessToken,
                preparedRequest?.NamespaceName,
                preparedRequest?.UserId,
                preparedRequest?.MissionGroupName,
                missionTaskNames,
                (source, _) => source.SpeculativeExecution(preparedRequest),
                (source, _, __) => source.SpeculativeExecution(preparedRequest),
                null,
                preparedRequest?.MissionTaskName
            );
        }
    }
}
