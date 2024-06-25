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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Money2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class WithdrawByStampTaskResult : IResult
	{
        public Gs2.Gs2Money2.Model.Wallet Item { set; get; } = null!;
        public Gs2.Gs2Money2.Model.DepositTransaction[] WithdrawTransactions { set; get; } = null!;
        public string NewContextStack { set; get; } = null!;

        public WithdrawByStampTaskResult WithItem(Gs2.Gs2Money2.Model.Wallet item) {
            this.Item = item;
            return this;
        }

        public WithdrawByStampTaskResult WithWithdrawTransactions(Gs2.Gs2Money2.Model.DepositTransaction[] withdrawTransactions) {
            this.WithdrawTransactions = withdrawTransactions;
            return this;
        }

        public WithdrawByStampTaskResult WithNewContextStack(string newContextStack) {
            this.NewContextStack = newContextStack;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WithdrawByStampTaskResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WithdrawByStampTaskResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Money2.Model.Wallet.FromJson(data["item"]))
                .WithWithdrawTransactions(!data.Keys.Contains("withdrawTransactions") || data["withdrawTransactions"] == null || !data["withdrawTransactions"].IsArray ? new Gs2.Gs2Money2.Model.DepositTransaction[]{} : data["withdrawTransactions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money2.Model.DepositTransaction.FromJson(v);
                }).ToArray())
                .WithNewContextStack(!data.Keys.Contains("newContextStack") || data["newContextStack"] == null ? null : data["newContextStack"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData withdrawTransactionsJsonData = null;
            if (WithdrawTransactions != null && WithdrawTransactions.Length > 0)
            {
                withdrawTransactionsJsonData = new JsonData();
                foreach (var withdrawTransaction in WithdrawTransactions)
                {
                    withdrawTransactionsJsonData.Add(withdrawTransaction.ToJson());
                }
            }
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["withdrawTransactions"] = withdrawTransactionsJsonData,
                ["newContextStack"] = NewContextStack,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (WithdrawTransactions != null) {
                writer.WritePropertyName("withdrawTransactions");
                writer.WriteArrayStart();
                foreach (var withdrawTransaction in WithdrawTransactions)
                {
                    if (withdrawTransaction != null) {
                        withdrawTransaction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (NewContextStack != null) {
                writer.WritePropertyName("newContextStack");
                writer.Write(NewContextStack.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}