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
	public class GetStatusByUserIdRequest : Gs2Request<GetStatusByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public string GradeName { set; get; }
         public string PropertyId { set; get; }
        public GetStatusByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetStatusByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public GetStatusByUserIdRequest WithGradeName(string gradeName) {
            this.GradeName = gradeName;
            return this;
        }
        public GetStatusByUserIdRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetStatusByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetStatusByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithGradeName(!data.Keys.Contains("gradeName") || data["gradeName"] == null ? null : data["gradeName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["gradeName"] = GradeName,
                ["propertyId"] = PropertyId,
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
            if (GradeName != null) {
                writer.WritePropertyName("gradeName");
                writer.Write(GradeName.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += GradeName + ":";
            key += PropertyId + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply GetStatusByUserIdRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (GetStatusByUserIdRequest)x;
            return this;
        }
    }
}