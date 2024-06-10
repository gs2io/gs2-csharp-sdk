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
	public class UpdateSecurityPolicyRequest : Gs2Request<UpdateSecurityPolicyRequest>
	{
         public string SecurityPolicyName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Policy { set; get; } = null!;
        public UpdateSecurityPolicyRequest WithSecurityPolicyName(string securityPolicyName) {
            this.SecurityPolicyName = securityPolicyName;
            return this;
        }
        public UpdateSecurityPolicyRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateSecurityPolicyRequest WithPolicy(string policy) {
            this.Policy = policy;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateSecurityPolicyRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateSecurityPolicyRequest()
                .WithSecurityPolicyName(!data.Keys.Contains("securityPolicyName") || data["securityPolicyName"] == null ? null : data["securityPolicyName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithPolicy(!data.Keys.Contains("policy") || data["policy"] == null ? null : data["policy"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["securityPolicyName"] = SecurityPolicyName,
                ["description"] = Description,
                ["policy"] = Policy,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SecurityPolicyName != null) {
                writer.WritePropertyName("securityPolicyName");
                writer.Write(SecurityPolicyName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Policy != null) {
                writer.WritePropertyName("policy");
                writer.Write(Policy.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += SecurityPolicyName + ":";
            key += Description + ":";
            key += Policy + ":";
            return key;
        }
    }
}