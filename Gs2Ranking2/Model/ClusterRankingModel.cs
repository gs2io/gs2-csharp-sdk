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

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ClusterRankingModel : IComparable
	{
        public string ClusterRankingModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string ClusterType { set; get; } = null!;
        public long? MinimumValue { set; get; } = null!;
        public long? MaximumValue { set; get; } = null!;
        public bool? Sum { set; get; } = null!;
        public string OrderDirection { set; get; } = null!;
        public string EntryPeriodEventId { set; get; } = null!;
        public Gs2.Gs2Ranking2.Model.RankingReward[] RankingRewards { set; get; } = null!;
        public string AccessPeriodEventId { set; get; } = null!;
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
            return new ClusterRankingModel()
                .WithClusterRankingModelId(!data.Keys.Contains("clusterRankingModelId") || data["clusterRankingModelId"] == null ? null : data["clusterRankingModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithClusterType(!data.Keys.Contains("clusterType") || data["clusterType"] == null ? null : data["clusterType"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)(data["minimumValue"].ToString().Contains(".") ? (long)double.Parse(data["minimumValue"].ToString()) : long.Parse(data["minimumValue"].ToString())))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)(data["maximumValue"].ToString().Contains(".") ? (long)double.Parse(data["maximumValue"].ToString()) : long.Parse(data["maximumValue"].ToString())))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithRankingRewards(!data.Keys.Contains("rankingRewards") || data["rankingRewards"] == null || !data["rankingRewards"].IsArray ? null : data["rankingRewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Ranking2.Model.RankingReward.FromJson(v);
                }).ToArray())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString());
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ClusterRankingModel;
            var diff = 0;
            if (ClusterRankingModelId == null && ClusterRankingModelId == other.ClusterRankingModelId)
            {
                // null and null
            }
            else
            {
                diff += ClusterRankingModelId.CompareTo(other.ClusterRankingModelId);
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
            if (ClusterType == null && ClusterType == other.ClusterType)
            {
                // null and null
            }
            else
            {
                diff += ClusterType.CompareTo(other.ClusterType);
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
            return diff;
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
                RankingRewards = RankingRewards.Clone() as Gs2.Gs2Ranking2.Model.RankingReward[],
                AccessPeriodEventId = AccessPeriodEventId,
            };
        }
    }
}