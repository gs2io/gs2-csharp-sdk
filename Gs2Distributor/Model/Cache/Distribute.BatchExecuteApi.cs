/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Distributor.Model.Cache
{
    public static partial class DistributeExt
    {
        public static void PutCache(
            this BatchExecuteApiResult self,
            CacheDatabase cache,
            string userId,
            BatchExecuteApiRequest request
        ) {
            foreach (var res in self.Results) {
                if (res.StatusCode / 100 != 2) {
                    throw Gs2Exception.ExtractError(res.ResultPayload, res.StatusCode ?? 999);
                }
                var req = request.RequestPayloads.First(v => v.RequestId == res.RequestId);
                switch (req.Service) {
                    case "account":
                        Gs2Account.Model.Cache.Gs2Account.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "adReward":
                        Gs2AdReward.Model.Cache.Gs2AdReward.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "buff":
                        Gs2Buff.Model.Cache.Gs2Buff.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "chat":
                        Gs2Chat.Model.Cache.Gs2Chat.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "datastore":
                        Gs2Datastore.Model.Cache.Gs2Datastore.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "dictionary":
                        Gs2Dictionary.Model.Cache.Gs2Dictionary.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "distributor":
                        Gs2Distributor.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "enchant":
                        Gs2Enchant.Model.Cache.Gs2Enchant.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "enhance":
                        Gs2Enhance.Model.Cache.Gs2Enhance.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "exchange":
                        Gs2Exchange.Model.Cache.Gs2Exchange.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "experience":
                        Gs2Experience.Model.Cache.Gs2Experience.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "formation":
                        Gs2Formation.Model.Cache.Gs2Formation.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "friend":
                        Gs2Friend.Model.Cache.Gs2Friend.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "gateway":
                        Gs2Gateway.Model.Cache.Gs2Gateway.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "grade":
                        Gs2Grade.Model.Cache.Gs2Grade.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "guard":
                        Gs2Guard.Model.Cache.Gs2Guard.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "guild":
                        Gs2Guild.Model.Cache.Gs2Guild.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "identifier":
                        Gs2Identifier.Model.Cache.Gs2Identifier.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "idle":
                        Gs2Idle.Model.Cache.Gs2Idle.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "inbox":
                        Gs2Inbox.Model.Cache.Gs2Inbox.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "inventory":
                        Gs2Inventory.Model.Cache.Gs2Inventory.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "jobQueue":
                        Gs2JobQueue.Model.Cache.Gs2JobQueue.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "key":
                        Gs2Key.Model.Cache.Gs2Key.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "limit":
                        Gs2Limit.Model.Cache.Gs2Limit.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "lock":
                        Gs2Lock.Model.Cache.Gs2Lock.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "log":
                        Gs2Log.Model.Cache.Gs2Log.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "loginReward":
                        Gs2LoginReward.Model.Cache.Gs2LoginReward.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "lottery":
                        Gs2Lottery.Model.Cache.Gs2Lottery.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "matchmaking":
                        Gs2Matchmaking.Model.Cache.Gs2Matchmaking.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "megaField":
                        Gs2MegaField.Model.Cache.Gs2MegaField.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "mission":
                        Gs2Mission.Model.Cache.Gs2Mission.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "money":
                        Gs2Money.Model.Cache.Gs2Money.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "money2":
                        Gs2Money2.Model.Cache.Gs2Money2.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "news":
                        Gs2News.Model.Cache.Gs2News.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "quest":
                        Gs2Quest.Model.Cache.Gs2Quest.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "ranking":
                        Gs2Ranking.Model.Cache.Gs2Ranking.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "ranking2":
                        Gs2Ranking2.Model.Cache.Gs2Ranking2.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "realtime":
                        Gs2Realtime.Model.Cache.Gs2Realtime.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "schedule":
                        Gs2Schedule.Model.Cache.Gs2Schedule.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "script":
                        Gs2Script.Model.Cache.Gs2Script.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "seasonRating":
                        Gs2SeasonRating.Model.Cache.Gs2SeasonRating.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "serialKey":
                        Gs2SerialKey.Model.Cache.Gs2SerialKey.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "showcase":
                        Gs2Showcase.Model.Cache.Gs2Showcase.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "skillTree":
                        Gs2SkillTree.Model.Cache.Gs2SkillTree.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "stamina":
                        Gs2Stamina.Model.Cache.Gs2Stamina.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "stateMachine":
                        Gs2StateMachine.Model.Cache.Gs2StateMachine.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                    case "version":
                        Gs2Version.Model.Cache.Gs2Version.PutCache(
                            cache,
                            userId,
                            req.MethodName,
                            req.Parameter,
                            res.ResultPayload
                        );
                        break;
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BatchExecuteApiResult> InvokeFuture(
            this BatchExecuteApiRequest request,
            CacheDatabase cache,
            string userId,
            Func<IFuture<BatchExecuteApiResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<BatchExecuteApiResult> self)
            {
                var future = invokeImpl();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }

                future.Result.PutCache(
                    cache,
                    userId,
                    request
                );

                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<BatchExecuteApiResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BatchExecuteApiResult> InvokeAsync(
    #else
        public static async Task<BatchExecuteApiResult> InvokeAsync(
    #endif
            this BatchExecuteApiRequest request,
            CacheDatabase cache,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BatchExecuteApiResult>> invokeImpl
    #else
            Func<Task<BatchExecuteApiResult>> invokeImpl
    #endif
        )
        {
            var result = await invokeImpl();
            result.PutCache(
                cache,
                userId,
                request
            );
            return result;
        }
#endif
    }
}