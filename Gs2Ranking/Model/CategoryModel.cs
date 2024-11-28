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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class CategoryModel : IComparable
	{
        public string CategoryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? MinimumValue { set; get; } = null!;
        public long? MaximumValue { set; get; } = null!;
        public bool? Sum { set; get; } = null!;
        public string OrderDirection { set; get; } = null!;
        public string Scope { set; get; } = null!;
        public Gs2.Gs2Ranking.Model.GlobalRankingSetting GlobalRankingSetting { set; get; } = null!;
        public string EntryPeriodEventId { set; get; } = null!;
        public string AccessPeriodEventId { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public bool? UniqueByUserId { set; get; } = null!;
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
        public CategoryModel WithCategoryModelId(string categoryModelId) {
            this.CategoryModelId = categoryModelId;
            return this;
        }
        public CategoryModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public CategoryModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CategoryModel WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public CategoryModel WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public CategoryModel WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public CategoryModel WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public CategoryModel WithScope(string scope) {
            this.Scope = scope;
            return this;
        }
        public CategoryModel WithGlobalRankingSetting(Gs2.Gs2Ranking.Model.GlobalRankingSetting globalRankingSetting) {
            this.GlobalRankingSetting = globalRankingSetting;
            return this;
        }
        public CategoryModel WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public CategoryModel WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CategoryModel WithUniqueByUserId(bool? uniqueByUserId) {
            this.UniqueByUserId = uniqueByUserId;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CategoryModel WithCalculateFixedTimingHour(int? calculateFixedTimingHour) {
            this.CalculateFixedTimingHour = calculateFixedTimingHour;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CategoryModel WithCalculateFixedTimingMinute(int? calculateFixedTimingMinute) {
            this.CalculateFixedTimingMinute = calculateFixedTimingMinute;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CategoryModel WithCalculateIntervalMinutes(int? calculateIntervalMinutes) {
            this.CalculateIntervalMinutes = calculateIntervalMinutes;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CategoryModel WithAdditionalScopes(Gs2.Gs2Ranking.Model.Scope[] additionalScopes) {
            this.AdditionalScopes = additionalScopes;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CategoryModel WithIgnoreUserIds(string[] ignoreUserIds) {
            this.IgnoreUserIds = ignoreUserIds;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public CategoryModel WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):categoryModel:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):categoryModel:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):categoryModel:(?<categoryName>.+)",
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

        private static System.Text.RegularExpressions.Regex _categoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):categoryModel:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCategoryNameFromGrn(
            string grn
        )
        {
            var match = _categoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["categoryName"].Success)
            {
                return null;
            }
            return match.Groups["categoryName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CategoryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CategoryModel()
                .WithCategoryModelId(!data.Keys.Contains("categoryModelId") || data["categoryModelId"] == null ? null : data["categoryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)(data["minimumValue"].ToString().Contains(".") ? (long)double.Parse(data["minimumValue"].ToString()) : long.Parse(data["minimumValue"].ToString())))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)(data["maximumValue"].ToString().Contains(".") ? (long)double.Parse(data["maximumValue"].ToString()) : long.Parse(data["maximumValue"].ToString())))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithGlobalRankingSetting(!data.Keys.Contains("globalRankingSetting") || data["globalRankingSetting"] == null ? null : Gs2.Gs2Ranking.Model.GlobalRankingSetting.FromJson(data["globalRankingSetting"]))
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString())
                .WithUniqueByUserId(!data.Keys.Contains("uniqueByUserId") || data["uniqueByUserId"] == null ? null : (bool?)bool.Parse(data["uniqueByUserId"].ToString()))
                .WithCalculateFixedTimingHour(!data.Keys.Contains("calculateFixedTimingHour") || data["calculateFixedTimingHour"] == null ? null : (int?)(data["calculateFixedTimingHour"].ToString().Contains(".") ? (int)double.Parse(data["calculateFixedTimingHour"].ToString()) : int.Parse(data["calculateFixedTimingHour"].ToString())))
                .WithCalculateFixedTimingMinute(!data.Keys.Contains("calculateFixedTimingMinute") || data["calculateFixedTimingMinute"] == null ? null : (int?)(data["calculateFixedTimingMinute"].ToString().Contains(".") ? (int)double.Parse(data["calculateFixedTimingMinute"].ToString()) : int.Parse(data["calculateFixedTimingMinute"].ToString())))
                .WithCalculateIntervalMinutes(!data.Keys.Contains("calculateIntervalMinutes") || data["calculateIntervalMinutes"] == null ? null : (int?)(data["calculateIntervalMinutes"].ToString().Contains(".") ? (int)double.Parse(data["calculateIntervalMinutes"].ToString()) : int.Parse(data["calculateIntervalMinutes"].ToString())))
                .WithAdditionalScopes(!data.Keys.Contains("additionalScopes") || data["additionalScopes"] == null || !data["additionalScopes"].IsArray ? null : data["additionalScopes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Ranking.Model.Scope.FromJson(v);
                }).ToArray())
                .WithIgnoreUserIds(!data.Keys.Contains("ignoreUserIds") || data["ignoreUserIds"] == null || !data["ignoreUserIds"].IsArray ? null : data["ignoreUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString());
        }

        public JsonData ToJson()
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
                ["categoryModelId"] = CategoryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["sum"] = Sum,
                ["orderDirection"] = OrderDirection,
                ["scope"] = Scope,
                ["globalRankingSetting"] = GlobalRankingSetting?.ToJson(),
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["accessPeriodEventId"] = AccessPeriodEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CategoryModelId != null) {
                writer.WritePropertyName("categoryModelId");
                writer.Write(CategoryModelId.ToString());
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
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (GlobalRankingSetting != null) {
                writer.WritePropertyName("globalRankingSetting");
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
                    if (ignoreUserId != null) {
                        writer.Write(ignoreUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CategoryModel;
            var diff = 0;
            if (CategoryModelId == null && CategoryModelId == other.CategoryModelId)
            {
                // null and null
            }
            else
            {
                diff += CategoryModelId.CompareTo(other.CategoryModelId);
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
            if (Scope == null && Scope == other.Scope)
            {
                // null and null
            }
            else
            {
                diff += Scope.CompareTo(other.Scope);
            }
            if (GlobalRankingSetting == null && GlobalRankingSetting == other.GlobalRankingSetting)
            {
                // null and null
            }
            else
            {
                diff += GlobalRankingSetting.CompareTo(other.GlobalRankingSetting);
            }
            if (EntryPeriodEventId == null && EntryPeriodEventId == other.EntryPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += EntryPeriodEventId.CompareTo(other.EntryPeriodEventId);
            }
            if (AccessPeriodEventId == null && AccessPeriodEventId == other.AccessPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += AccessPeriodEventId.CompareTo(other.AccessPeriodEventId);
            }
            if (UniqueByUserId == null && UniqueByUserId == other.UniqueByUserId)
            {
                // null and null
            }
            else
            {
                diff += UniqueByUserId == other.UniqueByUserId ? 0 : 1;
            }
            if (CalculateFixedTimingHour == null && CalculateFixedTimingHour == other.CalculateFixedTimingHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(CalculateFixedTimingHour - other.CalculateFixedTimingHour);
            }
            if (CalculateFixedTimingMinute == null && CalculateFixedTimingMinute == other.CalculateFixedTimingMinute)
            {
                // null and null
            }
            else
            {
                diff += (int)(CalculateFixedTimingMinute - other.CalculateFixedTimingMinute);
            }
            if (CalculateIntervalMinutes == null && CalculateIntervalMinutes == other.CalculateIntervalMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(CalculateIntervalMinutes - other.CalculateIntervalMinutes);
            }
            if (AdditionalScopes == null && AdditionalScopes == other.AdditionalScopes)
            {
                // null and null
            }
            else
            {
                diff += AdditionalScopes.Length - other.AdditionalScopes.Length;
                for (var i = 0; i < AdditionalScopes.Length; i++)
                {
                    diff += AdditionalScopes[i].CompareTo(other.AdditionalScopes[i]);
                }
            }
            if (IgnoreUserIds == null && IgnoreUserIds == other.IgnoreUserIds)
            {
                // null and null
            }
            else
            {
                diff += IgnoreUserIds.Length - other.IgnoreUserIds.Length;
                for (var i = 0; i < IgnoreUserIds.Length; i++)
                {
                    diff += IgnoreUserIds[i].CompareTo(other.IgnoreUserIds[i]);
                }
            }
            if (Generation == null && Generation == other.Generation)
            {
                // null and null
            }
            else
            {
                diff += Generation.CompareTo(other.Generation);
            }
            return diff;
        }

        public void Validate() {
            {
                if (CategoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.categoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (MinimumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.minimumValue.error.invalid"),
                    });
                }
                if (MinimumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.minimumValue.error.invalid"),
                    });
                }
            }
            {
                if (MaximumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.maximumValue.error.invalid"),
                    });
                }
                if (MaximumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.maximumValue.error.invalid"),
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
                            new RequestError("categoryModel", "ranking.categoryModel.orderDirection.error.invalid"),
                        });
                }
            }
            {
                switch (Scope) {
                    case "global":
                    case "scoped":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("categoryModel", "ranking.categoryModel.scope.error.invalid"),
                        });
                }
            }
            if (Scope == "global") {
            }
            {
                if (EntryPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.entryPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (AccessPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "ranking.categoryModel.accessPeriodEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new CategoryModel {
                CategoryModelId = CategoryModelId,
                Name = Name,
                Metadata = Metadata,
                MinimumValue = MinimumValue,
                MaximumValue = MaximumValue,
                Sum = Sum,
                OrderDirection = OrderDirection,
                Scope = Scope,
                GlobalRankingSetting = GlobalRankingSetting.Clone() as Gs2.Gs2Ranking.Model.GlobalRankingSetting,
                EntryPeriodEventId = EntryPeriodEventId,
                AccessPeriodEventId = AccessPeriodEventId,
                UniqueByUserId = UniqueByUserId,
                CalculateFixedTimingHour = CalculateFixedTimingHour,
                CalculateFixedTimingMinute = CalculateFixedTimingMinute,
                CalculateIntervalMinutes = CalculateIntervalMinutes,
                AdditionalScopes = AdditionalScopes.Clone() as Gs2.Gs2Ranking.Model.Scope[],
                IgnoreUserIds = IgnoreUserIds.Clone() as string[],
                Generation = Generation,
            };
        }
    }
}