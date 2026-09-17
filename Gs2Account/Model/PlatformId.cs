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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class PlatformId : IComparable
	{
        public string Value { set; get; }
        public string UserId { set; get; }
        public int? Type { set; get; }
        public string UserIdentifier { set; get; }
        public long? CreatedAt { set; get; }
        public long? Revision { set; get; }
        public PlatformId WithValue(string value) {
            this.Value = value;
            return this;
        }
        public PlatformId WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public PlatformId WithType(int? type) {
            this.Type = type;
            return this;
        }
        public PlatformId WithUserIdentifier(string userIdentifier) {
            this.UserIdentifier = userIdentifier;
            return this;
        }
        public PlatformId WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public PlatformId WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):platformId:(?<type>.+):(?<userIdentifier>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):platformId:(?<type>.+):(?<userIdentifier>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):platformId:(?<type>.+):(?<userIdentifier>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):platformId:(?<type>.+):(?<userIdentifier>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):platformId:(?<type>.+):(?<userIdentifier>.+)",
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
        public static PlatformId FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new PlatformId()
                .WithValue(!data.Keys.Contains("platformId") || data["platformId"] == null ? null : data["platformId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["type"].ToString()))
                .WithUserIdentifier(!data.Keys.Contains("userIdentifier") || data["userIdentifier"] == null ? null : data["userIdentifier"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["platformId"] = Value,
                ["userId"] = UserId,
                ["type"] = Type,
                ["userIdentifier"] = UserIdentifier,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(Value.ToString());
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
            var other = obj as PlatformId;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type PlatformId.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Value, other.Value);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Type, other.Type);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserIdentifier, other.UserIdentifier);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Value.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.platformId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Type < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.type.error.invalid"),
                    });
                }
                if (Type > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.type.error.invalid"),
                    });
                }
            }
            {
                if (UserIdentifier.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.userIdentifier.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("platformId", "account.platformId.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new PlatformId {
                Value = Value,
                UserId = UserId,
                Type = Type,
                UserIdentifier = UserIdentifier,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}