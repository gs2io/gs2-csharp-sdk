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

namespace Gs2.Gs2Limit.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Counter : IComparable
	{
        public string CounterId { set; get; }
        public string LimitName { set; get; }
        public string Name { set; get; }
        public string UserId { set; get; }
        public int? Count { set; get; }
        public long? NextResetAt { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
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
        public Counter WithRevision(long? revision) {
            this.Revision = revision;
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Counter()
                .WithCounterId(!data.Keys.Contains("counterId") || data["counterId"] == null ? null : data["counterId"].ToString())
                .WithLimitName(!data.Keys.Contains("limitName") || data["limitName"] == null ? null : data["limitName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["count"].ToString()))
                .WithNextResetAt(!data.Keys.Contains("nextResetAt") || data["nextResetAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["nextResetAt"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
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
                ["revision"] = Revision,
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
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            if (NextResetAt != null) {
                writer.WritePropertyName("nextResetAt");
                writer.Write((NextResetAt.ToString().Contains(".") ? (long)double.Parse(NextResetAt.ToString()) : long.Parse(NextResetAt.ToString())));
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
            var other = obj as Counter;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Counter.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(CounterId, other.CounterId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(LimitName, other.LimitName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Count, other.Count);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(NextResetAt, other.NextResetAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
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
                if (CounterId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.counterId.error.tooLong"),
                    });
                }
            }
            {
                if (LimitName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.limitName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.name.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.count.error.invalid"),
                    });
                }
                if (Count > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.count.error.invalid"),
                    });
                }
            }
            {
                if (NextResetAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.nextResetAt.error.invalid"),
                    });
                }
                if (NextResetAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.nextResetAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counter", "limit.counter.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Counter {
                CounterId = CounterId,
                LimitName = LimitName,
                Name = Name,
                UserId = UserId,
                Count = Count,
                NextResetAt = NextResetAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}