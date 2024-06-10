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
	public class ExecuteStampSheetLog : IComparable
	{
        public long? Timestamp { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public string Service { set; get; } = null!;
        public string Method { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string Action { set; get; } = null!;
        public string Args { set; get; } = null!;
        public ExecuteStampSheetLog WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }
        public ExecuteStampSheetLog WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public ExecuteStampSheetLog WithService(string service) {
            this.Service = service;
            return this;
        }
        public ExecuteStampSheetLog WithMethod(string method) {
            this.Method = method;
            return this;
        }
        public ExecuteStampSheetLog WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ExecuteStampSheetLog WithAction(string action) {
            this.Action = action;
            return this;
        }
        public ExecuteStampSheetLog WithArgs(string args) {
            this.Args = args;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ExecuteStampSheetLog FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ExecuteStampSheetLog()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : (long?)(data["timestamp"].ToString().Contains(".") ? (long)double.Parse(data["timestamp"].ToString()) : long.Parse(data["timestamp"].ToString())))
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
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
                ["transactionId"] = TransactionId,
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
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
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
            var other = obj as ExecuteStampSheetLog;
            var diff = 0;
            if (Timestamp == null && Timestamp == other.Timestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(Timestamp - other.Timestamp);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
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
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.timestamp.error.invalid"),
                    });
                }
            }
            {
                if (TransactionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (Service.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.service.error.tooLong"),
                    });
                }
            }
            {
                if (Method.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.method.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Action.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.action.error.tooLong"),
                    });
                }
            }
            {
                if (Args.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLog", "log.executeStampSheetLog.args.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ExecuteStampSheetLog {
                Timestamp = Timestamp,
                TransactionId = TransactionId,
                Service = Service,
                Method = Method,
                UserId = UserId,
                Action = Action,
                Args = Args,
            };
        }
    }
}