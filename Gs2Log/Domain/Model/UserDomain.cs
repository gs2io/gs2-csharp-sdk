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
 * deny overwrite
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Log.Domain.Iterator;
using Gs2.Gs2Log.Model.Cache;
using Gs2.Gs2Log.Request;
using Gs2.Gs2Log.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Log.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LogRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;
        public long? TotalCount { get; set; } = null!;
        public long? ScanSize { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LogRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
/* diff --- start
        //#if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.InGameLog> InGameLog(
            Gs2.Gs2Log.Model.InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryInGameLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                tags,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        //#endif

        //#if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.InGameLog> InGameLogAsync(
        //#else
        public QueryInGameLogIterator InGameLogAsync(
        //#endif
            Gs2.Gs2Log.Model.InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryInGameLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                tags,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }

        public ulong SubscribeInGameLog(
            Action<Gs2.Gs2Log.Model.InGameLog[]> callback,
            Gs2.Gs2Log.Model.InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.InGameLog>(
                (null as Gs2.Gs2Log.Model.InGameLog).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => tags == null || item.Tags == tags)
                    .Where(item => begin == null || item.Timestamp >= begin)
                    .Where(item => end == null || item.Timestamp <= end)
                    .ToArray()),
                () =>
                {
        //#if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        //#else
                    async Task Impl() {
        //#endif
                        try {
        //#if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        //#endif
                            callback.Invoke(await InGameLogAsync(
                                tags,
                                begin,
                                end,
                                longTerm
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        //#if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeInGameLogWithInitialCallAsync(
        //#else
        public async Task<ulong> SubscribeInGameLogWithInitialCallAsync(
        //#endif
            Action<Gs2.Gs2Log.Model.InGameLog[]> callback,
            Gs2.Gs2Log.Model.InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            var items = await InGameLogAsync(
                tags,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeInGameLog(
                callback,
                tags,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeInGameLog(
            ulong callbackId,
            Gs2.Gs2Log.Model.InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.InGameLog>(
                (null as Gs2.Gs2Log.Model.InGameLog).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateInGameLog(
            Gs2.Gs2Log.Model.InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Log.Model.InGameLog>(
                (null as Gs2.Gs2Log.Model.InGameLog).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                )
            );
        }

 diff --- end */
/* diff +++ start */
        
/* diff +++ end */
        public Gs2.Gs2Log.Domain.Model.InGameLogDomain InGameLog(
            string requestId
        ) {
            return new Gs2.Gs2Log.Domain.Model.InGameLogDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                requestId
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.InGameLogDomain> SendInGameLogFuture(
            SendInGameLogByUserIdRequest request
        ) => SendInGameLogAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.InGameLogDomain> SendInGameLogAsync(
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.InGameLogDomain> SendInGameLogAsync(
        #endif
            SendInGameLogByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SendInGameLogByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Log.Domain.Model.InGameLogDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.RequestId
            );

            return domain;
        }

    }

    public partial class UserDomain {

    }
}
