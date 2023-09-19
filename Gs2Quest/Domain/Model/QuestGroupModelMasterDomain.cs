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
using Gs2.Gs2Quest.Domain.Iterator;
using Gs2.Gs2Quest.Request;
using Gs2.Gs2Quest.Result;
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

namespace Gs2.Gs2Quest.Domain.Model
{

    public partial class QuestGroupModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2QuestRestClient _client;
        private readonly string _namespaceName;
        private readonly string _questGroupName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string QuestGroupName => _questGroupName;

        public QuestGroupModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string questGroupName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2QuestRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._questGroupName = questGroupName;
            this._parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "QuestGroupModelMaster"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Quest.Model.QuestModelMaster> QuestModelMasters(
        )
        {
            return new DescribeQuestModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.QuestGroupName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Quest.Model.QuestModelMaster> QuestModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Quest.Model.QuestModelMaster> QuestModelMasters(
            #endif
        #else
        public DescribeQuestModelMastersIterator QuestModelMasters(
        #endif
        )
        {
            return new DescribeQuestModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.QuestGroupName
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

        public Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain QuestModelMaster(
            string questName
        ) {
            return new Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.QuestGroupName,
                questName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string questGroupName,
            string childType
        )
        {
            return string.Join(
                ":",
                "quest",
                namespaceName ?? "null",
                questGroupName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string questGroupName
        )
        {
            return string.Join(
                ":",
                questGroupName ?? "null"
            );
        }

    }

    public partial class QuestGroupModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster> GetFuture(
            GetQuestGroupModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithQuestGroupName(this.QuestGroupName);
                var future = this._client.GetQuestGroupModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            request.QuestGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "questGroupModelMaster")
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
                    .WithQuestGroupName(this.QuestGroupName);
                GetQuestGroupModelMasterResult result = null;
                try {
                    result = await this._client.GetQuestGroupModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        request.QuestGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "questGroupModelMaster")
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
                        var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "QuestGroupModelMaster"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Quest.Model.QuestGroupModelMaster> GetAsync(
            GetQuestGroupModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName);
            var future = this._client.GetQuestGroupModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        request.QuestGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "questGroupModelMaster")
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
                .WithQuestGroupName(this.QuestGroupName);
            GetQuestGroupModelMasterResult result = null;
            try {
                result = await this._client.GetQuestGroupModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                    request.QuestGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "questGroupModelMaster")
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
                    var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "QuestGroupModelMaster"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
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
        public IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> UpdateFuture(
            UpdateQuestGroupModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithQuestGroupName(this.QuestGroupName);
                var future = this._client.UpdateQuestGroupModelMasterFuture(
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
                    .WithQuestGroupName(this.QuestGroupName);
                UpdateQuestGroupModelMasterResult result = null;
                    result = await this._client.UpdateQuestGroupModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "QuestGroupModelMaster"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> UpdateAsync(
            UpdateQuestGroupModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName);
            var future = this._client.UpdateQuestGroupModelMasterFuture(
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
                .WithQuestGroupName(this.QuestGroupName);
            UpdateQuestGroupModelMasterResult result = null;
                result = await this._client.UpdateQuestGroupModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "QuestGroupModelMaster"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> UpdateAsync(
            UpdateQuestGroupModelMasterRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> Update(
            UpdateQuestGroupModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> DeleteFuture(
            DeleteQuestGroupModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithQuestGroupName(this.QuestGroupName);
                var future = this._client.DeleteQuestGroupModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            request.QuestGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "questGroupModelMaster")
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
                    .WithQuestGroupName(this.QuestGroupName);
                DeleteQuestGroupModelMasterResult result = null;
                try {
                    result = await this._client.DeleteQuestGroupModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        request.QuestGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "questGroupModelMaster")
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
                        var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "QuestGroupModelMaster"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> DeleteAsync(
            DeleteQuestGroupModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName);
            var future = this._client.DeleteQuestGroupModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        request.QuestGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "questGroupModelMaster")
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
                .WithQuestGroupName(this.QuestGroupName);
            DeleteQuestGroupModelMasterResult result = null;
            try {
                result = await this._client.DeleteQuestGroupModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                    request.QuestGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "questGroupModelMaster")
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
                    var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "QuestGroupModelMaster"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> DeleteAsync(
            DeleteQuestGroupModelMasterRequest request
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
        public IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> Delete(
            DeleteQuestGroupModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain> CreateQuestModelMasterFuture(
            CreateQuestModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithQuestGroupName(this.QuestGroupName);
                var future = this._client.CreateQuestModelMasterFuture(
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
                    .WithQuestGroupName(this.QuestGroupName);
                CreateQuestModelMasterResult result = null;
                    result = await this._client.CreateQuestModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.QuestGroupName,
                            "QuestModelMaster"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.QuestGroupName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain> CreateQuestModelMasterAsync(
            CreateQuestModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName);
            var future = this._client.CreateQuestModelMasterFuture(
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
                .WithQuestGroupName(this.QuestGroupName);
            CreateQuestModelMasterResult result = null;
                result = await this._client.CreateQuestModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.QuestGroupName,
                        "QuestModelMaster"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.QuestGroupName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain> CreateQuestModelMasterAsync(
            CreateQuestModelMasterRequest request
        ) {
            var future = CreateQuestModelMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateQuestModelMasterFuture.")]
        public IFuture<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain> CreateQuestModelMaster(
            CreateQuestModelMasterRequest request
        ) {
            return CreateQuestModelMasterFuture(request);
        }
        #endif

    }

    public partial class QuestGroupModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                    _parentKey,
                    Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        this.QuestGroupName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetQuestGroupModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                                    this.QuestGroupName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "questGroupModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            this.QuestGroupName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Quest.Model.QuestGroupModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                    _parentKey,
                    Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        this.QuestGroupName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetQuestGroupModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                                    this.QuestGroupName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "questGroupModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            this.QuestGroupName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Model.QuestGroupModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Quest.Model.QuestGroupModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Quest.Model.QuestGroupModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
