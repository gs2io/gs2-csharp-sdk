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
using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Showcase.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Showcase.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetSalesItemGroupMasterRequest : Gs2Request<GetSalesItemGroupMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string SalesItemGroupName { set; get; }
        public GetSalesItemGroupMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetSalesItemGroupMasterRequest WithSalesItemGroupName(string salesItemGroupName) {
            this.SalesItemGroupName = salesItemGroupName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetSalesItemGroupMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetSalesItemGroupMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSalesItemGroupName(!data.Keys.Contains("salesItemGroupName") || data["salesItemGroupName"] == null ? null : data["salesItemGroupName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["salesItemGroupName"] = SalesItemGroupName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (SalesItemGroupName != null) {
                writer.WritePropertyName("salesItemGroupName");
                writer.Write(SalesItemGroupName.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}