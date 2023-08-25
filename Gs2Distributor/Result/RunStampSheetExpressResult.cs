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
	public class RunStampSheetExpressResult : IResult
	{
        public string[] TaskResults { set; get; }
        public string SheetResult { set; get; }

        public RunStampSheetExpressResult WithTaskResults(string[] taskResults) {
            this.TaskResults = taskResults;
            return this;
        }

        public RunStampSheetExpressResult WithSheetResult(string sheetResult) {
            this.SheetResult = sheetResult;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RunStampSheetExpressResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunStampSheetExpressResult()
                .WithTaskResults(!data.Keys.Contains("taskResults") || data["taskResults"] == null ? new string[]{} : data["taskResults"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSheetResult(!data.Keys.Contains("sheetResult") || data["sheetResult"] == null ? null : data["sheetResult"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData taskResultsJsonData = null;
            if (TaskResults != null)
            {
                taskResultsJsonData = new JsonData();
                foreach (var taskResult in TaskResults)
                {
                    taskResultsJsonData.Add(taskResult);
                }
            }
            return new JsonData {
                ["taskResults"] = taskResultsJsonData,
                ["sheetResult"] = SheetResult,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WriteArrayStart();
            foreach (var taskResult in TaskResults)
            {
                if (taskResult != null) {
                    writer.Write(taskResult.ToString());
                }
            }
            writer.WriteArrayEnd();
            if (SheetResult != null) {
                writer.WritePropertyName("sheetResult");
                writer.Write(SheetResult.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}