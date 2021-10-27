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

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class LotteryModel : IComparable
	{
        public string LotteryModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string Mode { set; get; }
        public string Method { set; get; }
        public string PrizeTableName { set; get; }
        public string ChoicePrizeTableScriptId { set; get; }

        public LotteryModel WithLotteryModelId(string lotteryModelId) {
            this.LotteryModelId = lotteryModelId;
            return this;
        }

        public LotteryModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public LotteryModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public LotteryModel WithMode(string mode) {
            this.Mode = mode;
            return this;
        }

        public LotteryModel WithMethod(string method) {
            this.Method = method;
            return this;
        }

        public LotteryModel WithPrizeTableName(string prizeTableName) {
            this.PrizeTableName = prizeTableName;
            return this;
        }

        public LotteryModel WithChoicePrizeTableScriptId(string choicePrizeTableScriptId) {
            this.ChoicePrizeTableScriptId = choicePrizeTableScriptId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LotteryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LotteryModel()
                .WithLotteryModelId(!data.Keys.Contains("lotteryModelId") || data["lotteryModelId"] == null ? null : data["lotteryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithPrizeTableName(!data.Keys.Contains("prizeTableName") || data["prizeTableName"] == null ? null : data["prizeTableName"].ToString())
                .WithChoicePrizeTableScriptId(!data.Keys.Contains("choicePrizeTableScriptId") || data["choicePrizeTableScriptId"] == null ? null : data["choicePrizeTableScriptId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["lotteryModelId"] = LotteryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["mode"] = Mode,
                ["method"] = Method,
                ["prizeTableName"] = PrizeTableName,
                ["choicePrizeTableScriptId"] = ChoicePrizeTableScriptId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (LotteryModelId != null) {
                writer.WritePropertyName("lotteryModelId");
                writer.Write(LotteryModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Mode != null) {
                writer.WritePropertyName("mode");
                writer.Write(Mode.ToString());
            }
            if (Method != null) {
                writer.WritePropertyName("method");
                writer.Write(Method.ToString());
            }
            if (PrizeTableName != null) {
                writer.WritePropertyName("prizeTableName");
                writer.Write(PrizeTableName.ToString());
            }
            if (ChoicePrizeTableScriptId != null) {
                writer.WritePropertyName("choicePrizeTableScriptId");
                writer.Write(ChoicePrizeTableScriptId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as LotteryModel;
            var diff = 0;
            if (LotteryModelId == null && LotteryModelId == other.LotteryModelId)
            {
                // null and null
            }
            else
            {
                diff += LotteryModelId.CompareTo(other.LotteryModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Mode == null && Mode == other.Mode)
            {
                // null and null
            }
            else
            {
                diff += Mode.CompareTo(other.Mode);
            }
            if (Method == null && Method == other.Method)
            {
                // null and null
            }
            else
            {
                diff += Method.CompareTo(other.Method);
            }
            if (PrizeTableName == null && PrizeTableName == other.PrizeTableName)
            {
                // null and null
            }
            else
            {
                diff += PrizeTableName.CompareTo(other.PrizeTableName);
            }
            if (ChoicePrizeTableScriptId == null && ChoicePrizeTableScriptId == other.ChoicePrizeTableScriptId)
            {
                // null and null
            }
            else
            {
                diff += ChoicePrizeTableScriptId.CompareTo(other.ChoicePrizeTableScriptId);
            }
            return diff;
        }
    }
}