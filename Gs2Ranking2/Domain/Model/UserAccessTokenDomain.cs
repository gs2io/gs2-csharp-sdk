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
 */
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable ArrangeThisQualifier
// ReSharper disable NotAccessedField.Local

#pragma warning disable 1998
#pragma warning disable CS0169, CS0168

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Ranking2.Domain.Iterator;
using Gs2.Gs2Ranking2.Model.Cache;
using Gs2.Gs2Ranking2.Request;
using Gs2.Gs2Ranking2.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Ranking2.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.SubscribeUser> Subscribes(
            string rankingName
        )
        {
            return new DescribeSubscribesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.SubscribeUser> SubscribesAsync(
            #else
        public DescribeSubscribesIterator SubscribesAsync(
            #endif
            string rankingName
        )
        {
            return new DescribeSubscribesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSubscribes(
            Action<Gs2.Gs2Ranking2.Model.SubscribeUser[]> callback,
            string rankingName
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await SubscribesAsync(
                                rankingName
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSubscribesWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.SubscribeUser[]> callback,
            string rankingName
        )
        {
            var items = await SubscribesAsync(
                rankingName
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribes(
                callback,
                rankingName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscribes(
            ulong callbackId,
            string rankingName
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateSubscribes(
            string rankingName
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking2.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.SubscribeAccessTokenDomain Subscribe(
            string rankingName
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.SubscribeAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            );
        }

    }
}
