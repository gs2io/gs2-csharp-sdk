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
using Gs2.Gs2Quest.Model.Cache;
using Gs2.Gs2Quest.Request;
using Gs2.Gs2Quest.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Quest.Domain.Model
{

    public partial class QuestGroupModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2QuestRestClient _client;
        public string NamespaceName { get; } = null!;
        public string QuestGroupName { get; } = null!;

        public QuestGroupModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string questGroupName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2QuestRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.QuestGroupName = questGroupName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Quest.Model.QuestModel> QuestModels(
        )
        {
            return new DescribeQuestModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.QuestGroupName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Quest.Model.QuestModel> QuestModelsAsync(
            #else
        public DescribeQuestModelsIterator QuestModelsAsync(
            #endif
        )
        {
            return new DescribeQuestModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.QuestGroupName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeQuestModels(
            Action<Gs2.Gs2Quest.Model.QuestModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Quest.Model.QuestModel>(
                (null as Gs2.Gs2Quest.Model.QuestModel).CacheParentKey(
                    this.NamespaceName,
                    this.QuestGroupName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeQuestModelsWithInitialCallAsync(
            Action<Gs2.Gs2Quest.Model.QuestModel[]> callback
        )
        {
            var items = await QuestModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeQuestModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeQuestModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Quest.Model.QuestModel>(
                (null as Gs2.Gs2Quest.Model.QuestModel).CacheParentKey(
                    this.NamespaceName,
                    this.QuestGroupName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Quest.Domain.Model.QuestModelDomain QuestModel(
            string questName
        ) {
            return new Gs2.Gs2Quest.Domain.Model.QuestModelDomain(
                this._gs2,
                this.NamespaceName,
                this.QuestGroupName,
                questName
            );
        }

    }

    public partial class QuestGroupModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Quest.Model.QuestGroupModel> GetFuture(
            GetQuestGroupModelRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestGroupModel> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithQuestGroupName(this.QuestGroupName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetQuestGroupModelFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestGroupModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Quest.Model.QuestGroupModel> GetAsync(
            #else
        private async Task<Gs2.Gs2Quest.Model.QuestGroupModel> GetAsync(
            #endif
            GetQuestGroupModelRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithQuestGroupName(this.QuestGroupName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetQuestGroupModelAsync(request)
            );
            return result?.Item;
        }
        #endif

    }

    public partial class QuestGroupModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Quest.Model.QuestGroupModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Model.QuestGroupModel> self)
            {
                var (value, find) = (null as Gs2.Gs2Quest.Model.QuestGroupModel).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.QuestGroupName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Quest.Model.QuestGroupModel).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.QuestGroupName,
                    () => this.GetFuture(
                        new GetQuestGroupModelRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Model.QuestGroupModel>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Quest.Model.QuestGroupModel> ModelAsync()
            #else
        public async Task<Gs2.Gs2Quest.Model.QuestGroupModel> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Quest.Model.QuestGroupModel).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.QuestGroupName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Quest.Model.QuestGroupModel).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.QuestGroupName,
                () => this.GetAsync(
                    new GetQuestGroupModelRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Quest.Model.QuestGroupModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Quest.Model.QuestGroupModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Quest.Model.QuestGroupModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Quest.Model.QuestGroupModel).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.QuestGroupName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Quest.Model.QuestGroupModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Quest.Model.QuestGroupModel).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Quest.Model.QuestGroupModel).CacheKey(
                    this.QuestGroupName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Quest.Model.QuestGroupModel>(
                (null as Gs2.Gs2Quest.Model.QuestGroupModel).CacheParentKey(
                    this.NamespaceName
                ),
                (null as Gs2.Gs2Quest.Model.QuestGroupModel).CacheKey(
                    this.QuestGroupName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Quest.Model.QuestGroupModel> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Quest.Model.QuestGroupModel> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Quest.Model.QuestGroupModel> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}
