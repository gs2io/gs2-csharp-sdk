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
	public class UpdateSalesItemGroupMasterRequest : Gs2Request<UpdateSalesItemGroupMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string SalesItemGroupName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string[] SalesItemNames { set; get; }
        public UpdateSalesItemGroupMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateSalesItemGroupMasterRequest WithSalesItemGroupName(string salesItemGroupName) {
            this.SalesItemGroupName = salesItemGroupName;
            return this;
        }
        public UpdateSalesItemGroupMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateSalesItemGroupMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateSalesItemGroupMasterRequest WithSalesItemNames(string[] salesItemNames) {
            this.SalesItemNames = salesItemNames;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateSalesItemGroupMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateSalesItemGroupMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSalesItemGroupName(!data.Keys.Contains("salesItemGroupName") || data["salesItemGroupName"] == null ? null : data["salesItemGroupName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSalesItemNames(!data.Keys.Contains("salesItemNames") || data["salesItemNames"] == null ? new string[]{} : data["salesItemNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData salesItemNamesJsonData = null;
            if (SalesItemNames != null)
            {
                salesItemNamesJsonData = new JsonData();
                foreach (var salesItemName in SalesItemNames)
                {
                    salesItemNamesJsonData.Add(salesItemName);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["salesItemGroupName"] = SalesItemGroupName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["salesItemNames"] = salesItemNamesJsonData,
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
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            writer.WriteArrayStart();
            foreach (var salesItemName in SalesItemNames)
            {
                writer.Write(salesItemName.ToString());
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += SalesItemGroupName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += SalesItemNames + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateSalesItemGroupMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateSalesItemGroupMasterRequest)x;
            return this;
        }
    }
}