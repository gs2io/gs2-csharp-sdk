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
using Cysharp.Threading.Tasks.Linq;
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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
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
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string fromUserId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
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
                        this._gs2.Cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "receiveFriendRequest")
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Friend.Model.FriendRequest> GetAsync(
            #else
        private async Task<Gs2.Gs2Friend.Model.FriendRequest> GetAsync(
            #endif
            GetReceiveRequestByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "receiveFriendRequest")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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
                        this._gs2.Cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "receiveFriendRequest")
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                    }
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.FromUserId?.ToString(),
                            "SendFriendRequest"
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "ReceiveFriendRequest"
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "True",
                            "FriendUser"
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "False",
                            "FriendUser"
                        )
                    );
                }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> AcceptAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> AcceptAsync(
            #endif
            AcceptRequestByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "receiveFriendRequest")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                }
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.FromUserId?.ToString(),
                        "SendFriendRequest"
                    )
                );
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "ReceiveFriendRequest"
                    )
                );
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "True",
                        "FriendUser"
                    )
                );
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "False",
                        "FriendUser"
                    )
                );
            }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                        this._gs2.Cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "receiveFriendRequest")
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                    }
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.FromUserId?.ToString(),
                            "SendFriendRequest"
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "ReceiveFriendRequest"
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "True",
                            "FriendUser"
                        )
                    );
                    _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "False",
                            "FriendUser"
                        )
                    );
                }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> RejectAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> RejectAsync(
            #endif
            RejectRequestByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "receiveFriendRequest")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                }
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.FromUserId?.ToString(),
                        "SendFriendRequest"
                    )
                );
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "ReceiveFriendRequest"
                    )
                );
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "True",
                        "FriendUser"
                    )
                );
                _gs2.Cache.ClearListCache<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "False",
                        "FriendUser"
                    )
                );
            }
                var domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    this._fromUserId,
                    "ReceiveFriendRequest"
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
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
                            this._gs2.Cache.Put<Gs2.Gs2Friend.Model.FriendRequest>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "friendRequest")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Model.FriendRequest> ModelAsync()
            #else
        public async Task<Gs2.Gs2Friend.Model.FriendRequest> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.FriendRequest>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    this.FromUserId?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
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
                        this._gs2.Cache.Put<Gs2.Gs2Friend.Model.FriendRequest>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "friendRequest")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
                        _parentKey,
                        Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                            this.FromUserId?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    this.FromUserId.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.FriendRequest> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    this.FromUserId.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.FriendRequest>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    this.FromUserId.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Friend.Model.FriendRequest> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.FriendRequest> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Friend.Model.FriendRequest> callback)
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
