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
            return new DumpProgress()
                .WithDumpProgressId(!data.Keys.Contains("dumpProgressId") || data["dumpProgressId"] == null ? null : data["dumpProgressId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDumped(!data.Keys.Contains("dumped") || data["dumped"] == null ? null : (int?)(data["dumped"].ToString().Contains(".") ? (int)double.Parse(data["dumped"].ToString()) : int.Parse(data["dumped"].ToString())))
                .WithMicroserviceCount(!data.Keys.Contains("microserviceCount") || data["microserviceCount"] == null ? null : (int?)(data["microserviceCount"].ToString().Contains(".") ? (int)double.Parse(data["microserviceCount"].ToString()) : int.Parse(data["microserviceCount"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
            var diff = 0;
            if (DumpProgressId == null && DumpProgressId == other.DumpProgressId)
            {
                // null and null
            }
            else
            {
                diff += DumpProgressId.CompareTo(other.DumpProgressId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Dumped == null && Dumped == other.Dumped)
            {
                // null and null
            }
            else
            {
                diff += (int)(Dumped - other.Dumped);
            }
            if (MicroserviceCount == null && MicroserviceCount == other.MicroserviceCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MicroserviceCount - other.MicroserviceCount);
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