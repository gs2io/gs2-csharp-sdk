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
	public class SetFormByUserIdRequest : Gs2Request<SetFormByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string MoldName { set; get; }
        public int? Index { set; get; }
        public Gs2.Gs2Formation.Model.Slot[] Slots { set; get; }
        public string DuplicationAvoider { set; get; }

        public SetFormByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public SetFormByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public SetFormByUserIdRequest WithMoldName(string moldName) {
            this.MoldName = moldName;
            return this;
        }

        public SetFormByUserIdRequest WithIndex(int? index) {
            this.Index = index;
            return this;
        }

        public SetFormByUserIdRequest WithSlots(Gs2.Gs2Formation.Model.Slot[] slots) {
            this.Slots = slots;
            return this;
        }

        public SetFormByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetFormByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetFormByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMoldName(!data.Keys.Contains("moldName") || data["moldName"] == null ? null : data["moldName"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : (int?)int.Parse(data["index"].ToString()))
                .WithSlots(!data.Keys.Contains("slots") || data["slots"] == null ? new Gs2.Gs2Formation.Model.Slot[]{} : data["slots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Formation.Model.Slot.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["moldName"] = MoldName,
                ["index"] = Index,
                ["slots"] = new JsonData(Slots == null ? new JsonData[]{} :
                        Slots.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (MoldName != null) {
                writer.WritePropertyName("moldName");
                writer.Write(MoldName.ToString());
            }
            if (Index != null) {
                writer.WritePropertyName("index");
                writer.Write(int.Parse(Index.ToString()));
            }
            writer.WriteArrayStart();
            foreach (var slot in Slots)
            {
                if (slot != null) {
                    slot.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}