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
	public class DepositEvent : IComparable
	{
        public int? Slot { set; get; } = null!;
        public Gs2.Gs2Money2.Model.DepositTransaction[] DepositTransactions { set; get; } = null!;
        public Gs2.Gs2Money2.Model.WalletSummary Status { set; get; } = null!;
        public DepositEvent WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }
        public DepositEvent WithDepositTransactions(Gs2.Gs2Money2.Model.DepositTransaction[] depositTransactions) {
            this.DepositTransactions = depositTransactions;
            return this;
        }
        public DepositEvent WithStatus(Gs2.Gs2Money2.Model.WalletSummary status) {
            this.Status = status;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DepositEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DepositEvent()
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)(data["slot"].ToString().Contains(".") ? (int)double.Parse(data["slot"].ToString()) : int.Parse(data["slot"].ToString())))
                .WithDepositTransactions(!data.Keys.Contains("depositTransactions") || data["depositTransactions"] == null || !data["depositTransactions"].IsArray ? null : data["depositTransactions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money2.Model.DepositTransaction.FromJson(v);
                }).ToArray())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : Gs2.Gs2Money2.Model.WalletSummary.FromJson(data["status"]));
        }

        public JsonData ToJson()
        {
            JsonData depositTransactionsJsonData = null;
            if (DepositTransactions != null && DepositTransactions.Length > 0)
            {
                depositTransactionsJsonData = new JsonData();
                foreach (var depositTransaction in DepositTransactions)
                {
                    depositTransactionsJsonData.Add(depositTransaction.ToJson());
                }
            }
            return new JsonData {
                ["slot"] = Slot,
                ["depositTransactions"] = depositTransactionsJsonData,
                ["status"] = Status?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Slot != null) {
                writer.WritePropertyName("slot");
                writer.Write((Slot.ToString().Contains(".") ? (int)double.Parse(Slot.ToString()) : int.Parse(Slot.ToString())));
            }
            if (DepositTransactions != null) {
                writer.WritePropertyName("depositTransactions");
                writer.WriteArrayStart();
                foreach (var depositTransaction in DepositTransactions)
                {
                    if (depositTransaction != null) {
                        depositTransaction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                Status.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DepositEvent;
            var diff = 0;
            if (Slot == null && Slot == other.Slot)
            {
                // null and null
            }
            else
            {
                diff += (int)(Slot - other.Slot);
            }
            if (DepositTransactions == null && DepositTransactions == other.DepositTransactions)
            {
                // null and null
            }
            else
            {
                diff += DepositTransactions.Length - other.DepositTransactions.Length;
                for (var i = 0; i < DepositTransactions.Length; i++)
                {
                    diff += DepositTransactions[i].CompareTo(other.DepositTransactions[i]);
                }
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Slot < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositEvent", "money2.depositEvent.slot.error.invalid"),
                    });
                }
                if (Slot > 100000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositEvent", "money2.depositEvent.slot.error.invalid"),
                    });
                }
            }
            {
                if (DepositTransactions.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("depositEvent", "money2.depositEvent.depositTransactions.error.tooMany"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new DepositEvent {
                Slot = Slot,
                DepositTransactions = DepositTransactions.Clone() as Gs2.Gs2Money2.Model.DepositTransaction[],
                Status = Status.Clone() as Gs2.Gs2Money2.Model.WalletSummary,
            };
        }
    }
}