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
using UnityEngine.Scripting;

namespace Gs2.Gs2Distributor.Result
{
	[Preserve]
	[System.Serializable]
	public class RunStampTaskResult : IResult
	{
        public string ContextStack { set; get; }
        public string Result { set; get; }

        public RunStampTaskResult WithContextStack(string contextStack) {
            this.ContextStack = contextStack;
            return this;
        }

        public RunStampTaskResult WithResult(string result) {
            this.Result = result;
            return this;
        }

    	[Preserve]
        public static RunStampTaskResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunStampTaskResult()
                .WithContextStack(!data.Keys.Contains("contextStack") || data["contextStack"] == null ? null : data["contextStack"].ToString())
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["contextStack"] = ContextStack,
                ["result"] = Result,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ContextStack != null) {
                writer.WritePropertyName("contextStack");
                writer.Write(ContextStack.ToString());
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}