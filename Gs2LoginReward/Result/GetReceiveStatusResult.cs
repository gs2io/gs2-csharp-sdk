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
using Gs2.Gs2LoginReward.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2LoginReward.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetReceiveStatusResult : IResult
	{
        public Gs2.Gs2LoginReward.Model.ReceiveStatus Item { set; get; }
        public Gs2.Gs2LoginReward.Model.BonusModel BonusModel { set; get; }
        public ResultMetadata Metadata { set; get; }

        public GetReceiveStatusResult WithItem(Gs2.Gs2LoginReward.Model.ReceiveStatus item) {
            this.Item = item;
            return this;
        }

        public GetReceiveStatusResult WithBonusModel(Gs2.Gs2LoginReward.Model.BonusModel bonusModel) {
            this.BonusModel = bonusModel;
            return this;
        }

        public GetReceiveStatusResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetReceiveStatusResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetReceiveStatusResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2LoginReward.Model.ReceiveStatus.FromJson(data["item"]))
                .WithBonusModel(!data.Keys.Contains("bonusModel") || data["bonusModel"] == null ? null : Gs2.Gs2LoginReward.Model.BonusModel.FromJson(data["bonusModel"]))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["bonusModel"] = BonusModel?.ToJson(),
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (BonusModel != null) {
                BonusModel.WriteJson(writer);
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}