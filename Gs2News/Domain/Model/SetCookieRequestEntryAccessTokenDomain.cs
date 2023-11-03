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

    public partial class SetCookieRequestEntryAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2NewsRestClient _client;
        private readonly string _namespaceName;
        private readonly AccessToken _accessToken;
        private readonly string _key;
        private readonly string _value;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string Key => _key;
        public string Value => _value;

        public SetCookieRequestEntryAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string key,
            string value
        ) {
            this._gs2 = gs2;
            this._client = new Gs2NewsRestClient(
                this._gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._key = key;
            this._value = value;
            this._parentKey = Gs2.Gs2News.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                "SetCookieRequestEntry"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string key,
            string value,
            string childType
        )
        {
            return string.Join(
                ":",
                "news",
                namespaceName ?? "null",
                userId ?? "null",
                key ?? "null",
                value ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string key,
            string value
        )
        {
            return string.Join(
                ":",
                key ?? "null",
                value ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2News.Model.SetCookieRequestEntry> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2News.Model.SetCookieRequestEntry> self)
            {
                var (value, find) = this._gs2.Cache.Get<Gs2.Gs2News.Model.SetCookieRequestEntry>(
                    _parentKey,
                    Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain.CreateCacheKey(
                        this.Key?.ToString(),
                        this.Value?.ToString()
                    )
                );
                self.OnComplete(value);
                yield return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2News.Model.SetCookieRequestEntry>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2News.Model.SetCookieRequestEntry> ModelAsync()
            #else
        public async Task<Gs2.Gs2News.Model.SetCookieRequestEntry> ModelAsync()
            #endif
        {
            var (value, find) = this._gs2.Cache.Get<Gs2.Gs2News.Model.SetCookieRequestEntry>(
                _parentKey,
                Gs2.Gs2News.Domain.Model.SetCookieRequestEntryDomain.CreateCacheKey(
                    this.Key?.ToString(),
                    this.Value?.ToString()
                )
            );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2News.Model.SetCookieRequestEntry> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2News.Model.SetCookieRequestEntry> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2News.Model.SetCookieRequestEntry> Model()
        {
            return await ModelAsync();
        }
        #endif
        
    }
}
