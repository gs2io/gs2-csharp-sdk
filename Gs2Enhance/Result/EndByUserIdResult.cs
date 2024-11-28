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
using Gs2.Gs2Enhance.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Enhance.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class EndByUserIdResult : IResult
	{
        public Gs2.Gs2Enhance.Model.Progress Item { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public string StampSheet { set; get; } = null!;
        public string StampSheetEncryptionKeyId { set; get; } = null!;
        public bool? AutoRunStampSheet { set; get; } = null!;
        public bool? AtomicCommit { set; get; } = null!;
        public string Transaction { set; get; } = null!;
        public Gs2.Core.Model.TransactionResult TransactionResult { set; get; } = null!;
        public long? AcquireExperience { set; get; } = null!;
        public float? BonusRate { set; get; } = null!;

        public EndByUserIdResult WithItem(Gs2.Gs2Enhance.Model.Progress item) {
            this.Item = item;
            return this;
        }

        public EndByUserIdResult WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }

        public EndByUserIdResult WithStampSheet(string stampSheet) {
            this.StampSheet = stampSheet;
            return this;
        }

        public EndByUserIdResult WithStampSheetEncryptionKeyId(string stampSheetEncryptionKeyId) {
            this.StampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
            return this;
        }

        public EndByUserIdResult WithAutoRunStampSheet(bool? autoRunStampSheet) {
            this.AutoRunStampSheet = autoRunStampSheet;
            return this;
        }

        public EndByUserIdResult WithAtomicCommit(bool? atomicCommit) {
            this.AtomicCommit = atomicCommit;
            return this;
        }

        public EndByUserIdResult WithTransaction(string transaction) {
            this.Transaction = transaction;
            return this;
        }

        public EndByUserIdResult WithTransactionResult(Gs2.Core.Model.TransactionResult transactionResult) {
            this.TransactionResult = transactionResult;
            return this;
        }

        public EndByUserIdResult WithAcquireExperience(long? acquireExperience) {
            this.AcquireExperience = acquireExperience;
            return this;
        }

        public EndByUserIdResult WithBonusRate(float? bonusRate) {
            this.BonusRate = bonusRate;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static EndByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new EndByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Enhance.Model.Progress.FromJson(data["item"]))
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithStampSheet(!data.Keys.Contains("stampSheet") || data["stampSheet"] == null ? null : data["stampSheet"].ToString())
                .WithStampSheetEncryptionKeyId(!data.Keys.Contains("stampSheetEncryptionKeyId") || data["stampSheetEncryptionKeyId"] == null ? null : data["stampSheetEncryptionKeyId"].ToString())
                .WithAutoRunStampSheet(!data.Keys.Contains("autoRunStampSheet") || data["autoRunStampSheet"] == null ? null : (bool?)bool.Parse(data["autoRunStampSheet"].ToString()))
                .WithAtomicCommit(!data.Keys.Contains("atomicCommit") || data["atomicCommit"] == null ? null : (bool?)bool.Parse(data["atomicCommit"].ToString()))
                .WithTransaction(!data.Keys.Contains("transaction") || data["transaction"] == null ? null : data["transaction"].ToString())
                .WithTransactionResult(!data.Keys.Contains("transactionResult") || data["transactionResult"] == null ? null : Gs2.Core.Model.TransactionResult.FromJson(data["transactionResult"]))
                .WithAcquireExperience(!data.Keys.Contains("acquireExperience") || data["acquireExperience"] == null ? null : (long?)(data["acquireExperience"].ToString().Contains(".") ? (long)double.Parse(data["acquireExperience"].ToString()) : long.Parse(data["acquireExperience"].ToString())))
                .WithBonusRate(!data.Keys.Contains("bonusRate") || data["bonusRate"] == null ? null : (float?)float.Parse(data["bonusRate"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["transactionId"] = TransactionId,
                ["stampSheet"] = StampSheet,
                ["stampSheetEncryptionKeyId"] = StampSheetEncryptionKeyId,
                ["autoRunStampSheet"] = AutoRunStampSheet,
                ["atomicCommit"] = AtomicCommit,
                ["transaction"] = Transaction,
                ["transactionResult"] = TransactionResult?.ToJson(),
                ["acquireExperience"] = AcquireExperience,
                ["bonusRate"] = BonusRate,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (StampSheet != null) {
                writer.WritePropertyName("stampSheet");
                writer.Write(StampSheet.ToString());
            }
            if (StampSheetEncryptionKeyId != null) {
                writer.WritePropertyName("stampSheetEncryptionKeyId");
                writer.Write(StampSheetEncryptionKeyId.ToString());
            }
            if (AutoRunStampSheet != null) {
                writer.WritePropertyName("autoRunStampSheet");
                writer.Write(bool.Parse(AutoRunStampSheet.ToString()));
            }
            if (AtomicCommit != null) {
                writer.WritePropertyName("atomicCommit");
                writer.Write(bool.Parse(AtomicCommit.ToString()));
            }
            if (Transaction != null) {
                writer.WritePropertyName("transaction");
                writer.Write(Transaction.ToString());
            }
            if (TransactionResult != null) {
                TransactionResult.WriteJson(writer);
            }
            if (AcquireExperience != null) {
                writer.WritePropertyName("acquireExperience");
                writer.Write((AcquireExperience.ToString().Contains(".") ? (long)double.Parse(AcquireExperience.ToString()) : long.Parse(AcquireExperience.ToString())));
            }
            if (BonusRate != null) {
                writer.WritePropertyName("bonusRate");
                writer.Write(float.Parse(BonusRate.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}