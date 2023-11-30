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
using Gs2.Gs2SerialKey.Request;
using Gs2.Gs2SerialKey.Result;
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

namespace Gs2.Gs2SerialKey.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SerialKeyRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string Url { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SerialKeyRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2SerialKey.Model.SerialKey> SerialKeys(
            string campaignModelName,
            string issueJobName
        )
        {
            return new DescribeSerialKeysIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                campaignModelName,
                issueJobName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2SerialKey.Model.SerialKey> SerialKeysAsync(
            #else
        public Gs2Iterator<Gs2.Gs2SerialKey.Model.SerialKey> SerialKeys(
            #endif
        #else
        public DescribeSerialKeysIterator SerialKeysAsync(
        #endif
            string campaignModelName,
            string issueJobName
        )
        {
            return new DescribeSerialKeysIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                campaignModelName,
                issueJobName
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeSerialKeys(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SerialKey.Model.SerialKey>(
                Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "SerialKey"
                ),
                callback
            );
        }

        public void UnsubscribeSerialKeys(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SerialKey.Model.SerialKey>(
                Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "SerialKey"
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

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "serialKey",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
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
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DownloadSerialCodesFuture(
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
                if (resultModel != null) {
                    
                }
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
            request
                .WithNamespaceName(this.NamespaceName);
            DownloadSerialCodesResult result = null;
                result = await this._client.DownloadSerialCodesAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Url = domain.Url = result?.Url;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DownloadSerialCodesFuture.")]
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.UserDomain> DownloadSerialCodes(
            DownloadSerialCodesRequest request
        ) {
            return DownloadSerialCodesFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Model.SerialKey> GetSerialKeyFuture(
            GetSerialKeyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Model.SerialKey> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetSerialKeyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
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
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SerialKey"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                            resultModel.Item.Code.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.CampaignModel != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CampaignModel"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            resultModel.CampaignModel.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.CampaignModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2SerialKey.Model.SerialKey>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SerialKey.Model.SerialKey> GetSerialKeyAsync(
            #else
        public async Task<Gs2.Gs2SerialKey.Model.SerialKey> GetSerialKeyAsync(
            #endif
            GetSerialKeyRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            GetSerialKeyResult result = null;
            try {
                result = await this._client.GetSerialKeyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SerialKey"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                        resultModel.Item.Code.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.CampaignModel != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CampaignModel"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        resultModel.CampaignModel.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.CampaignModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to GetSerialKeyFuture.")]
        public IFuture<Gs2.Gs2SerialKey.Model.SerialKey> GetSerialKey(
            GetSerialKeyRequest request
        ) {
            return GetSerialKeyFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> RevertUseFuture(
            RevertUseByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.RevertUseByUserIdFuture(
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
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "SerialKey"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                            resultModel.Item.Code.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.CampaignModel != null) {
                        var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CampaignModel"
                        );
                        var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                            resultModel.CampaignModel.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.CampaignModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                    this._gs2,
                    request.NamespaceName,
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            RevertUseByUserIdResult result = null;
                result = await this._client.RevertUseByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SerialKey"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain.CreateCacheKey(
                        resultModel.Item.Code.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.CampaignModel != null) {
                    var parentKey = Gs2.Gs2SerialKey.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CampaignModel"
                    );
                    var key = Gs2.Gs2SerialKey.Domain.Model.CampaignModelDomain.CreateCacheKey(
                        resultModel.CampaignModel.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.CampaignModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain(
                    this._gs2,
                    request.NamespaceName,
                    request.UserId,
                    result?.Item?.Code
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to RevertUseFuture.")]
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyDomain> RevertUse(
            RevertUseByUserIdRequest request
        ) {
            return RevertUseFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}
