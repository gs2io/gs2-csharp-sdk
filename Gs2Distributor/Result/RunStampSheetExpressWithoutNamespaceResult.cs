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
	public class RunStampSheetExpressWithoutNamespaceResult : IResult
	{
        public int[] VerifyTaskResultCodes { set; get; } = null!;
        public string[] VerifyTaskResults { set; get; } = null!;
        public int[] TaskResultCodes { set; get; } = null!;
        public string[] TaskResults { set; get; } = null!;
        public int? SheetResultCode { set; get; } = null!;
        public string SheetResult { set; get; } = null!;

        public RunStampSheetExpressWithoutNamespaceResult WithVerifyTaskResultCodes(int[] verifyTaskResultCodes) {
            this.VerifyTaskResultCodes = verifyTaskResultCodes;
            return this;
        }

        public RunStampSheetExpressWithoutNamespaceResult WithVerifyTaskResults(string[] verifyTaskResults) {
            this.VerifyTaskResults = verifyTaskResults;
            return this;
        }

        public RunStampSheetExpressWithoutNamespaceResult WithTaskResultCodes(int[] taskResultCodes) {
            this.TaskResultCodes = taskResultCodes;
            return this;
        }

        public RunStampSheetExpressWithoutNamespaceResult WithTaskResults(string[] taskResults) {
            this.TaskResults = taskResults;
            return this;
        }

        public RunStampSheetExpressWithoutNamespaceResult WithSheetResultCode(int? sheetResultCode) {
            this.SheetResultCode = sheetResultCode;
            return this;
        }

        public RunStampSheetExpressWithoutNamespaceResult WithSheetResult(string sheetResult) {
            this.SheetResult = sheetResult;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RunStampSheetExpressWithoutNamespaceResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunStampSheetExpressWithoutNamespaceResult()
                .WithVerifyTaskResultCodes(!data.Keys.Contains("verifyTaskResultCodes") || data["verifyTaskResultCodes"] == null || !data["verifyTaskResultCodes"].IsArray ? null : data["verifyTaskResultCodes"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithVerifyTaskResults(!data.Keys.Contains("verifyTaskResults") || data["verifyTaskResults"] == null || !data["verifyTaskResults"].IsArray ? null : data["verifyTaskResults"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithTaskResultCodes(!data.Keys.Contains("taskResultCodes") || data["taskResultCodes"] == null || !data["taskResultCodes"].IsArray ? null : data["taskResultCodes"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithTaskResults(!data.Keys.Contains("taskResults") || data["taskResults"] == null || !data["taskResults"].IsArray ? null : data["taskResults"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSheetResultCode(!data.Keys.Contains("sheetResultCode") || data["sheetResultCode"] == null ? null : (int?)(data["sheetResultCode"].ToString().Contains(".") ? (int)double.Parse(data["sheetResultCode"].ToString()) : int.Parse(data["sheetResultCode"].ToString())))
                .WithSheetResult(!data.Keys.Contains("sheetResult") || data["sheetResult"] == null ? null : data["sheetResult"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData verifyTaskResultCodesJsonData = null;
            if (VerifyTaskResultCodes != null && VerifyTaskResultCodes.Length > 0)
            {
                verifyTaskResultCodesJsonData = new JsonData();
                foreach (var verifyTaskResultCode in VerifyTaskResultCodes)
                {
                    verifyTaskResultCodesJsonData.Add(verifyTaskResultCode);
                }
            }
            JsonData verifyTaskResultsJsonData = null;
            if (VerifyTaskResults != null && VerifyTaskResults.Length > 0)
            {
                verifyTaskResultsJsonData = new JsonData();
                foreach (var verifyTaskResult in VerifyTaskResults)
                {
                    verifyTaskResultsJsonData.Add(verifyTaskResult);
                }
            }
            JsonData taskResultCodesJsonData = null;
            if (TaskResultCodes != null && TaskResultCodes.Length > 0)
            {
                taskResultCodesJsonData = new JsonData();
                foreach (var taskResultCode in TaskResultCodes)
                {
                    taskResultCodesJsonData.Add(taskResultCode);
                }
            }
            JsonData taskResultsJsonData = null;
            if (TaskResults != null && TaskResults.Length > 0)
            {
                taskResultsJsonData = new JsonData();
                foreach (var taskResult in TaskResults)
                {
                    taskResultsJsonData.Add(taskResult);
                }
            }
            return new JsonData {
                ["verifyTaskResultCodes"] = verifyTaskResultCodesJsonData,
                ["verifyTaskResults"] = verifyTaskResultsJsonData,
                ["taskResultCodes"] = taskResultCodesJsonData,
                ["taskResults"] = taskResultsJsonData,
                ["sheetResultCode"] = SheetResultCode,
                ["sheetResult"] = SheetResult,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (VerifyTaskResultCodes != null) {
                writer.WritePropertyName("verifyTaskResultCodes");
                writer.WriteArrayStart();
                foreach (var verifyTaskResultCode in VerifyTaskResultCodes)
                {
                    writer.Write((verifyTaskResultCode.ToString().Contains(".") ? (int)double.Parse(verifyTaskResultCode.ToString()) : int.Parse(verifyTaskResultCode.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (VerifyTaskResults != null) {
                writer.WritePropertyName("verifyTaskResults");
                writer.WriteArrayStart();
                foreach (var verifyTaskResult in VerifyTaskResults)
                {
                    if (verifyTaskResult != null) {
                        writer.Write(verifyTaskResult.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (TaskResultCodes != null) {
                writer.WritePropertyName("taskResultCodes");
                writer.WriteArrayStart();
                foreach (var taskResultCode in TaskResultCodes)
                {
                    writer.Write((taskResultCode.ToString().Contains(".") ? (int)double.Parse(taskResultCode.ToString()) : int.Parse(taskResultCode.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (TaskResults != null) {
                writer.WritePropertyName("taskResults");
                writer.WriteArrayStart();
                foreach (var taskResult in TaskResults)
                {
                    if (taskResult != null) {
                        writer.Write(taskResult.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (SheetResultCode != null) {
                writer.WritePropertyName("sheetResultCode");
                writer.Write((SheetResultCode.ToString().Contains(".") ? (int)double.Parse(SheetResultCode.ToString()) : int.Parse(SheetResultCode.ToString())));
            }
            if (SheetResult != null) {
                writer.WritePropertyName("sheetResult");
                writer.Write(SheetResult.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}