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
using Gs2.Gs2Ranking.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateCategoryModelMasterRequest : Gs2Request<CreateCategoryModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public long? MinimumValue { set; get; } = null!;
         public long? MaximumValue { set; get; } = null!;
         public string OrderDirection { set; get; } = null!;
         public string Scope { set; get; } = null!;
         public Gs2.Gs2Ranking.Model.GlobalRankingSetting GlobalRankingSetting { set; get; } = null!;
         public string EntryPeriodEventId { set; get; } = null!;
         public string AccessPeriodEventId { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public bool? UniqueByUserId { set; get; } = null!;
         public bool? Sum { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public int? CalculateFixedTimingHour { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public int? CalculateFixedTimingMinute { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public int? CalculateIntervalMinutes { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public Gs2.Gs2Ranking.Model.Scope[] AdditionalScopes { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string[] IgnoreUserIds { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string Generation { set; get; } = null!;
        public CreateCategoryModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateCategoryModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateCategoryModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateCategoryModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateCategoryModelMasterRequest WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public CreateCategoryModelMasterRequest WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public CreateCategoryModelMasterRequest WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public CreateCategoryModelMasterRequest WithScope(string scope) {
            this.Scope = scope;
            return this;
        }
        public CreateCategoryModelMasterRequest WithGlobalRankingSetting(Gs2.Gs2Ranking.Model.GlobalRankingSetting globalRankingSetting) {
            this.GlobalRankingSetting = globalRankingSetting;
            return this;
        }
        public CreateCategoryModelMasterRequest WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public CreateCategoryModelMasterRequest WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateCategoryModelMasterRequest WithUniqueByUserId(bool? uniqueByUserId) {
            this.UniqueByUserId = uniqueByUserId;
            return this;
        }
        public CreateCategoryModelMasterRequest WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateCategoryModelMasterRequest WithCalculateFixedTimingHour(int? calculateFixedTimingHour) {
            this.CalculateFixedTimingHour = calculateFixedTimingHour;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateCategoryModelMasterRequest WithCalculateFixedTimingMinute(int? calculateFixedTimingMinute) {
            this.CalculateFixedTimingMinute = calculateFixedTimingMinute;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateCategoryModelMasterRequest WithCalculateIntervalMinutes(int? calculateIntervalMinutes) {
            this.CalculateIntervalMinutes = calculateIntervalMinutes;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateCategoryModelMasterRequest WithAdditionalScopes(Gs2.Gs2Ranking.Model.Scope[] additionalScopes) {
            this.AdditionalScopes = additionalScopes;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateCategoryModelMasterRequest WithIgnoreUserIds(string[] ignoreUserIds) {
            this.IgnoreUserIds = ignoreUserIds;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CreateCategoryModelMasterRequest WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateCategoryModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateCategoryModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)(data["minimumValue"].ToString().Contains(".") ? (long)double.Parse(data["minimumValue"].ToString()) : long.Parse(data["minimumValue"].ToString())))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)(data["maximumValue"].ToString().Contains(".") ? (long)double.Parse(data["maximumValue"].ToString()) : long.Parse(data["maximumValue"].ToString())))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithGlobalRankingSetting(!data.Keys.Contains("globalRankingSetting") || data["globalRankingSetting"] == null ? null : Gs2.Gs2Ranking.Model.GlobalRankingSetting.FromJson(data["globalRankingSetting"]))
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString())
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()));
        }

        public override JsonData ToJson()
        {
            JsonData additionalScopesJsonData = null;
            if (AdditionalScopes != null && AdditionalScopes.Length > 0)
            {
                additionalScopesJsonData = new JsonData();
                foreach (var additionalScope in AdditionalScopes)
                {
                    additionalScopesJsonData.Add(additionalScope.ToJson());
                }
            }
            JsonData ignoreUserIdsJsonData = null;
            if (IgnoreUserIds != null && IgnoreUserIds.Length > 0)
            {
                ignoreUserIdsJsonData = new JsonData();
                foreach (var ignoreUserId in IgnoreUserIds)
                {
                    ignoreUserIdsJsonData.Add(ignoreUserId);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["orderDirection"] = OrderDirection,
                ["scope"] = Scope,
                ["globalRankingSetting"] = GlobalRankingSetting?.ToJson(),
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["accessPeriodEventId"] = AccessPeriodEventId,
                ["sum"] = Sum,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
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
            if (MinimumValue != null) {
                writer.WritePropertyName("minimumValue");
                writer.Write((MinimumValue.ToString().Contains(".") ? (long)double.Parse(MinimumValue.ToString()) : long.Parse(MinimumValue.ToString())));
            }
            if (MaximumValue != null) {
                writer.WritePropertyName("maximumValue");
                writer.Write((MaximumValue.ToString().Contains(".") ? (long)double.Parse(MaximumValue.ToString()) : long.Parse(MaximumValue.ToString())));
            }
            if (OrderDirection != null) {
                writer.WritePropertyName("orderDirection");
                writer.Write(OrderDirection.ToString());
            }
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (GlobalRankingSetting != null) {
                GlobalRankingSetting.WriteJson(writer);
            }
            if (EntryPeriodEventId != null) {
                writer.WritePropertyName("entryPeriodEventId");
                writer.Write(EntryPeriodEventId.ToString());
            }
            if (AccessPeriodEventId != null) {
                writer.WritePropertyName("accessPeriodEventId");
                writer.Write(AccessPeriodEventId.ToString());
            }
            if (UniqueByUserId != null) {
                writer.WritePropertyName("uniqueByUserId");
                writer.Write(bool.Parse(UniqueByUserId.ToString()));
            }
            if (Sum != null) {
                writer.WritePropertyName("sum");
                writer.Write(bool.Parse(Sum.ToString()));
            }
            if (CalculateFixedTimingHour != null) {
                writer.WritePropertyName("calculateFixedTimingHour");
                writer.Write((CalculateFixedTimingHour.ToString().Contains(".") ? (int)double.Parse(CalculateFixedTimingHour.ToString()) : int.Parse(CalculateFixedTimingHour.ToString())));
            }
            if (CalculateFixedTimingMinute != null) {
                writer.WritePropertyName("calculateFixedTimingMinute");
                writer.Write((CalculateFixedTimingMinute.ToString().Contains(".") ? (int)double.Parse(CalculateFixedTimingMinute.ToString()) : int.Parse(CalculateFixedTimingMinute.ToString())));
            }
            if (CalculateIntervalMinutes != null) {
                writer.WritePropertyName("calculateIntervalMinutes");
                writer.Write((CalculateIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(CalculateIntervalMinutes.ToString()) : int.Parse(CalculateIntervalMinutes.ToString())));
            }
            if (AdditionalScopes != null) {
                writer.WritePropertyName("additionalScopes");
                writer.WriteArrayStart();
                foreach (var additionalScope in AdditionalScopes)
                {
                    if (additionalScope != null) {
                        additionalScope.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (IgnoreUserIds != null) {
                writer.WritePropertyName("ignoreUserIds");
                writer.WriteArrayStart();
                foreach (var ignoreUserId in IgnoreUserIds)
                {
                    writer.Write(ignoreUserId.ToString());
                }
                writer.WriteArrayEnd();
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += MinimumValue + ":";
            key += MaximumValue + ":";
            key += OrderDirection + ":";
            key += Scope + ":";
            key += GlobalRankingSetting + ":";
            key += EntryPeriodEventId + ":";
            key += AccessPeriodEventId + ":";
            key += Sum + ":";
            return key;
        }
    }
}