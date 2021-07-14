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
using Gs2.Gs2Inventory.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Inventory.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateInventoryModelMasterRequest : Gs2Request<UpdateInventoryModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string InventoryName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public int? InitialCapacity { set; get; }
        public int? MaxCapacity { set; get; }
        public bool? ProtectReferencedItem { set; get; }

        public UpdateInventoryModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateInventoryModelMasterRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }

        public UpdateInventoryModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdateInventoryModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public UpdateInventoryModelMasterRequest WithInitialCapacity(int? initialCapacity) {
            this.InitialCapacity = initialCapacity;
            return this;
        }

        public UpdateInventoryModelMasterRequest WithMaxCapacity(int? maxCapacity) {
            this.MaxCapacity = maxCapacity;
            return this;
        }

        public UpdateInventoryModelMasterRequest WithProtectReferencedItem(bool? protectReferencedItem) {
            this.ProtectReferencedItem = protectReferencedItem;
            return this;
        }

    	[Preserve]
        public static UpdateInventoryModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateInventoryModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInitialCapacity(!data.Keys.Contains("initialCapacity") || data["initialCapacity"] == null ? null : (int?)int.Parse(data["initialCapacity"].ToString()))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : (int?)int.Parse(data["maxCapacity"].ToString()))
                .WithProtectReferencedItem(!data.Keys.Contains("protectReferencedItem") || data["protectReferencedItem"] == null ? null : (bool?)bool.Parse(data["protectReferencedItem"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["initialCapacity"] = InitialCapacity,
                ["maxCapacity"] = MaxCapacity,
                ["protectReferencedItem"] = ProtectReferencedItem,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InitialCapacity != null) {
                writer.WritePropertyName("initialCapacity");
                writer.Write(int.Parse(InitialCapacity.ToString()));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write(int.Parse(MaxCapacity.ToString()));
            }
            if (ProtectReferencedItem != null) {
                writer.WritePropertyName("protectReferencedItem");
                writer.Write(bool.Parse(ProtectReferencedItem.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}