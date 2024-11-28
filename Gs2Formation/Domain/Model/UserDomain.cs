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
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Model.Cache;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FormationRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FormationRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Formation.Model.Mold> Molds(
            string timeOffsetToken = null
        )
        {
            return new DescribeMoldsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.Mold> MoldsAsync(
            #else
        public DescribeMoldsByUserIdIterator MoldsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeMoldsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeMolds(
            Action<Gs2.Gs2Formation.Model.Mold[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Formation.Model.Mold>(
                (null as Gs2.Gs2Formation.Model.Mold).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeMoldsWithInitialCallAsync(
            Action<Gs2.Gs2Formation.Model.Mold[]> callback
        )
        {
            var items = await MoldsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeMolds(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeMolds(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Formation.Model.Mold>(
                (null as Gs2.Gs2Formation.Model.Mold).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.MoldDomain Mold(
            string moldModelName
        ) {
            return new Gs2.Gs2Formation.Domain.Model.MoldDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                moldModelName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Formation.Model.PropertyForm> PropertyForms(
            string propertyFormModelName,
            string timeOffsetToken = null
        )
        {
            return new DescribePropertyFormsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                propertyFormModelName,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.PropertyForm> PropertyFormsAsync(
            #else
        public DescribePropertyFormsByUserIdIterator PropertyFormsAsync(
            #endif
            string propertyFormModelName,
            string timeOffsetToken = null
        )
        {
            return new DescribePropertyFormsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                propertyFormModelName,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribePropertyForms(
            Action<Gs2.Gs2Formation.Model.PropertyForm[]> callback,
            string propertyFormModelName
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Formation.Model.PropertyForm>(
                (null as Gs2.Gs2Formation.Model.PropertyForm).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribePropertyFormsWithInitialCallAsync(
            Action<Gs2.Gs2Formation.Model.PropertyForm[]> callback,
            string propertyFormModelName
        )
        {
            var items = await PropertyFormsAsync(
                propertyFormModelName
            ).ToArrayAsync();
            var callbackId = SubscribePropertyForms(
                callback,
                propertyFormModelName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribePropertyForms(
            ulong callbackId,
            string propertyFormModelName
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Formation.Model.PropertyForm>(
                (null as Gs2.Gs2Formation.Model.PropertyForm).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Formation.Domain.Model.PropertyFormDomain PropertyForm(
            string propertyFormModelName,
            string propertyId
        ) {
            return new Gs2.Gs2Formation.Domain.Model.PropertyFormDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                propertyFormModelName,
                propertyId
            );
        }

    }

    public partial class UserDomain {

    }

    public partial class UserDomain {

    }
}
