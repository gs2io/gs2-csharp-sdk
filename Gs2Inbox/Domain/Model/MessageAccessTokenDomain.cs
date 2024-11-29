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
using Gs2.Gs2Inbox.Domain.Iterator;
using Gs2.Gs2Inbox.Model.Cache;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
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

namespace Gs2.Gs2Inbox.Domain.Model
{

    public partial class MessageAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InboxRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string MessageName { get; } = null!;

        public MessageAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string messageName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InboxRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.MessageName = messageName;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inbox.Model.Message> GetFuture(
            GetMessageRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Model.Message> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithMessageName(this.MessageName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetMessageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Model.Message>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Inbox.Model.Message> GetAsync(
            #else
        private async Task<Gs2.Gs2Inbox.Model.Message> GetAsync(
            #endif
            GetMessageRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithMessageName(this.MessageName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.GetMessageAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> OpenFuture(
            OpenMessageRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithMessageName(this.MessageName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.OpenMessageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> OpenAsync(
            #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> OpenAsync(
            #endif
            OpenMessageRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithMessageName(this.MessageName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.OpenMessageAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> ReadFuture(
            ReadMessageRequest request,
            bool speculativeExecute = true
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithMessageName(this.MessageName);

                if (speculativeExecute) {
                    var speculativeExecuteFuture = Gs2.Gs2Inbox.Domain.Transaction.SpeculativeExecutor.ReadMessageByUserIdSpeculativeExecutor.ExecuteFuture(
                        this._gs2,
                        AccessToken,
                        ReadMessageByUserIdRequest.FromJson(request.ToJson())
                    );
                    yield return speculativeExecuteFuture;
                    if (speculativeExecuteFuture.Error != null)
                    {
                        self.OnError(speculativeExecuteFuture.Error);
                        yield break;
                    }
                    var commit = speculativeExecuteFuture.Result;
                    commit?.Invoke();
                }
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ReadMessageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> ReadAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> ReadAsync(
            #endif
            ReadMessageRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithMessageName(this.MessageName);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2Inbox.Domain.Transaction.SpeculativeExecutor.ReadMessageByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    ReadMessageByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ReadMessageAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> DeleteFuture(
            DeleteMessageRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithMessageName(this.MessageName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteMessageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> DeleteAsync(
            #endif
            DeleteMessageRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithMessageName(this.MessageName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteMessageAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Model.Message> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Model.Message> self)
            {
                var (value, find) = (null as Gs2.Gs2Inbox.Model.Message).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.MessageName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Inbox.Model.Message).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.MessageName,
                    () => this.GetFuture(
                        new GetMessageRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Model.Message>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inbox.Model.Message> ModelAsync()
            #else
        public async Task<Gs2.Gs2Inbox.Model.Message> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Inbox.Model.Message).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.MessageName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Inbox.Model.Message).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.MessageName,
                () => this.GetAsync(
                    new GetMessageRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inbox.Model.Message> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inbox.Model.Message> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inbox.Model.Message> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inbox.Model.Message).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.MessageName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inbox.Model.Message> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Inbox.Model.Message).CacheKey(
                    this.MessageName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Inbox.Model.Message).CacheKey(
                    this.MessageName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inbox.Model.Message> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inbox.Model.Message> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inbox.Model.Message> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}
