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
using UnityEngine.Scripting;

namespace Gs2.Gs2Enhance.Result
{
	[Preserve]
	[System.Serializable]
	public class DirectEnhanceByStampSheetResult : IResult
	{
        public Gs2.Gs2Enhance.Model.RateModel Item { set; get; }
        public string StampSheet { set; get; }
        public string StampSheetEncryptionKeyId { set; get; }
        public long? AcquireExperience { set; get; }
        public float? BonusRate { set; get; }

        public DirectEnhanceByStampSheetResult WithItem(Gs2.Gs2Enhance.Model.RateModel item) {
            this.Item = item;
            return this;
        }

        public DirectEnhanceByStampSheetResult WithStampSheet(string stampSheet) {
            this.StampSheet = stampSheet;
            return this;
        }

        public DirectEnhanceByStampSheetResult WithStampSheetEncryptionKeyId(string stampSheetEncryptionKeyId) {
            this.StampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
            return this;
        }

        public DirectEnhanceByStampSheetResult WithAcquireExperience(long? acquireExperience) {
            this.AcquireExperience = acquireExperience;
            return this;
        }

        public DirectEnhanceByStampSheetResult WithBonusRate(float? bonusRate) {
            this.BonusRate = bonusRate;
            return this;
        }

    	[Preserve]
        public static DirectEnhanceByStampSheetResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DirectEnhanceByStampSheetResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Enhance.Model.RateModel.FromJson(data["item"]))
                .WithStampSheet(!data.Keys.Contains("stampSheet") || data["stampSheet"] == null ? null : data["stampSheet"].ToString())
                .WithStampSheetEncryptionKeyId(!data.Keys.Contains("stampSheetEncryptionKeyId") || data["stampSheetEncryptionKeyId"] == null ? null : data["stampSheetEncryptionKeyId"].ToString())
                .WithAcquireExperience(!data.Keys.Contains("acquireExperience") || data["acquireExperience"] == null ? null : (long?)long.Parse(data["acquireExperience"].ToString()))
                .WithBonusRate(!data.Keys.Contains("bonusRate") || data["bonusRate"] == null ? null : (float?)float.Parse(data["bonusRate"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["stampSheet"] = StampSheet,
                ["stampSheetEncryptionKeyId"] = StampSheetEncryptionKeyId,
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
            if (StampSheet != null) {
                writer.WritePropertyName("stampSheet");
                writer.Write(StampSheet.ToString());
            }
            if (StampSheetEncryptionKeyId != null) {
                writer.WritePropertyName("stampSheetEncryptionKeyId");
                writer.Write(StampSheetEncryptionKeyId.ToString());
            }
            if (AcquireExperience != null) {
                writer.WritePropertyName("acquireExperience");
                writer.Write(long.Parse(AcquireExperience.ToString()));
            }
            if (BonusRate != null) {
                writer.WritePropertyName("bonusRate");
                writer.Write(float.Parse(BonusRate.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}