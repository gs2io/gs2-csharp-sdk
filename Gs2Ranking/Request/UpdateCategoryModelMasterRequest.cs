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
	public class UpdateCategoryModelMasterRequest : Gs2Request<UpdateCategoryModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string CategoryName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public long? MinimumValue { set; get; }
        public long? MaximumValue { set; get; }
        public string OrderDirection { set; get; }
        public string Scope { set; get; }
        public bool? UniqueByUserId { set; get; }
        public int? CalculateFixedTimingHour { set; get; }
        public int? CalculateFixedTimingMinute { set; get; }
        public int? CalculateIntervalMinutes { set; get; }
        public string EntryPeriodEventId { set; get; }
        public string AccessPeriodEventId { set; get; }
        public string Generation { set; get; }

        public UpdateCategoryModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithScope(string scope) {
            this.Scope = scope;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithUniqueByUserId(bool? uniqueByUserId) {
            this.UniqueByUserId = uniqueByUserId;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithCalculateFixedTimingHour(int? calculateFixedTimingHour) {
            this.CalculateFixedTimingHour = calculateFixedTimingHour;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithCalculateFixedTimingMinute(int? calculateFixedTimingMinute) {
            this.CalculateFixedTimingMinute = calculateFixedTimingMinute;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithCalculateIntervalMinutes(int? calculateIntervalMinutes) {
            this.CalculateIntervalMinutes = calculateIntervalMinutes;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }

        public UpdateCategoryModelMasterRequest WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateCategoryModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateCategoryModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)long.Parse(data["minimumValue"].ToString()))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)long.Parse(data["maximumValue"].ToString()))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithUniqueByUserId(!data.Keys.Contains("uniqueByUserId") || data["uniqueByUserId"] == null ? null : (bool?)bool.Parse(data["uniqueByUserId"].ToString()))
                .WithCalculateFixedTimingHour(!data.Keys.Contains("calculateFixedTimingHour") || data["calculateFixedTimingHour"] == null ? null : (int?)int.Parse(data["calculateFixedTimingHour"].ToString()))
                .WithCalculateFixedTimingMinute(!data.Keys.Contains("calculateFixedTimingMinute") || data["calculateFixedTimingMinute"] == null ? null : (int?)int.Parse(data["calculateFixedTimingMinute"].ToString()))
                .WithCalculateIntervalMinutes(!data.Keys.Contains("calculateIntervalMinutes") || data["calculateIntervalMinutes"] == null ? null : (int?)int.Parse(data["calculateIntervalMinutes"].ToString()))
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["categoryName"] = CategoryName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["orderDirection"] = OrderDirection,
                ["scope"] = Scope,
                ["uniqueByUserId"] = UniqueByUserId,
                ["calculateFixedTimingHour"] = CalculateFixedTimingHour,
                ["calculateFixedTimingMinute"] = CalculateFixedTimingMinute,
                ["calculateIntervalMinutes"] = CalculateIntervalMinutes,
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["accessPeriodEventId"] = AccessPeriodEventId,
                ["generation"] = Generation,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
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
                writer.Write(long.Parse(MinimumValue.ToString()));
            }
            if (MaximumValue != null) {
                writer.WritePropertyName("maximumValue");
                writer.Write(long.Parse(MaximumValue.ToString()));
            }
            if (OrderDirection != null) {
                writer.WritePropertyName("orderDirection");
                writer.Write(OrderDirection.ToString());
            }
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (UniqueByUserId != null) {
                writer.WritePropertyName("uniqueByUserId");
                writer.Write(bool.Parse(UniqueByUserId.ToString()));
            }
            if (CalculateFixedTimingHour != null) {
                writer.WritePropertyName("calculateFixedTimingHour");
                writer.Write(int.Parse(CalculateFixedTimingHour.ToString()));
            }
            if (CalculateFixedTimingMinute != null) {
                writer.WritePropertyName("calculateFixedTimingMinute");
                writer.Write(int.Parse(CalculateFixedTimingMinute.ToString()));
            }
            if (CalculateIntervalMinutes != null) {
                writer.WritePropertyName("calculateIntervalMinutes");
                writer.Write(int.Parse(CalculateIntervalMinutes.ToString()));
            }
            if (EntryPeriodEventId != null) {
                writer.WritePropertyName("entryPeriodEventId");
                writer.Write(EntryPeriodEventId.ToString());
            }
            if (AccessPeriodEventId != null) {
                writer.WritePropertyName("accessPeriodEventId");
                writer.Write(AccessPeriodEventId.ToString());
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}