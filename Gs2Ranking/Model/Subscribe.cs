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
        public string SubscribeId { set; get; } = null!;
        public string CategoryName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string[] TargetUserIds { set; get; } = null!;
        public string[] SubscribedUserIds { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Subscribe WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _categoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCategoryNameFromGrn(
            string grn
        )
        {
            var match = _categoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["categoryName"].Success)
            {
                return null;
            }
            return match.Groups["categoryName"].Value;
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
                .WithTargetUserIds(!data.Keys.Contains("targetUserIds") || data["targetUserIds"] == null || !data["targetUserIds"].IsArray ? null : data["targetUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSubscribedUserIds(!data.Keys.Contains("subscribedUserIds") || data["subscribedUserIds"] == null || !data["subscribedUserIds"].IsArray ? null : data["subscribedUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData targetUserIdsJsonData = null;
            if (TargetUserIds != null && TargetUserIds.Length > 0)
            {
                targetUserIdsJsonData = new JsonData();
                foreach (var targetUserId in TargetUserIds)
                {
                    targetUserIdsJsonData.Add(targetUserId);
                }
            }
            JsonData subscribedUserIdsJsonData = null;
            if (SubscribedUserIds != null && SubscribedUserIds.Length > 0)
            {
                subscribedUserIdsJsonData = new JsonData();
                foreach (var subscribedUserId in SubscribedUserIds)
                {
                    subscribedUserIdsJsonData.Add(subscribedUserId);
                }
            }
            return new JsonData {
                ["subscribeId"] = SubscribeId,
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["targetUserIds"] = targetUserIdsJsonData,
                ["subscribedUserIds"] = subscribedUserIdsJsonData,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
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
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (SubscribeId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.subscribeId.error.tooLong"),
                    });
                }
            }
            {
                if (CategoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.categoryName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TargetUserIds.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.targetUserIds.error.tooMany"),
                    });
                }
            }
            {
                if (SubscribedUserIds.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.subscribedUserIds.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking.subscribe.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Subscribe {
                SubscribeId = SubscribeId,
                CategoryName = CategoryName,
                UserId = UserId,
                TargetUserIds = TargetUserIds.Clone() as string[],
                SubscribedUserIds = SubscribedUserIds.Clone() as string[],
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}