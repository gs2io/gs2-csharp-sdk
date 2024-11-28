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

namespace Gs2.Gs2Money.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Wallet : IComparable
	{
        public string WalletId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public int? Slot { set; get; } = null!;
        public int? Paid { set; get; } = null!;
        public int? Free { set; get; } = null!;
        public Gs2.Gs2Money.Model.WalletDetail[] Detail { set; get; } = null!;
        public bool? ShareFree { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Wallet WithWalletId(string walletId) {
            this.WalletId = walletId;
            return this;
        }
        public Wallet WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Wallet WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }
        public Wallet WithPaid(int? paid) {
            this.Paid = paid;
            return this;
        }
        public Wallet WithFree(int? free) {
            this.Free = free;
            return this;
        }
        public Wallet WithDetail(Gs2.Gs2Money.Model.WalletDetail[] detail) {
            this.Detail = detail;
            return this;
        }
        public Wallet WithShareFree(bool? shareFree) {
            this.ShareFree = shareFree;
            return this;
        }
        public Wallet WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Wallet WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Wallet WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
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

        private static System.Text.RegularExpressions.Regex _slotRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money:(?<namespaceName>.+):user:(?<userId>.+):wallet:(?<slot>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSlotFromGrn(
            string grn
        )
        {
            var match = _slotRegex.Match(grn);
            if (!match.Success || !match.Groups["slot"].Success)
            {
                return null;
            }
            return match.Groups["slot"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Wallet FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Wallet()
                .WithWalletId(!data.Keys.Contains("walletId") || data["walletId"] == null ? null : data["walletId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)(data["slot"].ToString().Contains(".") ? (int)double.Parse(data["slot"].ToString()) : int.Parse(data["slot"].ToString())))
                .WithPaid(!data.Keys.Contains("paid") || data["paid"] == null ? null : (int?)(data["paid"].ToString().Contains(".") ? (int)double.Parse(data["paid"].ToString()) : int.Parse(data["paid"].ToString())))
                .WithFree(!data.Keys.Contains("free") || data["free"] == null ? null : (int?)(data["free"].ToString().Contains(".") ? (int)double.Parse(data["free"].ToString()) : int.Parse(data["free"].ToString())))
                .WithDetail(!data.Keys.Contains("detail") || data["detail"] == null || !data["detail"].IsArray ? null : data["detail"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money.Model.WalletDetail.FromJson(v);
                }).ToArray())
                .WithShareFree(!data.Keys.Contains("shareFree") || data["shareFree"] == null ? null : (bool?)bool.Parse(data["shareFree"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData detailJsonData = null;
            if (Detail != null && Detail.Length > 0)
            {
                detailJsonData = new JsonData();
                foreach (var detai in Detail)
                {
                    detailJsonData.Add(detai.ToJson());
                }
            }
            return new JsonData {
                ["walletId"] = WalletId,
                ["userId"] = UserId,
                ["slot"] = Slot,
                ["paid"] = Paid,
                ["free"] = Free,
                ["detail"] = detailJsonData,
                ["shareFree"] = ShareFree,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (WalletId != null) {
                writer.WritePropertyName("walletId");
                writer.Write(WalletId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Slot != null) {
                writer.WritePropertyName("slot");
                writer.Write((Slot.ToString().Contains(".") ? (int)double.Parse(Slot.ToString()) : int.Parse(Slot.ToString())));
            }
            if (Paid != null) {
                writer.WritePropertyName("paid");
                writer.Write((Paid.ToString().Contains(".") ? (int)double.Parse(Paid.ToString()) : int.Parse(Paid.ToString())));
            }
            if (Free != null) {
                writer.WritePropertyName("free");
                writer.Write((Free.ToString().Contains(".") ? (int)double.Parse(Free.ToString()) : int.Parse(Free.ToString())));
            }
            if (Detail != null) {
                writer.WritePropertyName("detail");
                writer.WriteArrayStart();
                foreach (var detai in Detail)
                {
                    if (detai != null) {
                        detai.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ShareFree != null) {
                writer.WritePropertyName("shareFree");
                writer.Write(bool.Parse(ShareFree.ToString()));
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
            var other = obj as Wallet;
            var diff = 0;
            if (WalletId == null && WalletId == other.WalletId)
            {
                // null and null
            }
            else
            {
                diff += WalletId.CompareTo(other.WalletId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Slot == null && Slot == other.Slot)
            {
                // null and null
            }
            else
            {
                diff += (int)(Slot - other.Slot);
            }
            if (Paid == null && Paid == other.Paid)
            {
                // null and null
            }
            else
            {
                diff += (int)(Paid - other.Paid);
            }
            if (Free == null && Free == other.Free)
            {
                // null and null
            }
            else
            {
                diff += (int)(Free - other.Free);
            }
            if (Detail == null && Detail == other.Detail)
            {
                // null and null
            }
            else
            {
                diff += Detail.Length - other.Detail.Length;
                for (var i = 0; i < Detail.Length; i++)
                {
                    diff += Detail[i].CompareTo(other.Detail[i]);
                }
            }
            if (ShareFree == null && ShareFree == other.ShareFree)
            {
                // null and null
            }
            else
            {
                diff += ShareFree == other.ShareFree ? 0 : 1;
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
                if (WalletId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.walletId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Slot < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.slot.error.invalid"),
                    });
                }
                if (Slot > 100000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.slot.error.invalid"),
                    });
                }
            }
            {
                if (Paid < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.paid.error.invalid"),
                    });
                }
                if (Paid > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.paid.error.invalid"),
                    });
                }
            }
            {
                if (Free < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.free.error.invalid"),
                    });
                }
                if (Free > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.free.error.invalid"),
                    });
                }
            }
            {
                if (Detail.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.detail.error.tooMany"),
                    });
                }
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("wallet", "money.wallet.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Wallet {
                WalletId = WalletId,
                UserId = UserId,
                Slot = Slot,
                Paid = Paid,
                Free = Free,
                Detail = Detail.Clone() as Gs2.Gs2Money.Model.WalletDetail[],
                ShareFree = ShareFree,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}