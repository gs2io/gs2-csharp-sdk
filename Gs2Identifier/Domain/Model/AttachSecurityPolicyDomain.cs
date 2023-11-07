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

    public partial class AttachSecurityPolicyDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _userName;

        private readonly String _parentKey;
        public string UserName => _userName;

        public AttachSecurityPolicyDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string userName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
            );
            this._userName = userName;
            this._parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                this.UserName,
                "AttachSecurityPolicy"
            );
        }

        public static string CreateCacheParentKey(
            string userName,
            string childType
        )
        {
            return string.Join(
                ":",
                "identifier",
                userName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class AttachSecurityPolicyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> GetHasSecurityPolicyFuture(
            GetHasSecurityPolicyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> self)
            {
                request
                    .WithUserName(this.UserName);
                var future = this._client.GetHasSecurityPolicyFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain.CreateCacheKey(
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "attachSecurityPolicy")
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
                    
                }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                        this._gs2,
                        result.Items[i]?.Name
                    );
                    var parentKey = "identifier:SecurityPolicy";
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> GetHasSecurityPolicyAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> GetHasSecurityPolicyAsync(
            #endif
            GetHasSecurityPolicyRequest request
        ) {
            request
                .WithUserName(this.UserName);
            GetHasSecurityPolicyResult result = null;
            try {
                result = await this._client.GetHasSecurityPolicyAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain.CreateCacheKey(
                    );
                this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "attachSecurityPolicy")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                        this._gs2,
                        result.Items[i]?.Name
                    );
                    var parentKey = "identifier:SecurityPolicy";
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to GetHasSecurityPolicyFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> GetHasSecurityPolicy(
            GetHasSecurityPolicyRequest request
        ) {
            return GetHasSecurityPolicyFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> AttachSecurityPolicyFuture(
            AttachSecurityPolicyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> self)
            {
                request
                    .WithUserName(this.UserName);
                var future = this._client.AttachSecurityPolicyFuture(
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
                    
                }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                        this._gs2,
                        result.Items[i]?.Name
                    );
                    var parentKey = "identifier:SecurityPolicy";
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> AttachSecurityPolicyAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> AttachSecurityPolicyAsync(
            #endif
            AttachSecurityPolicyRequest request
        ) {
            request
                .WithUserName(this.UserName);
            AttachSecurityPolicyResult result = null;
                result = await this._client.AttachSecurityPolicyAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                        this._gs2,
                        result.Items[i]?.Name
                    );
                    var parentKey = "identifier:SecurityPolicy";
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AttachSecurityPolicyFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> AttachSecurityPolicy(
            AttachSecurityPolicyRequest request
        ) {
            return AttachSecurityPolicyFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> DetachSecurityPolicyFuture(
            DetachSecurityPolicyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> self)
            {
                request
                    .WithUserName(this.UserName);
                var future = this._client.DetachSecurityPolicyFuture(
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
                    
                }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                        this._gs2,
                        result.Items[i]?.Name
                    );
                    var parentKey = "identifier:SecurityPolicy";
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> DetachSecurityPolicyAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> DetachSecurityPolicyAsync(
            #endif
            DetachSecurityPolicyRequest request
        ) {
            request
                .WithUserName(this.UserName);
            DetachSecurityPolicyResult result = null;
                result = await this._client.DetachSecurityPolicyAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                        this._gs2,
                        result.Items[i]?.Name
                    );
                    var parentKey = "identifier:SecurityPolicy";
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DetachSecurityPolicyFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain[]> DetachSecurityPolicy(
            DetachSecurityPolicyRequest request
        ) {
            return DetachSecurityPolicyFuture(request);
        }
        #endif

    }

    public partial class AttachSecurityPolicyDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain.CreateCacheKey(
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> ModelAsync()
            #else
        public async Task<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain.CreateCacheKey(
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.AttachSecurityPolicy> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain.CreateCacheKey(
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Identifier.Model.AttachSecurityPolicy>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

    }
}
