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
        public string AccountId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string Password { set; get; } = null!;
        public int? TimeOffset { set; get; } = null!;
        public Gs2.Gs2Account.Model.BanStatus[] BanStatuses { set; get; } = null!;
        public bool? Banned { set; get; } = null!;
        public long? LastAuthenticatedAt { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Account WithLastAuthenticatedAt(long? lastAuthenticatedAt) {
            this.LastAuthenticatedAt = lastAuthenticatedAt;
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
                .WithTimeOffset(!data.Keys.Contains("timeOffset") || data["timeOffset"] == null ? null : (int?)(data["timeOffset"].ToString().Contains(".") ? (int)double.Parse(data["timeOffset"].ToString()) : int.Parse(data["timeOffset"].ToString())))
                .WithBanStatuses(!data.Keys.Contains("banStatuses") || data["banStatuses"] == null || !data["banStatuses"].IsArray ? null : data["banStatuses"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Account.Model.BanStatus.FromJson(v);
                }).ToArray())
                .WithBanned(!data.Keys.Contains("banned") || data["banned"] == null ? null : (bool?)bool.Parse(data["banned"].ToString()))
                .WithLastAuthenticatedAt(!data.Keys.Contains("lastAuthenticatedAt") || data["lastAuthenticatedAt"] == null ? null : (long?)(data["lastAuthenticatedAt"].ToString().Contains(".") ? (long)double.Parse(data["lastAuthenticatedAt"].ToString()) : long.Parse(data["lastAuthenticatedAt"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData banStatusesJsonData = null;
            if (BanStatuses != null && BanStatuses.Length > 0)
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
                ["lastAuthenticatedAt"] = LastAuthenticatedAt,
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
                writer.Write((TimeOffset.ToString().Contains(".") ? (int)double.Parse(TimeOffset.ToString()) : int.Parse(TimeOffset.ToString())));
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
            if (LastAuthenticatedAt != null) {
                writer.WritePropertyName("lastAuthenticatedAt");
                writer.Write((LastAuthenticatedAt.ToString().Contains(".") ? (long)double.Parse(LastAuthenticatedAt.ToString()) : long.Parse(LastAuthenticatedAt.ToString())));
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
            if (LastAuthenticatedAt == null && LastAuthenticatedAt == other.LastAuthenticatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(LastAuthenticatedAt - other.LastAuthenticatedAt);
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
                if (AccountId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.accountId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Password.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.password.error.tooLong"),
                    });
                }
            }
            {
                if (TimeOffset < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.timeOffset.error.invalid"),
                    });
                }
                if (TimeOffset > 315360000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.timeOffset.error.invalid"),
                    });
                }
            }
            {
                if (BanStatuses.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.banStatuses.error.tooMany"),
                    });
                }
            }
            {
            }
            {
                if (LastAuthenticatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.lastAuthenticatedAt.error.invalid"),
                    });
                }
                if (LastAuthenticatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.lastAuthenticatedAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "account.account.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Account {
                AccountId = AccountId,
                UserId = UserId,
                Password = Password,
                TimeOffset = TimeOffset,
                BanStatuses = BanStatuses.Clone() as Gs2.Gs2Account.Model.BanStatus[],
                Banned = Banned,
                LastAuthenticatedAt = LastAuthenticatedAt,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}