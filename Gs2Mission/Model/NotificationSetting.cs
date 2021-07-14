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
using UnityEngine.Scripting;

namespace Gs2.Gs2Mission.Model
{

	[Preserve]
	public class NotificationSetting : IComparable
	{
        public string GatewayNamespaceId { set; get; }
        public bool? EnableTransferMobileNotification { set; get; }
        public string Sound { set; get; }

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

    	[Preserve]
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
    }
}