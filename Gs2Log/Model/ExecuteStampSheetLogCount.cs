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
	public partial class ExecuteStampSheetLogCount : IComparable
	{
        public string Service { set; get; }
        public string Method { set; get; }
        public string UserId { set; get; }
        public string Action { set; get; }
        public long? Count { set; get; }
        public ExecuteStampSheetLogCount WithService(string service) {
            this.Service = service;
            return this;
        }
        public ExecuteStampSheetLogCount WithMethod(string method) {
            this.Method = method;
            return this;
        }
        public ExecuteStampSheetLogCount WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ExecuteStampSheetLogCount WithAction(string action) {
            this.Action = action;
            return this;
        }
        public ExecuteStampSheetLogCount WithCount(long? count) {
            this.Count = count;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ExecuteStampSheetLogCount FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new ExecuteStampSheetLogCount()
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["count"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["service"] = Service,
                ["method"] = Method,
                ["userId"] = UserId,
                ["action"] = Action,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (long)double.Parse(Count.ToString()) : long.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ExecuteStampSheetLogCount;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ExecuteStampSheetLogCount.", nameof(obj));
            }
            var diff = 0;
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
            diff = ModelComparer.Compare(Count, other.Count);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Service.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLogCount", "log.executeStampSheetLogCount.service.error.tooLong"),
                    });
                }
            }
            {
                if (Method.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLogCount", "log.executeStampSheetLogCount.method.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLogCount", "log.executeStampSheetLogCount.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Action.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLogCount", "log.executeStampSheetLogCount.action.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLogCount", "log.executeStampSheetLogCount.count.error.invalid"),
                    });
                }
                if (Count > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("executeStampSheetLogCount", "log.executeStampSheetLogCount.count.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ExecuteStampSheetLogCount {
                Service = Service,
                Method = Method,
                UserId = UserId,
                Action = Action,
                Count = Count,
            };
        }
    }
}