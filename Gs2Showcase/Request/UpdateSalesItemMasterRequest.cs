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
	public class UpdateSalesItemMasterRequest : Gs2Request<UpdateSalesItemMasterRequest>
	{
         public string NamespaceName { set; get; }
         public string SalesItemName { set; get; }
         public string Description { set; get; }
         public string Metadata { set; get; }
         public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
         public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public UpdateSalesItemMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateSalesItemMasterRequest WithSalesItemName(string salesItemName) {
            this.SalesItemName = salesItemName;
            return this;
        }
        public UpdateSalesItemMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateSalesItemMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateSalesItemMasterRequest WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }
        public UpdateSalesItemMasterRequest WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateSalesItemMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateSalesItemMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSalesItemName(!data.Keys.Contains("salesItemName") || data["salesItemName"] == null ? null : data["salesItemName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? new Gs2.Core.Model.ConsumeAction[]{} : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? new Gs2.Core.Model.AcquireAction[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null && ConsumeActions.Length > 0)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["salesItemName"] = SalesItemName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["consumeActions"] = consumeActionsJsonData,
                ["acquireActions"] = acquireActionsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (SalesItemName != null) {
                writer.WritePropertyName("salesItemName");
                writer.Write(SalesItemName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ConsumeActions != null) {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach (var consumeAction in ConsumeActions)
                {
                    if (consumeAction != null) {
                        consumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += SalesItemName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ConsumeActions + ":";
            key += AcquireActions + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateSalesItemMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateSalesItemMasterRequest)x;
            return this;
        }
    }
}