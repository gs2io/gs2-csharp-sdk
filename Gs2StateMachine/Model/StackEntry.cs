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

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class StackEntry : IComparable
	{
        public string StateMachineName { set; get; } = null!;
        public string TaskName { set; get; } = null!;
        public StackEntry WithStateMachineName(string stateMachineName) {
            this.StateMachineName = stateMachineName;
            return this;
        }
        public StackEntry WithTaskName(string taskName) {
            this.TaskName = taskName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StackEntry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StackEntry()
                .WithStateMachineName(!data.Keys.Contains("stateMachineName") || data["stateMachineName"] == null ? null : data["stateMachineName"].ToString())
                .WithTaskName(!data.Keys.Contains("taskName") || data["taskName"] == null ? null : data["taskName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["stateMachineName"] = StateMachineName,
                ["taskName"] = TaskName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StateMachineName != null) {
                writer.WritePropertyName("stateMachineName");
                writer.Write(StateMachineName.ToString());
            }
            if (TaskName != null) {
                writer.WritePropertyName("taskName");
                writer.Write(TaskName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as StackEntry;
            var diff = 0;
            if (StateMachineName == null && StateMachineName == other.StateMachineName)
            {
                // null and null
            }
            else
            {
                diff += StateMachineName.CompareTo(other.StateMachineName);
            }
            if (TaskName == null && TaskName == other.TaskName)
            {
                // null and null
            }
            else
            {
                diff += TaskName.CompareTo(other.TaskName);
            }
            return diff;
        }

        public void Validate() {
            {
                if (StateMachineName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stackEntry", "stateMachine.stackEntry.stateMachineName.error.tooLong"),
                    });
                }
            }
            {
                if (TaskName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stackEntry", "stateMachine.stackEntry.taskName.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new StackEntry {
                StateMachineName = StateMachineName,
                TaskName = TaskName,
            };
        }
    }
}