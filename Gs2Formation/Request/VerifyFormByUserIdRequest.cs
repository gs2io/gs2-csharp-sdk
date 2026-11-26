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
	public class VerifyFormByUserIdRequest : Gs2Request<VerifyFormByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string MoldModelName { set; get; } = null!;
         public int? Index { set; get; } = null!;
         public string SlotName { set; get; } = null!;
         public string PropertyIdRegex { set; get; } = null!;
         public string CountType { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public int? Count { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyFormByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyFormByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyFormByUserIdRequest WithMoldModelName(string moldModelName) {
            this.MoldModelName = moldModelName;
            return this;
        }
        public VerifyFormByUserIdRequest WithIndex(int? index) {
            this.Index = index;
            return this;
        }
        public VerifyFormByUserIdRequest WithSlotName(string slotName) {
            this.SlotName = slotName;
            return this;
        }
        public VerifyFormByUserIdRequest WithPropertyIdRegex(string propertyIdRegex) {
            this.PropertyIdRegex = propertyIdRegex;
            return this;
        }
        public VerifyFormByUserIdRequest WithCountType(string countType) {
            this.CountType = countType;
            return this;
        }
        public VerifyFormByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyFormByUserIdRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }
        public VerifyFormByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public VerifyFormByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyFormByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyFormByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMoldModelName(!data.Keys.Contains("moldModelName") || data["moldModelName"] == null ? null : data["moldModelName"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["index"].ToString()))
                .WithSlotName(!data.Keys.Contains("slotName") || data["slotName"] == null ? null : data["slotName"].ToString())
                .WithPropertyIdRegex(!data.Keys.Contains("propertyIdRegex") || data["propertyIdRegex"] == null ? null : data["propertyIdRegex"].ToString())
                .WithCountType(!data.Keys.Contains("countType") || data["countType"] == null ? null : data["countType"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["count"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["moldModelName"] = MoldModelName,
                ["index"] = Index,
                ["slotName"] = SlotName,
                ["propertyIdRegex"] = PropertyIdRegex,
                ["countType"] = CountType,
                ["verifyType"] = VerifyType,
                ["count"] = Count,
                ["timeOffsetToken"] = TimeOffsetToken,
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
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += MoldModelName + ":";
            key += Index + ":";
            key += SlotName + ":";
            key += PropertyIdRegex + ":";
            key += CountType + ":";
            key += VerifyType + ":";
            key += Count + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}