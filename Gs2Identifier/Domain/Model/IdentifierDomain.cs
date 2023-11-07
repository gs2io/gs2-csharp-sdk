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

    public partial class IdentifierDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _userName;
        private readonly string _clientId;

        private readonly String _parentKey;
        public string ClientSecret { get; set; }
        public string Status { get; set; }
        public string UserName => _userName;
        public string ClientId => _clientId;

        public IdentifierDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string userName,
            string clientId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
            );
            this._userName = userName;
            this._clientId = clientId;
            this._parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                this.UserName,
                "Identifier"
            );
        }

        public static string CreateCacheParentKey(
            string userName,
            string clientId,
            string childType
        )
        {
            return string.Join(
                ":",
                "identifier",
                userName ?? "null",
                clientId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string clientId
        )
        {
            return string.Join(
                ":",
                clientId ?? "null"
            );
        }

    }

    public partial class IdentifierDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Identifier.Model.Identifier> GetFuture(
            GetIdentifierRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.Identifier> self)
            {
                request
                    .WithUserName(this.UserName)
                    .WithClientId(this.ClientId);
                var future = this._client.GetIdentifierFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                            request.ClientId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.Identifier>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "identifier")
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
                        var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.UserName,
                            "Identifier"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                            resultModel.Item.ClientId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.Identifier>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Identifier.Model.Identifier> GetAsync(
            #else
        private async Task<Gs2.Gs2Identifier.Model.Identifier> GetAsync(
            #endif
            GetIdentifierRequest request
        ) {
            request
                .WithUserName(this.UserName)
                .WithClientId(this.ClientId);
            GetIdentifierResult result = null;
            try {
                result = await this._client.GetIdentifierAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                    request.ClientId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.Identifier>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "identifier")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.UserName,
                        "Identifier"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                        resultModel.Item.ClientId.ToString()
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
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> DeleteFuture(
            DeleteIdentifierRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> self)
            {
                request
                    .WithUserName(this.UserName)
                    .WithClientId(this.ClientId);
                var future = this._client.DeleteIdentifierFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                            request.ClientId.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.Identifier>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "identifier")
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
                        var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.UserName,
                            "Identifier"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                            resultModel.Item.ClientId.ToString()
                        );
                        cache.Delete<Gs2.Gs2Identifier.Model.Identifier>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> DeleteAsync(
            #endif
            DeleteIdentifierRequest request
        ) {
            request
                .WithUserName(this.UserName)
                .WithClientId(this.ClientId);
            DeleteIdentifierResult result = null;
            try {
                result = await this._client.DeleteIdentifierAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                    request.ClientId.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.Identifier>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "identifier")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.UserName,
                        "Identifier"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                        resultModel.Item.ClientId.ToString()
                    );
                    cache.Delete<Gs2.Gs2Identifier.Model.Identifier>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> Delete(
            DeleteIdentifierRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class IdentifierDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.Identifier> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.Identifier> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.Identifier>(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                        this.ClientId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetIdentifierRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                                    this.ClientId?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.Identifier>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "identifier")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.Identifier>(
                        _parentKey,
                        Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                            this.ClientId?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.Identifier>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Model.Identifier> ModelAsync()
            #else
        public async Task<Gs2.Gs2Identifier.Model.Identifier> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.Identifier>(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                        this.ClientId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetIdentifierRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                                    this.ClientId?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Identifier.Model.Identifier>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors.Length == 0 || e.errors[0].component != "identifier")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.Identifier>(
                        _parentKey,
                        Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                            this.ClientId?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Identifier.Model.Identifier> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Identifier.Model.Identifier> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Identifier.Model.Identifier> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.Identifier> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                    this.ClientId.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Identifier.Model.Identifier>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                    this.ClientId.ToString()
                ),
                callbackId
            );
        }

    }
}
