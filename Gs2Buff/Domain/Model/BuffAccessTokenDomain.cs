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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Buff.Domain.Iterator;
using Gs2.Gs2Buff.Model.Cache;
using Gs2.Gs2Buff.Request;
using Gs2.Gs2Buff.Result;
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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Buff.Domain.Model
{

    public partial class BuffAccessTokenDomain {
/* diff --- start
        private readonly Gs2.Core.Domain.Gs2 _gs2;
 diff --- end */
        public readonly Gs2.Core.Domain.Gs2 Gs2; /* diff +++ */
        private readonly Gs2BuffRestClient _client;
/* diff --- start
        public string NamespaceName { get; } = null!;
 diff --- end */
        public string NamespaceName { get; } /* diff +++ */
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public Gs2.Gs2Buff.Model.BuffEntryModel[] BuffEntryModels; /* diff +++ */

        public BuffAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
/* diff --- start
            this._gs2 = gs2;
 diff --- end */
            this.Gs2 = gs2; /* diff +++ */
            this._client = new Gs2BuffRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public IFuture<Gs2.Gs2Buff.Domain.Model.BuffEntryModelAccessTokenDomain[]> ApplyFuture(
 diff --- end */
        public IFuture<Gs2.Core.Domain.Gs2> ApplyFuture( /* diff +++ */
            ApplyBuffRequest request
        ) => ApplyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public async UniTask<Gs2.Gs2Buff.Domain.Model.BuffEntryModelAccessTokenDomain[]> ApplyAsync(
 diff --- end */
        public async UniTask<Gs2.Core.Domain.Gs2> ApplyAsync( /* diff +++ */
        #else
/* diff --- start
        public async Task<Gs2.Gs2Buff.Domain.Model.BuffEntryModelAccessTokenDomain[]> ApplyAsync(
 diff --- end */
        public async Task<Gs2.Core.Domain.Gs2> ApplyAsync( /* diff +++ */
        #endif
            ApplyBuffRequest request
        ) {
            request = request
/* diff --- start
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
 diff --- end */
                .WithContextStack(this.Gs2.DefaultContextStack) /* diff +++ */
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
/* diff --- start
                _gs2.Cache,
 diff --- end */
                Gs2.Cache, /* diff +++ */
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.ApplyBuffAsync(request)
            );
/* diff --- start
            var domain = result?.Items?.Select(v => new Gs2.Gs2Buff.Domain.Model.BuffEntryModelAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2Buff.Domain.Model.BuffEntryModelAccessTokenDomain>();
            return domain;
 diff --- end */
/* diff +++ start */
            this.BuffEntryModels = result?.Items;
            var newGs2 =  new Core.Domain.Gs2(
                this.Gs2.RestSession,
                this.Gs2.WebSocketSession,
                this.Gs2.DistributorNamespaceName
            );
            newGs2.DefaultContextStack = result?.NewContextStack;
            return newGs2;
/* diff +++ end */
        }

    }
}
