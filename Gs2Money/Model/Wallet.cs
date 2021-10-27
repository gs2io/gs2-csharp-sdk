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
        public string WalletId { set; get; }
        public string UserId { set; get; }
        public int? Slot { set; get; }
        public int? Paid { set; get; }
        public int? Free { set; get; }
        public Gs2.Gs2Money.Model.WalletDetail[] Detail { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

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

        public Wallet WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Wallet WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
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
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)int.Parse(data["slot"].ToString()))
                .WithPaid(!data.Keys.Contains("paid") || data["paid"] == null ? null : (int?)int.Parse(data["paid"].ToString()))
                .WithFree(!data.Keys.Contains("free") || data["free"] == null ? null : (int?)int.Parse(data["free"].ToString()))
                .WithDetail(!data.Keys.Contains("detail") || data["detail"] == null ? new Gs2.Gs2Money.Model.WalletDetail[]{} : data["detail"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money.Model.WalletDetail.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["walletId"] = WalletId,
                ["userId"] = UserId,
                ["slot"] = Slot,
                ["paid"] = Paid,
                ["free"] = Free,
                ["detail"] = new JsonData(Detail == null ? new JsonData[]{} :
                        Detail.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
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
                writer.Write(int.Parse(Slot.ToString()));
            }
            if (Paid != null) {
                writer.WritePropertyName("paid");
                writer.Write(int.Parse(Paid.ToString()));
            }
            if (Free != null) {
                writer.WritePropertyName("free");
                writer.Write(int.Parse(Free.ToString()));
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
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
            return diff;
        }
    }
}