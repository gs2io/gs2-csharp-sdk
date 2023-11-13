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
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Friend.Domain.Model
{

    public partial class BlackListDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public BlackListDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FriendRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "BlackList"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "friend",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class BlackListDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain> RegisterFuture(
            RegisterBlackListByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.RegisterBlackListByUserIdFuture(
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BlackList"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    cache.ClearListCache<string>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            resultModel.Item.UserId.ToString(),
                            "BlackList"
                        )
                    );
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.BlackListDomain> RegisterAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.BlackListDomain> RegisterAsync(
            #endif
            RegisterBlackListByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            RegisterBlackListByUserIdResult result = null;
                result = await this._client.RegisterBlackListByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BlackList"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                cache.ClearListCache<string>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        resultModel.Item.UserId.ToString(),
                        "BlackList"
                    )
                );
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to RegisterFuture.")]
        public IFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain> Register(
            RegisterBlackListByUserIdRequest request
        ) {
            return RegisterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain> UnregisterFuture(
            UnregisterBlackListByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.UnregisterBlackListByUserIdFuture(
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BlackList"
                        );
                        var key = Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    cache.ClearListCache<string>(
                        Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            resultModel.Item.UserId.ToString(),
                            "BlackList"
                        )
                    );
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Domain.Model.BlackListDomain> UnregisterAsync(
            #else
        public async Task<Gs2.Gs2Friend.Domain.Model.BlackListDomain> UnregisterAsync(
            #endif
            UnregisterBlackListByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            UnregisterBlackListByUserIdResult result = null;
                result = await this._client.UnregisterBlackListByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BlackList"
                    );
                    var key = Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                cache.ClearListCache<string>(
                    Gs2.Gs2Friend.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        resultModel.Item.UserId.ToString(),
                        "BlackList"
                    )
                );
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to UnregisterFuture.")]
        public IFuture<Gs2.Gs2Friend.Domain.Model.BlackListDomain> Unregister(
            UnregisterBlackListByUserIdRequest request
        ) {
            return UnregisterFuture(request);
        }
        #endif

    }

    public partial class BlackListDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Friend.Model.BlackList> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Friend.Model.BlackList> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Friend.Model.BlackList>(
                    _parentKey,
                    Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Friend.Model.BlackList>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Friend.Model.BlackList> ModelAsync()
            #else
        public async Task<Gs2.Gs2Friend.Model.BlackList> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.BlackList>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Friend.Model.BlackList>(
                    _parentKey,
                    Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                    )
                );
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Friend.Model.BlackList> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Friend.Model.BlackList> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Friend.Model.BlackList> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Friend.Model.BlackList> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Friend.Model.BlackList>(
                _parentKey,
                Gs2.Gs2Friend.Domain.Model.BlackListDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

    }
}
