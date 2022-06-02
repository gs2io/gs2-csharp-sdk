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

    public partial class GlobalMessageMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InboxRestClient _client;
        private readonly string _namespaceName;
        private readonly string _globalMessageName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string GlobalMessageName => _globalMessageName;

        public GlobalMessageMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string globalMessageName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2InboxRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._globalMessageName = globalMessageName;
            this._parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "GlobalMessageMaster"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inbox.Model.GlobalMessageMaster> GetAsync(
            #else
        private IFuture<Gs2.Gs2Inbox.Model.GlobalMessageMaster> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Inbox.Model.GlobalMessageMaster> GetAsync(
        #endif
            GetGlobalMessageMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Model.GlobalMessageMaster> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithGlobalMessageName(this._globalMessageName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetGlobalMessageMasterFuture(
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
                var parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                        "GlobalMessageMaster"
                );
                var key = Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.GetGlobalMessageMasterAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                        "GlobalMessageMaster"
                );
                var key = Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> UpdateAsync(
        #endif
            UpdateGlobalMessageMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithGlobalMessageName(this._globalMessageName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateGlobalMessageMasterFuture(
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
                var parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                        "GlobalMessageMaster"
                );
                var key = Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.UpdateGlobalMessageMasterAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                        "GlobalMessageMaster"
                );
                var key = Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #endif
            Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> DeleteAsync(
        #endif
            DeleteGlobalMessageMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithGlobalMessageName(this._globalMessageName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteGlobalMessageMasterFuture(
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
                var parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                        "GlobalMessageMaster"
                );
                var key = Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Delete<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(parentKey, key);
            }
            #else
            DeleteGlobalMessageMasterResult result = null;
            try {
                result = await this._client.DeleteGlobalMessageMasterAsync(
                    request
                );
                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
              
                {
                    var parentKey = Gs2.Gs2Inbox.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        _namespaceName.ToString(),
                            "GlobalMessageMaster"
                    );
                    var key = Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(parentKey, key);
                }
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string globalMessageName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inbox",
                namespaceName ?? "null",
                globalMessageName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string globalMessageName
        )
        {
            return string.Join(
                ":",
                globalMessageName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inbox.Model.GlobalMessageMaster> Model() {
            #else
        public IFuture<Gs2.Gs2Inbox.Model.GlobalMessageMaster> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Inbox.Model.GlobalMessageMaster> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Model.GlobalMessageMaster> self)
            {
        #endif
            Gs2.Gs2Inbox.Model.GlobalMessageMaster value = _cache.Get<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(
                _parentKey,
                Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                    this.GlobalMessageName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetGlobalMessageMasterRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "globalMessageMaster")
                            {
                                _cache.Delete<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(
                                    _parentKey,
                                    Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                                        this.GlobalMessageName?.ToString()
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
                    if (e.errors[0].component == "globalMessageMaster")
                    {
                        _cache.Delete<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(
                            _parentKey,
                            Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                                this.GlobalMessageName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(
                    _parentKey,
                    Gs2.Gs2Inbox.Domain.Model.GlobalMessageMasterDomain.CreateCacheKey(
                        this.GlobalMessageName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Model.GlobalMessageMaster>(Impl);
        #endif
        }

    }
}
