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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Subscribe : IComparable
	{
        public string SubscribeId { set; get; }
        public string CategoryName { set; get; }
        public string UserId { set; get; }
        public string[] TargetUserIds { set; get; }
        public string[] SubscribedUserIds { set; get; }
        public long? CreatedAt { set; get; }

        public Subscribe WithSubscribeId(string subscribeId) {
            this.SubscribeId = subscribeId;
            return this;
        }

        public Subscribe WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }

        public Subscribe WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Subscribe WithTargetUserIds(string[] targetUserIds) {
            this.TargetUserIds = targetUserIds;
            return this;
        }

        public Subscribe WithSubscribedUserIds(string[] subscribedUserIds) {
            this.SubscribedUserIds = subscribedUserIds;
            return this;
        }

        public Subscribe WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Subscribe FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Subscribe()
                .WithSubscribeId(!data.Keys.Contains("subscribeId") || data["subscribeId"] == null ? null : data["subscribeId"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetUserIds(!data.Keys.Contains("targetUserIds") || data["targetUserIds"] == null ? new string[]{} : data["targetUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSubscribedUserIds(!data.Keys.Contains("subscribedUserIds") || data["subscribedUserIds"] == null ? new string[]{} : data["subscribedUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscribeId"] = SubscribeId,
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["targetUserIds"] = new JsonData(TargetUserIds == null ? new JsonData[]{} :
                        TargetUserIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["subscribedUserIds"] = new JsonData(SubscribedUserIds == null ? new JsonData[]{} :
                        SubscribedUserIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SubscribeId != null) {
                writer.WritePropertyName("subscribeId");
                writer.Write(SubscribeId.ToString());
            }
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TargetUserIds != null) {
                writer.WritePropertyName("targetUserIds");
                writer.WriteArrayStart();
                foreach (var targetUserId in TargetUserIds)
                {
                    if (targetUserId != null) {
                        writer.Write(targetUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (SubscribedUserIds != null) {
                writer.WritePropertyName("subscribedUserIds");
                writer.WriteArrayStart();
                foreach (var subscribedUserId in SubscribedUserIds)
                {
                    if (subscribedUserId != null) {
                        writer.Write(subscribedUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Subscribe;
            var diff = 0;
            if (SubscribeId == null && SubscribeId == other.SubscribeId)
            {
                // null and null
            }
            else
            {
                diff += SubscribeId.CompareTo(other.SubscribeId);
            }
            if (CategoryName == null && CategoryName == other.CategoryName)
            {
                // null and null
            }
            else
            {
                diff += CategoryName.CompareTo(other.CategoryName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TargetUserIds == null && TargetUserIds == other.TargetUserIds)
            {
                // null and null
            }
            else
            {
                diff += TargetUserIds.Length - other.TargetUserIds.Length;
                for (var i = 0; i < TargetUserIds.Length; i++)
                {
                    diff += TargetUserIds[i].CompareTo(other.TargetUserIds[i]);
                }
            }
            if (SubscribedUserIds == null && SubscribedUserIds == other.SubscribedUserIds)
            {
                // null and null
            }
            else
            {
                diff += SubscribedUserIds.Length - other.SubscribedUserIds.Length;
                for (var i = 0; i < SubscribedUserIds.Length; i++)
                {
                    diff += SubscribedUserIds[i].CompareTo(other.SubscribedUserIds[i]);
                }
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }
    }
}