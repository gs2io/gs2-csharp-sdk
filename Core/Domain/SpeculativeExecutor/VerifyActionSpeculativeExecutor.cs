using System;
using System.Numerics;
using Gs2.Core.Model;
using Gs2.Gs2Auth.Model;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.SpeculativeExecutor
{
    internal interface IPreparedSpeculativeVerification
    {
        bool IsStillSatisfied();
    }

    public static class VerifyActionSpeculativeExecutor
    {
#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyAction verifyAction,
            BigInteger rate
        ) {
            if (domain?.RestSession == null || accessToken == null ||
                string.IsNullOrEmpty(verifyAction?.Action) ||
                verifyAction.Request == null) {
                return null;
            }
            verifyAction = verifyAction.Clone() as VerifyAction;

            var commit = await Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Enchant.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Experience.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Matchmaking.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Grade.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Inventory.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Ranking2.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Schedule.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Stamina.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Guild.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain, accessToken, verifyAction, rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Mission.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2SerialKey.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Limit.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            return await Gs2.Gs2Distributor.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteInverseAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyAction verifyAction,
            BigInteger rate
        ) {
            if (domain?.RestSession == null || accessToken == null ||
                string.IsNullOrEmpty(verifyAction?.Action) ||
                verifyAction.Request == null) {
                return null;
            }
            verifyAction = verifyAction.Clone() as VerifyAction;

            var commit = await Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Enchant.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Experience.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Matchmaking.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Grade.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Inventory.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Ranking2.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Schedule.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Stamina.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Guild.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain, accessToken, verifyAction, rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Mission.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2SerialKey.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            commit = await Gs2.Gs2Limit.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
            if (commit != null) {
                return commit;
            }

            return await Gs2.Gs2Distributor.Domain.SpeculativeExecutor
                .VerifyActionSpeculativeExecutorIndex.ExecuteAsync(
                    domain,
                    accessToken,
                    verifyAction,
                    rate
                );
        }
    }
}
