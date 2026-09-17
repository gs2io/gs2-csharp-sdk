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
	public partial class ClusterRankingModel : IComparable
	{
        public string ClusterRankingModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string ClusterType { set; get; }
        public long? MinimumValue { set; get; }
        public long? MaximumValue { set; get; }
        public bool? Sum { set; get; }
        public string OrderDirection { set; get; }
        public string EntryPeriodEventId { set; get; }
        public Gs2.Gs2Ranking2.Model.RankingReward[] RankingRewards { set; get; }
        public string AccessPeriodEventId { set; get; }
        public string RewardCalculationIndex { set; get; }
        public ClusterRankingModel WithClusterRankingModelId(string clusterRankingModelId) {
            this.ClusterRankingModelId = clusterRankingModelId;
            return this;
        }
        public ClusterRankingModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public ClusterRankingModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public ClusterRankingModel WithClusterType(string clusterType) {
            this.ClusterType = clusterType;
            return this;
        }
        public ClusterRankingModel WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public ClusterRankingModel WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public ClusterRankingModel WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public ClusterRankingModel WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public ClusterRankingModel WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public ClusterRankingModel WithRankingRewards(Gs2.Gs2Ranking2.Model.RankingReward[] rankingRewards) {
            this.RankingRewards = rankingRewards;
            return this;
        }
        public ClusterRankingModel WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }
        public ClusterRankingModel WithRewardCalculationIndex(string rewardCalculationIndex) {
            this.RewardCalculationIndex = rewardCalculationIndex;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+)",
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
        public static ClusterRankingModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new ClusterRankingModel()
                .WithClusterRankingModelId(!data.Keys.Contains("clusterRankingModelId") || data["clusterRankingModelId"] == null ? null : data["clusterRankingModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithClusterType(!data.Keys.Contains("clusterType") || data["clusterType"] == null ? null : data["clusterType"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["minimumValue"].ToString()))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["maximumValue"].ToString()))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableBool(data["sum"].ToString()))
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
                ["clusterRankingModelId"] = ClusterRankingModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["clusterType"] = ClusterType,
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
            if (ClusterRankingModelId != null) {
                writer.WritePropertyName("clusterRankingModelId");
                writer.Write(ClusterRankingModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ClusterType != null) {
                writer.WritePropertyName("clusterType");
                writer.Write(ClusterType.ToString());
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
            var other = obj as ClusterRankingModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ClusterRankingModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(ClusterRankingModelId, other.ClusterRankingModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ClusterType, other.ClusterType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MinimumValue, other.MinimumValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaximumValue, other.MaximumValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Sum, other.Sum);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(OrderDirection, other.OrderDirection);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EntryPeriodEventId, other.EntryPeriodEventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(RankingRewards, other.RankingRewards);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AccessPeriodEventId, other.AccessPeriodEventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RewardCalculationIndex, other.RewardCalculationIndex);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (ClusterRankingModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.clusterRankingModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                switch (ClusterType) {
                    case "Raw":
                    case "Gs2Guild::Guild":
                    case "Gs2Matchmaking::SeasonGathering":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.clusterType.error.invalid"),
                        });
                }
            }
            {
                if (MinimumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.minimumValue.error.invalid"),
                    });
                }
                if (MinimumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.minimumValue.error.invalid"),
                    });
                }
            }
            {
                if (MaximumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.maximumValue.error.invalid"),
                    });
                }
                if (MaximumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.maximumValue.error.invalid"),
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
                            new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.orderDirection.error.invalid"),
                        });
                }
            }
            {
                if (EntryPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.entryPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingRewards.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.rankingRewards.error.tooMany"),
                    });
                }
            }
            {
                if (AccessPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.accessPeriodEventId.error.tooLong"),
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
                            new RequestError("clusterRankingModel", "ranking2.clusterRankingModel.rewardCalculationIndex.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new ClusterRankingModel {
                ClusterRankingModelId = ClusterRankingModelId,
                Name = Name,
                Metadata = Metadata,
                ClusterType = ClusterType,
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