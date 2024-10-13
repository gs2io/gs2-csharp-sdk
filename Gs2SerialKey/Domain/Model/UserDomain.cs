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
using Gs2.Gs2SerialKey.Domain.Iterator;
using Gs2.Gs2SerialKey.Model.Cache;
using Gs2.Gs2SerialKey.Request;
using Gs2.Gs2SerialKey.Result;
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

namespace Gs2.Gs2SerialKey.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SerialKeyRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string Url { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SerialKeyRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2SerialKey.Model.SerialKey> SerialKeys(
            string campaignModelName,
            string issueJobName = null
        )
        {
            return new DescribeSerialKeysIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                campaignModelName,
                issueJobName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2SerialKey.Model.SerialKey> SerialKeysAsync(
            #else
        public DescribeSerialKeysIterator SerialKeysAsync(
            #endif
            string campaignModelName,
            string issueJobName = null
        )
        {
            return new DescribeSerialKeysIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                campaignModelName,
                issueJobName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSerialKeys(
            Action<Gs2.Gs2SerialKey.Model.SerialKey[]> callback,
            string campaignModelName,
            string issueJobName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SerialKey.Model.SerialKey>(
                (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSerialKeysWithInitialCallAsync(
            Action<Gs2.Gs2SerialKey.Model.SerialKey[]> callback,
            string campaignModelName,
            string issueJobName = null
        )
        {
            var items = await SerialKeysAsync(
                campaignModelName,
                issueJobName
            ).ToArrayAsync();
            var callbackId = SubscribeSerialKeys(
                callback,
                campaignModelName,
                issueJobName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSerialKeys(
            ulong callbackId,
            string campaignModelName,
            string issueJobName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SerialKey.Model.SerialKey>(
                (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain SerialKey(
            string serialKeyCode
        ) {
            return new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                serialKeyCode
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.UserDomain> DownloadSerialCodesFuture(
            DownloadSerialCodesRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.UserDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DownloadSerialCodesFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;
                this.Url = domain.Url = result?.Url;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.UserDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.UserDomain> DownloadSerialCodesAsync(
            #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.UserDomain> DownloadSerialCodesAsync(
            #endif
            DownloadSerialCodesRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.DownloadSerialCodesAsync(request)
            );
            var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> IssueOnceFuture(
            IssueOnceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.IssueOnceFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.UserId,
                    result?.Item?.Code
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> IssueOnceAsync(
            #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> IssueOnceAsync(
            #endif
            IssueOnceRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.IssueOnceAsync(request)
            );
            var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                result?.Item?.Code
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> VerifyCodeFuture(
            VerifyCodeByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.VerifyCodeByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                    this._gs2,
                    this.NamespaceName,
                    request.UserId,
                    result?.Item?.Code
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> VerifyCodeAsync(
            #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> VerifyCodeAsync(
            #endif
            VerifyCodeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.VerifyCodeByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                this._gs2,
                this.NamespaceName,
                request.UserId,
                result?.Item?.Code
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> RevertUseFuture(
            RevertUseByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.RevertUseByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                    this._gs2,
                    this.NamespaceName,
                    request.UserId,
                    result?.Item?.Code
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> RevertUseAsync(
            #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> RevertUseAsync(
            #endif
            RevertUseByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.RevertUseByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                this._gs2,
                this.NamespaceName,
                request.UserId,
                result?.Item?.Code
            );

            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}
