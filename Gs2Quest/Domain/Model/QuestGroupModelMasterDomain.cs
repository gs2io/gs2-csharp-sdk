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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2QuestRestClient _client;
        private readonly string _namespaceName;
        private readonly string _questGroupName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string QuestGroupName => _questGroupName;

        public QuestGroupModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string questGroupName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2QuestRestClient(
                gs2.RestSession
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
                this._gs2.Cache,
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
        public DescribeQuestModelMastersIterator QuestModelMastersAsync(
        #endif
        )
        {
            return new DescribeQuestModelMastersIterator(
                this._gs2.Cache,
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

        public ulong SubscribeQuestModelMasters(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Quest.Model.QuestModelMaster>(
                Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.QuestGroupName,
                    "QuestModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribeQuestModelMasters(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Quest.Model.QuestModelMaster>(
                Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.QuestGroupName,
                    "QuestModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain QuestModelMaster(
            string questName
        ) {
            return new Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain(
                this._gs2,
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
                        this._gs2.Cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "questGroupModelMaster")
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
                        var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "QuestGroupModelMaster"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Quest.Model.QuestGroupModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Quest.Model.QuestGroupModelMaster> GetAsync(
            #endif
            GetQuestGroupModelMasterRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "questGroupModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "QuestGroupModelMaster"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
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
        public IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> UpdateFuture(
            UpdateQuestGroupModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> self)
            {
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "QuestGroupModelMaster"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> UpdateAsync(
            #endif
            UpdateQuestGroupModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName);
            UpdateQuestGroupModelMasterResult result = null;
                result = await this._client.UpdateQuestGroupModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "QuestGroupModelMaster"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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
                        this._gs2.Cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "questGroupModelMaster")
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
                        var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "QuestGroupModelMaster"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain> DeleteAsync(
            #endif
            DeleteQuestGroupModelMasterRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "questGroupModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "QuestGroupModelMaster"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.QuestGroupName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain> CreateQuestModelMasterAsync(
            #else
        public async Task<Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain> CreateQuestModelMasterAsync(
            #endif
            CreateQuestModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName);
            CreateQuestModelMasterResult result = null;
                result = await this._client.CreateQuestModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Quest.Domain.Model.QuestModelMasterDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.QuestGroupName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
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
                            this._gs2.Cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "questGroupModelMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Quest.Model.QuestGroupModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Quest.Model.QuestGroupModelMaster> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                    this.QuestGroupName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
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
                        this._gs2.Cache.Put<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "questGroupModelMaster")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                        _parentKey,
                        Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                            this.QuestGroupName?.ToString()
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


        public ulong Subscribe(Action<Gs2.Gs2Quest.Model.QuestGroupModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                    this.QuestGroupName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Quest.Model.QuestGroupModelMaster>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.QuestGroupModelMasterDomain.CreateCacheKey(
                    this.QuestGroupName.ToString()
                ),
                callbackId
            );
        }

    }
}
