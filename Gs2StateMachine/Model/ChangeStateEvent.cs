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
	public class ChangeStateEvent : IComparable
	{
        public string TaskName { set; get; } = null!;
        public string Hash { set; get; } = null!;
        public long? Timestamp { set; get; } = null!;
        public ChangeStateEvent WithTaskName(string taskName) {
            this.TaskName = taskName;
            return this;
        }
        public ChangeStateEvent WithHash(string hash) {
            this.Hash = hash;
            return this;
        }
        public ChangeStateEvent WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ChangeStateEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ChangeStateEvent()
                .WithTaskName(!data.Keys.Contains("taskName") || data["taskName"] == null ? null : data["taskName"].ToString())
                .WithHash(!data.Keys.Contains("hash") || data["hash"] == null ? null : data["hash"].ToString())
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : (long?)(data["timestamp"].ToString().Contains(".") ? (long)double.Parse(data["timestamp"].ToString()) : long.Parse(data["timestamp"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["taskName"] = TaskName,
                ["hash"] = Hash,
                ["timestamp"] = Timestamp,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TaskName != null) {
                writer.WritePropertyName("taskName");
                writer.Write(TaskName.ToString());
            }
            if (Hash != null) {
                writer.WritePropertyName("hash");
                writer.Write(Hash.ToString());
            }
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ChangeStateEvent;
            var diff = 0;
            if (TaskName == null && TaskName == other.TaskName)
            {
                // null and null
            }
            else
            {
                diff += TaskName.CompareTo(other.TaskName);
            }
            if (Hash == null && Hash == other.Hash)
            {
                // null and null
            }
            else
            {
                diff += Hash.CompareTo(other.Hash);
            }
            if (Timestamp == null && Timestamp == other.Timestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(Timestamp - other.Timestamp);
            }
            return diff;
        }

        public void Validate() {
            {
                if (TaskName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("changeStateEvent", "stateMachine.changeStateEvent.taskName.error.tooLong"),
                    });
                }
            }
            {
                if (Hash.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("changeStateEvent", "stateMachine.changeStateEvent.hash.error.tooLong"),
                    });
                }
            }
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("changeStateEvent", "stateMachine.changeStateEvent.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("changeStateEvent", "stateMachine.changeStateEvent.timestamp.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ChangeStateEvent {
                TaskName = TaskName,
                Hash = Hash,
                Timestamp = Timestamp,
            };
        }
    }
}