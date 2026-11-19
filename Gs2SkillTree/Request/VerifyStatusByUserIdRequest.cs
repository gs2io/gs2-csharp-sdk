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
using Gs2.Gs2SkillTree.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SkillTree.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VerifyStatusByUserIdRequest : Gs2Request<VerifyStatusByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public string NodeModelName { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyStatusByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyStatusByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyStatusByUserIdRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public VerifyStatusByUserIdRequest WithNodeModelName(string nodeModelName) {
            this.NodeModelName = nodeModelName;
            return this;
        }
        public VerifyStatusByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyStatusByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public VerifyStatusByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyStatusByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyStatusByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithNodeModelName(!data.Keys.Contains("nodeModelName") || data["nodeModelName"] == null ? null : data["nodeModelName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["propertyId"] = PropertyId,
                ["nodeModelName"] = NodeModelName,
                ["verifyType"] = VerifyType,
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
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (NodeModelName != null) {
                writer.WritePropertyName("nodeModelName");
                writer.Write(NodeModelName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
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
            key += PropertyId + ":";
            key += NodeModelName + ":";
            key += VerifyType + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}