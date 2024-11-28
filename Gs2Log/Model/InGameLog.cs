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
	public class InGameLog : IComparable
	{
        public long? Timestamp { set; get; } = null!;
        public string RequestId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public Gs2.Gs2Log.Model.InGameLogTag[] Tags { set; get; } = null!;
        public string Payload { set; get; } = null!;
        public InGameLog WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }
        public InGameLog WithRequestId(string requestId) {
            this.RequestId = requestId;
            return this;
        }
        public InGameLog WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public InGameLog WithTags(Gs2.Gs2Log.Model.InGameLogTag[] tags) {
            this.Tags = tags;
            return this;
        }
        public InGameLog WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static InGameLog FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new InGameLog()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : (long?)(data["timestamp"].ToString().Contains(".") ? (long)double.Parse(data["timestamp"].ToString()) : long.Parse(data["timestamp"].ToString())))
                .WithRequestId(!data.Keys.Contains("requestId") || data["requestId"] == null ? null : data["requestId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTags(!data.Keys.Contains("tags") || data["tags"] == null || !data["tags"].IsArray ? null : data["tags"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.InGameLogTag.FromJson(v);
                }).ToArray())
                .WithPayload(!data.Keys.Contains("payload") || data["payload"] == null ? null : data["payload"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData tagsJsonData = null;
            if (Tags != null && Tags.Length > 0)
            {
                tagsJsonData = new JsonData();
                foreach (var tag in Tags)
                {
                    tagsJsonData.Add(tag.ToJson());
                }
            }
            return new JsonData {
                ["timestamp"] = Timestamp,
                ["requestId"] = RequestId,
                ["userId"] = UserId,
                ["tags"] = tagsJsonData,
                ["payload"] = Payload,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            if (RequestId != null) {
                writer.WritePropertyName("requestId");
                writer.Write(RequestId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Tags != null) {
                writer.WritePropertyName("tags");
                writer.WriteArrayStart();
                foreach (var tag in Tags)
                {
                    if (tag != null) {
                        tag.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Payload != null) {
                writer.WritePropertyName("payload");
                writer.Write(Payload.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as InGameLog;
            var diff = 0;
            if (Timestamp == null && Timestamp == other.Timestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(Timestamp - other.Timestamp);
            }
            if (RequestId == null && RequestId == other.RequestId)
            {
                // null and null
            }
            else
            {
                diff += RequestId.CompareTo(other.RequestId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Tags == null && Tags == other.Tags)
            {
                // null and null
            }
            else
            {
                diff += Tags.Length - other.Tags.Length;
                for (var i = 0; i < Tags.Length; i++)
                {
                    diff += Tags[i].CompareTo(other.Tags[i]);
                }
            }
            if (Payload == null && Payload == other.Payload)
            {
                // null and null
            }
            else
            {
                diff += Payload.CompareTo(other.Payload);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inGameLog", "log.inGameLog.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inGameLog", "log.inGameLog.timestamp.error.invalid"),
                    });
                }
            }
            {
                if (RequestId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inGameLog", "log.inGameLog.requestId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inGameLog", "log.inGameLog.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Tags.Length > 20) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inGameLog", "log.inGameLog.tags.error.tooMany"),
                    });
                }
            }
            {
                if (Payload.Length > 10485760) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inGameLog", "log.inGameLog.payload.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new InGameLog {
                Timestamp = Timestamp,
                RequestId = RequestId,
                UserId = UserId,
                Tags = Tags.Clone() as Gs2.Gs2Log.Model.InGameLogTag[],
                Payload = Payload,
            };
        }
    }
}