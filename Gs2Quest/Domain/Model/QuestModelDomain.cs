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
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._questGroupName != null ? this._questGroupName.ToString() : null,
                "QuestModel"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Quest.Model.QuestModel> GetAsync(
            #else
        private IFuture<Gs2.Gs2Quest.Model.QuestModel> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Quest.Model.QuestModel> GetAsync(
        #endif
            GetQuestModelRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestModel> self)
          {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithQuestGroupName(this._questGroupName)
                .WithQuestName(this._questName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetQuestModelFuture(
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
            var result = await this._client.GetQuestModelAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                        request.QuestName != null ? request.QuestName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestModel>(Impl);
        #endif
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Quest.Model.QuestModel> Model() {
            #else
        public IFuture<Gs2.Gs2Quest.Model.QuestModel> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Quest.Model.QuestModel> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestModel> self)
            {
        #endif
            Gs2.Gs2Quest.Model.QuestModel value = _cache.Get<Gs2.Gs2Quest.Model.QuestModel>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                    this.QuestName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetQuestModelRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Quest.Model.QuestModel>(
                            _parentKey,
                            Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                                this.QuestName?.ToString()
                            )
                        );
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException) {
                    _cache.Delete<Gs2.Gs2Quest.Model.QuestModel>(
                        _parentKey,
                        Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                            this.QuestName?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Quest.Model.QuestModel>(
                _parentKey,
                Gs2.Gs2Quest.Domain.Model.QuestModelDomain.CreateCacheKey(
                    this.QuestName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestModel>(Impl);
        #endif
        }

    }
}
