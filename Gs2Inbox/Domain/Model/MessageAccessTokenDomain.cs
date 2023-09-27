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

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Inbox.Domain.Iterator;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
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
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InboxRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _messageName;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string MessageName => _messageName;

        public MessageAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string messageName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2InboxRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._messageName = messageName;
            this._parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Message"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inbox.Model.Message> GetFuture(
            GetMessageRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Model.Message> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                var future = this._client.GetMessageFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            request.MessageName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "message")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                GetMessageResult result = null;
                try {
                    result = await this._client.GetMessageAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        request.MessageName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "message")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Message"
                        );
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Model.Message>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Inbox.Model.Message> GetAsync(
            GetMessageRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            var future = this._client.GetMessageFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        request.MessageName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "message")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            GetMessageResult result = null;
            try {
                result = await this._client.GetMessageAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                    request.MessageName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "message")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Message"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> OpenFuture(
            OpenMessageRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                var future = this._client.OpenMessageFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            request.MessageName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "message")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                OpenMessageResult result = null;
                try {
                    result = await this._client.OpenMessageAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        request.MessageName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "message")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Message"
                        );
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inbox.Model.Message>(parentKey, key);
                        cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                            parentKey
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> OpenAsync(
            OpenMessageRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            var future = this._client.OpenMessageFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        request.MessageName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "message")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            OpenMessageResult result = null;
            try {
                result = await this._client.OpenMessageAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                    request.MessageName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "message")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Message"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inbox.Model.Message>(parentKey, key);
                    cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                        parentKey
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> OpenAsync(
            OpenMessageRequest request
        ) {
            var future = OpenFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to OpenFuture.")]
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> Open(
            OpenMessageRequest request
        ) {
            return OpenFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> ReadFuture(
            ReadMessageRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                var future = this._client.ReadMessageFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                ReadMessageResult result = null;
                    result = await this._client.ReadMessageAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Message"
                        );
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var stampSheet = new Gs2.Core.Domain.TransactionAccessTokenDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId

                );
                if (result?.StampSheet != null)
                {
                    var future2 = stampSheet.Wait();
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }

            self.OnComplete(stampSheet);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> ReadAsync(
            ReadMessageRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            var future = this._client.ReadMessageFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            ReadMessageResult result = null;
                result = await this._client.ReadMessageAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Message"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            var stampSheet = new Gs2.Core.Domain.TransactionAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId

            );
            if (result?.StampSheet != null)
            {
                await stampSheet.WaitAsync();
            }

            return stampSheet;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> ReadAsync(
            ReadMessageRequest request
        ) {
            var future = ReadFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ReadFuture.")]
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> Read(
            ReadMessageRequest request
        ) {
            return ReadFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> DeleteFuture(
            DeleteMessageRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                var future = this._client.DeleteMessageFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            request.MessageName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "message")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithMessageName(this.MessageName);
                DeleteMessageResult result = null;
                try {
                    result = await this._client.DeleteMessageAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        request.MessageName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "message")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Message"
                        );
                        var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inbox.Model.Message>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> DeleteAsync(
            DeleteMessageRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            var future = this._client.DeleteMessageFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        request.MessageName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "message")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithMessageName(this.MessageName);
            DeleteMessageResult result = null;
            try {
                result = await this._client.DeleteMessageAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                    request.MessageName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "message")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Message"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inbox.Model.Message>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> DeleteAsync(
            DeleteMessageRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain> Delete(
            DeleteMessageRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string messageName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inbox",
                namespaceName ?? "null",
                userId ?? "null",
                messageName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string messageName
        )
        {
            return string.Join(
                ":",
                messageName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Model.Message> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Model.Message> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Inbox.Model.Message>(
                    _parentKey,
                    Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        this.MessageName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMessageRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                                    this.MessageName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "message")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _cache.Get<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            this.MessageName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Model.Message>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inbox.Model.Message> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Inbox.Model.Message>(
                    _parentKey,
                    Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                        this.MessageName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetMessageRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                                    this.MessageName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "message")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Inbox.Model.Message>(
                        _parentKey,
                        Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                            this.MessageName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Model.Message> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

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


        public ulong Subscribe(Action<Gs2.Gs2Inbox.Model.Message> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                    this.MessageName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Inbox.Model.Message>(
                _parentKey,
                Gs2.Gs2Inbox.Domain.Model.MessageDomain.CreateCacheKey(
                    this.MessageName.ToString()
                ),
                callbackId
            );
        }

    }
}
