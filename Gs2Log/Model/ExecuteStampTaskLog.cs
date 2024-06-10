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

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ExecuteStampTaskLog : IComparable
	{
        public long? Timestamp { set; get; } = null!;
        public string TaskId { set; get; } = null!;
        public string Service { set; get; } = null!;
        public string Method { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string Action { set; get; } = null!;
        public string Args { set; get; } = null!;
        public ExecuteStampTaskLog WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }
        public ExecuteStampTaskLog WithTaskId(string taskId) {
            this.TaskId = taskId;
            return this;
        }
        public ExecuteStampTaskLog WithService(string service) {
            this.Service = service;
            return this;
        }
        public ExecuteStampTaskLog WithMethod(string method) {
            this.Method = method;
            return this;
        }
        public ExecuteStampTaskLog WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ExecuteStampTaskLog WithAction(string action) {
            this.Action = action;
            return this;
        }
        public ExecuteStampTaskLog WithArgs(string args) {
            this.Args = args;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ExecuteStampTaskLog FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ExecuteStampTaskLog()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : (long?)(data["timestamp"].ToString().Contains(".") ? (long)double.Parse(data["timestamp"].ToString()) : long.Parse(data["timestamp"].ToString())))
                .WithTaskId(!data.Keys.Contains("taskId") || data["taskId"] == null ? null : data["taskId"].ToString())
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["timestamp"] = Timestamp,
                ["taskId"] = TaskId,
                ["service"] = Service,
                ["method"] = Method,
                ["userId"] = UserId,
                ["action"] = Action,
                ["args"] = Args,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            if (TaskId != null) {
                writer.WritePropertyName("taskId");
                writer.Write(TaskId.ToString());
            }
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            if (Method != null) {
                writer.WritePropertyName("method");
                writer.Write(Method.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ExecuteStampTaskLog;
            var diff = 0;
            if (Timestamp == null && Timestamp == other.Timestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(Timestamp - other.Timestamp);
            }
            if (TaskId == null && TaskId == other.TaskId)
            {
                // null and null
            }
            else
            {
                diff += TaskId.CompareTo(other.TaskId);
            }
            if (Service == null && Service == other.Service)
            {
                // null and null
            }
            else
            {
                diff += Service.CompareTo(other.Service);
            }
            if (Method == null && Method == other.Method)
            {
                // null and null
            }
            else
            {
                diff += Method.CompareTo(other.Method);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Action == null && Action == other.Action)
            {
                // null and null
            }
            else
            {
                diff += Action.CompareTo(other.Action);
            }
            if (Args == null && Args == other.Args)
            {
                // null and null
            }
            else
            {
                diff += Args.CompareTo(other.Args);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.timestamp.error.invalid"),
                    });
                }
            }
            {
                if (TaskId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.taskId.error.tooLong"),
                    });
                }
            }
            {
                if (Service.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.service.error.tooLong"),
                    });
                }
            }
            {
                if (Method.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.method.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Action.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.action.error.tooLong"),
                    });
                }
            }
            {
                if (Args.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampTaskLog", "log.executeStampTaskLog.args.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ExecuteStampTaskLog {
                Timestamp = Timestamp,
                TaskId = TaskId,
                Service = Service,
                Method = Method,
                UserId = UserId,
                Action = Action,
                Args = Args,
            };
        }
    }
}