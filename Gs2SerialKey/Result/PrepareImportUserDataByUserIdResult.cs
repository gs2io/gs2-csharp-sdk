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
using Gs2.Gs2SerialKey.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SerialKey.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PrepareImportUserDataByUserIdResult : IResult
	{
        public string UploadToken { set; get; }
        public string UploadUrl { set; get; }
        public ResultMetadata Metadata { set; get; }

        public PrepareImportUserDataByUserIdResult WithUploadToken(string uploadToken) {
            this.UploadToken = uploadToken;
            return this;
        }

        public PrepareImportUserDataByUserIdResult WithUploadUrl(string uploadUrl) {
            this.UploadUrl = uploadUrl;
            return this;
        }

        public PrepareImportUserDataByUserIdResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PrepareImportUserDataByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrepareImportUserDataByUserIdResult()
                .WithUploadToken(!data.Keys.Contains("uploadToken") || data["uploadToken"] == null ? null : data["uploadToken"].ToString())
                .WithUploadUrl(!data.Keys.Contains("uploadUrl") || data["uploadUrl"] == null ? null : data["uploadUrl"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["uploadToken"] = UploadToken,
                ["uploadUrl"] = UploadUrl,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UploadToken != null) {
                writer.WritePropertyName("uploadToken");
                writer.Write(UploadToken.ToString());
            }
            if (UploadUrl != null) {
                writer.WritePropertyName("uploadUrl");
                writer.Write(UploadUrl.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}