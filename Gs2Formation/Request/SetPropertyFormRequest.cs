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
	public class SetPropertyFormRequest : Gs2Request<SetPropertyFormRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string PropertyFormModelName { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public Gs2.Gs2Formation.Model.Slot[] Slots { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SetPropertyFormRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SetPropertyFormRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public SetPropertyFormRequest WithPropertyFormModelName(string propertyFormModelName) {
            this.PropertyFormModelName = propertyFormModelName;
            return this;
        }
        public SetPropertyFormRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public SetPropertyFormRequest WithSlots(Gs2.Gs2Formation.Model.Slot[] slots) {
            this.Slots = slots;
            return this;
        }

        public SetPropertyFormRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetPropertyFormRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetPropertyFormRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithPropertyFormModelName(!data.Keys.Contains("propertyFormModelName") || data["propertyFormModelName"] == null ? null : data["propertyFormModelName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithSlots(!data.Keys.Contains("slots") || data["slots"] == null || !data["slots"].IsArray ? null : data["slots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Formation.Model.Slot.FromJson(v);
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
                ["accessToken"] = AccessToken,
                ["propertyFormModelName"] = PropertyFormModelName,
                ["propertyId"] = PropertyId,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (PropertyFormModelName != null) {
                writer.WritePropertyName("propertyFormModelName");
                writer.Write(PropertyFormModelName.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
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
            key += AccessToken + ":";
            key += PropertyFormModelName + ":";
            key += PropertyId + ":";
            key += Slots + ":";
            return key;
        }
    }
}