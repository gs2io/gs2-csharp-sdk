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
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
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

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FormationRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.Mold> Molds(
        )
        {
            return new DescribeMoldsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.Mold> MoldsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.Mold> Molds(
            #endif
        #else
        public DescribeMoldsIterator Molds(
        #endif
        )
        {
            return new DescribeMoldsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeMolds(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.Mold>(
                Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Mold"
                ),
                callback
            );
        }

        public void UnsubscribeMolds(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.Mold>(
                Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Mold"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain Mold(
            string moldModelName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken,
                moldModelName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Formation.Model.PropertyForm> PropertyForms(
            string propertyFormModelName
        )
        {
            return new DescribePropertyFormsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                propertyFormModelName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.PropertyForm> PropertyFormsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Formation.Model.PropertyForm> PropertyForms(
            #endif
        #else
        public DescribePropertyFormsIterator PropertyForms(
        #endif
            string propertyFormModelName
        )
        {
            return new DescribePropertyFormsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                propertyFormModelName
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

        public ulong SubscribePropertyForms(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Formation.Model.PropertyForm>(
                Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "PropertyForm"
                ),
                callback
            );
        }

        public void UnsubscribePropertyForms(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Formation.Model.PropertyForm>(
                Gs2.Gs2Formation.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "PropertyForm"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain PropertyForm(
            string propertyFormModelName,
            string propertyId
        ) {
            return new Gs2.Gs2Formation.Domain.Model.PropertyFormAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken,
                propertyFormModelName,
                propertyId
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "formation",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }
}
