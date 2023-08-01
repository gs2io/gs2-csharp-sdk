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

    public partial class FollowUserAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _targetUserId;
        private readonly bool? _withProfile;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string TargetUserId => _targetUserId;
        public bool? WithProfile => _withProfile;

        public FollowUserAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string targetUserId,
            bool? withProfile
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FriendRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._targetUserId = targetUserId;
            this._withProfile = withProfile;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "FollowUser:" + (_withProfile == true)
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Friend.Model.FollowUser> GetAsync(
            #else
        private IFuture<Gs2.Gs2Friend.Model.FollowUser> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Friend.Model.FollowUser> GetAsync(
        #endif
            GetFollowRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FollowUser> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithTargetUserId(this.TargetUserId)
                .WithWithProfile(this.WithProfile);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetFollowFuture(
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
            var result = await this._client.GetFollowAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "FollowUser:" + (_withProfile == true)
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FollowUser>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> FollowAsync(
            #else
        public IFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> Follow(
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> FollowAsync(
        #endif
            FollowRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithTargetUserId(this.TargetUserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.FollowFuture(
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
            var result = await this._client.FollowAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "FollowUser:" + (_withProfile == true)
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> UnfollowAsync(
            #else
        public IFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> Unfollow(
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> UnfollowAsync(
        #endif
            UnfollowRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithTargetUserId(this.TargetUserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UnfollowFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                        request.TargetUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Friend.Model.FollowUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            UnfollowResult result = null;
            try {
                result = await this._client.UnfollowAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "followUser")
                {
                    var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                        request.TargetUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Friend.Model.FollowUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else
                {
                    throw e;
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
                        "FollowUser:" + (_withProfile == true)
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FollowUser>(parentKey, key);
                }
                {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "FollowUser:" + (_withProfile != true)
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FollowUser>(parentKey, key);
                }
            }
            Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FollowUserAccessTokenDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string targetUserId,
            string childType
        )
        {
            return string.Join(
                ":",
                "friend",
                namespaceName ?? "null",
                userId ?? "null",
                targetUserId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string targetUserId
        )
        {
            return string.Join(
                ":",
                targetUserId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Model.FollowUser> Model() {
            #else
        public IFuture<Gs2.Gs2Friend.Model.FollowUser> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Model.FollowUser> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FollowUser> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Friend.Model.FollowUser>(
                       _parentKey,
                       Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                            this.TargetUserId?.ToString()
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Friend.Model.FollowUser>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                    this.TargetUserId?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetFollowRequest()
                                    .WithWithProfile(this.WithProfile)
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                                    this.TargetUserId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Friend.Model.FollowUser>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "followUser")
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
                    var key = Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                            this.TargetUserId?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Friend.Model.FollowUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "followUser")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Friend.Model.FollowUser>(
                    _parentKey,
                    Gs2.Gs2Friend.Domain.Model.FollowUserDomain.CreateCacheKey(
                        this.TargetUserId?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FollowUser>(Impl);
        #endif
        }

    }
}
