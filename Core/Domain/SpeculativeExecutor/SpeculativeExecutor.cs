using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.SpeculativeExecutor
{
    internal interface IComposableSpeculativeCommit
    {
        string CompositionKey { get; }
        bool TryCompose(object current, bool hasCurrent, out object next);
        object Commit(object value);
    }

    public class SpeculativeExecutor
    {
        private sealed class PreparedAtomicVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Func<object>[] _commits;
            private readonly Func<object> _atomicCommit;

            internal PreparedAtomicVerification(
                Func<object>[] commits,
                Func<object> atomicCommit
            ) {
                _commits = commits;
                _atomicCommit = atomicCommit;
            }

            public bool IsStillSatisfied()
            {
                return _commits.All(commit =>
                    commit.Target is IPreparedSpeculativeVerification guard &&
                    guard.IsStillSatisfied()
                );
            }

            internal object Commit()
            {
                if (!IsStillSatisfied()) {
                    return null;
                }
                return _atomicCommit.Invoke();
            }
        }

        private ConsumeAction[] _consumeActions;
        private AcquireAction[] _acquireActions;
        private BigInteger _rate;

        public SpeculativeExecutor(
            ConsumeAction[] consumeActions,
            AcquireAction[] acquireActions,
            double rate
        ) {
            this._consumeActions = Transaction.Rate(consumeActions, rate);
            this._acquireActions = Transaction.Rate(acquireActions, rate);
            this._rate = (BigInteger) rate;
        }

        public SpeculativeExecutor(
            ConsumeAction[] consumeActions,
            AcquireAction[] acquireActions,
            BigInteger rate
        ) {
            this._consumeActions = Transaction.Rate(consumeActions, rate);
            this._acquireActions = Transaction.Rate(acquireActions, rate);
            this._rate = rate;
        }

        public static Func<object> BuildAtomicCommit(
            IReadOnlyCollection<Func<object>> commits,
            int expectedActionCount
        ) {
            if (commits == null || expectedActionCount < 0) {
                return null;
            }

            var commitSnapshot = commits.ToArray();
            if (commitSnapshot.Length > expectedActionCount) {
                return null;
            }

            var preparedCommits = commitSnapshot
                .Where(commit => commit != null)
                .ToArray();
            if (preparedCommits.Length == 0 && expectedActionCount != 0) {
                return null;
            }

            return () =>
            {
                IComposableSpeculativeCommit pendingTarget = null;
                object pendingValue = null;
                var pendingValid = false;
                Action flush = () =>
                {
                    if (pendingValid) {
                        pendingTarget.Commit(pendingValue);
                    }
                    pendingTarget = null;
                    pendingValue = null;
                    pendingValid = false;
                };
                foreach (var commit in preparedCommits) {
                    if (commit.Target is not IComposableSpeculativeCommit target) {
                        flush();
                        commit.Invoke();
                        continue;
                    }
                    if (pendingTarget == null ||
                        pendingTarget.CompositionKey != target.CompositionKey) {
                        flush();
                        pendingTarget = target;
                        pendingValid = target.TryCompose(
                            null,
                            false,
                            out pendingValue
                        );
                        continue;
                    }
                    if (!pendingValid) {
                        continue;
                    }
                    pendingValid = target.TryCompose(
                        pendingValue,
                        true,
                        out var next
                    );
                    pendingTarget = target;
                    pendingValue = next;
                }
                flush();
                return null;
            };
        }

        internal static Func<object> BuildAtomicVerificationCommit(
            IReadOnlyCollection<Func<object>> commits,
            int expectedActionCount
        ) {
            if (commits == null) {
                return null;
            }

            var commitSnapshot = commits.ToArray();
            if (commitSnapshot.Any(commit =>
                    commit?.Target is not IPreparedSpeculativeVerification
                )) {
                return null;
            }

            var atomicCommit = BuildAtomicCommit(
                commitSnapshot,
                expectedActionCount
            );
            if (atomicCommit == null) {
                return null;
            }

            var prepared = new PreparedAtomicVerification(
                commitSnapshot.Where(commit => commit != null).ToArray(),
                atomicCommit
            );
            return prepared.Commit;
        }

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<Func<object>> ExecuteFuture(
            Core.Domain.Gs2 domain,
            AccessToken accessToken = null
        ) => ExecuteAsync(domain, accessToken).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public async UniTask<Func<object>> ExecuteAsync(
#else
        public async Task<Func<object>> ExecuteAsync(
#endif 
            Core.Domain.Gs2 domain,
            AccessToken accessToken = null
        ) {
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(accessToken?.UserId)) {
                return null;
            }
            var commit = new List<Func<object>>();
            if (this._consumeActions != null) {
                foreach (var consumeAction in this._consumeActions) {
                    if (!IsDispatchable(
                            consumeAction?.Action,
                            consumeAction?.Request
                        )) {
                        continue;
                    }
                    {
                        var c = await Gs2.Gs2Account.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2AdReward.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Auth.Domain.SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Chat.Domain.SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Datastore.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Distributor.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Enchant.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Enhance.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Exchange.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Experience.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Formation.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Friend.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Gateway.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Grade.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Guild.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Identifier.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Idle.Domain.SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Inbox.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Inventory.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2JobQueue.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Key.Domain.SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Limit.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Lock.Domain.SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2LoginReward.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Lottery.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Matchmaking.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2MegaField.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Mission.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Money.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Money2.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2News.Domain.SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Quest.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Ranking.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Realtime.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Schedule.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Script.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2SerialKey.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Showcase.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2SkillTree.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Stamina.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2StateMachine.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Version.Domain.SpeculativeExecutor
                            .ConsumeActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                consumeAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
#if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: " + consumeAction.Action);
#else
                    System.Console.WriteLine("Speculative execution not supported on this action: " + consumeAction.Action);
#endif
                }
            }
            if (this._acquireActions != null) {
                foreach (var acquireAction in this._acquireActions) {
                    if (!IsDispatchable(
                            acquireAction?.Action,
                            acquireAction?.Request
                        )) {
                        continue;
                    }
                    {
                        var c = await Gs2.Gs2Account.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2AdReward.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Auth.Domain.SpeculativeExecutor.AcquireActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Chat.Domain.SpeculativeExecutor.AcquireActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Datastore.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Distributor.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Enchant.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Enhance.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Exchange.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Experience.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Formation.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Friend.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Gateway.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Grade.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Guild.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Identifier.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Idle.Domain.SpeculativeExecutor.AcquireActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Inbox.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Inventory.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2JobQueue.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Key.Domain.SpeculativeExecutor.AcquireActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Limit.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Lock.Domain.SpeculativeExecutor.AcquireActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2LoginReward.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Lottery.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Matchmaking.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2MegaField.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Mission.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Money.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Money2.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2News.Domain.SpeculativeExecutor.AcquireActionSpeculativeExecutorIndex
                            .ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Quest.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Ranking.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Realtime.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Schedule.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Script.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2SerialKey.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Showcase.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2SkillTree.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Stamina.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2StateMachine.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
                    {
                        var c = await Gs2.Gs2Version.Domain.SpeculativeExecutor
                            .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                                domain,
                                accessToken,
                                acquireAction,
                                this._rate
                            );
                        if (c != null) {
                            commit.Add(c);
                            continue;
                        }
                    }
#if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: " + acquireAction.Action);
#else
                    System.Console.WriteLine("Speculative execution not supported on this action: " + acquireAction.Action);
#endif
                }
            }
            return BuildAtomicCommit(
                commit,
                (this._consumeActions?.Length ?? 0) +
                (this._acquireActions?.Length ?? 0)
            );
        }

        private static bool IsDispatchable(string action, string request)
        {
            if (string.IsNullOrEmpty(action) || request == null) {
                return false;
            }
            try {
                JsonMapper.ToObject(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }
    }
}
