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
	public class ClusterRankingModelMaster : IComparable
	{
        public string ClusterRankingModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string ClusterType { set; get; } = null!;
        public long? MinimumValue { set; get; } = null!;
        public long? MaximumValue { set; get; } = null!;
        public bool? Sum { set; get; } = null!;
        public int? ScoreTtlDays { set; get; } = null!;
        public string OrderDirection { set; get; } = null!;
        public string EntryPeriodEventId { set; get; } = null!;
        public Gs2.Gs2Ranking2.Model.RankingReward[] RankingRewards { set; get; } = null!;
        public string AccessPeriodEventId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public ClusterRankingModelMaster WithClusterRankingModelId(string clusterRankingModelId) {
            this.ClusterRankingModelId = clusterRankingModelId;
            return this;
        }
        public ClusterRankingModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public ClusterRankingModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public ClusterRankingModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public ClusterRankingModelMaster WithClusterType(string clusterType) {
            this.ClusterType = clusterType;
            return this;
        }
        public ClusterRankingModelMaster WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public ClusterRankingModelMaster WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public ClusterRankingModelMaster WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public ClusterRankingModelMaster WithScoreTtlDays(int? scoreTtlDays) {
            this.ScoreTtlDays = scoreTtlDays;
            return this;
        }
        public ClusterRankingModelMaster WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public ClusterRankingModelMaster WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public ClusterRankingModelMaster WithRankingRewards(Gs2.Gs2Ranking2.Model.RankingReward[] rankingRewards) {
            this.RankingRewards = rankingRewards;
            return this;
        }
        public ClusterRankingModelMaster WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }
        public ClusterRankingModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public ClusterRankingModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public ClusterRankingModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:cluster:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:cluster:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:cluster:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:cluster:(?<rankingName>.+)",
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
        public static ClusterRankingModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ClusterRankingModelMaster()
                .WithClusterRankingModelId(!data.Keys.Contains("clusterRankingModelId") || data["clusterRankingModelId"] == null ? null : data["clusterRankingModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithClusterType(!data.Keys.Contains("clusterType") || data["clusterType"] == null ? null : data["clusterType"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)(data["minimumValue"].ToString().Contains(".") ? (long)double.Parse(data["minimumValue"].ToString()) : long.Parse(data["minimumValue"].ToString())))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)(data["maximumValue"].ToString().Contains(".") ? (long)double.Parse(data["maximumValue"].ToString()) : long.Parse(data["maximumValue"].ToString())))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()))
                .WithScoreTtlDays(!data.Keys.Contains("scoreTtlDays") || data["scoreTtlDays"] == null ? null : (int?)(data["scoreTtlDays"].ToString().Contains(".") ? (int)double.Parse(data["scoreTtlDays"].ToString()) : int.Parse(data["scoreTtlDays"].ToString())))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithRankingRewards(!data.Keys.Contains("rankingRewards") || data["rankingRewards"] == null || !data["rankingRewards"].IsArray ? null : data["rankingRewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Ranking2.Model.RankingReward.FromJson(v);
                }).ToArray())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["clusterType"] = ClusterType,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["sum"] = Sum,
                ["scoreTtlDays"] = ScoreTtlDays,
                ["orderDirection"] = OrderDirection,
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["rankingRewards"] = rankingRewardsJsonData,
                ["accessPeriodEventId"] = AccessPeriodEventId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
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
            if (ScoreTtlDays != null) {
                writer.WritePropertyName("scoreTtlDays");
                writer.Write((ScoreTtlDays.ToString().Contains(".") ? (int)double.Parse(ScoreTtlDays.ToString()) : int.Parse(ScoreTtlDays.ToString())));
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
            var other = obj as ClusterRankingModelMaster;
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
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
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
            if (ScoreTtlDays == null && ScoreTtlDays == other.ScoreTtlDays)
            {
                // null and null
            }
            else
            {
                diff += (int)(ScoreTtlDays - other.ScoreTtlDays);
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
                if (ClusterRankingModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.clusterRankingModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.metadata.error.tooLong"),
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
                            new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.clusterType.error.invalid"),
                        });
                }
            }
            {
                if (MinimumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.minimumValue.error.invalid"),
                    });
                }
                if (MinimumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.minimumValue.error.invalid"),
                    });
                }
            }
            {
                if (MaximumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.maximumValue.error.invalid"),
                    });
                }
                if (MaximumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.maximumValue.error.invalid"),
                    });
                }
            }
            {
            }
            {
                if (ScoreTtlDays < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.scoreTtlDays.error.invalid"),
                    });
                }
                if (ScoreTtlDays > 365) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.scoreTtlDays.error.invalid"),
                    });
                }
            }
            {
                switch (OrderDirection) {
                    case "asc":
                    case "desc":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.orderDirection.error.invalid"),
                        });
                }
            }
            {
                if (EntryPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.entryPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingRewards.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.rankingRewards.error.tooMany"),
                    });
                }
            }
            {
                if (AccessPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.accessPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingModelMaster", "ranking2.clusterRankingModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ClusterRankingModelMaster {
                ClusterRankingModelId = ClusterRankingModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                ClusterType = ClusterType,
                MinimumValue = MinimumValue,
                MaximumValue = MaximumValue,
                Sum = Sum,
                ScoreTtlDays = ScoreTtlDays,
                OrderDirection = OrderDirection,
                EntryPeriodEventId = EntryPeriodEventId,
                RankingRewards = RankingRewards.Clone() as Gs2.Gs2Ranking2.Model.RankingReward[],
                AccessPeriodEventId = AccessPeriodEventId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}