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
 *
 * deny overwrite
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
using Gs2.Gs2Distributor.Domain.Iterator;
using Gs2.Gs2Distributor.Model.Cache;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
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

namespace Gs2.Gs2Distributor.Domain.Model
{

    public partial class DistributeDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;
        public string NamespaceName { get; } = null!;
        public string InboxNamespaceId { get; set; } = null!;
        public string Result { get; set; } = null!;
        public string ContextStack { get; set; } = null!;
        public int? StatusCode { get; set; } = null!;
        public int[] VerifyTaskResultCodes { get; set; } = null!;
        public string[] VerifyTaskResults { get; set; } = null!;
        public int[] TaskResultCodes { get; set; } = null!;
        public string[] TaskResults { get; set; } = null!;
        public int? SheetResultCode { get; set; } = null!;
        public string SheetResult { get; set; } = null!;

        public DistributeDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

    }

    public partial class DistributeDomain {

    }
}
