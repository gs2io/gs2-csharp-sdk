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
using Gs2.Gs2Script.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Script.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class InvokeScriptResult : IResult
	{
        public int? Code { set; get; }
        public string Result { set; get; }
        public string Transaction { set; get; }
        public int? ExecuteTime { set; get; }
        public int? Charged { set; get; }
        public string[] Output { set; get; }

        public InvokeScriptResult WithCode(int? code) {
            this.Code = code;
            return this;
        }

        public InvokeScriptResult WithResult(string result) {
            this.Result = result;
            return this;
        }

        public InvokeScriptResult WithTransaction(string transaction) {
            this.Transaction = transaction;
            return this;
        }

        public InvokeScriptResult WithExecuteTime(int? executeTime) {
            this.ExecuteTime = executeTime;
            return this;
        }

        public InvokeScriptResult WithCharged(int? charged) {
            this.Charged = charged;
            return this;
        }

        public InvokeScriptResult WithOutput(string[] output) {
            this.Output = output;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static InvokeScriptResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new InvokeScriptResult()
                .WithCode(!data.Keys.Contains("code") || data["code"] == null ? null : (int?)int.Parse(data["code"].ToString()))
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithTransaction(!data.Keys.Contains("transaction") || data["transaction"] == null ? null : data["transaction"].ToString())
                .WithExecuteTime(!data.Keys.Contains("executeTime") || data["executeTime"] == null ? null : (int?)int.Parse(data["executeTime"].ToString()))
                .WithCharged(!data.Keys.Contains("charged") || data["charged"] == null ? null : (int?)int.Parse(data["charged"].ToString()))
                .WithOutput(!data.Keys.Contains("output") || data["output"] == null ? new string[]{} : data["output"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData outputJsonData = null;
            if (Output != null)
            {
                outputJsonData = new JsonData();
                foreach (var outpu in Output)
                {
                    outputJsonData.Add(outpu);
                }
            }
            return new JsonData {
                ["code"] = Code,
                ["result"] = Result,
                ["transaction"] = Transaction,
                ["executeTime"] = ExecuteTime,
                ["charged"] = Charged,
                ["output"] = outputJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Code != null) {
                writer.WritePropertyName("code");
                writer.Write(int.Parse(Code.ToString()));
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            if (Transaction != null) {
                writer.WritePropertyName("transaction");
                writer.Write(Transaction.ToString());
            }
            if (ExecuteTime != null) {
                writer.WritePropertyName("executeTime");
                writer.Write(int.Parse(ExecuteTime.ToString()));
            }
            if (Charged != null) {
                writer.WritePropertyName("charged");
                writer.Write(int.Parse(Charged.ToString()));
            }
            writer.WriteArrayStart();
            foreach (var outpu in Output)
            {
                if (outpu != null) {
                    writer.Write(outpu.ToString());
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}