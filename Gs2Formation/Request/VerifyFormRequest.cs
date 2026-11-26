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
	public class VerifyFormRequest : Gs2Request<VerifyFormRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string MoldModelName { set; get; } = null!;
         public int? Index { set; get; } = null!;
         public string SlotName { set; get; } = null!;
         public string PropertyIdRegex { set; get; } = null!;
         public string CountType { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public int? Count { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyFormRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyFormRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyFormRequest WithMoldModelName(string moldModelName) {
            this.MoldModelName = moldModelName;
            return this;
        }
        public VerifyFormRequest WithIndex(int? index) {
            this.Index = index;
            return this;
        }
        public VerifyFormRequest WithSlotName(string slotName) {
            this.SlotName = slotName;
            return this;
        }
        public VerifyFormRequest WithPropertyIdRegex(string propertyIdRegex) {
            this.PropertyIdRegex = propertyIdRegex;
            return this;
        }
        public VerifyFormRequest WithCountType(string countType) {
            this.CountType = countType;
            return this;
        }
        public VerifyFormRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyFormRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public VerifyFormRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyFormRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyFormRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithMoldModelName(!data.Keys.Contains("moldModelName") || data["moldModelName"] == null ? null : data["moldModelName"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["index"].ToString()))
                .WithSlotName(!data.Keys.Contains("slotName") || data["slotName"] == null ? null : data["slotName"].ToString())
                .WithPropertyIdRegex(!data.Keys.Contains("propertyIdRegex") || data["propertyIdRegex"] == null ? null : data["propertyIdRegex"].ToString())
                .WithCountType(!data.Keys.Contains("countType") || data["countType"] == null ? null : data["countType"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["count"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["moldModelName"] = MoldModelName,
                ["index"] = Index,
                ["slotName"] = SlotName,
                ["propertyIdRegex"] = PropertyIdRegex,
                ["countType"] = CountType,
                ["verifyType"] = VerifyType,
                ["count"] = Count,
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
            if (MoldModelName != null) {
                writer.WritePropertyName("moldModelName");
                writer.Write(MoldModelName.ToString());
            }
            if (Index != null) {
                writer.WritePropertyName("index");
                writer.Write((Index.ToString().Contains(".") ? (int)double.Parse(Index.ToString()) : int.Parse(Index.ToString())));
            }
            if (SlotName != null) {
                writer.WritePropertyName("slotName");
                writer.Write(SlotName.ToString());
            }
            if (PropertyIdRegex != null) {
                writer.WritePropertyName("propertyIdRegex");
                writer.Write(PropertyIdRegex.ToString());
            }
            if (CountType != null) {
                writer.WritePropertyName("countType");
                writer.Write(CountType.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += MoldModelName + ":";
            key += Index + ":";
            key += SlotName + ":";
            key += PropertyIdRegex + ":";
            key += CountType + ":";
            key += VerifyType + ":";
            key += Count + ":";
            return key;
        }
    }
}