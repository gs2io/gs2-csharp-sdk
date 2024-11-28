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

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Subscribe : IComparable
	{
        public string SubscribeId { set; get; } = null!;
        public string RankingName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string[] TargetUserIds { set; get; } = null!;
        public string[] FromUserIds { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Subscribe WithSubscribeId(string subscribeId) {
            this.SubscribeId = subscribeId;
            return this;
        }
        public Subscribe WithRankingName(string rankingName) {
            this.RankingName = rankingName;
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
        public Subscribe WithFromUserIds(string[] fromUserIds) {
            this.FromUserIds = fromUserIds;
            return this;
        }
        public Subscribe WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Subscribe WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Subscribe WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<rankingName>.+)",
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

        private static System.Text.RegularExpressions.Regex _rankingNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<rankingName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRankingNameFromGrn(
            string grn
        )
        {
            var match = _rankingNameRegex.Match(grn);
            if (!match.Success || !match.Groups["rankingName"].Success)
            {
                return null;
            }
            return match.Groups["rankingName"].Value;
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
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetUserIds(!data.Keys.Contains("targetUserIds") || data["targetUserIds"] == null || !data["targetUserIds"].IsArray ? null : data["targetUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithFromUserIds(!data.Keys.Contains("fromUserIds") || data["fromUserIds"] == null || !data["fromUserIds"].IsArray ? null : data["fromUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
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
            JsonData fromUserIdsJsonData = null;
            if (FromUserIds != null && FromUserIds.Length > 0)
            {
                fromUserIdsJsonData = new JsonData();
                foreach (var fromUserId in FromUserIds)
                {
                    fromUserIdsJsonData.Add(fromUserId);
                }
            }
            return new JsonData {
                ["subscribeId"] = SubscribeId,
                ["rankingName"] = RankingName,
                ["userId"] = UserId,
                ["targetUserIds"] = targetUserIdsJsonData,
                ["fromUserIds"] = fromUserIdsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
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
            if (RankingName != null) {
                writer.WritePropertyName("rankingName");
                writer.Write(RankingName.ToString());
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
            if (FromUserIds != null) {
                writer.WritePropertyName("fromUserIds");
                writer.WriteArrayStart();
                foreach (var fromUserId in FromUserIds)
                {
                    if (fromUserId != null) {
                        writer.Write(fromUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
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
            if (RankingName == null && RankingName == other.RankingName)
            {
                // null and null
            }
            else
            {
                diff += RankingName.CompareTo(other.RankingName);
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
            if (FromUserIds == null && FromUserIds == other.FromUserIds)
            {
                // null and null
            }
            else
            {
                diff += FromUserIds.Length - other.FromUserIds.Length;
                for (var i = 0; i < FromUserIds.Length; i++)
                {
                    diff += FromUserIds[i].CompareTo(other.FromUserIds[i]);
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
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
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
                        new RequestError("subscribe", "ranking2.subscribe.subscribeId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.rankingName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TargetUserIds.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.targetUserIds.error.tooMany"),
                    });
                }
            }
            {
                if (FromUserIds.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.fromUserIds.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "ranking2.subscribe.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Subscribe {
                SubscribeId = SubscribeId,
                RankingName = RankingName,
                UserId = UserId,
                TargetUserIds = TargetUserIds.Clone() as string[],
                FromUserIds = FromUserIds.Clone() as string[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}