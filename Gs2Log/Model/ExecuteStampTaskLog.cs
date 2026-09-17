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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class ExecuteStampTaskLog : IComparable
	{
        public long? Timestamp { set; get; }
        public string TaskId { set; get; }
        public string Service { set; get; }
        public string Method { set; get; }
        public string UserId { set; get; }
        public string Action { set; get; }
        public string Args { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new ExecuteStampTaskLog()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["timestamp"].ToString()))
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ExecuteStampTaskLog.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Timestamp, other.Timestamp);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TaskId, other.TaskId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Service, other.Service);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Method, other.Method);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Action, other.Action);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Args, other.Args);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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