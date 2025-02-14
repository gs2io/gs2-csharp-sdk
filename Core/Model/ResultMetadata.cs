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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Core.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ResultMetadata : IComparable
	{
        public string Uncommitted { set; get; } = null!;
        public Gs2.Core.Model.ScriptTransactionResult[] ScriptTransactionResults { set; get; } = null!;
        public ResultMetadata WithUncommitted(string uncommitted) {
            this.Uncommitted = uncommitted;
            return this;
        }
        public ResultMetadata WithScriptTransactionResults(Gs2.Core.Model.ScriptTransactionResult[] scriptTransactionResults) {
            this.ScriptTransactionResults = scriptTransactionResults;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ResultMetadata FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ResultMetadata()
                .WithUncommitted(!data.Keys.Contains("uncommitted") || data["uncommitted"] == null ? null : data["uncommitted"].ToString())
                .WithScriptTransactionResults(!data.Keys.Contains("scriptTransactionResults") || data["scriptTransactionResults"] == null || !data["scriptTransactionResults"].IsArray ? new Gs2.Core.Model.ScriptTransactionResult[]{} : data["scriptTransactionResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ScriptTransactionResult.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData scriptTransactionResultsJsonData = null;
            if (ScriptTransactionResults != null && ScriptTransactionResults.Length > 0)
            {
                scriptTransactionResultsJsonData = new JsonData();
                foreach (var verifyResult in ScriptTransactionResults)
                {
                    scriptTransactionResultsJsonData.Add(verifyResult.ToJson());
                }
            }
            return new JsonData {
                ["uncommitted"] = Uncommitted,
                ["scriptTransactionResults"] = scriptTransactionResultsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Uncommitted != null) {
                writer.WritePropertyName("uncommitted");
                writer.Write(Uncommitted);
            }
            if (ScriptTransactionResults != null) {
                writer.WritePropertyName("scriptTransactionResults");
                writer.WriteArrayStart();
                foreach (var verifyResult in ScriptTransactionResults)
                {
                    if (verifyResult != null) {
                        verifyResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ResultMetadata;
            var diff = 0;
            if (Uncommitted == null && Uncommitted == other.Uncommitted)
            {
                // null and null
            }
            else
            {
                diff += Uncommitted.CompareTo(other.Uncommitted);
            }
            if (ScriptTransactionResults == null && ScriptTransactionResults == other.ScriptTransactionResults)
            {
                // null and null
            }
            else
            {
                diff += ScriptTransactionResults.Length - other.ScriptTransactionResults.Length;
                for (var i = 0; i < ScriptTransactionResults.Length; i++)
                {
                    diff += ScriptTransactionResults[i].CompareTo(other.ScriptTransactionResults[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
            }
        }

        public object Clone() {
            return new ResultMetadata {
                Uncommitted = Uncommitted.Clone() as string,
                ScriptTransactionResults = ScriptTransactionResults?.Clone() as Gs2.Core.Model.ScriptTransactionResult[],
            };
        }
    }
}