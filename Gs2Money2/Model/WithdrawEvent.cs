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
	public class WithdrawEvent : IComparable
	{
        public int? Slot { set; get; } = null!;
        public Gs2.Gs2Money2.Model.DepositTransaction[] WithdrawDetails { set; get; } = null!;
        public Gs2.Gs2Money2.Model.WalletSummary Status { set; get; } = null!;
        public WithdrawEvent WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }
        public WithdrawEvent WithWithdrawDetails(Gs2.Gs2Money2.Model.DepositTransaction[] withdrawDetails) {
            this.WithdrawDetails = withdrawDetails;
            return this;
        }
        public WithdrawEvent WithStatus(Gs2.Gs2Money2.Model.WalletSummary status) {
            this.Status = status;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WithdrawEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WithdrawEvent()
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)(data["slot"].ToString().Contains(".") ? (int)double.Parse(data["slot"].ToString()) : int.Parse(data["slot"].ToString())))
                .WithWithdrawDetails(!data.Keys.Contains("withdrawDetails") || data["withdrawDetails"] == null || !data["withdrawDetails"].IsArray ? null : data["withdrawDetails"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money2.Model.DepositTransaction.FromJson(v);
                }).ToArray())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : Gs2.Gs2Money2.Model.WalletSummary.FromJson(data["status"]));
        }

        public JsonData ToJson()
        {
            JsonData withdrawDetailsJsonData = null;
            if (WithdrawDetails != null && WithdrawDetails.Length > 0)
            {
                withdrawDetailsJsonData = new JsonData();
                foreach (var withdrawDetail in WithdrawDetails)
                {
                    withdrawDetailsJsonData.Add(withdrawDetail.ToJson());
                }
            }
            return new JsonData {
                ["slot"] = Slot,
                ["withdrawDetails"] = withdrawDetailsJsonData,
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
            if (WithdrawDetails != null) {
                writer.WritePropertyName("withdrawDetails");
                writer.WriteArrayStart();
                foreach (var withdrawDetail in WithdrawDetails)
                {
                    if (withdrawDetail != null) {
                        withdrawDetail.WriteJson(writer);
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
            var other = obj as WithdrawEvent;
            var diff = 0;
            if (Slot == null && Slot == other.Slot)
            {
                // null and null
            }
            else
            {
                diff += (int)(Slot - other.Slot);
            }
            if (WithdrawDetails == null && WithdrawDetails == other.WithdrawDetails)
            {
                // null and null
            }
            else
            {
                diff += WithdrawDetails.Length - other.WithdrawDetails.Length;
                for (var i = 0; i < WithdrawDetails.Length; i++)
                {
                    diff += WithdrawDetails[i].CompareTo(other.WithdrawDetails[i]);
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
                        new RequestError("withdrawEvent", "money2.withdrawEvent.slot.error.invalid"),
                    });
                }
                if (Slot > 100000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("withdrawEvent", "money2.withdrawEvent.slot.error.invalid"),
                    });
                }
            }
            {
                if (WithdrawDetails.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("withdrawEvent", "money2.withdrawEvent.withdrawDetails.error.tooMany"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new WithdrawEvent {
                Slot = Slot,
                WithdrawDetails = WithdrawDetails.Clone() as Gs2.Gs2Money2.Model.DepositTransaction[],
                Status = Status.Clone() as Gs2.Gs2Money2.Model.WalletSummary,
            };
        }
    }
}