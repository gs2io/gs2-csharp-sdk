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

namespace Gs2.Gs2Chat.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class NotificationType : IComparable
	{
        public int? Category { set; get; } = null!;
        public bool? EnableTransferMobilePushNotification { set; get; } = null!;
        public NotificationType WithCategory(int? category) {
            this.Category = category;
            return this;
        }
        public NotificationType WithEnableTransferMobilePushNotification(bool? enableTransferMobilePushNotification) {
            this.EnableTransferMobilePushNotification = enableTransferMobilePushNotification;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static NotificationType FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new NotificationType()
                .WithCategory(!data.Keys.Contains("category") || data["category"] == null ? null : (int?)(data["category"].ToString().Contains(".") ? (int)double.Parse(data["category"].ToString()) : int.Parse(data["category"].ToString())))
                .WithEnableTransferMobilePushNotification(!data.Keys.Contains("enableTransferMobilePushNotification") || data["enableTransferMobilePushNotification"] == null ? null : (bool?)bool.Parse(data["enableTransferMobilePushNotification"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["category"] = Category,
                ["enableTransferMobilePushNotification"] = EnableTransferMobilePushNotification,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Category != null) {
                writer.WritePropertyName("category");
                writer.Write((Category.ToString().Contains(".") ? (int)double.Parse(Category.ToString()) : int.Parse(Category.ToString())));
            }
            if (EnableTransferMobilePushNotification != null) {
                writer.WritePropertyName("enableTransferMobilePushNotification");
                writer.Write(bool.Parse(EnableTransferMobilePushNotification.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as NotificationType;
            var diff = 0;
            if (Category == null && Category == other.Category)
            {
                // null and null
            }
            else
            {
                diff += (int)(Category - other.Category);
            }
            if (EnableTransferMobilePushNotification == null && EnableTransferMobilePushNotification == other.EnableTransferMobilePushNotification)
            {
                // null and null
            }
            else
            {
                diff += EnableTransferMobilePushNotification == other.EnableTransferMobilePushNotification ? 0 : 1;
            }
            return diff;
        }

        public void Validate() {
            {
                if (Category < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("notificationType", "chat.notificationType.category.error.invalid"),
                    });
                }
                if (Category > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("notificationType", "chat.notificationType.category.error.invalid"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new NotificationType {
                Category = Category,
                EnableTransferMobilePushNotification = EnableTransferMobilePushNotification,
            };
        }
    }
}