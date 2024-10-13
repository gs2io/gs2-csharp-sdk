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

namespace Gs2.Gs2Dictionary.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Entry : IComparable
	{
        public string EntryId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public long? AcquiredAt { set; get; } = null!;
        public Entry WithEntryId(string entryId) {
            this.EntryId = entryId;
            return this;
        }
        public Entry WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Entry WithName(string name) {
            this.Name = name;
            return this;
        }
        public Entry WithAcquiredAt(long? acquiredAt) {
            this.AcquiredAt = acquiredAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):dictionary:(?<namespaceName>.+):user:(?<userId>.+):entry:(?<entryModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):dictionary:(?<namespaceName>.+):user:(?<userId>.+):entry:(?<entryModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):dictionary:(?<namespaceName>.+):user:(?<userId>.+):entry:(?<entryModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):dictionary:(?<namespaceName>.+):user:(?<userId>.+):entry:(?<entryModelName>.+)",
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

        private static System.Text.RegularExpressions.Regex _entryModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):dictionary:(?<namespaceName>.+):user:(?<userId>.+):entry:(?<entryModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetEntryModelNameFromGrn(
            string grn
        )
        {
            var match = _entryModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["entryModelName"].Success)
            {
                return null;
            }
            return match.Groups["entryModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Entry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Entry()
                .WithEntryId(!data.Keys.Contains("entryId") || data["entryId"] == null ? null : data["entryId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithAcquiredAt(!data.Keys.Contains("acquiredAt") || data["acquiredAt"] == null ? null : (long?)(data["acquiredAt"].ToString().Contains(".") ? (long)double.Parse(data["acquiredAt"].ToString()) : long.Parse(data["acquiredAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["entryId"] = EntryId,
                ["userId"] = UserId,
                ["name"] = Name,
                ["acquiredAt"] = AcquiredAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (EntryId != null) {
                writer.WritePropertyName("entryId");
                writer.Write(EntryId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (AcquiredAt != null) {
                writer.WritePropertyName("acquiredAt");
                writer.Write((AcquiredAt.ToString().Contains(".") ? (long)double.Parse(AcquiredAt.ToString()) : long.Parse(AcquiredAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Entry;
            var diff = 0;
            if (EntryId == null && EntryId == other.EntryId)
            {
                // null and null
            }
            else
            {
                diff += EntryId.CompareTo(other.EntryId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (AcquiredAt == null && AcquiredAt == other.AcquiredAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(AcquiredAt - other.AcquiredAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (EntryId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("entry", "dictionary.entry.entryId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("entry", "dictionary.entry.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("entry", "dictionary.entry.name.error.tooLong"),
                    });
                }
            }
            {
                if (AcquiredAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("entry", "dictionary.entry.acquiredAt.error.invalid"),
                    });
                }
                if (AcquiredAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("entry", "dictionary.entry.acquiredAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Entry {
                EntryId = EntryId,
                UserId = UserId,
                Name = Name,
                AcquiredAt = AcquiredAt,
            };
        }
    }
}