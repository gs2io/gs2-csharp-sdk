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

    public partial class ProgressDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2QuestRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _transactionId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public ProgressDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2QuestRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Quest.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._userId != null ? this._userId.ToString() : null,
                "Progress"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Quest.Model.Progress> GetAsync(
            #else
        private IFuture<Gs2.Gs2Quest.Model.Progress> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Quest.Model.Progress> GetAsync(
        #endif
            GetProgressByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.Progress> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetProgressByUserIdFuture(
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
                var parentKey = Gs2.Gs2Quest.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "Progress"
                );
                var key = Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "QuestGroupModel"
                );
                var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelDomain.CreateCacheKey(
                    resultModel.QuestGroup.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.QuestGroup,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Quest.Domain.Model.QuestGroupModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.QuestGroup.Name.ToString(),
                    "QuestModel"
                );
                var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                    resultModel.Quest.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Quest,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.GetProgressByUserIdAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Quest.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "Progress"
                );
                var key = Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Quest.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "QuestGroupModel"
                );
                var key = Gs2.Gs2Quest.Domain.Model.QuestGroupModelDomain.CreateCacheKey(
                    resultModel.QuestGroup.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.QuestGroup,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Quest.Domain.Model.QuestGroupModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.QuestGroup.Name.ToString(),
                    "QuestModel"
                );
                var key = Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                    resultModel.Quest.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Quest,
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
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.Progress>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Domain.Model.ProgressDomain> EndAsync(
            #else
        public IFuture<Gs2.Gs2Quest.Domain.Model.ProgressDomain> End(
            #endif
        #else
        public async Task<Gs2.Gs2Quest.Domain.Model.ProgressDomain> EndAsync(
        #endif
            EndByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.ProgressDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future2 = Model();
            yield return future2;
            if (future2.Error != null)
            {
                self.OnError(future2.Error);
                yield break;
            }
            var model = future2.Result;
            #else
            var model = await Model();
            #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithTransactionId(model.TransactionId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.EndByUserIdFuture(
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
                var parentKey = Gs2.Gs2Quest.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "Progress"
                );
                var key = Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                cache.Delete<Gs2.Gs2Quest.Model.CompletedQuestList>(
                    Gs2.Gs2Quest.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "CompletedQuestList"
                    ),
                    Gs2.Gs2Quest.Domain.Model.CompletedQuestListDomain.CreateCacheKey(
                        result.Item?.QuestModelId.Split(':')[7]
                    )
                );
            #else
            var result = await this._client.EndByUserIdAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Quest.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.UserId.ToString(),
                    "Progress"
                );
                var key = Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                cache.Delete<Gs2.Gs2Quest.Model.CompletedQuestList>(
                    Gs2.Gs2Quest.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId?.ToString(),
                        "CompletedQuestList"
                    ),
                    Gs2.Gs2Quest.Domain.Model.CompletedQuestListDomain.CreateCacheKey(
                        result.Item?.QuestModelId.Split(':')[7]
                    )
                );
            #endif
            if (result?.StampSheet != null)
            {
                Gs2.Core.Domain.StampSheetDomain stampSheet = new Gs2.Core.Domain.StampSheetDomain(
                        _cache,
                        _jobQueueDomain,
                        _session,
                        result?.StampSheet,
                        result?.StampSheetEncryptionKeyId,
                        _stampSheetConfiguration.NamespaceName,
                        _stampSheetConfiguration.StampTaskEventHandler,
                        _stampSheetConfiguration.StampSheetEventHandler
                );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                yield return stampSheet.Run();
        #else
                try {
                    await stampSheet.RunAsync();
                } catch (Gs2.Core.Exception.Gs2Exception e) {
                    throw new Gs2.Core.Exception.TransactionException(stampSheet, e);
                }
        #endif
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(this);
        #else
            return this;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.ProgressDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "quest",
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Model.Progress> Model() {
            #else
        public IFuture<Gs2.Gs2Quest.Model.Progress> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Quest.Model.Progress> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.Progress> self)
            {
        #endif
            Gs2.Gs2Quest.Model.Progress value = _cache.Get<Gs2.Gs2Quest.Model.Progress>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetProgressByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "progress")
                            {
                                _cache.Delete<Gs2.Gs2Quest.Model.Progress>(
                                    _parentKey,
                                    Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
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
                    if (e.errors[0].component == "progress")
                    {
                    _cache.Delete<Gs2.Gs2Quest.Model.Progress>(
                            _parentKey,
                            Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Quest.Model.Progress>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.ProgressDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.Progress>(Impl);
        #endif
        }

    }
}
