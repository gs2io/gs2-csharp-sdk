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
using Gs2.Gs2Friend.Domain.Iterator;
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
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

namespace Gs2.Gs2Friend.Domain.Model
{

    public partial class ReceiveFriendRequestDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _targetUserId;
        private readonly string _fromUserId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string FromUserId => _fromUserId;

        public ReceiveFriendRequestDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string fromUserId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FriendRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._fromUserId = fromUserId;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "ReceiveFriendRequest"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string fromUserId,
            string childType
        )
        {
            return string.Join(
                ":",
                "friend",
                namespaceName ?? "null",
                userId ?? "null",
                fromUserId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string fromUserId
        )
        {
            return string.Join(
                ":",
                fromUserId ?? "null"
            );
        }

    }

    public partial class ReceiveFriendRequestDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Friend.Model.FriendRequest> GetFuture(
            GetReceiveRequestByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FriendRequest> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithFromUserId(this.FromUserId);
                var future = this._client.GetReceiveRequestByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                            request.FromUserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "receiveFriendRequest")
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
                    .WithUserId(this.UserId)
                    .WithFromUserId(this.FromUserId);
                GetReceiveRequestByUserIdResult result = null;
                try {
                    result = await this._client.GetReceiveRequestByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        request.FromUserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "receiveFriendRequest")
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
                        var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "FriendRequest"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                            resultModel.Item.TargetUserId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FriendRequest>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Friend.Model.FriendRequest> GetAsync(
            GetReceiveRequestByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithFromUserId(this.FromUserId);
            var future = this._client.GetReceiveRequestByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        request.FromUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "receiveFriendRequest")
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
                .WithUserId(this.UserId)
                .WithFromUserId(this.FromUserId);
            GetReceiveRequestByUserIdResult result = null;
            try {
                result = await this._client.GetReceiveRequestByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    request.FromUserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "receiveFriendRequest")
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
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "FriendRequest"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> AcceptFuture(
            AcceptRequestByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithFromUserId(this.FromUserId);
                var future = this._client.AcceptRequestByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                            request.FromUserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "receiveFriendRequest")
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
                    .WithUserId(this.UserId)
                    .WithFromUserId(this.FromUserId);
                AcceptRequestByUserIdResult result = null;
                try {
                    result = await this._client.AcceptRequestByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        request.FromUserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "receiveFriendRequest")
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
                        var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "FriendRequest"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                            resultModel.Item.TargetUserId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                    }
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.FromUserId?.ToString(),
                            "SendFriendRequest"
                        )
                    );
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "ReceiveFriendRequest"
                        )
                    );
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "True",
                            "FriendUser"
                        )
                    );
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "False",
                            "FriendUser"
                        )
                    );
                }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> AcceptAsync(
            AcceptRequestByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithFromUserId(this.FromUserId);
            var future = this._client.AcceptRequestByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        request.FromUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "receiveFriendRequest")
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
                .WithUserId(this.UserId)
                .WithFromUserId(this.FromUserId);
            AcceptRequestByUserIdResult result = null;
            try {
                result = await this._client.AcceptRequestByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    request.FromUserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "receiveFriendRequest")
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
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "FriendRequest"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                }
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.FromUserId?.ToString(),
                        "SendFriendRequest"
                    )
                );
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "ReceiveFriendRequest"
                    )
                );
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "True",
                        "FriendUser"
                    )
                );
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "False",
                        "FriendUser"
                    )
                );
            }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> AcceptAsync(
            AcceptRequestByUserIdRequest request
        ) {
            var future = AcceptFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to AcceptFuture.")]
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> Accept(
            AcceptRequestByUserIdRequest request
        ) {
            return AcceptFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> RejectFuture(
            RejectRequestByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithFromUserId(this.FromUserId);
                var future = this._client.RejectRequestByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                            request.FromUserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "receiveFriendRequest")
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
                    .WithUserId(this.UserId)
                    .WithFromUserId(this.FromUserId);
                RejectRequestByUserIdResult result = null;
                try {
                    result = await this._client.RejectRequestByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        request.FromUserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "receiveFriendRequest")
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
                        var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "FriendRequest"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                            resultModel.Item.TargetUserId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                    }
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.FromUserId?.ToString(),
                            "SendFriendRequest"
                        )
                    );
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "ReceiveFriendRequest"
                        )
                    );
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "True",
                            "FriendUser"
                        )
                    );
                    cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "False",
                            "FriendUser"
                        )
                    );
                }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> RejectAsync(
            RejectRequestByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithFromUserId(this.FromUserId);
            var future = this._client.RejectRequestByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        request.FromUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "receiveFriendRequest")
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
                .WithUserId(this.UserId)
                .WithFromUserId(this.FromUserId);
            RejectRequestByUserIdResult result = null;
            try {
                result = await this._client.RejectRequestByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    request.FromUserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "receiveFriendRequest")
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
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "FriendRequest"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                }
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.FromUserId?.ToString(),
                        "SendFriendRequest"
                    )
                );
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "ReceiveFriendRequest"
                    )
                );
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "True",
                        "FriendUser"
                    )
                );
                cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "False",
                        "FriendUser"
                    )
                );
            }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> RejectAsync(
            RejectRequestByUserIdRequest request
        ) {
            var future = RejectFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to RejectFuture.")]
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> Reject(
            RejectRequestByUserIdRequest request
        ) {
            return RejectFuture(request);
        }
        #endif

    }

    public partial class ReceiveFriendRequestDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Model.FriendRequest> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FriendRequest> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
                    _parentKey,
                    Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        this.FromUserId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetReceiveRequestByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                                    this.FromUserId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Friend.Model.FriendRequest>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "friendRequest")
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
                    (value, _) = _cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
                        _parentKey,
                        Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                            this.FromUserId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FriendRequest>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Friend.Model.FriendRequest> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
                    _parentKey,
                    Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                        this.FromUserId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetReceiveRequestByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                                    this.FromUserId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Friend.Model.FriendRequest>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "friendRequest")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
                        _parentKey,
                        Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                            this.FromUserId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Model.FriendRequest> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Friend.Model.FriendRequest> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Friend.Model.FriendRequest> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Friend.Model.FriendRequest> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
