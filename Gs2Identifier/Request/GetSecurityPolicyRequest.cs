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

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Identifier.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetSecurityPolicyRequest : Gs2Request<GetSecurityPolicyRequest>
	{
         public string SecurityPolicyName { set; get; } = null!;
        public GetSecurityPolicyRequest WithSecurityPolicyName(string securityPolicyName) {
            this.SecurityPolicyName = securityPolicyName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetSecurityPolicyRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetSecurityPolicyRequest()
                .WithSecurityPolicyName(!data.Keys.Contains("securityPolicyName") || data["securityPolicyName"] == null ? null : data["securityPolicyName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["securityPolicyName"] = SecurityPolicyName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SecurityPolicyName != null) {
                writer.WritePropertyName("securityPolicyName");
                writer.Write(SecurityPolicyName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += SecurityPolicyName + ":";
            return key;
        }
    }
}