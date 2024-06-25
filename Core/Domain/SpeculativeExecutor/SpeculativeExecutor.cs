using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Gs2Auth.Model;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.SpeculativeExecutor
{
    public class SpeculativeExecutor
    {
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

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<Func<object>> ExecuteFuture(
            Core.Domain.Gs2 domain,
            AccessToken accessToken = null
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                var commit = new List<Func<object>>();
                if (this._consumeActions != null) {
                    foreach (var consumeAction in this._consumeActions) {
                        {
                            var future = Gs2.Gs2Account.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2AdReward.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Auth.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Chat.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Datastore.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Distributor.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Enchant.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Exchange.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Experience.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Formation.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Friend.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Gateway.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Identifier.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Idle.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Inbox.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Inventory.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2JobQueue.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Key.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Limit.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Lock.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2LoginReward.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Lottery.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Matchmaking.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2MegaField.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Mission.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Money.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2News.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Quest.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Ranking.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Realtime.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Schedule.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Script.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2SerialKey.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Showcase.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2SkillTree.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Stamina.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2StateMachine.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Version.Domain.SpeculativeExecutor
                                .ConsumeActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    consumeAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: " +
                                                     consumeAction.Action);
                    }
                }
                if (this._acquireActions != null) {
                    foreach (var acquireAction in this._acquireActions) {
                        {
                            var future = Gs2.Gs2Account.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2AdReward.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Auth.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Chat.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Datastore.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Distributor.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Enchant.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Exchange.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Experience.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Formation.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Friend.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Gateway.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Identifier.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Idle.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Inbox.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Inventory.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2JobQueue.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Key.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Limit.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Lock.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2LoginReward.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Lottery.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Matchmaking.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2MegaField.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Mission.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Money.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2News.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Quest.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Ranking.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Realtime.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Schedule.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Script.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2SerialKey.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Showcase.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2SkillTree.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Stamina.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2StateMachine.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        {
                            var future = Gs2.Gs2Version.Domain.SpeculativeExecutor
                                .AcquireActionSpeculativeExecutorIndex.ExecuteFuture(
                                    domain,
                                    accessToken,
                                    acquireAction,
                                    this._rate
                                );
                            yield return future;
                            if (future.Error != null) {
                                result.OnError(future.Error);
                                yield break;
                            }
                            if (future.Result != null) {
                                commit.Add(future.Result);
                                continue;
                            }
                        }
                        UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: " +
                                                     acquireAction.Action);
                    }
                }
                result.OnComplete(() =>
                {
                    foreach (var c in commit) {
                        c?.Invoke();
                    }
                    return null;
                });
                yield return null;
            }

            return new Gs2InlineFuture<Func<object>>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask<Func<object>> ExecuteAsync(
    #else
        public async Task<Func<object>> ExecuteAsync(
    #endif 
            Core.Domain.Gs2 domain,
            AccessToken accessToken = null
        ) {
            var commit = new List<Func<object>>();
            if (this._consumeActions != null) {
                foreach (var consumeAction in this._consumeActions) {
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
            return () =>
            {
                foreach (var c in commit) {
                    c?.Invoke();
                }
                return null;
            };
        }
#endif
    }
}
