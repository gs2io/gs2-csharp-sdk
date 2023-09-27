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

    public partial class QuestModelDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2QuestRestClient _client;
        private readonly string _namespaceName;
        private readonly string _questGroupName;
        private readonly string _questName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string QuestGroupName => _questGroupName;
        public string QuestName => _questName;

        public QuestModelDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string questGroupName,
            string questName
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
            this._questName = questName;
            this._parentKey = Gs2.Gs2Quest.Domain.Model.QuestGroupModelDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.QuestGroupName,
                "QuestModel"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string questGroupName,
            string questName,
            string childType
        )
        {
            return string.Join(
                ":",
                "quest",
                namespaceName ?? "null",
                questGroupName ?? "null",
                questName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string questName
        )
        {
            return string.Join(
                ":",
                questName ?? "null"
            );
        }

    }

    public partial class QuestModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Quest.Model.QuestModel> GetFuture(
            GetQuestModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestModel> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithQuestGroupName(this.QuestGroupName)
                    .WithQuestName(this.QuestName);
                var future = this._client.GetQuestModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                            request.QuestName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Quest.Model.QuestModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "questModel")
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
                    .WithQuestGroupName(this.QuestGroupName)
                    .WithQuestName(this.QuestName);
                GetQuestModelResult result = null;
                try {
                    result = await this._client.GetQuestModelAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                        request.QuestName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "questModel")
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
                        var parentKey = Gs2.Gs2Quest.Domain.Model.QuestGroupModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.QuestGroupName,
                            "QuestModel"
                        );
                        var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestModel>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Quest.Model.QuestModel> GetAsync(
            GetQuestModelRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName)
                .WithQuestName(this.QuestName);
            var future = this._client.GetQuestModelFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                        request.QuestName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "questModel")
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
                .WithQuestGroupName(this.QuestGroupName)
                .WithQuestName(this.QuestName);
            GetQuestModelResult result = null;
            try {
                result = await this._client.GetQuestModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                    request.QuestName.ToString()
                    );
                _cache.Put<Gs2.Gs2Quest.Model.QuestModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "questModel")
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
                    var parentKey = Gs2.Gs2Quest.Domain.Model.QuestGroupModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.QuestGroupName,
                        "QuestModel"
                    );
                    var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
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

    }

    public partial class QuestModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Quest.Model.QuestModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestModel> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Quest.Model.QuestModel>(
                    _parentKey,
                    Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                        this.QuestName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetQuestModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                                    this.QuestName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Quest.Model.QuestModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "questModel")
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
                    (value, _) = _cache.Get<Gs2.Gs2Quest.Model.QuestModel>(
                        _parentKey,
                        Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                            this.QuestName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestModel>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Quest.Model.QuestModel> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Quest.Model.QuestModel>(
                    _parentKey,
                    Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                        this.QuestName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetQuestModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                                    this.QuestName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Quest.Model.QuestModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "questModel")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Quest.Model.QuestModel>(
                        _parentKey,
                        Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                            this.QuestName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Model.QuestModel> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Quest.Model.QuestModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Quest.Model.QuestModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Quest.Model.QuestModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Quest.Model.QuestModel> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                    this.QuestName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Quest.Model.QuestModel>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                    this.QuestName.ToString()
                ),
                callbackId
            );
        }

    }
}
