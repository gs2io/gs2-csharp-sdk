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

namespace Gs2.Gs2Account.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Account : IComparable
	{
        public string AccountId { set; get; }
        public string UserId { set; get; }
        public string Password { set; get; }
        public int? TimeOffset { set; get; }
        public Gs2.Gs2Account.Model.BanStatus[] BanStatuses { set; get; }
        public bool? Banned { set; get; }
        public long? CreatedAt { set; get; }
        public long? Revision { set; get; }

        public Account WithAccountId(string accountId) {
            this.AccountId = accountId;
            return this;
        }

        public Account WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Account WithPassword(string password) {
            this.Password = password;
            return this;
        }

        public Account WithTimeOffset(int? timeOffset) {
            this.TimeOffset = timeOffset;
            return this;
        }

        public Account WithBanStatuses(Gs2.Gs2Account.Model.BanStatus[] banStatuses) {
            this.BanStatuses = banStatuses;
            return this;
        }

        public Account WithBanned(bool? banned) {
            this.Banned = banned;
            return this;
        }

        public Account WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Account WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):account:(?<userId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):account:(?<userId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):account:(?<userId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):account:(?<userId>.+)",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Account FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Account()
                .WithAccountId(!data.Keys.Contains("accountId") || data["accountId"] == null ? null : data["accountId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithTimeOffset(!data.Keys.Contains("timeOffset") || data["timeOffset"] == null ? null : (int?)int.Parse(data["timeOffset"].ToString()))
                .WithBanStatuses(!data.Keys.Contains("banStatuses") || data["banStatuses"] == null || !data["banStatuses"].IsArray ? new Gs2.Gs2Account.Model.BanStatus[]{} : data["banStatuses"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Account.Model.BanStatus.FromJson(v);
                }).ToArray())
                .WithBanned(!data.Keys.Contains("banned") || data["banned"] == null ? null : (bool?)bool.Parse(data["banned"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData banStatusesJsonData = null;
            if (BanStatuses != null)
            {
                banStatusesJsonData = new JsonData();
                foreach (var banStatus in BanStatuses)
                {
                    banStatusesJsonData.Add(banStatus.ToJson());
                }
            }
            return new JsonData {
                ["accountId"] = AccountId,
                ["userId"] = UserId,
                ["password"] = Password,
                ["timeOffset"] = TimeOffset,
                ["banStatuses"] = banStatusesJsonData,
                ["banned"] = Banned,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccountId != null) {
                writer.WritePropertyName("accountId");
                writer.Write(AccountId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (TimeOffset != null) {
                writer.WritePropertyName("timeOffset");
                writer.Write(int.Parse(TimeOffset.ToString()));
            }
            if (BanStatuses != null) {
                writer.WritePropertyName("banStatuses");
                writer.WriteArrayStart();
                foreach (var banStatus in BanStatuses)
                {
                    if (banStatus != null) {
                        banStatus.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Banned != null) {
                writer.WritePropertyName("banned");
                writer.Write(bool.Parse(Banned.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write(long.Parse(Revision.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Account;
            var diff = 0;
            if (AccountId == null && AccountId == other.AccountId)
            {
                // null and null
            }
            else
            {
                diff += AccountId.CompareTo(other.AccountId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Password == null && Password == other.Password)
            {
                // null and null
            }
            else
            {
                diff += Password.CompareTo(other.Password);
            }
            if (TimeOffset == null && TimeOffset == other.TimeOffset)
            {
                // null and null
            }
            else
            {
                diff += (int)(TimeOffset - other.TimeOffset);
            }
            if (BanStatuses == null && BanStatuses == other.BanStatuses)
            {
                // null and null
            }
            else
            {
                diff += BanStatuses.Length - other.BanStatuses.Length;
                for (var i = 0; i < BanStatuses.Length; i++)
                {
                    diff += BanStatuses[i].CompareTo(other.BanStatuses[i]);
                }
            }
            if (Banned == null && Banned == other.Banned)
            {
                // null and null
            }
            else
            {
                diff += Banned == other.Banned ? 0 : 1;
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
    }
}