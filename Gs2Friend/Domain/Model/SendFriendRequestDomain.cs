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
 *
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

    public partial class SendFriendRequestDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _targetUserId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string TargetUserId => _targetUserId;

        public SendFriendRequestDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string targetUserId
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
            this._targetUserId = targetUserId;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._userId != null ? this._userId.ToString() : null,
                "SendFriendRequest"
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
            GetSendRequestByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FriendRequest> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithTargetUserId(this._targetUserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetSendRequestByUserIdFuture(
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
                        "SendFriendRequest"
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
            var result = await this._client.GetSendRequestByUserIdAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                        "SendFriendRequest"
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
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> DeleteAsync(
        #endif
            DeleteRequestByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendRequestDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithTargetUserId(this._targetUserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteRequestByUserIdFuture(
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
                    requestModel.UserId.ToString(),
                        "SendFriendRequest"
                );
                var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                    resultModel.Item.TargetUserId.ToString()
                );
                cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
            }
            #else
            DeleteRequestByUserIdResult result = null;
            try {
                result = await this._client.DeleteRequestByUserIdAsync(
                    request
                );
                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
              
                {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        _namespaceName.ToString(),
                        requestModel.UserId.ToString(),
                            "SendFriendRequest"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendRequestDomain.CreateCacheKey(
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendRequest>(parentKey, key);
                }
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            Gs2.Gs2Friend.Domain.Model.FriendRequestDomain domain = new Gs2.Gs2Friend.Domain.Model.FriendRequestDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.TargetUserId,
                "SendFriendRequest"
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
                Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain.CreateCacheKey(
                    this.TargetUserId?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetSendRequestByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "friendRequest")
                            {
                                _cache.Delete<Gs2.Gs2Friend.Model.SendFriendRequest>(
                                    _parentKey,
                                    Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain.CreateCacheKey(
                                        this.TargetUserId?.ToString()
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
                        _cache.Delete<Gs2.Gs2Friend.Model.SendFriendRequest>(
                            _parentKey,
                            Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain.CreateCacheKey(
                                this.TargetUserId?.ToString()
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
                    Gs2.Gs2Friend.Domain.Model.SendFriendRequestDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FriendRequest>(Impl);
        #endif
        }

    }
}
