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

namespace Gs2.Gs2Guild.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class NotificationSetting : IComparable
	{
        public string GatewayNamespaceId { set; get; }
        public bool? EnableTransferMobileNotification { set; get; }
        public string Sound { set; get; }
        public Gs2.Gs2Guild.Model.MobileNotificationMessage[] MobileNotificationMessages { set; get; }
        public string Enable { set; get; }
        public NotificationSetting WithGatewayNamespaceId(string gatewayNamespaceId) {
            this.GatewayNamespaceId = gatewayNamespaceId;
            return this;
        }
        public NotificationSetting WithEnableTransferMobileNotification(bool? enableTransferMobileNotification) {
            this.EnableTransferMobileNotification = enableTransferMobileNotification;
            return this;
        }
        public NotificationSetting WithSound(string sound) {
            this.Sound = sound;
            return this;
        }
        public NotificationSetting WithMobileNotificationMessages(Gs2.Gs2Guild.Model.MobileNotificationMessage[] mobileNotificationMessages) {
            this.MobileNotificationMessages = mobileNotificationMessages;
            return this;
        }
        public NotificationSetting WithEnable(string enable) {
            this.Enable = enable;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static NotificationSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new NotificationSetting()
                .WithGatewayNamespaceId(!data.Keys.Contains("gatewayNamespaceId") || data["gatewayNamespaceId"] == null ? null : data["gatewayNamespaceId"].ToString())
                .WithEnableTransferMobileNotification(!data.Keys.Contains("enableTransferMobileNotification") || data["enableTransferMobileNotification"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableBool(data["enableTransferMobileNotification"].ToString()))
                .WithSound(!data.Keys.Contains("sound") || data["sound"] == null ? null : data["sound"].ToString())
                .WithMobileNotificationMessages(!data.Keys.Contains("mobileNotificationMessages") || data["mobileNotificationMessages"] == null || !data["mobileNotificationMessages"].IsArray ? null : data["mobileNotificationMessages"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.MobileNotificationMessage.FromJson(v);
                }).ToArray())
                .WithEnable(!data.Keys.Contains("enable") || data["enable"] == null ? null : data["enable"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData mobileNotificationMessagesJsonData = null;
            if (MobileNotificationMessages != null && MobileNotificationMessages.Length > 0)
            {
                mobileNotificationMessagesJsonData = new JsonData();
                foreach (var mobileNotificationMessage in MobileNotificationMessages)
                {
                    mobileNotificationMessagesJsonData.Add(mobileNotificationMessage.ToJson());
                }
            }
            return new JsonData {
                ["gatewayNamespaceId"] = GatewayNamespaceId,
                ["enableTransferMobileNotification"] = EnableTransferMobileNotification,
                ["sound"] = Sound,
                ["mobileNotificationMessages"] = mobileNotificationMessagesJsonData,
                ["enable"] = Enable,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GatewayNamespaceId != null) {
                writer.WritePropertyName("gatewayNamespaceId");
                writer.Write(GatewayNamespaceId.ToString());
            }
            if (EnableTransferMobileNotification != null) {
                writer.WritePropertyName("enableTransferMobileNotification");
                writer.Write(bool.Parse(EnableTransferMobileNotification.ToString()));
            }
            if (Sound != null) {
                writer.WritePropertyName("sound");
                writer.Write(Sound.ToString());
            }
            if (MobileNotificationMessages != null) {
                writer.WritePropertyName("mobileNotificationMessages");
                writer.WriteArrayStart();
                foreach (var mobileNotificationMessage in MobileNotificationMessages)
                {
                    if (mobileNotificationMessage != null) {
                        mobileNotificationMessage.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Enable != null) {
                writer.WritePropertyName("enable");
                writer.Write(Enable.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as NotificationSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type NotificationSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(GatewayNamespaceId, other.GatewayNamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableTransferMobileNotification, other.EnableTransferMobileNotification);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Sound, other.Sound);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(MobileNotificationMessages, other.MobileNotificationMessages);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Enable, other.Enable);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (GatewayNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("notificationSetting", "guild.notificationSetting.gatewayNamespaceId.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (Sound.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("notificationSetting", "guild.notificationSetting.sound.error.tooLong"),
                    });
                }
            }
            {
                if (MobileNotificationMessages.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("notificationSetting", "guild.notificationSetting.mobileNotificationMessages.error.tooMany"),
                    });
                }
            }
            {
                switch (Enable) {
                    case "Enabled":
                    case "Disabled":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("notificationSetting", "guild.notificationSetting.enable.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new NotificationSetting {
                GatewayNamespaceId = GatewayNamespaceId,
                EnableTransferMobileNotification = EnableTransferMobileNotification,
                Sound = Sound,
                MobileNotificationMessages = MobileNotificationMessages?.Clone() as Gs2.Gs2Guild.Model.MobileNotificationMessage[],
                Enable = Enable,
            };
        }
    }
}