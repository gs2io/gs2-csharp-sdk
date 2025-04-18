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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdatePropertyFormModelMasterRequest : Gs2Request<UpdatePropertyFormModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string PropertyFormModelName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public Gs2.Gs2Formation.Model.SlotModel[] Slots { set; get; } = null!;
        public UpdatePropertyFormModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdatePropertyFormModelMasterRequest WithPropertyFormModelName(string propertyFormModelName) {
            this.PropertyFormModelName = propertyFormModelName;
            return this;
        }
        public UpdatePropertyFormModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdatePropertyFormModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdatePropertyFormModelMasterRequest WithSlots(Gs2.Gs2Formation.Model.SlotModel[] slots) {
            this.Slots = slots;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdatePropertyFormModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdatePropertyFormModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithPropertyFormModelName(!data.Keys.Contains("propertyFormModelName") || data["propertyFormModelName"] == null ? null : data["propertyFormModelName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSlots(!data.Keys.Contains("slots") || data["slots"] == null || !data["slots"].IsArray ? null : data["slots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Formation.Model.SlotModel.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData slotsJsonData = null;
            if (Slots != null && Slots.Length > 0)
            {
                slotsJsonData = new JsonData();
                foreach (var slot in Slots)
                {
                    slotsJsonData.Add(slot.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["propertyFormModelName"] = PropertyFormModelName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["slots"] = slotsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (PropertyFormModelName != null) {
                writer.WritePropertyName("propertyFormModelName");
                writer.Write(PropertyFormModelName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Slots != null) {
                writer.WritePropertyName("slots");
                writer.WriteArrayStart();
                foreach (var slot in Slots)
                {
                    if (slot != null) {
                        slot.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += PropertyFormModelName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Slots + ":";
            return key;
        }
    }
}