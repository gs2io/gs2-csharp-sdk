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
using Gs2.Gs2News.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2News.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PrepareUpdateCurrentNewsMasterResult : IResult
	{
        public string UploadToken { set; get; } = null!;
        public string TemplateUploadUrl { set; get; } = null!;

        public PrepareUpdateCurrentNewsMasterResult WithUploadToken(string uploadToken) {
            this.UploadToken = uploadToken;
            return this;
        }

        public PrepareUpdateCurrentNewsMasterResult WithTemplateUploadUrl(string templateUploadUrl) {
            this.TemplateUploadUrl = templateUploadUrl;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PrepareUpdateCurrentNewsMasterResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrepareUpdateCurrentNewsMasterResult()
                .WithUploadToken(!data.Keys.Contains("uploadToken") || data["uploadToken"] == null ? null : data["uploadToken"].ToString())
                .WithTemplateUploadUrl(!data.Keys.Contains("templateUploadUrl") || data["templateUploadUrl"] == null ? null : data["templateUploadUrl"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["uploadToken"] = UploadToken,
                ["templateUploadUrl"] = TemplateUploadUrl,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UploadToken != null) {
                writer.WritePropertyName("uploadToken");
                writer.Write(UploadToken.ToString());
            }
            if (TemplateUploadUrl != null) {
                writer.WritePropertyName("templateUploadUrl");
                writer.Write(TemplateUploadUrl.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}