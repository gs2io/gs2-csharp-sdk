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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SubscribeTransaction : IComparable
	{
        public string SubscribeTransactionId { set; get; } = null!;
        public string ContentName { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public string Store { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string StatusDetail { set; get; } = null!;
        public long? ExpiresAt { set; get; } = null!;
        public long? LastAllocatedAt { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public SubscribeTransaction WithSubscribeTransactionId(string subscribeTransactionId) {
            this.SubscribeTransactionId = subscribeTransactionId;
            return this;
        }
        public SubscribeTransaction WithContentName(string contentName) {
            this.ContentName = contentName;
            return this;
        }
        public SubscribeTransaction WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public SubscribeTransaction WithStore(string store) {
            this.Store = store;
            return this;
        }
        public SubscribeTransaction WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SubscribeTransaction WithStatusDetail(string statusDetail) {
            this.StatusDetail = statusDetail;
            return this;
        }
        public SubscribeTransaction WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }
        public SubscribeTransaction WithLastAllocatedAt(long? lastAllocatedAt) {
            this.LastAllocatedAt = lastAllocatedAt;
            return this;
        }
        public SubscribeTransaction WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SubscribeTransaction WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public SubscribeTransaction WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):subscriptionTransaction:(?<contentName>.+):(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):subscriptionTransaction:(?<contentName>.+):(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):subscriptionTransaction:(?<contentName>.+):(?<transactionId>.+)",
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

        private static System.Text.RegularExpressions.Regex _contentNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):subscriptionTransaction:(?<contentName>.+):(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetContentNameFromGrn(
            string grn
        )
        {
            var match = _contentNameRegex.Match(grn);
            if (!match.Success || !match.Groups["contentName"].Success)
            {
                return null;
            }
            return match.Groups["contentName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):subscriptionTransaction:(?<contentName>.+):(?<transactionId>.+)",
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
        public static SubscribeTransaction FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscribeTransaction()
                .WithSubscribeTransactionId(!data.Keys.Contains("subscribeTransactionId") || data["subscribeTransactionId"] == null ? null : data["subscribeTransactionId"].ToString())
                .WithContentName(!data.Keys.Contains("contentName") || data["contentName"] == null ? null : data["contentName"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithStore(!data.Keys.Contains("store") || data["store"] == null ? null : data["store"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithStatusDetail(!data.Keys.Contains("statusDetail") || data["statusDetail"] == null ? null : data["statusDetail"].ToString())
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithLastAllocatedAt(!data.Keys.Contains("lastAllocatedAt") || data["lastAllocatedAt"] == null ? null : (long?)(data["lastAllocatedAt"].ToString().Contains(".") ? (long)double.Parse(data["lastAllocatedAt"].ToString()) : long.Parse(data["lastAllocatedAt"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscribeTransactionId"] = SubscribeTransactionId,
                ["contentName"] = ContentName,
                ["transactionId"] = TransactionId,
                ["store"] = Store,
                ["userId"] = UserId,
                ["statusDetail"] = StatusDetail,
                ["expiresAt"] = ExpiresAt,
                ["lastAllocatedAt"] = LastAllocatedAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SubscribeTransactionId != null) {
                writer.WritePropertyName("subscribeTransactionId");
                writer.Write(SubscribeTransactionId.ToString());
            }
            if (ContentName != null) {
                writer.WritePropertyName("contentName");
                writer.Write(ContentName.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (Store != null) {
                writer.WritePropertyName("store");
                writer.Write(Store.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (StatusDetail != null) {
                writer.WritePropertyName("statusDetail");
                writer.Write(StatusDetail.ToString());
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (LastAllocatedAt != null) {
                writer.WritePropertyName("lastAllocatedAt");
                writer.Write((LastAllocatedAt.ToString().Contains(".") ? (long)double.Parse(LastAllocatedAt.ToString()) : long.Parse(LastAllocatedAt.ToString())));
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
            var other = obj as SubscribeTransaction;
            var diff = 0;
            if (SubscribeTransactionId == null && SubscribeTransactionId == other.SubscribeTransactionId)
            {
                // null and null
            }
            else
            {
                diff += SubscribeTransactionId.CompareTo(other.SubscribeTransactionId);
            }
            if (ContentName == null && ContentName == other.ContentName)
            {
                // null and null
            }
            else
            {
                diff += ContentName.CompareTo(other.ContentName);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (Store == null && Store == other.Store)
            {
                // null and null
            }
            else
            {
                diff += Store.CompareTo(other.Store);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (StatusDetail == null && StatusDetail == other.StatusDetail)
            {
                // null and null
            }
            else
            {
                diff += StatusDetail.CompareTo(other.StatusDetail);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
            }
            if (LastAllocatedAt == null && LastAllocatedAt == other.LastAllocatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(LastAllocatedAt - other.LastAllocatedAt);
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
                if (SubscribeTransactionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.subscribeTransactionId.error.tooLong"),
                    });
                }
            }
            {
                if (ContentName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.contentName.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.transactionId.error.tooLong"),
                    });
                }
            }
            {
                switch (Store) {
                    case "AppleAppStore":
                    case "GooglePlay":
                    case "fake":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("subscribeTransaction", "money2.subscribeTransaction.store.error.invalid"),
                        });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.userId.error.tooLong"),
                    });
                }
            }
            {
                switch (StatusDetail) {
                    case "active@active":
                    case "active@converted_from_trial":
                    case "active@in_trial":
                    case "active@in_intro_offer":
                    case "grace@canceled":
                    case "grace@grace_period":
                    case "grace@on_hold":
                    case "inactive@expired":
                    case "inactive@revoked":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("subscribeTransaction", "money2.subscribeTransaction.statusDetail.error.invalid"),
                        });
                }
            }
            {
                if (ExpiresAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.expiresAt.error.invalid"),
                    });
                }
                if (ExpiresAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.expiresAt.error.invalid"),
                    });
                }
            }
            {
                if (LastAllocatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.lastAllocatedAt.error.invalid"),
                    });
                }
                if (LastAllocatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.lastAllocatedAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeTransaction", "money2.subscribeTransaction.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SubscribeTransaction {
                SubscribeTransactionId = SubscribeTransactionId,
                ContentName = ContentName,
                TransactionId = TransactionId,
                Store = Store,
                UserId = UserId,
                StatusDetail = StatusDetail,
                ExpiresAt = ExpiresAt,
                LastAllocatedAt = LastAllocatedAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}