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
	public class AccessLogWithTelemetry : IComparable
	{
        public long? Timestamp { set; get; } = null!;
        public string SourceRequestId { set; get; } = null!;
        public string RequestId { set; get; } = null!;
        public long? Duration { set; get; } = null!;
        public string Service { set; get; } = null!;
        public string Method { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string Request { set; get; } = null!;
        public string Result { set; get; } = null!;
        public string Status { set; get; } = null!;
        public AccessLogWithTelemetry WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }
        public AccessLogWithTelemetry WithSourceRequestId(string sourceRequestId) {
            this.SourceRequestId = sourceRequestId;
            return this;
        }
        public AccessLogWithTelemetry WithRequestId(string requestId) {
            this.RequestId = requestId;
            return this;
        }
        public AccessLogWithTelemetry WithDuration(long? duration) {
            this.Duration = duration;
            return this;
        }
        public AccessLogWithTelemetry WithService(string service) {
            this.Service = service;
            return this;
        }
        public AccessLogWithTelemetry WithMethod(string method) {
            this.Method = method;
            return this;
        }
        public AccessLogWithTelemetry WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public AccessLogWithTelemetry WithRequest(string request) {
            this.Request = request;
            return this;
        }
        public AccessLogWithTelemetry WithResult(string result) {
            this.Result = result;
            return this;
        }
        public AccessLogWithTelemetry WithStatus(string status) {
            this.Status = status;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AccessLogWithTelemetry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AccessLogWithTelemetry()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : (long?)(data["timestamp"].ToString().Contains(".") ? (long)double.Parse(data["timestamp"].ToString()) : long.Parse(data["timestamp"].ToString())))
                .WithSourceRequestId(!data.Keys.Contains("sourceRequestId") || data["sourceRequestId"] == null ? null : data["sourceRequestId"].ToString())
                .WithRequestId(!data.Keys.Contains("requestId") || data["requestId"] == null ? null : data["requestId"].ToString())
                .WithDuration(!data.Keys.Contains("duration") || data["duration"] == null ? null : (long?)(data["duration"].ToString().Contains(".") ? (long)double.Parse(data["duration"].ToString()) : long.Parse(data["duration"].ToString())))
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRequest(!data.Keys.Contains("request") || data["request"] == null ? null : data["request"].ToString())
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["timestamp"] = Timestamp,
                ["sourceRequestId"] = SourceRequestId,
                ["requestId"] = RequestId,
                ["duration"] = Duration,
                ["service"] = Service,
                ["method"] = Method,
                ["userId"] = UserId,
                ["request"] = Request,
                ["result"] = Result,
                ["status"] = Status,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            if (SourceRequestId != null) {
                writer.WritePropertyName("sourceRequestId");
                writer.Write(SourceRequestId.ToString());
            }
            if (RequestId != null) {
                writer.WritePropertyName("requestId");
                writer.Write(RequestId.ToString());
            }
            if (Duration != null) {
                writer.WritePropertyName("duration");
                writer.Write((Duration.ToString().Contains(".") ? (long)double.Parse(Duration.ToString()) : long.Parse(Duration.ToString())));
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
            if (Request != null) {
                writer.WritePropertyName("request");
                writer.Write(Request.ToString());
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AccessLogWithTelemetry;
            var diff = 0;
            if (Timestamp == null && Timestamp == other.Timestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(Timestamp - other.Timestamp);
            }
            if (SourceRequestId == null && SourceRequestId == other.SourceRequestId)
            {
                // null and null
            }
            else
            {
                diff += SourceRequestId.CompareTo(other.SourceRequestId);
            }
            if (RequestId == null && RequestId == other.RequestId)
            {
                // null and null
            }
            else
            {
                diff += RequestId.CompareTo(other.RequestId);
            }
            if (Duration == null && Duration == other.Duration)
            {
                // null and null
            }
            else
            {
                diff += (int)(Duration - other.Duration);
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
            if (Request == null && Request == other.Request)
            {
                // null and null
            }
            else
            {
                diff += Request.CompareTo(other.Request);
            }
            if (Result == null && Result == other.Result)
            {
                // null and null
            }
            else
            {
                diff += Result.CompareTo(other.Result);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.timestamp.error.invalid"),
                    });
                }
            }
            {
                if (SourceRequestId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.sourceRequestId.error.tooLong"),
                    });
                }
            }
            {
                if (RequestId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.requestId.error.tooLong"),
                    });
                }
            }
            {
                if (Duration < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.duration.error.invalid"),
                    });
                }
                if (Duration > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.duration.error.invalid"),
                    });
                }
            }
            {
                if (Service.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.service.error.tooLong"),
                    });
                }
            }
            {
                if (Method.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.method.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Request.Length > 10485760) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.request.error.tooLong"),
                    });
                }
            }
            {
                if (Result.Length > 10485760) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.result.error.tooLong"),
                    });
                }
            }
            {
                switch (Status) {
                    case "ok":
                    case "error":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("accessLogWithTelemetry", "log.accessLogWithTelemetry.status.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new AccessLogWithTelemetry {
                Timestamp = Timestamp,
                SourceRequestId = SourceRequestId,
                RequestId = RequestId,
                Duration = Duration,
                Service = Service,
                Method = Method,
                UserId = UserId,
                Request = Request,
                Result = Result,
                Status = Status,
            };
        }
    }
}