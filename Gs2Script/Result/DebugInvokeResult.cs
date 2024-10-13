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
 * 
 * deny overwrite
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
	public class DebugInvokeResult : IResult
	{
        public int? Code { set; get; } = null!;
        public string Result { set; get; } = null!;
        public Gs2.Gs2Script.Model.RandomStatus RandomStatus { set; get; } = null!;
        public int? ExecuteTime { set; get; } = null!;
        public int? Charged { set; get; } = null!;
        public string[] Output { set; get; } = null!;

        public DebugInvokeResult WithCode(int? code) {
            this.Code = code;
            return this;
        }

        public DebugInvokeResult WithResult(string result) {
            this.Result = result;
            return this;
        }

        public DebugInvokeResult WithRandomStatus(Gs2.Gs2Script.Model.RandomStatus randomStatus) {
            this.RandomStatus = randomStatus;
            return this;
        }

        public DebugInvokeResult WithExecuteTime(int? executeTime) {
            this.ExecuteTime = executeTime;
            return this;
        }

        public DebugInvokeResult WithCharged(int? charged) {
            this.Charged = charged;
            return this;
        }

        public DebugInvokeResult WithOutput(string[] output) {
            this.Output = output;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DebugInvokeResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DebugInvokeResult()
                .WithCode(!data.Keys.Contains("code") || data["code"] == null ? null : (int?)(data["code"].ToString().Contains(".") ? (int)double.Parse(data["code"].ToString()) : int.Parse(data["code"].ToString())))
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithRandomStatus(!data.Keys.Contains("randomStatus") || data["randomStatus"] == null ? null : Gs2.Gs2Script.Model.RandomStatus.FromJson(data["randomStatus"]))
                .WithExecuteTime(!data.Keys.Contains("executeTime") || data["executeTime"] == null ? null : (int?)(data["executeTime"].ToString().Contains(".") ? (int)double.Parse(data["executeTime"].ToString()) : int.Parse(data["executeTime"].ToString())))
                .WithCharged(!data.Keys.Contains("charged") || data["charged"] == null ? null : (int?)(data["charged"].ToString().Contains(".") ? (int)double.Parse(data["charged"].ToString()) : int.Parse(data["charged"].ToString())))
                .WithOutput(!data.Keys.Contains("output") || data["output"] == null || !data["output"].IsArray ? new string[]{} : data["output"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData outputJsonData = null;
            if (Output != null && Output.Length > 0)
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
                ["randomStatus"] = RandomStatus?.ToJson(),
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
                writer.Write((Code.ToString().Contains(".") ? (int)double.Parse(Code.ToString()) : int.Parse(Code.ToString())));
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            if (RandomStatus != null) {
                RandomStatus.WriteJson(writer);
            }
            if (ExecuteTime != null) {
                writer.WritePropertyName("executeTime");
                writer.Write((ExecuteTime.ToString().Contains(".") ? (int)double.Parse(ExecuteTime.ToString()) : int.Parse(ExecuteTime.ToString())));
            }
            if (Charged != null) {
                writer.WritePropertyName("charged");
                writer.Write((Charged.ToString().Contains(".") ? (int)double.Parse(Charged.ToString()) : int.Parse(Charged.ToString())));
            }
            if (Output != null) {
                writer.WritePropertyName("output");
                writer.WriteArrayStart();
                foreach (var outpu in Output)
                {
                    if (outpu != null) {
                        writer.Write(outpu.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }
    }
}