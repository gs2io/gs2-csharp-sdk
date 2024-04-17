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
using Gs2.Gs2Lottery.Domain.Iterator;
using Gs2.Gs2Lottery.Model.Cache;
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
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

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class NamespaceDomain {
#if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Lottery.Model.DrawnPrize> DrawnPrizes(
        )
        {
            return new DescribeDrawnPrizesIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.DrawnPrize> DrawnPrizesAsync(
    #else
        public DescribeDrawnPrizesIterator DrawnPrizesAsync(
    #endif
        )
        {
            return new DescribeDrawnPrizesIterator(
                this._gs2,
                this._client,
                this.NamespaceName
    #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
    #else
            );
    #endif
        }
#endif
        
        public Gs2.Gs2Lottery.Domain.Model.DrawnPrizeDomain DrawnPrize(
            int index
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.DrawnPrizeDomain(
                this._gs2,
                this.NamespaceName,
                index
            );
        }
        
        public ulong SubscribeDrawnPrizes(
            Action<Gs2.Gs2Lottery.Model.DrawnPrize[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Lottery.Model.DrawnPrize>(
                (null as Gs2.Gs2Lottery.Model.DrawnPrize).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }
        
        public void UnsubscribeDrawnPrizes(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Lottery.Model.DrawnPrize>(
                (null as Gs2.Gs2Lottery.Model.DrawnPrize).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }
    }
}
