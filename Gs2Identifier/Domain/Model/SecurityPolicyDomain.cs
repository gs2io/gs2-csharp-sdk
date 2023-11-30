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
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Domain.Model
{

    public partial class SecurityPolicyDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _securityPolicyName;

        private readonly String _parentKey;
        public string SecurityPolicyName => _securityPolicyName;

        public SecurityPolicyDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string securityPolicyName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
            );
            this._securityPolicyName = securityPolicyName;
            this._parentKey = "identifier:SecurityPolicy";
        }

        public static string CreateCacheParentKey(
            string securityPolicyName,
            string childType
        )
        {
            return string.Join(
                ":",
                "identifier",
                securityPolicyName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string securityPolicyName
        )
        {
            return string.Join(
                ":",
                securityPolicyName ?? "null"
            );
        }

    }

    public partial class SecurityPolicyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> UpdateFuture(
            UpdateSecurityPolicyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> self)
            {
                request
                    .WithSecurityPolicyName(this.SecurityPolicyName);
                var future = this._client.UpdateSecurityPolicyFuture(
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
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "SecurityPolicy"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> UpdateAsync(
            #endif
            UpdateSecurityPolicyRequest request
        ) {
            request
                .WithSecurityPolicyName(this.SecurityPolicyName);
            UpdateSecurityPolicyResult result = null;
                result = await this._client.UpdateSecurityPolicyAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "SecurityPolicy"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> Update(
            UpdateSecurityPolicyRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Identifier.Model.SecurityPolicy> GetFuture(
            GetSecurityPolicyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.SecurityPolicy> self)
            {
                request
                    .WithSecurityPolicyName(this.SecurityPolicyName);
                var future = this._client.GetSecurityPolicyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                            request.SecurityPolicyName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "securityPolicy")
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
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "SecurityPolicy"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.SecurityPolicy>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Identifier.Model.SecurityPolicy> GetAsync(
            #else
        private async Task<Gs2.Gs2Identifier.Model.SecurityPolicy> GetAsync(
            #endif
            GetSecurityPolicyRequest request
        ) {
            request
                .WithSecurityPolicyName(this.SecurityPolicyName);
            GetSecurityPolicyResult result = null;
            try {
                result = await this._client.GetSecurityPolicyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                    request.SecurityPolicyName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "securityPolicy")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "SecurityPolicy"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> DeleteFuture(
            DeleteSecurityPolicyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> self)
            {
                request
                    .WithSecurityPolicyName(this.SecurityPolicyName);
                var future = this._client.DeleteSecurityPolicyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                            request.SecurityPolicyName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "securityPolicy")
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
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "SecurityPolicy"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Identifier.Model.SecurityPolicy>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> DeleteAsync(
            #endif
            DeleteSecurityPolicyRequest request
        ) {
            request
                .WithSecurityPolicyName(this.SecurityPolicyName);
            DeleteSecurityPolicyResult result = null;
            try {
                result = await this._client.DeleteSecurityPolicyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                    request.SecurityPolicyName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "securityPolicy")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "SecurityPolicy"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Delete<Gs2.Gs2Identifier.Model.SecurityPolicy>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> Delete(
            DeleteSecurityPolicyRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class SecurityPolicyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.SecurityPolicy> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.SecurityPolicy> self)
            {
                var parentKey = string.Join(
                    ":",
                    "identifier",
                    "SecurityPolicy"
                );
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        this.SecurityPolicyName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetSecurityPolicyRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                                    this.SecurityPolicyName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "securityPolicy")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                        parentKey,
                        Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                            this.SecurityPolicyName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.SecurityPolicy>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Model.SecurityPolicy> ModelAsync()
            #else
        public async Task<Gs2.Gs2Identifier.Model.SecurityPolicy> ModelAsync()
            #endif
        {
            var parentKey = string.Join(
                ":",
                "identifier",
                "SecurityPolicy"
            );
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                    this.SecurityPolicyName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        this.SecurityPolicyName?.ToString()
                    )
                );
                if (!find) {
                    try {
                        await this.GetAsync(
                            new GetSecurityPolicyRequest()
                        );
                    } catch (Gs2.Core.Exception.NotFoundException e) {
                        var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                                    this.SecurityPolicyName?.ToString()
                                );
                        this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                            parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "securityPolicy")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                        parentKey,
                        Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                            this.SecurityPolicyName?.ToString()
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
        public async UniTask<Gs2.Gs2Identifier.Model.SecurityPolicy> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Identifier.Model.SecurityPolicy> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Identifier.Model.SecurityPolicy> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.SecurityPolicy> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                    this.SecurityPolicyName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                    this.SecurityPolicyName.ToString()
                ),
                callbackId
            );
        }

    }
}
