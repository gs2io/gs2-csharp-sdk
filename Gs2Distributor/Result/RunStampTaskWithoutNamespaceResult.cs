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
using Gs2.Gs2Distributor.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Distributor.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class RunStampTaskWithoutNamespaceResult : IResult
	{
        public string ContextStack { set; get; }
        public int? StatusCode { set; get; }
        public string Result { set; get; }
        public ResultMetadata Metadata { set; get; }

        public RunStampTaskWithoutNamespaceResult WithContextStack(string contextStack) {
            this.ContextStack = contextStack;
            return this;
        }

        public RunStampTaskWithoutNamespaceResult WithStatusCode(int? statusCode) {
            this.StatusCode = statusCode;
            return this;
        }

        public RunStampTaskWithoutNamespaceResult WithResult(string result) {
            this.Result = result;
            return this;
        }

        public RunStampTaskWithoutNamespaceResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RunStampTaskWithoutNamespaceResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunStampTaskWithoutNamespaceResult()
                .WithContextStack(!data.Keys.Contains("contextStack") || data["contextStack"] == null ? null : data["contextStack"].ToString())
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)(data["statusCode"].ToString().Contains(".") ? (int)double.Parse(data["statusCode"].ToString()) : int.Parse(data["statusCode"].ToString())))
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["contextStack"] = ContextStack,
                ["statusCode"] = StatusCode,
                ["result"] = Result,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ContextStack != null) {
                writer.WritePropertyName("contextStack");
                writer.Write(ContextStack.ToString());
            }
            if (StatusCode != null) {
                writer.WritePropertyName("statusCode");
                writer.Write((StatusCode.ToString().Contains(".") ? (int)double.Parse(StatusCode.ToString()) : int.Parse(StatusCode.ToString())));
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}