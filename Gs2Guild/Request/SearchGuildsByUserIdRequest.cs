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
using Gs2.Gs2Guild.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Guild.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SearchGuildsByUserIdRequest : Gs2Request<SearchGuildsByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GuildModelName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string DisplayName { set; get; } = null!;
         public int[] Attributes1 { set; get; } = null!;
         public int[] Attributes2 { set; get; } = null!;
         public int[] Attributes3 { set; get; } = null!;
         public int[] Attributes4 { set; get; } = null!;
         public int[] Attributes5 { set; get; } = null!;
         public string[] JoinPolicies { set; get; } = null!;
         public bool? IncludeFullMembersGuild { set; get; } = null!;
         public string OrderBy { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SearchGuildsByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SearchGuildsByUserIdRequest WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public SearchGuildsByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SearchGuildsByUserIdRequest WithDisplayName(string displayName) {
            this.DisplayName = displayName;
            return this;
        }
        public SearchGuildsByUserIdRequest WithAttributes1(int[] attributes1) {
            this.Attributes1 = attributes1;
            return this;
        }
        public SearchGuildsByUserIdRequest WithAttributes2(int[] attributes2) {
            this.Attributes2 = attributes2;
            return this;
        }
        public SearchGuildsByUserIdRequest WithAttributes3(int[] attributes3) {
            this.Attributes3 = attributes3;
            return this;
        }
        public SearchGuildsByUserIdRequest WithAttributes4(int[] attributes4) {
            this.Attributes4 = attributes4;
            return this;
        }
        public SearchGuildsByUserIdRequest WithAttributes5(int[] attributes5) {
            this.Attributes5 = attributes5;
            return this;
        }
        public SearchGuildsByUserIdRequest WithJoinPolicies(string[] joinPolicies) {
            this.JoinPolicies = joinPolicies;
            return this;
        }
        public SearchGuildsByUserIdRequest WithIncludeFullMembersGuild(bool? includeFullMembersGuild) {
            this.IncludeFullMembersGuild = includeFullMembersGuild;
            return this;
        }
        public SearchGuildsByUserIdRequest WithOrderBy(string orderBy) {
            this.OrderBy = orderBy;
            return this;
        }
        public SearchGuildsByUserIdRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public SearchGuildsByUserIdRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }
        public SearchGuildsByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public SearchGuildsByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SearchGuildsByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SearchGuildsByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDisplayName(!data.Keys.Contains("displayName") || data["displayName"] == null ? null : data["displayName"].ToString())
                .WithAttributes1(!data.Keys.Contains("attributes1") || data["attributes1"] == null || !data["attributes1"].IsArray ? null : data["attributes1"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithAttributes2(!data.Keys.Contains("attributes2") || data["attributes2"] == null || !data["attributes2"].IsArray ? null : data["attributes2"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithAttributes3(!data.Keys.Contains("attributes3") || data["attributes3"] == null || !data["attributes3"].IsArray ? null : data["attributes3"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithAttributes4(!data.Keys.Contains("attributes4") || data["attributes4"] == null || !data["attributes4"].IsArray ? null : data["attributes4"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithAttributes5(!data.Keys.Contains("attributes5") || data["attributes5"] == null || !data["attributes5"].IsArray ? null : data["attributes5"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithJoinPolicies(!data.Keys.Contains("joinPolicies") || data["joinPolicies"] == null || !data["joinPolicies"].IsArray ? null : data["joinPolicies"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithIncludeFullMembersGuild(!data.Keys.Contains("includeFullMembersGuild") || data["includeFullMembersGuild"] == null ? null : (bool?)bool.Parse(data["includeFullMembersGuild"].ToString()))
                .WithOrderBy(!data.Keys.Contains("orderBy") || data["orderBy"] == null ? null : data["orderBy"].ToString())
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData attributes1JsonData = null;
            if (Attributes1 != null && Attributes1.Length > 0)
            {
                attributes1JsonData = new JsonData();
                foreach (var attribute1 in Attributes1)
                {
                    attributes1JsonData.Add(attribute1);
                }
            }
            JsonData attributes2JsonData = null;
            if (Attributes2 != null && Attributes2.Length > 0)
            {
                attributes2JsonData = new JsonData();
                foreach (var attribute2 in Attributes2)
                {
                    attributes2JsonData.Add(attribute2);
                }
            }
            JsonData attributes3JsonData = null;
            if (Attributes3 != null && Attributes3.Length > 0)
            {
                attributes3JsonData = new JsonData();
                foreach (var attribute3 in Attributes3)
                {
                    attributes3JsonData.Add(attribute3);
                }
            }
            JsonData attributes4JsonData = null;
            if (Attributes4 != null && Attributes4.Length > 0)
            {
                attributes4JsonData = new JsonData();
                foreach (var attribute4 in Attributes4)
                {
                    attributes4JsonData.Add(attribute4);
                }
            }
            JsonData attributes5JsonData = null;
            if (Attributes5 != null && Attributes5.Length > 0)
            {
                attributes5JsonData = new JsonData();
                foreach (var attribute5 in Attributes5)
                {
                    attributes5JsonData.Add(attribute5);
                }
            }
            JsonData joinPoliciesJsonData = null;
            if (JoinPolicies != null && JoinPolicies.Length > 0)
            {
                joinPoliciesJsonData = new JsonData();
                foreach (var joinPolicy in JoinPolicies)
                {
                    joinPoliciesJsonData.Add(joinPolicy);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["guildModelName"] = GuildModelName,
                ["userId"] = UserId,
                ["displayName"] = DisplayName,
                ["attributes1"] = attributes1JsonData,
                ["attributes2"] = attributes2JsonData,
                ["attributes3"] = attributes3JsonData,
                ["attributes4"] = attributes4JsonData,
                ["attributes5"] = attributes5JsonData,
                ["joinPolicies"] = joinPoliciesJsonData,
                ["includeFullMembersGuild"] = IncludeFullMembersGuild,
                ["orderBy"] = OrderBy,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (GuildModelName != null) {
                writer.WritePropertyName("guildModelName");
                writer.Write(GuildModelName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (DisplayName != null) {
                writer.WritePropertyName("displayName");
                writer.Write(DisplayName.ToString());
            }
            if (Attributes1 != null) {
                writer.WritePropertyName("attributes1");
                writer.WriteArrayStart();
                foreach (var attribute1 in Attributes1)
                {
                    writer.Write((attribute1.ToString().Contains(".") ? (int)double.Parse(attribute1.ToString()) : int.Parse(attribute1.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (Attributes2 != null) {
                writer.WritePropertyName("attributes2");
                writer.WriteArrayStart();
                foreach (var attribute2 in Attributes2)
                {
                    writer.Write((attribute2.ToString().Contains(".") ? (int)double.Parse(attribute2.ToString()) : int.Parse(attribute2.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (Attributes3 != null) {
                writer.WritePropertyName("attributes3");
                writer.WriteArrayStart();
                foreach (var attribute3 in Attributes3)
                {
                    writer.Write((attribute3.ToString().Contains(".") ? (int)double.Parse(attribute3.ToString()) : int.Parse(attribute3.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (Attributes4 != null) {
                writer.WritePropertyName("attributes4");
                writer.WriteArrayStart();
                foreach (var attribute4 in Attributes4)
                {
                    writer.Write((attribute4.ToString().Contains(".") ? (int)double.Parse(attribute4.ToString()) : int.Parse(attribute4.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (Attributes5 != null) {
                writer.WritePropertyName("attributes5");
                writer.WriteArrayStart();
                foreach (var attribute5 in Attributes5)
                {
                    writer.Write((attribute5.ToString().Contains(".") ? (int)double.Parse(attribute5.ToString()) : int.Parse(attribute5.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (JoinPolicies != null) {
                writer.WritePropertyName("joinPolicies");
                writer.WriteArrayStart();
                foreach (var joinPolicy in JoinPolicies)
                {
                    writer.Write(joinPolicy.ToString());
                }
                writer.WriteArrayEnd();
            }
            if (IncludeFullMembersGuild != null) {
                writer.WritePropertyName("includeFullMembersGuild");
                writer.Write(bool.Parse(IncludeFullMembersGuild.ToString()));
            }
            if (OrderBy != null) {
                writer.WritePropertyName("orderBy");
                writer.Write(OrderBy.ToString());
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GuildModelName + ":";
            key += UserId + ":";
            key += DisplayName + ":";
            key += Attributes1 + ":";
            key += Attributes2 + ":";
            key += Attributes3 + ":";
            key += Attributes4 + ":";
            key += Attributes5 + ":";
            key += JoinPolicies + ":";
            key += IncludeFullMembersGuild + ":";
            key += OrderBy + ":";
            key += PageToken + ":";
            key += Limit + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}