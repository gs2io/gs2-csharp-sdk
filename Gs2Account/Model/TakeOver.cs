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
	public class TakeOver : IComparable
	{
        public string TakeOverId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public int? Type { set; get; } = null!;
        public string UserIdentifier { set; get; } = null!;
        public string Password { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public TakeOver WithTakeOverId(string takeOverId) {
            this.TakeOverId = takeOverId;
            return this;
        }
        public TakeOver WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public TakeOver WithType(int? type) {
            this.Type = type;
            return this;
        }
        public TakeOver WithUserIdentifier(string userIdentifier) {
            this.UserIdentifier = userIdentifier;
            return this;
        }
        public TakeOver WithPassword(string password) {
            this.Password = password;
            return this;
        }
        public TakeOver WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public TakeOver WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):takeOver:(?<type>.+):(?<userIdentifier>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):takeOver:(?<type>.+):(?<userIdentifier>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):takeOver:(?<type>.+):(?<userIdentifier>.+)",
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

        private static System.Text.RegularExpressions.Regex _typeRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):takeOver:(?<type>.+):(?<userIdentifier>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTypeFromGrn(
            string grn
        )
        {
            var match = _typeRegex.Match(grn);
            if (!match.Success || !match.Groups["type"].Success)
            {
                return null;
            }
            return match.Groups["type"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdentifierRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):takeOver:(?<type>.+):(?<userIdentifier>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdentifierFromGrn(
            string grn
        )
        {
            var match = _userIdentifierRegex.Match(grn);
            if (!match.Success || !match.Groups["userIdentifier"].Success)
            {
                return null;
            }
            return match.Groups["userIdentifier"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TakeOver FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TakeOver()
                .WithTakeOverId(!data.Keys.Contains("takeOverId") || data["takeOverId"] == null ? null : data["takeOverId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : (int?)(data["type"].ToString().Contains(".") ? (int)double.Parse(data["type"].ToString()) : int.Parse(data["type"].ToString())))
                .WithUserIdentifier(!data.Keys.Contains("userIdentifier") || data["userIdentifier"] == null ? null : data["userIdentifier"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["takeOverId"] = TakeOverId,
                ["userId"] = UserId,
                ["type"] = Type,
                ["userIdentifier"] = UserIdentifier,
                ["password"] = Password,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TakeOverId != null) {
                writer.WritePropertyName("takeOverId");
                writer.Write(TakeOverId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write((Type.ToString().Contains(".") ? (int)double.Parse(Type.ToString()) : int.Parse(Type.ToString())));
            }
            if (UserIdentifier != null) {
                writer.WritePropertyName("userIdentifier");
                writer.Write(UserIdentifier.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
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
            var other = obj as TakeOver;
            var diff = 0;
            if (TakeOverId == null && TakeOverId == other.TakeOverId)
            {
                // null and null
            }
            else
            {
                diff += TakeOverId.CompareTo(other.TakeOverId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += (int)(Type - other.Type);
            }
            if (UserIdentifier == null && UserIdentifier == other.UserIdentifier)
            {
                // null and null
            }
            else
            {
                diff += UserIdentifier.CompareTo(other.UserIdentifier);
            }
            if (Password == null && Password == other.Password)
            {
                // null and null
            }
            else
            {
                diff += Password.CompareTo(other.Password);
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
                if (TakeOverId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.takeOverId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Type < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.type.error.invalid"),
                    });
                }
                if (Type > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.type.error.invalid"),
                    });
                }
            }
            {
                if (UserIdentifier.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.userIdentifier.error.tooLong"),
                    });
                }
            }
            {
                if (Password.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.password.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOver", "account.takeOver.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new TakeOver {
                TakeOverId = TakeOverId,
                UserId = UserId,
                Type = Type,
                UserIdentifier = UserIdentifier,
                Password = Password,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}