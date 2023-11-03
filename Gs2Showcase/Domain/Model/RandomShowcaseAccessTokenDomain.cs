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
using Gs2.Gs2Showcase.Domain.Iterator;
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
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

namespace Gs2.Gs2Showcase.Domain.Model
{

    public partial class RandomShowcaseAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _showcaseName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string ShowcaseName => _showcaseName;

        public RandomShowcaseAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string showcaseName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2ShowcaseRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._showcaseName = showcaseName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "RandomShowcase"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItems(
        )
        {
            return new DescribeRandomDisplayItemsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.ShowcaseName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItemsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Showcase.Model.RandomDisplayItem> RandomDisplayItems(
            #endif
        #else
        public DescribeRandomDisplayItemsIterator RandomDisplayItemsAsync(
        #endif
        )
        {
            return new DescribeRandomDisplayItemsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.ShowcaseName,
                this.AccessToken
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

        public ulong SubscribeRandomDisplayItems(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.ShowcaseName,
                    "RandomDisplayItem"
                ),
                callback
            );
        }

        public void UnsubscribeRandomDisplayItems(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.ShowcaseName,
                    "RandomDisplayItem"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemAccessTokenDomain RandomDisplayItem(
            string displayItemName
        ) {
            return new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                this.ShowcaseName,
                displayItemName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string showcaseName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                userId ?? "null",
                showcaseName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string showcaseName
        )
        {
            return string.Join(
                ":",
                showcaseName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Showcase.Model.RandomShowcase>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                        this.ShowcaseName?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.RandomShowcase>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Showcase.Model.RandomShowcase> ModelAsync()
            #else
        public async Task<Gs2.Gs2Showcase.Model.RandomShowcase> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Showcase.Model.RandomShowcase>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                        this.ShowcaseName?.ToString()
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Showcase.Model.RandomShowcase> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.RandomShowcase> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.RandomShowcase> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Showcase.Model.RandomShowcase> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                    this.ShowcaseName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Showcase.Model.RandomShowcase>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheKey(
                    this.ShowcaseName.ToString()
                ),
                callbackId
            );
        }

    }
}
