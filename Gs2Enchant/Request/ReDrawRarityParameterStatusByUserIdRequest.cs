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
	public class ReDrawRarityParameterStatusByUserIdRequest : Gs2Request<ReDrawRarityParameterStatusByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string ParameterName { set; get; }
        public string PropertyId { set; get; }
        public string[] FixedParameterNames { set; get; }
        public string DuplicationAvoider { set; get; }

        public ReDrawRarityParameterStatusByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public ReDrawRarityParameterStatusByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public ReDrawRarityParameterStatusByUserIdRequest WithParameterName(string parameterName) {
            this.ParameterName = parameterName;
            return this;
        }

        public ReDrawRarityParameterStatusByUserIdRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }

        public ReDrawRarityParameterStatusByUserIdRequest WithFixedParameterNames(string[] fixedParameterNames) {
            this.FixedParameterNames = fixedParameterNames;
            return this;
        }

        public ReDrawRarityParameterStatusByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReDrawRarityParameterStatusByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReDrawRarityParameterStatusByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithParameterName(!data.Keys.Contains("parameterName") || data["parameterName"] == null ? null : data["parameterName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithFixedParameterNames(!data.Keys.Contains("fixedParameterNames") || data["fixedParameterNames"] == null || !data["fixedParameterNames"].IsArray ? new string[]{} : data["fixedParameterNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData fixedParameterNamesJsonData = null;
            if (FixedParameterNames != null && FixedParameterNames.Length > 0)
            {
                fixedParameterNamesJsonData = new JsonData();
                foreach (var fixedParameterName in FixedParameterNames)
                {
                    fixedParameterNamesJsonData.Add(fixedParameterName);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["parameterName"] = ParameterName,
                ["propertyId"] = PropertyId,
                ["fixedParameterNames"] = fixedParameterNamesJsonData,
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
            if (ParameterName != null) {
                writer.WritePropertyName("parameterName");
                writer.Write(ParameterName.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (FixedParameterNames != null) {
                writer.WritePropertyName("fixedParameterNames");
                writer.WriteArrayStart();
                foreach (var fixedParameterName in FixedParameterNames)
                {
                    writer.Write(fixedParameterName.ToString());
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += ParameterName + ":";
            key += PropertyId + ":";
            key += FixedParameterNames + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new ReDrawRarityParameterStatusByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                ParameterName = ParameterName,
                PropertyId = PropertyId,
                FixedParameterNames = FixedParameterNames,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ReDrawRarityParameterStatusByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values ReDrawRarityParameterStatusByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values ReDrawRarityParameterStatusByUserIdRequest::userId");
            }
            if (ParameterName != y.ParameterName) {
                throw new ArithmeticException("mismatch parameter values ReDrawRarityParameterStatusByUserIdRequest::parameterName");
            }
            if (PropertyId != y.PropertyId) {
                throw new ArithmeticException("mismatch parameter values ReDrawRarityParameterStatusByUserIdRequest::propertyId");
            }
            if (FixedParameterNames != y.FixedParameterNames) {
                throw new ArithmeticException("mismatch parameter values ReDrawRarityParameterStatusByUserIdRequest::fixedParameterNames");
            }
            return new ReDrawRarityParameterStatusByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                ParameterName = ParameterName,
                PropertyId = PropertyId,
                FixedParameterNames = FixedParameterNames,
            };
        }
    }
}