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
using Gs2.Gs2JobQueue.Domain.Iterator;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
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

namespace Gs2.Gs2JobQueue.Domain.Model
{

    public partial class DeadLetterJobDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2JobQueueRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _deadLetterJobName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string DeadLetterJobName => _deadLetterJobName;

        public DeadLetterJobDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string deadLetterJobName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2JobQueueRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._deadLetterJobName = deadLetterJobName;
            this._parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "DeadLetterJob"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string deadLetterJobName,
            string childType
        )
        {
            return string.Join(
                ":",
                "jobQueue",
                namespaceName ?? "null",
                userId ?? "null",
                deadLetterJobName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string deadLetterJobName
        )
        {
            return string.Join(
                ":",
                deadLetterJobName ?? "null"
            );
        }

    }

    public partial class DeadLetterJobDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2JobQueue.Model.DeadLetterJob> GetFuture(
            GetDeadLetterJobByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.DeadLetterJob> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDeadLetterJobName(this.DeadLetterJobName);
                var future = this._client.GetDeadLetterJobByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                            request.DeadLetterJobName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "deadLetterJob")
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DeadLetterJob"
                        );
                        var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.DeadLetterJob>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2JobQueue.Model.DeadLetterJob> GetAsync(
            #else
        private async Task<Gs2.Gs2JobQueue.Model.DeadLetterJob> GetAsync(
            #endif
            GetDeadLetterJobByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDeadLetterJobName(this.DeadLetterJobName);
            GetDeadLetterJobByUserIdResult result = null;
            try {
                result = await this._client.GetDeadLetterJobByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                    request.DeadLetterJobName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "deadLetterJob")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DeadLetterJob"
                    );
                    var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain> DeleteFuture(
            DeleteDeadLetterJobByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithDeadLetterJobName(this.DeadLetterJobName);
                var future = this._client.DeleteDeadLetterJobByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                            request.DeadLetterJobName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "deadLetterJob")
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
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "DeadLetterJob"
                        );
                        var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2JobQueue.Model.DeadLetterJob>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain> DeleteAsync(
            #endif
            DeleteDeadLetterJobByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithDeadLetterJobName(this.DeadLetterJobName);
            DeleteDeadLetterJobByUserIdResult result = null;
            try {
                result = await this._client.DeleteDeadLetterJobByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                    request.DeadLetterJobName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "deadLetterJob")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2JobQueue.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "DeadLetterJob"
                    );
                    var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2JobQueue.Model.DeadLetterJob>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain> Delete(
            DeleteDeadLetterJobByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class DeadLetterJobDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2JobQueue.Model.DeadLetterJob> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2JobQueue.Model.DeadLetterJob> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                    _parentKey,
                    Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                        this.DeadLetterJobName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetDeadLetterJobByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                                    this.DeadLetterJobName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "deadLetterJob")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                        _parentKey,
                        Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                            this.DeadLetterJobName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2JobQueue.Model.DeadLetterJob>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2JobQueue.Model.DeadLetterJob> ModelAsync()
            #else
        public async Task<Gs2.Gs2JobQueue.Model.DeadLetterJob> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                    _parentKey,
                    Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                        this.DeadLetterJobName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetDeadLetterJobByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                                    this.DeadLetterJobName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "deadLetterJob")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                        _parentKey,
                        Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                            this.DeadLetterJobName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2JobQueue.Model.DeadLetterJob> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2JobQueue.Model.DeadLetterJob> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2JobQueue.Model.DeadLetterJob> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2JobQueue.Model.DeadLetterJob> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                    this.DeadLetterJobName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2JobQueue.Model.DeadLetterJob>(
                _parentKey,
                Gs2.Gs2JobQueue.Domain.Model.DeadLetterJobDomain.CreateCacheKey(
                    this.DeadLetterJobName.ToString()
                ),
                callbackId
            );
        }

    }
}
