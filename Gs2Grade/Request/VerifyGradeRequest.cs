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
using Gs2.Gs2Grade.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Grade.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VerifyGradeRequest : Gs2Request<VerifyGradeRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string GradeName { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public long? GradeValue { set; get; } = null!;
         public bool? MultiplyValueSpecifyingQuantity { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyGradeRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyGradeRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyGradeRequest WithGradeName(string gradeName) {
            this.GradeName = gradeName;
            return this;
        }
        public VerifyGradeRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyGradeRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public VerifyGradeRequest WithGradeValue(long? gradeValue) {
            this.GradeValue = gradeValue;
            return this;
        }
        public VerifyGradeRequest WithMultiplyValueSpecifyingQuantity(bool? multiplyValueSpecifyingQuantity) {
            this.MultiplyValueSpecifyingQuantity = multiplyValueSpecifyingQuantity;
            return this;
        }

        public VerifyGradeRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyGradeRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyGradeRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithGradeName(!data.Keys.Contains("gradeName") || data["gradeName"] == null ? null : data["gradeName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithGradeValue(!data.Keys.Contains("gradeValue") || data["gradeValue"] == null ? null : (long?)(data["gradeValue"].ToString().Contains(".") ? (long)double.Parse(data["gradeValue"].ToString()) : long.Parse(data["gradeValue"].ToString())))
                .WithMultiplyValueSpecifyingQuantity(!data.Keys.Contains("multiplyValueSpecifyingQuantity") || data["multiplyValueSpecifyingQuantity"] == null ? null : (bool?)bool.Parse(data["multiplyValueSpecifyingQuantity"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["gradeName"] = GradeName,
                ["verifyType"] = VerifyType,
                ["propertyId"] = PropertyId,
                ["gradeValue"] = GradeValue,
                ["multiplyValueSpecifyingQuantity"] = MultiplyValueSpecifyingQuantity,
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
            if (GradeName != null) {
                writer.WritePropertyName("gradeName");
                writer.Write(GradeName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (GradeValue != null) {
                writer.WritePropertyName("gradeValue");
                writer.Write((GradeValue.ToString().Contains(".") ? (long)double.Parse(GradeValue.ToString()) : long.Parse(GradeValue.ToString())));
            }
            if (MultiplyValueSpecifyingQuantity != null) {
                writer.WritePropertyName("multiplyValueSpecifyingQuantity");
                writer.Write(bool.Parse(MultiplyValueSpecifyingQuantity.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += GradeName + ":";
            key += VerifyType + ":";
            key += PropertyId + ":";
            key += GradeValue + ":";
            key += MultiplyValueSpecifyingQuantity + ":";
            return key;
        }
    }
}