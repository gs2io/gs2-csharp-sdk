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
using Gs2.Gs2AdReward.Domain.Iterator;
using Gs2.Gs2AdReward.Request;
using Gs2.Gs2AdReward.Result;
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

namespace Gs2.Gs2AdReward.Domain.Model
{

    public partial class HistoryDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2AdRewardRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _transactionId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string TransactionId => _transactionId;

        public HistoryDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string transactionId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2AdRewardRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._transactionId = transactionId;
            this._parentKey = Gs2.Gs2AdReward.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "History"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string transactionId,
            string childType
        )
        {
            return string.Join(
                ":",
                "adReward",
                namespaceName ?? "null",
                userId ?? "null",
                transactionId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string transactionId
        )
        {
            return string.Join(
                ":",
                transactionId ?? "null"
            );
        }

    }

    public partial class HistoryDomain {

    }

    public partial class HistoryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2AdReward.Model.History> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2AdReward.Model.History> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2AdReward.Model.History>(
                    _parentKey,
                    Gs2.Gs2AdReward.Domain.Model.HistoryDomain.CreateCacheKey(
                        this.TransactionId?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2AdReward.Model.History>(Impl);
        }
        #else
        public async Task<Gs2.Gs2AdReward.Model.History> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2AdReward.Model.History>(
                    _parentKey,
                    Gs2.Gs2AdReward.Domain.Model.HistoryDomain.CreateCacheKey(
                        this.TransactionId?.ToString()
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2AdReward.Model.History> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2AdReward.Model.History> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2AdReward.Model.History> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2AdReward.Model.History> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
