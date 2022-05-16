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
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._userId != null ? this._userId.ToString() : null,
                "ReceiveFriendRequest"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Friend.Model.FriendRequest> GetAsync(
            #else
        private IFuture<Gs2.Gs2Friend.Model.FriendRequest> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Friend.Model.FriendRequest> GetAsync(
        #endif
            GetReceiveRequestByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FriendRequest> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithFromUserId(this._fromUserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetReceiveRequestByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "ReceiveFriendRequest"
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
            #else
            var result = await this._client.GetReceiveRequestByUserIdAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "ReceiveFriendRequest"
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
            #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FriendRequest>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> AcceptAsync(
            #else
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> Accept(
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> AcceptAsync(
        #endif
            AcceptRequestByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithFromUserId(this._fromUserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AcceptRequestByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "ReceiveFriendRequest"
                );
                var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                    resultModel.Item.TargetUserId.ToString()
                );
                cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
            }
                cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this._fromUserId?.ToString(),
                        "SendFriendRequest"
                    )
                );
                cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "ReceiveFriendRequest"
                    )
                );
                cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "FriendUser"
                    )
                );
            #else
            AcceptRequestByUserIdResult result = null;
            try {
                result = await this._client.AcceptRequestByUserIdAsync(
                    request
                );
                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
              
                {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        _namespaceName.ToString(),
                        resultModel.Item.UserId.ToString(),
                        "ReceiveFriendRequest"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                }
                    cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this._fromUserId?.ToString(),
                            "SendFriendRequest"
                        )
                    );
                    cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "ReceiveFriendRequest"
                        )
                    );
                    cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "FriendUser"
                        )
                    );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            Gs2.Gs2Friend.Domain.Model.FriendRequestDomain domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.UserId,
                this._fromUserId
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> RejectAsync(
            #else
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> Reject(
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> RejectAsync(
        #endif
            RejectRequestByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithFromUserId(this._fromUserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.RejectRequestByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "ReceiveFriendRequest"
                );
                var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                    resultModel.Item.TargetUserId.ToString()
                );
                cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
            }
                cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this._fromUserId?.ToString(),
                        "SendFriendRequest"
                    )
                );
                cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "ReceiveFriendRequest"
                    )
                );
                cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "FriendUser"
                    )
                );
            #else
            RejectRequestByUserIdResult result = null;
            try {
                result = await this._client.RejectRequestByUserIdAsync(
                    request
                );
                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
              
                {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        _namespaceName.ToString(),
                        resultModel.Item.UserId.ToString(),
                        "ReceiveFriendRequest"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                }
                    cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this._fromUserId?.ToString(),
                            "SendFriendRequest"
                        )
                    );
                    cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendRequest>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "ReceiveFriendRequest"
                        )
                    );
                    cache.ListCacheClear<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId?.ToString(),
                            "FriendUser"
                        )
                    );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            Gs2.Gs2Friend.Domain.Model.FriendRequestDomain domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.UserId,
                this._fromUserId
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain>(Impl);
        #endif
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Model.FriendRequest> Model() {
            #else
        public IFuture<Gs2.Gs2Friend.Model.FriendRequest> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Model.FriendRequest> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FriendRequest> self)
            {
        #endif
            Gs2.Gs2Friend.Model.FriendRequest value = _cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    this.FromUserId?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetReceiveRequestByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "friendRequest")
                            {
                                _cache.Delete<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                                    _parentKey,
                                    Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                                        this.FromUserId?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "friendRequest")
                    {
                    _cache.Delete<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                            _parentKey,
                            Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                                this.FromUserId?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Friend.Model.FriendRequest>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.ReceiveFriendRequestDomain.CreateCacheKey(
                    this.FromUserId?.ToString()
                )
            );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FriendRequest>(Impl);
        #endif
        }

    }
}
