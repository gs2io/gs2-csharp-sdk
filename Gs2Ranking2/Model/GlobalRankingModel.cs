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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class GlobalRankingModel : IComparable
	{
        public string GlobalRankingModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public long? MinimumValue { set; get; }
        public long? MaximumValue { set; get; }
        public bool? Sum { set; get; }
        public string OrderDirection { set; get; }
        public string EntryPeriodEventId { set; get; }
        public Gs2.Gs2Ranking2.Model.RankingReward[] RankingRewards { set; get; }
        public string AccessPeriodEventId { set; get; }
        public string RewardCalculationIndex { set; get; }
        public GlobalRankingModel WithGlobalRankingModelId(string globalRankingModelId) {
            this.GlobalRankingModelId = globalRankingModelId;
            return this;
        }
        public GlobalRankingModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public GlobalRankingModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public GlobalRankingModel WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public GlobalRankingModel WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public GlobalRankingModel WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public GlobalRankingModel WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public GlobalRankingModel WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public GlobalRankingModel WithRankingRewards(Gs2.Gs2Ranking2.Model.RankingReward[] rankingRewards) {
            this.RankingRewards = rankingRewards;
            return this;
        }
        public GlobalRankingModel WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }
        public GlobalRankingModel WithRewardCalculationIndex(string rewardCalculationIndex) {
            this.RewardCalculationIndex = rewardCalculationIndex;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):global:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):global:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):global:(?<rankingName>.+)",
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

        private static System.Text.RegularExpressions.Regex _rankingNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):global:(?<rankingName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRankingNameFromGrn(
            string grn
        )
        {
            var match = _rankingNameRegex.Match(grn);
            if (!match.Success || !match.Groups["rankingName"].Success)
            {
                return null;
            }
            return match.Groups["rankingName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GlobalRankingModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GlobalRankingModel()
                .WithGlobalRankingModelId(!data.Keys.Contains("globalRankingModelId") || data["globalRankingModelId"] == null ? null : data["globalRankingModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)(data["minimumValue"].ToString().Contains(".") ? (long)double.Parse(data["minimumValue"].ToString()) : long.Parse(data["minimumValue"].ToString())))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)(data["maximumValue"].ToString().Contains(".") ? (long)double.Parse(data["maximumValue"].ToString()) : long.Parse(data["maximumValue"].ToString())))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithRankingRewards(!data.Keys.Contains("rankingRewards") || data["rankingRewards"] == null || !data["rankingRewards"].IsArray ? null : data["rankingRewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Ranking2.Model.RankingReward.FromJson(v);
                }).ToArray())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString())
                .WithRewardCalculationIndex(!data.Keys.Contains("rewardCalculationIndex") || data["rewardCalculationIndex"] == null ? null : data["rewardCalculationIndex"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData rankingRewardsJsonData = null;
            if (RankingRewards != null && RankingRewards.Length > 0)
            {
                rankingRewardsJsonData = new JsonData();
                foreach (var rankingReward in RankingRewards)
                {
                    rankingRewardsJsonData.Add(rankingReward.ToJson());
                }
            }
            return new JsonData {
                ["globalRankingModelId"] = GlobalRankingModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["sum"] = Sum,
                ["orderDirection"] = OrderDirection,
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["rankingRewards"] = rankingRewardsJsonData,
                ["accessPeriodEventId"] = AccessPeriodEventId,
                ["rewardCalculationIndex"] = RewardCalculationIndex,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GlobalRankingModelId != null) {
                writer.WritePropertyName("globalRankingModelId");
                writer.Write(GlobalRankingModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (MinimumValue != null) {
                writer.WritePropertyName("minimumValue");
                writer.Write((MinimumValue.ToString().Contains(".") ? (long)double.Parse(MinimumValue.ToString()) : long.Parse(MinimumValue.ToString())));
            }
            if (MaximumValue != null) {
                writer.WritePropertyName("maximumValue");
                writer.Write((MaximumValue.ToString().Contains(".") ? (long)double.Parse(MaximumValue.ToString()) : long.Parse(MaximumValue.ToString())));
            }
            if (Sum != null) {
                writer.WritePropertyName("sum");
                writer.Write(bool.Parse(Sum.ToString()));
            }
            if (OrderDirection != null) {
                writer.WritePropertyName("orderDirection");
                writer.Write(OrderDirection.ToString());
            }
            if (EntryPeriodEventId != null) {
                writer.WritePropertyName("entryPeriodEventId");
                writer.Write(EntryPeriodEventId.ToString());
            }
            if (RankingRewards != null) {
                writer.WritePropertyName("rankingRewards");
                writer.WriteArrayStart();
                foreach (var rankingReward in RankingRewards)
                {
                    if (rankingReward != null) {
                        rankingReward.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AccessPeriodEventId != null) {
                writer.WritePropertyName("accessPeriodEventId");
                writer.Write(AccessPeriodEventId.ToString());
            }
            if (RewardCalculationIndex != null) {
                writer.WritePropertyName("rewardCalculationIndex");
                writer.Write(RewardCalculationIndex.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GlobalRankingModel;
            var diff = 0;
            if (GlobalRankingModelId == null && GlobalRankingModelId == other.GlobalRankingModelId)
            {
                // null and null
            }
            else
            {
                diff += GlobalRankingModelId.CompareTo(other.GlobalRankingModelId);
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
            if (MinimumValue == null && MinimumValue == other.MinimumValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(MinimumValue - other.MinimumValue);
            }
            if (MaximumValue == null && MaximumValue == other.MaximumValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumValue - other.MaximumValue);
            }
            if (Sum == null && Sum == other.Sum)
            {
                // null and null
            }
            else
            {
                diff += Sum == other.Sum ? 0 : 1;
            }
            if (OrderDirection == null && OrderDirection == other.OrderDirection)
            {
                // null and null
            }
            else
            {
                diff += OrderDirection.CompareTo(other.OrderDirection);
            }
            if (EntryPeriodEventId == null && EntryPeriodEventId == other.EntryPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += EntryPeriodEventId.CompareTo(other.EntryPeriodEventId);
            }
            if (RankingRewards == null && RankingRewards == other.RankingRewards)
            {
                // null and null
            }
            else
            {
                diff += RankingRewards.Length - other.RankingRewards.Length;
                for (var i = 0; i < RankingRewards.Length; i++)
                {
                    diff += RankingRewards[i].CompareTo(other.RankingRewards[i]);
                }
            }
            if (AccessPeriodEventId == null && AccessPeriodEventId == other.AccessPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += AccessPeriodEventId.CompareTo(other.AccessPeriodEventId);
            }
            if (RewardCalculationIndex == null && RewardCalculationIndex == other.RewardCalculationIndex)
            {
                // null and null
            }
            else
            {
                diff += RewardCalculationIndex.CompareTo(other.RewardCalculationIndex);
            }
            return diff;
        }

        public void Validate() {
            {
                if (GlobalRankingModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.globalRankingModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (MinimumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.minimumValue.error.invalid"),
                    });
                }
                if (MinimumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.minimumValue.error.invalid"),
                    });
                }
            }
            {
                if (MaximumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.maximumValue.error.invalid"),
                    });
                }
                if (MaximumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.maximumValue.error.invalid"),
                    });
                }
            }
            {
            }
            {
                switch (OrderDirection) {
                    case "asc":
                    case "desc":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("globalRankingModel", "ranking2.globalRankingModel.orderDirection.error.invalid"),
                        });
                }
            }
            {
                if (EntryPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.entryPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingRewards.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.rankingRewards.error.tooMany"),
                    });
                }
            }
            {
                if (AccessPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingModel", "ranking2.globalRankingModel.accessPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                switch (RewardCalculationIndex) {
                    case "rank":
                    case "index":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("globalRankingModel", "ranking2.globalRankingModel.rewardCalculationIndex.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new GlobalRankingModel {
                GlobalRankingModelId = GlobalRankingModelId,
                Name = Name,
                Metadata = Metadata,
                MinimumValue = MinimumValue,
                MaximumValue = MaximumValue,
                Sum = Sum,
                OrderDirection = OrderDirection,
                EntryPeriodEventId = EntryPeriodEventId,
                RankingRewards = RankingRewards?.Clone() as Gs2.Gs2Ranking2.Model.RankingReward[],
                AccessPeriodEventId = AccessPeriodEventId,
                RewardCalculationIndex = RewardCalculationIndex,
            };
        }
    }
}