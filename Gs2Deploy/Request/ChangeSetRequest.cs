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
using Gs2.Gs2Deploy.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Deploy.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ChangeSetRequest : Gs2Request<ChangeSetRequest>
	{
         public string StackName { set; get; } = null!;
         public string Template { set; get; } = null!;
        public ChangeSetRequest WithStackName(string stackName) {
            this.StackName = stackName;
            return this;
        }
        public ChangeSetRequest WithTemplate(string template) {
            this.Template = template;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ChangeSetRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ChangeSetRequest()
                .WithStackName(!data.Keys.Contains("stackName") || data["stackName"] == null ? null : data["stackName"].ToString())
                .WithTemplate(!data.Keys.Contains("template") || data["template"] == null ? null : data["template"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["stackName"] = StackName,
                ["template"] = Template,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StackName != null) {
                writer.WritePropertyName("stackName");
                writer.Write(StackName.ToString());
            }
            if (Template != null) {
                writer.WritePropertyName("template");
                writer.Write(Template.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += StackName + ":";
            key += Template + ":";
            return key;
        }
    }
}