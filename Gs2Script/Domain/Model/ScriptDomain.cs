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

    public partial class ScriptDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ScriptRestClient _client;
        private readonly string _namespaceName;
        private readonly string _scriptName;

        private readonly String _parentKey;
        public int? Code { get; set; }
        public string Result { get; set; }
        public string Transaction { get; set; }
        public int? ExecuteTime { get; set; }
        public int? Charged { get; set; }
        public string[] Output { get; set; }
        public string NamespaceName => _namespaceName;
        public string ScriptName => _scriptName;

        public ScriptDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string scriptName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ScriptRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._scriptName = scriptName;
            this._parentKey = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Script"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string scriptName,
            string childType
        )
        {
            return string.Join(
                ":",
                "script",
                namespaceName ?? "null",
                scriptName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string scriptName
        )
        {
            return string.Join(
                ":",
                scriptName ?? "null"
            );
        }

    }

    public partial class ScriptDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Script.Model.Script> GetFuture(
            GetScriptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Model.Script> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithScriptName(this.ScriptName);
                var future = this._client.GetScriptFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                            request.ScriptName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Script.Model.Script>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "script")
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
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Model.Script>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Script.Model.Script> GetAsync(
            #else
        private async Task<Gs2.Gs2Script.Model.Script> GetAsync(
            #endif
            GetScriptRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithScriptName(this.ScriptName);
            GetScriptResult result = null;
            try {
                result = await this._client.GetScriptAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                    request.ScriptName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Script.Model.Script>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "script")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
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
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> UpdateFuture(
            UpdateScriptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithScriptName(this.ScriptName);
                var future = this._client.UpdateScriptFuture(
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
                var cache = this._gs2.Cache;
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Script.Domain.Model.ScriptDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Script.Domain.Model.ScriptDomain> UpdateAsync(
            #endif
            UpdateScriptRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithScriptName(this.ScriptName);
            UpdateScriptResult result = null;
                result = await this._client.UpdateScriptAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> Update(
            UpdateScriptRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> UpdateFromGitHubFuture(
            UpdateScriptFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithScriptName(this.ScriptName);
                var future = this._client.UpdateScriptFromGitHubFuture(
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
                var cache = this._gs2.Cache;
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Script.Domain.Model.ScriptDomain> UpdateFromGitHubAsync(
            #else
        public async Task<Gs2.Gs2Script.Domain.Model.ScriptDomain> UpdateFromGitHubAsync(
            #endif
            UpdateScriptFromGitHubRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithScriptName(this.ScriptName);
            UpdateScriptFromGitHubResult result = null;
                result = await this._client.UpdateScriptFromGitHubAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to UpdateFromGitHubFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> UpdateFromGitHub(
            UpdateScriptFromGitHubRequest request
        ) {
            return UpdateFromGitHubFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> DeleteFuture(
            DeleteScriptRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithScriptName(this.ScriptName);
                var future = this._client.DeleteScriptFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                            request.ScriptName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Script.Model.Script>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "script")
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
                        var parentKey = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Script"
                        );
                        var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Script.Model.Script>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Script.Domain.Model.ScriptDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Script.Domain.Model.ScriptDomain> DeleteAsync(
            #endif
            DeleteScriptRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithScriptName(this.ScriptName);
            DeleteScriptResult result = null;
            try {
                result = await this._client.DeleteScriptAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                    request.ScriptName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Script.Model.Script>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "script")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Script.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Script"
                    );
                    var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Script.Model.Script>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Script.Domain.Model.ScriptDomain> Delete(
            DeleteScriptRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class ScriptDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Script.Model.Script> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Script.Model.Script> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Script.Model.Script>(
                    _parentKey,
                    Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                        this.ScriptName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetScriptRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                                    this.ScriptName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Script.Model.Script>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "script")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Script.Model.Script>(
                        _parentKey,
                        Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                            this.ScriptName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Script.Model.Script>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Script.Model.Script> ModelAsync()
            #else
        public async Task<Gs2.Gs2Script.Model.Script> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Script.Model.Script>(
                    _parentKey,
                    Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                        this.ScriptName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetScriptRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                                    this.ScriptName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Script.Model.Script>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "script")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Script.Model.Script>(
                        _parentKey,
                        Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                            this.ScriptName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Script.Model.Script> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Script.Model.Script> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Script.Model.Script> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Script.Model.Script> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                    this.ScriptName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Script.Model.Script>(
                _parentKey,
                Gs2.Gs2Script.Domain.Model.ScriptDomain.CreateCacheKey(
                    this.ScriptName.ToString()
                ),
                callbackId
            );
        }

    }
}
