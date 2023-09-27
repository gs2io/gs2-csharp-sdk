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
using Gs2.Gs2News.Domain.Iterator;
using Gs2.Gs2News.Request;
using Gs2.Gs2News.Result;
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

namespace Gs2.Gs2News.Domain.Model
{

    public partial class ProgressDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2NewsRestClient _client;
        private readonly string _namespaceName;
        private readonly string _uploadToken;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UploadToken => _uploadToken;

        public ProgressDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string uploadToken
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2NewsRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._uploadToken = uploadToken;
            this._parentKey = Gs2.Gs2News.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Progress"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2News.Model.Output> Outputs(
        )
        {
            return new DescribeOutputsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.UploadToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2News.Model.Output> OutputsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2News.Model.Output> Outputs(
            #endif
        #else
        public DescribeOutputsIterator Outputs(
        #endif
        )
        {
            return new DescribeOutputsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.UploadToken
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

        public ulong SubscribeOutputs(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2News.Model.Output>(
                Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UploadToken,
                    "Output"
                ),
                callback
            );
        }

        public void UnsubscribeOutputs(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2News.Model.Output>(
                Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UploadToken,
                    "Output"
                ),
                callbackId
            );
        }

        public Gs2.Gs2News.Domain.Model.OutputDomain Output(
            string outputName
        ) {
            return new Gs2.Gs2News.Domain.Model.OutputDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UploadToken,
                outputName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string uploadToken,
            string childType
        )
        {
            return string.Join(
                ":",
                "news",
                namespaceName ?? "null",
                uploadToken ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string uploadToken
        )
        {
            return string.Join(
                ":",
                uploadToken ?? "null"
            );
        }

    }

    public partial class ProgressDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2News.Model.Progress> GetFuture(
            GetProgressRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2News.Model.Progress> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUploadToken(this.UploadToken);
                var future = this._client.GetProgressFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                            request.UploadToken.ToString()
                        );
                        _cache.Put<Gs2.Gs2News.Model.Progress>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "progress")
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
                    .WithUploadToken(this.UploadToken);
                GetProgressResult result = null;
                try {
                    result = await this._client.GetProgressAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                        request.UploadToken.ToString()
                        );
                    _cache.Put<Gs2.Gs2News.Model.Progress>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "progress")
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
                        var parentKey = Gs2.Gs2News.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Progress"
                        );
                        var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                            resultModel.Item.UploadToken.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2News.Model.Progress>(Impl);
        }
        #else
        private async Task<Gs2.Gs2News.Model.Progress> GetAsync(
            GetProgressRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUploadToken(this.UploadToken);
            var future = this._client.GetProgressFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                        request.UploadToken.ToString()
                    );
                    _cache.Put<Gs2.Gs2News.Model.Progress>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "progress")
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
                .WithUploadToken(this.UploadToken);
            GetProgressResult result = null;
            try {
                result = await this._client.GetProgressAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                    request.UploadToken.ToString()
                    );
                _cache.Put<Gs2.Gs2News.Model.Progress>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "progress")
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
                    var parentKey = Gs2.Gs2News.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Progress"
                    );
                    var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                        resultModel.Item.UploadToken.ToString()
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

    public partial class ProgressDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Model.Progress> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2News.Model.Progress> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2News.Model.Progress>(
                    _parentKey,
                    Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                        this.UploadToken?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetProgressRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                                    this.UploadToken?.ToString()
                                );
                            _cache.Put<Gs2.Gs2News.Model.Progress>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "progress")
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
                    (value, _) = _cache.Get<Gs2.Gs2News.Model.Progress>(
                        _parentKey,
                        Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                            this.UploadToken?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Model.Progress>(Impl);
        }
        #else
        public async Task<Gs2.Gs2News.Model.Progress> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2News.Model.Progress>(
                    _parentKey,
                    Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                        this.UploadToken?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetProgressRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                                    this.UploadToken?.ToString()
                                );
                    _cache.Put<Gs2.Gs2News.Model.Progress>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "progress")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2News.Model.Progress>(
                        _parentKey,
                        Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                            this.UploadToken?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2News.Model.Progress> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2News.Model.Progress> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2News.Model.Progress> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2News.Model.Progress> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2News.Model.Progress> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                    this.UploadToken.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2News.Model.Progress>(
                _parentKey,
                Gs2.Gs2News.Domain.Model.ProgressDomain.CreateCacheKey(
                    this.UploadToken.ToString()
                ),
                callbackId
            );
        }

    }
}
