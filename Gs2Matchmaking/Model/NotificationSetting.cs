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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class NotificationSetting : IComparable
	{
        public string GatewayNamespaceId { set; get; } = null!;
        public bool? EnableTransferMobileNotification { set; get; } = null!;
        public string Sound { set; get; } = null!;
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static NotificationSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new NotificationSetting()
                .WithGatewayNamespaceId(!data.Keys.Contains("gatewayNamespaceId") || data["gatewayNamespaceId"] == null ? null : data["gatewayNamespaceId"].ToString())
                .WithEnableTransferMobileNotification(!data.Keys.Contains("enableTransferMobileNotification") || data["enableTransferMobileNotification"] == null ? null : (bool?)bool.Parse(data["enableTransferMobileNotification"].ToString()))
                .WithSound(!data.Keys.Contains("sound") || data["sound"] == null ? null : data["sound"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["gatewayNamespaceId"] = GatewayNamespaceId,
                ["enableTransferMobileNotification"] = EnableTransferMobileNotification,
                ["sound"] = Sound,
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as NotificationSetting;
            var diff = 0;
            if (GatewayNamespaceId == null && GatewayNamespaceId == other.GatewayNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += GatewayNamespaceId.CompareTo(other.GatewayNamespaceId);
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
                if (GatewayNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("notificationSetting", "matchmaking.notificationSetting.gatewayNamespaceId.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (Sound.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("notificationSetting", "matchmaking.notificationSetting.sound.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new NotificationSetting {
                GatewayNamespaceId = GatewayNamespaceId,
                EnableTransferMobileNotification = EnableTransferMobileNotification,
                Sound = Sound,
            };
        }
    }
}