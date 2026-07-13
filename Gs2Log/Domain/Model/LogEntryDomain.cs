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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Log.Domain.Iterator;
using Gs2.Gs2Log.Model.Cache;
using Gs2.Gs2Log.Request;
using Gs2.Gs2Log.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Log.Domain.Model
{

    public partial class LogEntryDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LogRestClient _client;
        public Gs2.Gs2Log.Model.Trace[] Parallels { get; set; } = null!;
        public bool? ParallelTruncated { get; set; } = null!;

        public LogEntryDomain(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LogRestClient(
                gs2.RestSession
            );
        }

    }

    public partial class LogEntryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.LogEntryDomain> GetLogFuture(
            GetLogRequest request
        ) => GetLogAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.LogEntryDomain> GetLogAsync(
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.LogEntryDomain> GetLogAsync(
        #endif
            GetLogRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetLogAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.FacetDomain[]> FacetsFuture(
            QueryFacetsRequest request
        ) => FacetsAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.FacetDomain[]> FacetsAsync(
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.FacetDomain[]> FacetsAsync(
        #endif
            QueryFacetsRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.QueryFacetsAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2Log.Domain.Model.FacetDomain(
                this._gs2
            )).ToArray() ?? Array.Empty<Gs2.Gs2Log.Domain.Model.FacetDomain>();
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.LogEntryDomain> GetTraceFuture(
            GetTraceRequest request
        ) => GetTraceAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.LogEntryDomain> GetTraceAsync(
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.LogEntryDomain> GetTraceAsync(
        #endif
            GetTraceRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetTraceAsync(request)
            );
            var domain = this;
            this.Parallels = domain.Parallels = result?.Parallels;
            this.ParallelTruncated = domain.ParallelTruncated = result?.ParallelTruncated;
            return domain;
        }

    }

    public partial class LogEntryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Model.LogEntry> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Model.LogEntry> ModelAsync()
        #else
        public async Task<Gs2.Gs2Log.Model.LogEntry> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Log.Model.LogEntry>(
                        (null as Gs2.Gs2Log.Model.LogEntry).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Log.Model.LogEntry).CacheKey(
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Log.Model.LogEntry).GetCache(
                    this._gs2.Cache,
                    null
                );
                if (find) {
                    return value;
                }
                return null;
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Log.Model.LogEntry> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Log.Model.LogEntry> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Log.Model.LogEntry> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Log.Model.LogEntry).DeleteCache(
                this._gs2.Cache,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Log.Model.LogEntry> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Log.Model.LogEntry).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Log.Model.LogEntry).CacheKey(
                ),
                callback,
                () =>
                {
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
                    Impl().Forget();
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Log.Model.LogEntry>(
                (null as Gs2.Gs2Log.Model.LogEntry).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Log.Model.LogEntry).CacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Log.Model.LogEntry> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Log.Model.LogEntry> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Log.Model.LogEntry> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
