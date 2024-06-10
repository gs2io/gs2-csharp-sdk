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
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class IssuePasswordRequest : Gs2Request<IssuePasswordRequest>
	{
         public string IssuePasswordToken { set; get; } = null!;
        public IssuePasswordRequest WithIssuePasswordToken(string issuePasswordToken) {
            this.IssuePasswordToken = issuePasswordToken;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IssuePasswordRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IssuePasswordRequest()
                .WithIssuePasswordToken(!data.Keys.Contains("issuePasswordToken") || data["issuePasswordToken"] == null ? null : data["issuePasswordToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["issuePasswordToken"] = IssuePasswordToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (IssuePasswordToken != null) {
                writer.WritePropertyName("issuePasswordToken");
                writer.Write(IssuePasswordToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += IssuePasswordToken + ":";
            return key;
        }
    }
}