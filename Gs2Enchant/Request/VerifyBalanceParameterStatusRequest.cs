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
	public class VerifyBalanceParameterStatusRequest : Gs2Request<VerifyBalanceParameterStatusRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string ParameterName { set; get; } = null!;
         public string PropertyId { set; get; } = null!;
         public string ParameterValueName { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public long? ParameterValue { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyBalanceParameterStatusRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyBalanceParameterStatusRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyBalanceParameterStatusRequest WithParameterName(string parameterName) {
            this.ParameterName = parameterName;
            return this;
        }
        public VerifyBalanceParameterStatusRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public VerifyBalanceParameterStatusRequest WithParameterValueName(string parameterValueName) {
            this.ParameterValueName = parameterValueName;
            return this;
        }
        public VerifyBalanceParameterStatusRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyBalanceParameterStatusRequest WithParameterValue(long? parameterValue) {
            this.ParameterValue = parameterValue;
            return this;
        }

        public VerifyBalanceParameterStatusRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyBalanceParameterStatusRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyBalanceParameterStatusRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithParameterName(!data.Keys.Contains("parameterName") || data["parameterName"] == null ? null : data["parameterName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithParameterValueName(!data.Keys.Contains("parameterValueName") || data["parameterValueName"] == null ? null : data["parameterValueName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithParameterValue(!data.Keys.Contains("parameterValue") || data["parameterValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["parameterValue"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["parameterName"] = ParameterName,
                ["propertyId"] = PropertyId,
                ["parameterValueName"] = ParameterValueName,
                ["verifyType"] = VerifyType,
                ["parameterValue"] = ParameterValue,
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
            if (ParameterName != null) {
                writer.WritePropertyName("parameterName");
                writer.Write(ParameterName.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (ParameterValueName != null) {
                writer.WritePropertyName("parameterValueName");
                writer.Write(ParameterValueName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (ParameterValue != null) {
                writer.WritePropertyName("parameterValue");
                writer.Write((ParameterValue.ToString().Contains(".") ? (long)double.Parse(ParameterValue.ToString()) : long.Parse(ParameterValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += ParameterName + ":";
            key += PropertyId + ":";
            key += ParameterValueName + ":";
            key += VerifyType + ":";
            key += ParameterValue + ":";
            return key;
        }
    }
}