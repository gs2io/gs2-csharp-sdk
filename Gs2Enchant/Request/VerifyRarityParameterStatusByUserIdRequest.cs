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
using Gs2.Gs2Enchant.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Enchant.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VerifyRarityParameterStatusByUserIdRequest : Gs2Request<VerifyRarityParameterStatusByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ParameterName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public string ParameterValueName { set; get; } = null!;
         public int? ParameterCount { set; get; } = null!;
         public bool? MultiplyValueSpecifyingQuantity { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyRarityParameterStatusByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithParameterName(string parameterName) {
            this.ParameterName = parameterName;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithParameterValueName(string parameterValueName) {
            this.ParameterValueName = parameterValueName;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithParameterCount(int? parameterCount) {
            this.ParameterCount = parameterCount;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithMultiplyValueSpecifyingQuantity(bool? multiplyValueSpecifyingQuantity) {
            this.MultiplyValueSpecifyingQuantity = multiplyValueSpecifyingQuantity;
            return this;
        }
        public VerifyRarityParameterStatusByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public VerifyRarityParameterStatusByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyRarityParameterStatusByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyRarityParameterStatusByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithParameterName(!data.Keys.Contains("parameterName") || data["parameterName"] == null ? null : data["parameterName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithParameterValueName(!data.Keys.Contains("parameterValueName") || data["parameterValueName"] == null ? null : data["parameterValueName"].ToString())
                .WithParameterCount(!data.Keys.Contains("parameterCount") || data["parameterCount"] == null ? null : (int?)(data["parameterCount"].ToString().Contains(".") ? (int)double.Parse(data["parameterCount"].ToString()) : int.Parse(data["parameterCount"].ToString())))
                .WithMultiplyValueSpecifyingQuantity(!data.Keys.Contains("multiplyValueSpecifyingQuantity") || data["multiplyValueSpecifyingQuantity"] == null ? null : (bool?)bool.Parse(data["multiplyValueSpecifyingQuantity"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["parameterName"] = ParameterName,
                ["userId"] = UserId,
                ["propertyId"] = PropertyId,
                ["verifyType"] = VerifyType,
                ["parameterValueName"] = ParameterValueName,
                ["parameterCount"] = ParameterCount,
                ["multiplyValueSpecifyingQuantity"] = MultiplyValueSpecifyingQuantity,
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
            if (ParameterName != null) {
                writer.WritePropertyName("parameterName");
                writer.Write(ParameterName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (ParameterValueName != null) {
                writer.WritePropertyName("parameterValueName");
                writer.Write(ParameterValueName.ToString());
            }
            if (ParameterCount != null) {
                writer.WritePropertyName("parameterCount");
                writer.Write((ParameterCount.ToString().Contains(".") ? (int)double.Parse(ParameterCount.ToString()) : int.Parse(ParameterCount.ToString())));
            }
            if (MultiplyValueSpecifyingQuantity != null) {
                writer.WritePropertyName("multiplyValueSpecifyingQuantity");
                writer.Write(bool.Parse(MultiplyValueSpecifyingQuantity.ToString()));
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
            key += ParameterName + ":";
            key += UserId + ":";
            key += PropertyId + ":";
            key += VerifyType + ":";
            key += ParameterValueName + ":";
            key += ParameterCount + ":";
            key += MultiplyValueSpecifyingQuantity + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}