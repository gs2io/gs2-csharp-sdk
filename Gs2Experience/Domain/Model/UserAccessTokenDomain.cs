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
using Gs2.Gs2Experience.Domain.Iterator;
using Gs2.Gs2Experience.Model.Cache;
using Gs2.Gs2Experience.Request;
using Gs2.Gs2Experience.Result;
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

namespace Gs2.Gs2Experience.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ExperienceRestClient _client;
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
            this._client = new Gs2ExperienceRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Experience.Model.Status> Statuses(
            string experienceName = null
        )
        {
            return new DescribeStatusesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                experienceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Experience.Model.Status> StatusesAsync(
            #else
        public DescribeStatusesIterator StatusesAsync(
            #endif
            string experienceName = null
        )
        {
            return new DescribeStatusesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                experienceName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeStatuses(
            Action<Gs2.Gs2Experience.Model.Status[]> callback,
            string experienceName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Experience.Model.Status>(
                (null as Gs2.Gs2Experience.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await StatusesAsync(
                                experienceName
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
        public async UniTask<ulong> SubscribeStatusesWithInitialCallAsync(
            Action<Gs2.Gs2Experience.Model.Status[]> callback,
            string experienceName = null
        )
        {
            var items = await StatusesAsync(
                experienceName
            ).ToArrayAsync();
            var callbackId = SubscribeStatuses(
                callback,
                experienceName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeStatuses(
            ulong callbackId,
            string experienceName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Experience.Model.Status>(
                (null as Gs2.Gs2Experience.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public void InvalidateStatuses(
            string experienceName = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Experience.Model.Status>(
                (null as Gs2.Gs2Experience.Model.Status).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                )
            );
        }

        public Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain Status(
            string experienceName,
            string propertyId
        ) {
            return new Gs2.Gs2Experience.Domain.Model.StatusAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                experienceName,
                propertyId
            );
        }

    }
}
