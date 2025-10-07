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

namespace Gs2.Gs2Gateway.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class SendNotificationEntry : IComparable
	{
        public string UserId { set; get; }
        public string Issuer { set; get; }
        public string Subject { set; get; }
        public string Payload { set; get; }
        public bool? EnableTransferMobileNotification { set; get; }
        public string Sound { set; get; }
        public SendNotificationEntry WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SendNotificationEntry WithIssuer(string issuer) {
            this.Issuer = issuer;
            return this;
        }
        public SendNotificationEntry WithSubject(string subject) {
            this.Subject = subject;
            return this;
        }
        public SendNotificationEntry WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }
        public SendNotificationEntry WithEnableTransferMobileNotification(bool? enableTransferMobileNotification) {
            this.EnableTransferMobileNotification = enableTransferMobileNotification;
            return this;
        }
        public SendNotificationEntry WithSound(string sound) {
            this.Sound = sound;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SendNotificationEntry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SendNotificationEntry()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithIssuer(!data.Keys.Contains("issuer") || data["issuer"] == null ? null : data["issuer"].ToString())
                .WithSubject(!data.Keys.Contains("subject") || data["subject"] == null ? null : data["subject"].ToString())
                .WithPayload(!data.Keys.Contains("payload") || data["payload"] == null ? null : data["payload"].ToString())
                .WithEnableTransferMobileNotification(!data.Keys.Contains("enableTransferMobileNotification") || data["enableTransferMobileNotification"] == null ? null : (bool?)bool.Parse(data["enableTransferMobileNotification"].ToString()))
                .WithSound(!data.Keys.Contains("sound") || data["sound"] == null ? null : data["sound"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["issuer"] = Issuer,
                ["subject"] = Subject,
                ["payload"] = Payload,
                ["enableTransferMobileNotification"] = EnableTransferMobileNotification,
                ["sound"] = Sound,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Issuer != null) {
                writer.WritePropertyName("issuer");
                writer.Write(Issuer.ToString());
            }
            if (Subject != null) {
                writer.WritePropertyName("subject");
                writer.Write(Subject.ToString());
            }
            if (Payload != null) {
                writer.WritePropertyName("payload");
                writer.Write(Payload.ToString());
            }
            if (EnableTransferMobileNotification != null) {
                writer.WritePropertyName("enableTransferMobileNotification");
                writer.Write(bool.Parse(EnableTransferMobileNotification.ToString()));
            }
            if (Sound != null) {
                writer.WritePropertyName("sound");
                writer.Write(Sound.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SendNotificationEntry;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Issuer == null && Issuer == other.Issuer)
            {
                // null and null
            }
            else
            {
                diff += Issuer.CompareTo(other.Issuer);
            }
            if (Subject == null && Subject == other.Subject)
            {
                // null and null
            }
            else
            {
                diff += Subject.CompareTo(other.Subject);
            }
            if (Payload == null && Payload == other.Payload)
            {
                // null and null
            }
            else
            {
                diff += Payload.CompareTo(other.Payload);
            }
            if (EnableTransferMobileNotification == null && EnableTransferMobileNotification == other.EnableTransferMobileNotification)
            {
                // null and null
            }
            else
            {
                diff += EnableTransferMobileNotification == other.EnableTransferMobileNotification ? 0 : 1;
            }
            if (Sound == null && Sound == other.Sound)
            {
                // null and null
            }
            else
            {
                diff += Sound.CompareTo(other.Sound);
            }
            return diff;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendNotificationEntry", "gateway.sendNotificationEntry.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Issuer.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendNotificationEntry", "gateway.sendNotificationEntry.issuer.error.tooLong"),
                    });
                }
            }
            {
                if (Subject.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendNotificationEntry", "gateway.sendNotificationEntry.subject.error.tooLong"),
                    });
                }
            }
            {
                if (Payload.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendNotificationEntry", "gateway.sendNotificationEntry.payload.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (Sound.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendNotificationEntry", "gateway.sendNotificationEntry.sound.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SendNotificationEntry {
                UserId = UserId,
                Issuer = Issuer,
                Subject = Subject,
                Payload = Payload,
                EnableTransferMobileNotification = EnableTransferMobileNotification,
                Sound = Sound,
            };
        }
    }
}