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

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class PrizeLimit : IComparable
	{
        public string PrizeLimitId { set; get; } = null!;
        public string PrizeId { set; get; } = null!;
        public int? DrawnCount { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public PrizeLimit WithPrizeLimitId(string prizeLimitId) {
            this.PrizeLimitId = prizeLimitId;
            return this;
        }
        public PrizeLimit WithPrizeId(string prizeId) {
            this.PrizeId = prizeId;
            return this;
        }
        public PrizeLimit WithDrawnCount(int? drawnCount) {
            this.DrawnCount = drawnCount;
            return this;
        }
        public PrizeLimit WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public PrizeLimit WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public PrizeLimit WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):table:(?<prizeTableName>.+):prize:(?<prizeId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):table:(?<prizeTableName>.+):prize:(?<prizeId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):table:(?<prizeTableName>.+):prize:(?<prizeId>.+)",
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

        private static System.Text.RegularExpressions.Regex _prizeTableNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):table:(?<prizeTableName>.+):prize:(?<prizeId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetPrizeTableNameFromGrn(
            string grn
        )
        {
            var match = _prizeTableNameRegex.Match(grn);
            if (!match.Success || !match.Groups["prizeTableName"].Success)
            {
                return null;
            }
            return match.Groups["prizeTableName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _prizeIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):table:(?<prizeTableName>.+):prize:(?<prizeId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetPrizeIdFromGrn(
            string grn
        )
        {
            var match = _prizeIdRegex.Match(grn);
            if (!match.Success || !match.Groups["prizeId"].Success)
            {
                return null;
            }
            return match.Groups["prizeId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PrizeLimit FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrizeLimit()
                .WithPrizeLimitId(!data.Keys.Contains("prizeLimitId") || data["prizeLimitId"] == null ? null : data["prizeLimitId"].ToString())
                .WithPrizeId(!data.Keys.Contains("prizeId") || data["prizeId"] == null ? null : data["prizeId"].ToString())
                .WithDrawnCount(!data.Keys.Contains("drawnCount") || data["drawnCount"] == null ? null : (int?)(data["drawnCount"].ToString().Contains(".") ? (int)double.Parse(data["drawnCount"].ToString()) : int.Parse(data["drawnCount"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["prizeLimitId"] = PrizeLimitId,
                ["prizeId"] = PrizeId,
                ["drawnCount"] = DrawnCount,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PrizeLimitId != null) {
                writer.WritePropertyName("prizeLimitId");
                writer.Write(PrizeLimitId.ToString());
            }
            if (PrizeId != null) {
                writer.WritePropertyName("prizeId");
                writer.Write(PrizeId.ToString());
            }
            if (DrawnCount != null) {
                writer.WritePropertyName("drawnCount");
                writer.Write((DrawnCount.ToString().Contains(".") ? (int)double.Parse(DrawnCount.ToString()) : int.Parse(DrawnCount.ToString())));
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
            var other = obj as PrizeLimit;
            var diff = 0;
            if (PrizeLimitId == null && PrizeLimitId == other.PrizeLimitId)
            {
                // null and null
            }
            else
            {
                diff += PrizeLimitId.CompareTo(other.PrizeLimitId);
            }
            if (PrizeId == null && PrizeId == other.PrizeId)
            {
                // null and null
            }
            else
            {
                diff += PrizeId.CompareTo(other.PrizeId);
            }
            if (DrawnCount == null && DrawnCount == other.DrawnCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(DrawnCount - other.DrawnCount);
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
                if (PrizeLimitId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.prizeLimitId.error.tooLong"),
                    });
                }
            }
            {
                if (PrizeId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.prizeId.error.tooLong"),
                    });
                }
            }
            {
                if (DrawnCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.drawnCount.error.invalid"),
                    });
                }
                if (DrawnCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.drawnCount.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("prizeLimit", "lottery.prizeLimit.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new PrizeLimit {
                PrizeLimitId = PrizeLimitId,
                PrizeId = PrizeId,
                DrawnCount = DrawnCount,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}