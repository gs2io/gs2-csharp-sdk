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
        public string LotteryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Mode { set; get; } = null!;
        public string Method { set; get; } = null!;
        public string PrizeTableName { set; get; } = null!;
        public string ChoicePrizeTableScriptId { set; get; } = null!;
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

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):lotteryModel:(?<lotteryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):lotteryModel:(?<lotteryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):lotteryModel:(?<lotteryName>.+)",
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

        private static System.Text.RegularExpressions.Regex _lotteryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):lotteryModel:(?<lotteryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetLotteryNameFromGrn(
            string grn
        )
        {
            var match = _lotteryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["lotteryName"].Success)
            {
                return null;
            }
            return match.Groups["lotteryName"].Value;
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

        public void Validate() {
            {
                if (LotteryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModel", "lottery.lotteryModel.lotteryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModel", "lottery.lotteryModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModel", "lottery.lotteryModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                switch (Mode) {
                    case "normal":
                    case "box":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("lotteryModel", "lottery.lotteryModel.mode.error.invalid"),
                        });
                }
            }
            {
                switch (Method) {
                    case "prize_table":
                    case "script":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("lotteryModel", "lottery.lotteryModel.method.error.invalid"),
                        });
                }
            }
            if (Method == "prize_table") {
                if (PrizeTableName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModel", "lottery.lotteryModel.prizeTableName.error.tooLong"),
                    });
                }
            }
            if (Method == "script") {
                if (ChoicePrizeTableScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModel", "lottery.lotteryModel.choicePrizeTableScriptId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new LotteryModel {
                LotteryModelId = LotteryModelId,
                Name = Name,
                Metadata = Metadata,
                Mode = Mode,
                Method = Method,
                PrizeTableName = PrizeTableName,
                ChoicePrizeTableScriptId = ChoicePrizeTableScriptId,
            };
        }
    }
}