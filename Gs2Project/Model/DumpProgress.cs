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

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class DumpProgress : IComparable
	{
        public string DumpProgressId { set; get; }
        public string TransactionId { set; get; }
        public string UserId { set; get; }
        public int? Dumped { set; get; }
        public int? MicroserviceCount { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public DumpProgress WithDumpProgressId(string dumpProgressId) {
            this.DumpProgressId = dumpProgressId;
            return this;
        }
        public DumpProgress WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public DumpProgress WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DumpProgress WithDumped(int? dumped) {
            this.Dumped = dumped;
            return this;
        }
        public DumpProgress WithMicroserviceCount(int? microserviceCount) {
            this.MicroserviceCount = microserviceCount;
            return this;
        }
        public DumpProgress WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public DumpProgress WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public DumpProgress WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _accountNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):dump:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAccountNameFromGrn(
            string grn
        )
        {
            var match = _accountNameRegex.Match(grn);
            if (!match.Success || !match.Groups["accountName"].Success)
            {
                return null;
            }
            return match.Groups["accountName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _projectNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):dump:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetProjectNameFromGrn(
            string grn
        )
        {
            var match = _projectNameRegex.Match(grn);
            if (!match.Success || !match.Groups["projectName"].Success)
            {
                return null;
            }
            return match.Groups["projectName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):dump:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTransactionIdFromGrn(
            string grn
        )
        {
            var match = _transactionIdRegex.Match(grn);
            if (!match.Success || !match.Groups["transactionId"].Success)
            {
                return null;
            }
            return match.Groups["transactionId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DumpProgress FromJson(JsonData data)
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
            return new DumpProgress()
                .WithDumpProgressId(!data.Keys.Contains("dumpProgressId") || data["dumpProgressId"] == null ? null : data["dumpProgressId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDumped(!data.Keys.Contains("dumped") || data["dumped"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["dumped"].ToString()))
                .WithMicroserviceCount(!data.Keys.Contains("microserviceCount") || data["microserviceCount"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["microserviceCount"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["dumpProgressId"] = DumpProgressId,
                ["transactionId"] = TransactionId,
                ["userId"] = UserId,
                ["dumped"] = Dumped,
                ["microserviceCount"] = MicroserviceCount,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DumpProgressId != null) {
                writer.WritePropertyName("dumpProgressId");
                writer.Write(DumpProgressId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Dumped != null) {
                writer.WritePropertyName("dumped");
                writer.Write((Dumped.ToString().Contains(".") ? (int)double.Parse(Dumped.ToString()) : int.Parse(Dumped.ToString())));
            }
            if (MicroserviceCount != null) {
                writer.WritePropertyName("microserviceCount");
                writer.Write((MicroserviceCount.ToString().Contains(".") ? (int)double.Parse(MicroserviceCount.ToString()) : int.Parse(MicroserviceCount.ToString())));
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
            var other = obj as DumpProgress;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type DumpProgress.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(DumpProgressId, other.DumpProgressId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TransactionId, other.TransactionId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Dumped, other.Dumped);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MicroserviceCount, other.MicroserviceCount);
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
                if (DumpProgressId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.dumpProgressId.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Dumped < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.dumped.error.invalid"),
                    });
                }
                if (Dumped > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.dumped.error.invalid"),
                    });
                }
            }
            {
                if (MicroserviceCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.microserviceCount.error.invalid"),
                    });
                }
                if (MicroserviceCount > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.microserviceCount.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dumpProgress", "project.dumpProgress.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new DumpProgress {
                DumpProgressId = DumpProgressId,
                TransactionId = TransactionId,
                UserId = UserId,
                Dumped = Dumped,
                MicroserviceCount = MicroserviceCount,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}