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

    public partial class FriendUserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly bool? _withProfile;
        private readonly string _targetUserId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public bool? WithProfile => _withProfile;
        public string TargetUserId => _targetUserId;

        public FriendUserDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            bool? withProfile,
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
            this._withProfile = withProfile;
            this._targetUserId = targetUserId;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.WithProfile?.ToString() ?? "False",
                "FriendUser"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string withProfile,
            string targetUserId,
            string childType
        )
        {
            return string.Join(
                ":",
                "friend",
                namespaceName ?? "null",
                userId ?? "null",
                withProfile ?? "null",
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

    }

    public partial class FriendUserDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Friend.Model.FriendUser> GetFuture(
            GetFriendByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FriendUser> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithWithProfile(this.WithProfile)
                    .WithTargetUserId(this.TargetUserId);
                var future = this._client.GetFriendByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            request.TargetUserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "friendUser")
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
                    .WithWithProfile(this.WithProfile)
                    .WithTargetUserId(this.TargetUserId);
                GetFriendByUserIdResult result = null;
                try {
                    result = await this._client.GetFriendByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        request.TargetUserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "friendUser")
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
                        var parentKey = Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.WithProfile?.ToString() ?? "False",
                            "FriendUser"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
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
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FriendUser>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Friend.Model.FriendUser> GetAsync(
            GetFriendByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithWithProfile(this.WithProfile)
                .WithTargetUserId(this.TargetUserId);
            var future = this._client.GetFriendByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        request.TargetUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "friendUser")
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
                .WithWithProfile(this.WithProfile)
                .WithTargetUserId(this.TargetUserId);
            GetFriendByUserIdResult result = null;
            try {
                result = await this._client.GetFriendByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                    request.TargetUserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "friendUser")
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
                    var parentKey = Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.WithProfile?.ToString() ?? "False",
                        "FriendUser"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
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
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendUserDomain> DeleteFuture(
            DeleteFriendByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.FriendUserDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithTargetUserId(this.TargetUserId);
                var future = this._client.DeleteFriendByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            request.TargetUserId.ToString()
                        );
                        _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "friendUser")
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
                    .WithTargetUserId(this.TargetUserId);
                DeleteFriendByUserIdResult result = null;
                try {
                    result = await this._client.DeleteFriendByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        request.TargetUserId.ToString()
                        );
                    _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "friendUser")
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
                        var parentKey = Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.WithProfile?.ToString() ?? "False",
                            "FriendUser"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(parentKey, key);
                    }
                    {
                        var parentKey = Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.WithProfile?.ToString() ?? "False",
                            "FriendUser"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(parentKey, key);
                    }
                    cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            resultModel?.Item?.UserId?.ToString(),
                            "False",
                            "FriendUser"
                        ),
                        Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            this.UserId?.ToString()
                        )
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(
                        Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            resultModel?.Item?.UserId?.ToString(),
                            "True",
                            "FriendUser"
                        ),
                        Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            this.UserId?.ToString()
                        )
                    );
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.FriendUserDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Friend.Domain.Model.FriendUserDomain> DeleteAsync(
            DeleteFriendByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithTargetUserId(this.TargetUserId);
            var future = this._client.DeleteFriendByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        request.TargetUserId.ToString()
                    );
                    _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "friendUser")
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
                .WithTargetUserId(this.TargetUserId);
            DeleteFriendByUserIdResult result = null;
            try {
                result = await this._client.DeleteFriendByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                    request.TargetUserId.ToString()
                    );
                _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "friendUser")
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
                    var parentKey = Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.WithProfile?.ToString() ?? "False",
                        "FriendUser"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(parentKey, key);
                }
                {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.WithProfile?.ToString() ?? "False",
                        "FriendUser"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(parentKey, key);
                }
                cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        resultModel?.Item?.UserId?.ToString(),
                        "False",
                        "FriendUser"
                    ),
                    Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        this.UserId?.ToString()
                    )
                );
                cache.Delete<Gs2.Gs2Friend.Model.FriendUser>(
                    Gs2.Gs2Friend.Domain.Model.FriendDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        resultModel?.Item?.UserId?.ToString(),
                        "True",
                        "FriendUser"
                    ),
                    Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        this.UserId?.ToString()
                    )
                );
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Domain.Model.FriendUserDomain> DeleteAsync(
            DeleteFriendByUserIdRequest request
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
        public IFuture<Gs2.Gs2Friend.Domain.Model.FriendUserDomain> Delete(
            DeleteFriendByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class FriendUserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Model.FriendUser> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.FriendUser> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Friend.Model.FriendUser>(
                    _parentKey,
                    Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        this.TargetUserId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetFriendByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                                    this.TargetUserId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "friendUser")
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
                    (value, _) = _cache.Get<Gs2.Gs2Friend.Model.FriendUser>(
                        _parentKey,
                        Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            this.TargetUserId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.FriendUser>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Friend.Model.FriendUser> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Friend.Model.FriendUser>(
                    _parentKey,
                    Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                        this.TargetUserId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetFriendByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                                    this.TargetUserId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Friend.Model.FriendUser>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "friendUser")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Friend.Model.FriendUser>(
                        _parentKey,
                        Gs2.Gs2Friend.Domain.Model.FriendUserDomain.CreateCacheKey(
                            this.TargetUserId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Friend.Model.FriendUser> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Friend.Model.FriendUser> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Friend.Model.FriendUser> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Friend.Model.FriendUser> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
