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
using Gs2.Gs2Script.Domain.Iterator;
using Gs2.Gs2Script.Request;
using Gs2.Gs2Script.Result;
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

namespace Gs2.Gs2Script.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ScriptRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public int? Code { get; set; }
        public string Result { get; set; }
        public int? ExecuteTime { get; set; }
        public int? Charged { get; set; }
        public string[] Output { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;

        public NamespaceDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ScriptRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = "script:Namespace";
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Script.Model.Script> Scripts(
        )
        {
            return new DescribeScriptsIterator(
                this._cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Script.Model.Script> ScriptsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Script.Model.Script> Scripts(
            #endif
        #else
        public DescribeScriptsIterator Scripts(
        #endif
        )
        {
            return new DescribeScriptsIterator(
                this._cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeScripts(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Script.Model.Script>(
                Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Script"
                ),
                callback
            );
        }

        public void UnsubscribeScripts(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Script.Model.Script>(
                Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Script"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Script.Domain.Model.ScriptDomain Script(
            string scriptName
        ) {
            return new Gs2.Gs2Script.Domain.Model.ScriptDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                scriptName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "script",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string namespaceName
        )
        {
            return string.Join(
                ":",
                namespaceName ?? "null"
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceStatusResult result = null;
                try {
                    result = await this._client.GetNamespaceStatusAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceStatusFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceStatusResult result = null;
            try {
                result = await this._client.GetNamespaceStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Script.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            var future = GetStatusFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to GetStatusFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> GetStatus(
            GetNamespaceStatusRequest request
        ) {
            return GetStatusFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Script.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Model.Namespace> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceResult result = null;
                try {
                    result = await this._client.GetNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "script",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Script.Model.Namespace>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Script.Model.Namespace> GetAsync(
            GetNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceResult result = null;
            try {
                result = await this._client.GetNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "script",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateNamespaceFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                UpdateNamespaceResult result = null;
                    result = await this._client.UpdateNamespaceAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "script",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.UpdateNamespaceFuture(
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
                .WithNamespaceName(this.NamespaceName);
            UpdateNamespaceResult result = null;
                result = await this._client.UpdateNamespaceAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "script",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Script.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
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
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> Update(
            UpdateNamespaceRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
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
                    .WithNamespaceName(this.NamespaceName);
                DeleteNamespaceResult result = null;
                try {
                    result = await this._client.DeleteNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "script",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Script.Model.Namespace>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.DeleteNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
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
                .WithNamespaceName(this.NamespaceName);
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "script",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Script.Model.Namespace>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Script.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
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
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> Delete(
            DeleteNamespaceRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScriptFuture(
            CreateScriptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateScriptFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                CreateScriptResult result = null;
                    result = await this._client.CreateScriptAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Script"
                        );
                        var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Script.Domain.Model.ScriptDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScriptAsync(
            CreateScriptRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.CreateScriptFuture(
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
                .WithNamespaceName(this.NamespaceName);
            CreateScriptResult result = null;
                result = await this._client.CreateScriptAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Script"
                    );
                    var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Script.Domain.Model.ScriptDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScriptAsync(
            CreateScriptRequest request
        ) {
            var future = CreateScriptFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateScriptFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScript(
            CreateScriptRequest request
        ) {
            return CreateScriptFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScriptFromGitHubFuture(
            CreateScriptFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateScriptFromGitHubFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                CreateScriptFromGitHubResult result = null;
                    result = await this._client.CreateScriptFromGitHubAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Script"
                        );
                        var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Script.Domain.Model.ScriptDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScriptFromGitHubAsync(
            CreateScriptFromGitHubRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.CreateScriptFromGitHubFuture(
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
                .WithNamespaceName(this.NamespaceName);
            CreateScriptFromGitHubResult result = null;
                result = await this._client.CreateScriptFromGitHubAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Script"
                    );
                    var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Script.Domain.Model.ScriptDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScriptFromGitHubAsync(
            CreateScriptFromGitHubRequest request
        ) {
            var future = CreateScriptFromGitHubFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateScriptFromGitHubFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> CreateScriptFromGitHub(
            CreateScriptFromGitHubRequest request
        ) {
            return CreateScriptFromGitHubFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> InvokeScriptFuture(
            InvokeScriptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                var future = this._client.InvokeScriptFuture(
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
                InvokeScriptResult result = null;
                    result = await this._client.InvokeScriptAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Code = domain.Code = result?.Code;
                this.Result = domain.Result = result?.Result;
                this.ExecuteTime = domain.ExecuteTime = result?.ExecuteTime;
                this.Charged = domain.Charged = result?.Charged;
                this.Output = domain.Output = result?.Output;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Domain.Model.NamespaceDomain> InvokeScriptAsync(
            InvokeScriptRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            var future = this._client.InvokeScriptFuture(
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
            InvokeScriptResult result = null;
                result = await this._client.InvokeScriptAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Code = domain.Code = result?.Code;
            this.Result = domain.Result = result?.Result;
            this.ExecuteTime = domain.ExecuteTime = result?.ExecuteTime;
            this.Charged = domain.Charged = result?.Charged;
            this.Output = domain.Output = result?.Output;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Script.Domain.Model.NamespaceDomain> InvokeScriptAsync(
            InvokeScriptRequest request
        ) {
            var future = InvokeScriptFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to InvokeScriptFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> InvokeScript(
            InvokeScriptRequest request
        ) {
            return InvokeScriptFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> DebugInvokeFuture(
            DebugInvokeRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                var future = this._client.DebugInvokeFuture(
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
                DebugInvokeResult result = null;
                    result = await this._client.DebugInvokeAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Code = domain.Code = result?.Code;
                this.Result = domain.Result = result?.Result;
                this.ExecuteTime = domain.ExecuteTime = result?.ExecuteTime;
                this.Charged = domain.Charged = result?.Charged;
                this.Output = domain.Output = result?.Output;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Domain.Model.NamespaceDomain> DebugInvokeAsync(
            DebugInvokeRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            var future = this._client.DebugInvokeFuture(
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
            DebugInvokeResult result = null;
                result = await this._client.DebugInvokeAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Code = domain.Code = result?.Code;
            this.Result = domain.Result = result?.Result;
            this.ExecuteTime = domain.ExecuteTime = result?.ExecuteTime;
            this.Charged = domain.Charged = result?.Charged;
            this.Output = domain.Output = result?.Output;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Script.Domain.Model.NamespaceDomain> DebugInvokeAsync(
            DebugInvokeRequest request
        ) {
            var future = DebugInvokeFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DebugInvokeFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.NamespaceDomain> DebugInvoke(
            DebugInvokeRequest request
        ) {
            return DebugInvokeFuture(request);
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Script.Model.Namespace> self)
            {
                var parentKey = string.Join(
                    ":",
                    "script",
                    "Namespace"
                );
                var (value, find) = _cache.Get<Gs2.Gs2Script.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetNamespaceRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "namespace")
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
                    (value, _) = _cache.Get<Gs2.Gs2Script.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Model.Namespace>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Script.Model.Namespace> ModelAsync()
        {
            var parentKey = string.Join(
                ":",
                "script",
                "Namespace"
            );
            var (value, find) = _cache.Get<Gs2.Gs2Script.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetNamespaceRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Script.Model.Namespace>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "namespace")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Script.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Script.Model.Namespace> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Script.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Script.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Script.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Script.Model.Namespace> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Script.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callbackId
            );
        }

    }
}
