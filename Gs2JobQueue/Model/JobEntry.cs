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

namespace Gs2.Gs2JobQueue.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class JobEntry : IComparable
	{
        public string ScriptId { set; get; } = null!;
        public string Args { set; get; } = null!;
        public int? MaxTryCount { set; get; } = null!;
        public JobEntry WithScriptId(string scriptId) {
            this.ScriptId = scriptId;
            return this;
        }
        public JobEntry WithArgs(string args) {
            this.Args = args;
            return this;
        }
        public JobEntry WithMaxTryCount(int? maxTryCount) {
            this.MaxTryCount = maxTryCount;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static JobEntry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new JobEntry()
                .WithScriptId(!data.Keys.Contains("scriptId") || data["scriptId"] == null ? null : data["scriptId"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithMaxTryCount(!data.Keys.Contains("maxTryCount") || data["maxTryCount"] == null ? null : (int?)(data["maxTryCount"].ToString().Contains(".") ? (int)double.Parse(data["maxTryCount"].ToString()) : int.Parse(data["maxTryCount"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["scriptId"] = ScriptId,
                ["args"] = Args,
                ["maxTryCount"] = MaxTryCount,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ScriptId != null) {
                writer.WritePropertyName("scriptId");
                writer.Write(ScriptId.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            if (MaxTryCount != null) {
                writer.WritePropertyName("maxTryCount");
                writer.Write((MaxTryCount.ToString().Contains(".") ? (int)double.Parse(MaxTryCount.ToString()) : int.Parse(MaxTryCount.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as JobEntry;
            var diff = 0;
            if (ScriptId == null && ScriptId == other.ScriptId)
            {
                // null and null
            }
            else
            {
                diff += ScriptId.CompareTo(other.ScriptId);
            }
            if (Args == null && Args == other.Args)
            {
                // null and null
            }
            else
            {
                diff += Args.CompareTo(other.Args);
            }
            if (MaxTryCount == null && MaxTryCount == other.MaxTryCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxTryCount - other.MaxTryCount);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobEntry", "jobQueue.jobEntry.scriptId.error.tooLong"),
                    });
                }
            }
            {
                if (Args.Length > 131072) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobEntry", "jobQueue.jobEntry.args.error.tooLong"),
                    });
                }
            }
            {
                if (MaxTryCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobEntry", "jobQueue.jobEntry.maxTryCount.error.invalid"),
                    });
                }
                if (MaxTryCount > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobEntry", "jobQueue.jobEntry.maxTryCount.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new JobEntry {
                ScriptId = ScriptId,
                Args = Args,
                MaxTryCount = MaxTryCount,
            };
        }
    }
}