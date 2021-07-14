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
using Gs2.Gs2Identifier.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Identifier.Request
{
	[Preserve]
	[System.Serializable]
	public class DetachSecurityPolicyRequest : Gs2Request<DetachSecurityPolicyRequest>
	{
        public string UserName { set; get; }
        public string SecurityPolicyId { set; get; }

        public DetachSecurityPolicyRequest WithUserName(string userName) {
            this.UserName = userName;
            return this;
        }

        public DetachSecurityPolicyRequest WithSecurityPolicyId(string securityPolicyId) {
            this.SecurityPolicyId = securityPolicyId;
            return this;
        }

    	[Preserve]
        public static DetachSecurityPolicyRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DetachSecurityPolicyRequest()
                .WithUserName(!data.Keys.Contains("userName") || data["userName"] == null ? null : data["userName"].ToString())
                .WithSecurityPolicyId(!data.Keys.Contains("securityPolicyId") || data["securityPolicyId"] == null ? null : data["securityPolicyId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userName"] = UserName,
                ["securityPolicyId"] = SecurityPolicyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserName != null) {
                writer.WritePropertyName("userName");
                writer.Write(UserName.ToString());
            }
            if (SecurityPolicyId != null) {
                writer.WritePropertyName("securityPolicyId");
                writer.Write(SecurityPolicyId.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}