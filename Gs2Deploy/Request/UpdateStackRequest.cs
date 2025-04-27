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
	public class UpdateStackRequest : Gs2Request<UpdateStackRequest>
	{
         public string StackName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Mode { set; get; } = null!;
         public string Template { set; get; } = null!;
         public string UploadToken { set; get; } = null!;
        public UpdateStackRequest WithStackName(string stackName) {
            this.StackName = stackName;
            return this;
        }
        public UpdateStackRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateStackRequest WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public UpdateStackRequest WithTemplate(string template) {
            this.Template = template;
            return this;
        }
        public UpdateStackRequest WithUploadToken(string uploadToken) {
            this.UploadToken = uploadToken;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateStackRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateStackRequest()
                .WithStackName(!data.Keys.Contains("stackName") || data["stackName"] == null ? null : data["stackName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithTemplate(!data.Keys.Contains("template") || data["template"] == null ? null : data["template"].ToString())
                .WithUploadToken(!data.Keys.Contains("uploadToken") || data["uploadToken"] == null ? null : data["uploadToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["stackName"] = StackName,
                ["description"] = Description,
                ["mode"] = Mode,
                ["template"] = Template,
                ["uploadToken"] = UploadToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StackName != null) {
                writer.WritePropertyName("stackName");
                writer.Write(StackName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Mode != null) {
                writer.WritePropertyName("mode");
                writer.Write(Mode.ToString());
            }
            if (Template != null) {
                writer.WritePropertyName("template");
                writer.Write(Template.ToString());
            }
            if (UploadToken != null) {
                writer.WritePropertyName("uploadToken");
                writer.Write(UploadToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += StackName + ":";
            key += Description + ":";
            key += Mode + ":";
            key += Template + ":";
            key += UploadToken + ":";
            return key;
        }
    }
}