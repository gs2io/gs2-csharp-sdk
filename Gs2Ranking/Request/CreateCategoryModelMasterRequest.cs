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
        public string NamespaceName { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public long? MinimumValue { set; get; }
        public long? MaximumValue { set; get; }
        public string OrderDirection { set; get; }
        public string Scope { set; get; }
        public bool? UniqueByUserId { set; get; }
        public bool? Sum { set; get; }
        public int? CalculateFixedTimingHour { set; get; }
        public int? CalculateFixedTimingMinute { set; get; }
        public int? CalculateIntervalMinutes { set; get; }
        public Gs2.Gs2Ranking.Model.Scope[] AdditionalScopes { set; get; }
        public string EntryPeriodEventId { set; get; }
        public string AccessPeriodEventId { set; get; }
        public string[] IgnoreUserIds { set; get; }
        public string Generation { set; get; }
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
        public CreateCategoryModelMasterRequest WithUniqueByUserId(bool? uniqueByUserId) {
            this.UniqueByUserId = uniqueByUserId;
            return this;
        }
        public CreateCategoryModelMasterRequest WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public CreateCategoryModelMasterRequest WithCalculateFixedTimingHour(int? calculateFixedTimingHour) {
            this.CalculateFixedTimingHour = calculateFixedTimingHour;
            return this;
        }
        public CreateCategoryModelMasterRequest WithCalculateFixedTimingMinute(int? calculateFixedTimingMinute) {
            this.CalculateFixedTimingMinute = calculateFixedTimingMinute;
            return this;
        }
        public CreateCategoryModelMasterRequest WithCalculateIntervalMinutes(int? calculateIntervalMinutes) {
            this.CalculateIntervalMinutes = calculateIntervalMinutes;
            return this;
        }
        public CreateCategoryModelMasterRequest WithAdditionalScopes(Gs2.Gs2Ranking.Model.Scope[] additionalScopes) {
            this.AdditionalScopes = additionalScopes;
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
        public CreateCategoryModelMasterRequest WithIgnoreUserIds(string[] ignoreUserIds) {
            this.IgnoreUserIds = ignoreUserIds;
            return this;
        }
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
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)long.Parse(data["minimumValue"].ToString()))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)long.Parse(data["maximumValue"].ToString()))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithUniqueByUserId(!data.Keys.Contains("uniqueByUserId") || data["uniqueByUserId"] == null ? null : (bool?)bool.Parse(data["uniqueByUserId"].ToString()))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()))
                .WithCalculateFixedTimingHour(!data.Keys.Contains("calculateFixedTimingHour") || data["calculateFixedTimingHour"] == null ? null : (int?)int.Parse(data["calculateFixedTimingHour"].ToString()))
                .WithCalculateFixedTimingMinute(!data.Keys.Contains("calculateFixedTimingMinute") || data["calculateFixedTimingMinute"] == null ? null : (int?)int.Parse(data["calculateFixedTimingMinute"].ToString()))
                .WithCalculateIntervalMinutes(!data.Keys.Contains("calculateIntervalMinutes") || data["calculateIntervalMinutes"] == null ? null : (int?)int.Parse(data["calculateIntervalMinutes"].ToString()))
                .WithAdditionalScopes(!data.Keys.Contains("additionalScopes") || data["additionalScopes"] == null ? new Gs2.Gs2Ranking.Model.Scope[]{} : data["additionalScopes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Ranking.Model.Scope.FromJson(v);
                }).ToArray())
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString())
                .WithIgnoreUserIds(!data.Keys.Contains("ignoreUserIds") || data["ignoreUserIds"] == null ? new string[]{} : data["ignoreUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["orderDirection"] = OrderDirection,
                ["scope"] = Scope,
                ["uniqueByUserId"] = UniqueByUserId,
                ["sum"] = Sum,
                ["calculateFixedTimingHour"] = CalculateFixedTimingHour,
                ["calculateFixedTimingMinute"] = CalculateFixedTimingMinute,
                ["calculateIntervalMinutes"] = CalculateIntervalMinutes,
                ["additionalScopes"] = AdditionalScopes == null ? null : new JsonData(
                        AdditionalScopes.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["accessPeriodEventId"] = AccessPeriodEventId,
                ["ignoreUserIds"] = IgnoreUserIds == null ? null : new JsonData(
                        IgnoreUserIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
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
            if (Sum != null) {
                writer.WritePropertyName("sum");
                writer.Write(bool.Parse(Sum.ToString()));
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
            writer.WriteArrayStart();
            foreach (var additionalScope in AdditionalScopes)
            {
                if (additionalScope != null) {
                    additionalScope.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (EntryPeriodEventId != null) {
                writer.WritePropertyName("entryPeriodEventId");
                writer.Write(EntryPeriodEventId.ToString());
            }
            if (AccessPeriodEventId != null) {
                writer.WritePropertyName("accessPeriodEventId");
                writer.Write(AccessPeriodEventId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var ignoreUserId in IgnoreUserIds)
            {
                writer.Write(ignoreUserId.ToString());
            }
            writer.WriteArrayEnd();
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
            key += UniqueByUserId + ":";
            key += Sum + ":";
            key += CalculateFixedTimingHour + ":";
            key += CalculateFixedTimingMinute + ":";
            key += CalculateIntervalMinutes + ":";
            key += AdditionalScopes + ":";
            key += EntryPeriodEventId + ":";
            key += AccessPeriodEventId + ":";
            key += IgnoreUserIds + ":";
            key += Generation + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply CreateCategoryModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CreateCategoryModelMasterRequest)x;
            return this;
        }
    }
}