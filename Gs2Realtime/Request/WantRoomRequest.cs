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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Realtime.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Realtime.Request
{
	[Preserve]
	[System.Serializable]
	public class WantRoomRequest : Gs2Request<WantRoomRequest>
	{
        public string NamespaceName { set; get; }
        public string Name { set; get; }
        public string[] NotificationUserIds { set; get; }

        public WantRoomRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public WantRoomRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public WantRoomRequest WithNotificationUserIds(string[] notificationUserIds) {
            this.NotificationUserIds = notificationUserIds;
            return this;
        }

    	[Preserve]
        public static WantRoomRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WantRoomRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithNotificationUserIds(!data.Keys.Contains("notificationUserIds") || data["notificationUserIds"] == null ? new string[]{} : data["notificationUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["notificationUserIds"] = new JsonData(NotificationUserIds == null ? new JsonData[]{} :
                        NotificationUserIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            writer.WriteArrayStart();
            foreach (var notificationUserId in NotificationUserIds)
            {
                writer.Write(notificationUserId.ToString());
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}