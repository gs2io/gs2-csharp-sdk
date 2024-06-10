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
	public class LotteryModelMaster : IComparable
	{
        public string LotteryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Mode { set; get; } = null!;
        public string Method { set; get; } = null!;
        public string PrizeTableName { set; get; } = null!;
        public string ChoicePrizeTableScriptId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public LotteryModelMaster WithLotteryModelId(string lotteryModelId) {
            this.LotteryModelId = lotteryModelId;
            return this;
        }
        public LotteryModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public LotteryModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public LotteryModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public LotteryModelMaster WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public LotteryModelMaster WithMethod(string method) {
            this.Method = method;
            return this;
        }
        public LotteryModelMaster WithPrizeTableName(string prizeTableName) {
            this.PrizeTableName = prizeTableName;
            return this;
        }
        public LotteryModelMaster WithChoicePrizeTableScriptId(string choicePrizeTableScriptId) {
            this.ChoicePrizeTableScriptId = choicePrizeTableScriptId;
            return this;
        }
        public LotteryModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public LotteryModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public LotteryModelMaster WithRevision(long? revision) {
            this.Revision = revision;
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
        public static LotteryModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LotteryModelMaster()
                .WithLotteryModelId(!data.Keys.Contains("lotteryModelId") || data["lotteryModelId"] == null ? null : data["lotteryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithPrizeTableName(!data.Keys.Contains("prizeTableName") || data["prizeTableName"] == null ? null : data["prizeTableName"].ToString())
                .WithChoicePrizeTableScriptId(!data.Keys.Contains("choicePrizeTableScriptId") || data["choicePrizeTableScriptId"] == null ? null : data["choicePrizeTableScriptId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["lotteryModelId"] = LotteryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["description"] = Description,
                ["mode"] = Mode,
                ["method"] = Method,
                ["prizeTableName"] = PrizeTableName,
                ["choicePrizeTableScriptId"] = ChoicePrizeTableScriptId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
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
            var other = obj as LotteryModelMaster;
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
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
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
                if (LotteryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.lotteryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.description.error.tooLong"),
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
                            new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.mode.error.invalid"),
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
                            new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.method.error.invalid"),
                        });
                }
            }
            if (Method == "prize_table") {
                if (PrizeTableName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.prizeTableName.error.tooLong"),
                    });
                }
            }
            if (Method == "script") {
                if (ChoicePrizeTableScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.choicePrizeTableScriptId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("lotteryModelMaster", "lottery.lotteryModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new LotteryModelMaster {
                LotteryModelId = LotteryModelId,
                Name = Name,
                Metadata = Metadata,
                Description = Description,
                Mode = Mode,
                Method = Method,
                PrizeTableName = PrizeTableName,
                ChoicePrizeTableScriptId = ChoicePrizeTableScriptId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}