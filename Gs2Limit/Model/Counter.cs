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

namespace Gs2.Gs2Limit.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Counter : IComparable
	{
        public string CounterId { set; get; }
        public string LimitName { set; get; }
        public string Name { set; get; }
        public string UserId { set; get; }
        public int? Count { set; get; }
        public long? NextResetAt { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Counter WithCounterId(string counterId) {
            this.CounterId = counterId;
            return this;
        }

        public Counter WithLimitName(string limitName) {
            this.LimitName = limitName;
            return this;
        }

        public Counter WithName(string name) {
            this.Name = name;
            return this;
        }

        public Counter WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Counter WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public Counter WithNextResetAt(long? nextResetAt) {
            this.NextResetAt = nextResetAt;
            return this;
        }

        public Counter WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Counter WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):limit:(?<namespaceName>.+):user:(?<userId>.+):limit:(?<limitName>.+):counter:(?<counterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):limit:(?<namespaceName>.+):user:(?<userId>.+):limit:(?<limitName>.+):counter:(?<counterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):limit:(?<namespaceName>.+):user:(?<userId>.+):limit:(?<limitName>.+):counter:(?<counterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):limit:(?<namespaceName>.+):user:(?<userId>.+):limit:(?<limitName>.+):counter:(?<counterName>.+)",
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

        private static System.Text.RegularExpressions.Regex _limitNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):limit:(?<namespaceName>.+):user:(?<userId>.+):limit:(?<limitName>.+):counter:(?<counterName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetLimitNameFromGrn(
            string grn
        )
        {
            var match = _limitNameRegex.Match(grn);
            if (!match.Success || !match.Groups["limitName"].Success)
            {
                return null;
            }
            return match.Groups["limitName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _counterNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):limit:(?<namespaceName>.+):user:(?<userId>.+):limit:(?<limitName>.+):counter:(?<counterName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCounterNameFromGrn(
            string grn
        )
        {
            var match = _counterNameRegex.Match(grn);
            if (!match.Success || !match.Groups["counterName"].Success)
            {
                return null;
            }
            return match.Groups["counterName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Counter FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Counter()
                .WithCounterId(!data.Keys.Contains("counterId") || data["counterId"] == null ? null : data["counterId"].ToString())
                .WithLimitName(!data.Keys.Contains("limitName") || data["limitName"] == null ? null : data["limitName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)int.Parse(data["count"].ToString()))
                .WithNextResetAt(!data.Keys.Contains("nextResetAt") || data["nextResetAt"] == null ? null : (long?)long.Parse(data["nextResetAt"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["counterId"] = CounterId,
                ["limitName"] = LimitName,
                ["name"] = Name,
                ["userId"] = UserId,
                ["count"] = Count,
                ["nextResetAt"] = NextResetAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CounterId != null) {
                writer.WritePropertyName("counterId");
                writer.Write(CounterId.ToString());
            }
            if (LimitName != null) {
                writer.WritePropertyName("limitName");
                writer.Write(LimitName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(int.Parse(Count.ToString()));
            }
            if (NextResetAt != null) {
                writer.WritePropertyName("nextResetAt");
                writer.Write(long.Parse(NextResetAt.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Counter;
            var diff = 0;
            if (CounterId == null && CounterId == other.CounterId)
            {
                // null and null
            }
            else
            {
                diff += CounterId.CompareTo(other.CounterId);
            }
            if (LimitName == null && LimitName == other.LimitName)
            {
                // null and null
            }
            else
            {
                diff += LimitName.CompareTo(other.LimitName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            if (NextResetAt == null && NextResetAt == other.NextResetAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(NextResetAt - other.NextResetAt);
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
            return diff;
        }
    }
}