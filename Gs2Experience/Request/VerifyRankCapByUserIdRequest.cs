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
using Gs2.Gs2Experience.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Experience.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VerifyRankCapByUserIdRequest : Gs2Request<VerifyRankCapByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string ExperienceName { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public long? RankCapValue { set; get; } = null!;
         public bool? MultiplyValueSpecifyingQuantity { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyRankCapByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyRankCapByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyRankCapByUserIdRequest WithExperienceName(string experienceName) {
            this.ExperienceName = experienceName;
            return this;
        }
        public VerifyRankCapByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyRankCapByUserIdRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public VerifyRankCapByUserIdRequest WithRankCapValue(long? rankCapValue) {
            this.RankCapValue = rankCapValue;
            return this;
        }
        public VerifyRankCapByUserIdRequest WithMultiplyValueSpecifyingQuantity(bool? multiplyValueSpecifyingQuantity) {
            this.MultiplyValueSpecifyingQuantity = multiplyValueSpecifyingQuantity;
            return this;
        }
        public VerifyRankCapByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public VerifyRankCapByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyRankCapByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyRankCapByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithExperienceName(!data.Keys.Contains("experienceName") || data["experienceName"] == null ? null : data["experienceName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithRankCapValue(!data.Keys.Contains("rankCapValue") || data["rankCapValue"] == null ? null : (long?)(data["rankCapValue"].ToString().Contains(".") ? (long)double.Parse(data["rankCapValue"].ToString()) : long.Parse(data["rankCapValue"].ToString())))
                .WithMultiplyValueSpecifyingQuantity(!data.Keys.Contains("multiplyValueSpecifyingQuantity") || data["multiplyValueSpecifyingQuantity"] == null ? null : (bool?)bool.Parse(data["multiplyValueSpecifyingQuantity"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["experienceName"] = ExperienceName,
                ["verifyType"] = VerifyType,
                ["propertyId"] = PropertyId,
                ["rankCapValue"] = RankCapValue,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ExperienceName != null) {
                writer.WritePropertyName("experienceName");
                writer.Write(ExperienceName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (RankCapValue != null) {
                writer.WritePropertyName("rankCapValue");
                writer.Write((RankCapValue.ToString().Contains(".") ? (long)double.Parse(RankCapValue.ToString()) : long.Parse(RankCapValue.ToString())));
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
            key += UserId + ":";
            key += ExperienceName + ":";
            key += VerifyType + ":";
            key += PropertyId + ":";
            key += RankCapValue + ":";
            key += MultiplyValueSpecifyingQuantity + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}