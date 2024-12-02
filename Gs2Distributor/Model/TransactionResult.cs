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

namespace Gs2.Gs2Distributor.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class TransactionResult : IComparable
	{
        public string TransactionResultId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public Gs2.Gs2Distributor.Model.VerifyActionResult[] VerifyResults { set; get; } = null!;
        public Gs2.Gs2Distributor.Model.ConsumeActionResult[] ConsumeResults { set; get; } = null!;
        public Gs2.Gs2Distributor.Model.AcquireActionResult[] AcquireResults { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public TransactionResult WithTransactionResultId(string transactionResultId) {
            this.TransactionResultId = transactionResultId;
            return this;
        }
        public TransactionResult WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public TransactionResult WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public TransactionResult WithVerifyResults(Gs2.Gs2Distributor.Model.VerifyActionResult[] verifyResults) {
            this.VerifyResults = verifyResults;
            return this;
        }
        public TransactionResult WithConsumeResults(Gs2.Gs2Distributor.Model.ConsumeActionResult[] consumeResults) {
            this.ConsumeResults = consumeResults;
            return this;
        }
        public TransactionResult WithAcquireResults(Gs2.Gs2Distributor.Model.AcquireActionResult[] acquireResults) {
            this.AcquireResults = acquireResults;
            return this;
        }
        public TransactionResult WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public TransactionResult WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):transaction:result:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):transaction:result:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):transaction:result:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):transaction:result:(?<transactionId>.+)",
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

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):transaction:result:(?<transactionId>.+)",
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
        public static TransactionResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TransactionResult()
                .WithTransactionResultId(!data.Keys.Contains("transactionResultId") || data["transactionResultId"] == null ? null : data["transactionResultId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithVerifyResults(!data.Keys.Contains("verifyResults") || data["verifyResults"] == null || !data["verifyResults"].IsArray ? null : data["verifyResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Distributor.Model.VerifyActionResult.FromJson(v);
                }).ToArray())
                .WithConsumeResults(!data.Keys.Contains("consumeResults") || data["consumeResults"] == null || !data["consumeResults"].IsArray ? null : data["consumeResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Distributor.Model.ConsumeActionResult.FromJson(v);
                }).ToArray())
                .WithAcquireResults(!data.Keys.Contains("acquireResults") || data["acquireResults"] == null || !data["acquireResults"].IsArray ? null : data["acquireResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Distributor.Model.AcquireActionResult.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData verifyResultsJsonData = null;
            if (VerifyResults != null && VerifyResults.Length > 0)
            {
                verifyResultsJsonData = new JsonData();
                foreach (var verifyResult in VerifyResults)
                {
                    verifyResultsJsonData.Add(verifyResult.ToJson());
                }
            }
            JsonData consumeResultsJsonData = null;
            if (ConsumeResults != null && ConsumeResults.Length > 0)
            {
                consumeResultsJsonData = new JsonData();
                foreach (var consumeResult in ConsumeResults)
                {
                    consumeResultsJsonData.Add(consumeResult.ToJson());
                }
            }
            JsonData acquireResultsJsonData = null;
            if (AcquireResults != null && AcquireResults.Length > 0)
            {
                acquireResultsJsonData = new JsonData();
                foreach (var acquireResult in AcquireResults)
                {
                    acquireResultsJsonData.Add(acquireResult.ToJson());
                }
            }
            return new JsonData {
                ["transactionResultId"] = TransactionResultId,
                ["userId"] = UserId,
                ["transactionId"] = TransactionId,
                ["verifyResults"] = verifyResultsJsonData,
                ["consumeResults"] = consumeResultsJsonData,
                ["acquireResults"] = acquireResultsJsonData,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TransactionResultId != null) {
                writer.WritePropertyName("transactionResultId");
                writer.Write(TransactionResultId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (VerifyResults != null) {
                writer.WritePropertyName("verifyResults");
                writer.WriteArrayStart();
                foreach (var verifyResult in VerifyResults)
                {
                    if (verifyResult != null) {
                        verifyResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ConsumeResults != null) {
                writer.WritePropertyName("consumeResults");
                writer.WriteArrayStart();
                foreach (var consumeResult in ConsumeResults)
                {
                    if (consumeResult != null) {
                        consumeResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireResults != null) {
                writer.WritePropertyName("acquireResults");
                writer.WriteArrayStart();
                foreach (var acquireResult in AcquireResults)
                {
                    if (acquireResult != null) {
                        acquireResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            var other = obj as TransactionResult;
            var diff = 0;
            if (TransactionResultId == null && TransactionResultId == other.TransactionResultId)
            {
                // null and null
            }
            else
            {
                diff += TransactionResultId.CompareTo(other.TransactionResultId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (VerifyResults == null && VerifyResults == other.VerifyResults)
            {
                // null and null
            }
            else
            {
                diff += VerifyResults.Length - other.VerifyResults.Length;
                for (var i = 0; i < VerifyResults.Length; i++)
                {
                    diff += VerifyResults[i].CompareTo(other.VerifyResults[i]);
                }
            }
            if (ConsumeResults == null && ConsumeResults == other.ConsumeResults)
            {
                // null and null
            }
            else
            {
                diff += ConsumeResults.Length - other.ConsumeResults.Length;
                for (var i = 0; i < ConsumeResults.Length; i++)
                {
                    diff += ConsumeResults[i].CompareTo(other.ConsumeResults[i]);
                }
            }
            if (AcquireResults == null && AcquireResults == other.AcquireResults)
            {
                // null and null
            }
            else
            {
                diff += AcquireResults.Length - other.AcquireResults.Length;
                for (var i = 0; i < AcquireResults.Length; i++)
                {
                    diff += AcquireResults[i].CompareTo(other.AcquireResults[i]);
                }
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
                if (TransactionResultId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.transactionResultId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length < 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.transactionId.error.tooShort"),
                    });
                }
                if (TransactionId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyResults.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.verifyResults.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeResults.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.consumeResults.error.tooMany"),
                    });
                }
            }
            {
                if (AcquireResults.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.acquireResults.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "distributor.transactionResult.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new TransactionResult {
                TransactionResultId = TransactionResultId,
                UserId = UserId,
                TransactionId = TransactionId,
                VerifyResults = VerifyResults.Clone() as Gs2.Gs2Distributor.Model.VerifyActionResult[],
                ConsumeResults = ConsumeResults.Clone() as Gs2.Gs2Distributor.Model.ConsumeActionResult[],
                AcquireResults = AcquireResults.Clone() as Gs2.Gs2Distributor.Model.AcquireActionResult[],
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}