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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class GooglePlayRealtimeNotificationMessage : IComparable
	{
        public string Data { set; get; } = null!;
        public string MessageId { set; get; } = null!;
        public string PublishTime { set; get; } = null!;
        public GooglePlayRealtimeNotificationMessage WithData(string data) {
            this.Data = data;
            return this;
        }
        public GooglePlayRealtimeNotificationMessage WithMessageId(string messageId) {
            this.MessageId = messageId;
            return this;
        }
        public GooglePlayRealtimeNotificationMessage WithPublishTime(string publishTime) {
            this.PublishTime = publishTime;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GooglePlayRealtimeNotificationMessage FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GooglePlayRealtimeNotificationMessage()
                .WithData(!data.Keys.Contains("data") || data["data"] == null ? null : data["data"].ToString())
                .WithMessageId(!data.Keys.Contains("messageId") || data["messageId"] == null ? null : data["messageId"].ToString())
                .WithPublishTime(!data.Keys.Contains("publishTime") || data["publishTime"] == null ? null : data["publishTime"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["data"] = Data,
                ["messageId"] = MessageId,
                ["publishTime"] = PublishTime,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Data != null) {
                writer.WritePropertyName("data");
                writer.Write(Data.ToString());
            }
            if (MessageId != null) {
                writer.WritePropertyName("messageId");
                writer.Write(MessageId.ToString());
            }
            if (PublishTime != null) {
                writer.WritePropertyName("publishTime");
                writer.Write(PublishTime.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GooglePlayRealtimeNotificationMessage;
            var diff = 0;
            if (Data == null && Data == other.Data)
            {
                // null and null
            }
            else
            {
                diff += Data.CompareTo(other.Data);
            }
            if (MessageId == null && MessageId == other.MessageId)
            {
                // null and null
            }
            else
            {
                diff += MessageId.CompareTo(other.MessageId);
            }
            if (PublishTime == null && PublishTime == other.PublishTime)
            {
                // null and null
            }
            else
            {
                diff += PublishTime.CompareTo(other.PublishTime);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Data.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("googlePlayRealtimeNotificationMessage", "money2.googlePlayRealtimeNotificationMessage.data.error.tooLong"),
                    });
                }
            }
            {
                if (MessageId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("googlePlayRealtimeNotificationMessage", "money2.googlePlayRealtimeNotificationMessage.messageId.error.tooLong"),
                    });
                }
            }
            {
                if (PublishTime.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("googlePlayRealtimeNotificationMessage", "money2.googlePlayRealtimeNotificationMessage.publishTime.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GooglePlayRealtimeNotificationMessage {
                Data = Data,
                MessageId = MessageId,
                PublishTime = PublishTime,
            };
        }
    }
}