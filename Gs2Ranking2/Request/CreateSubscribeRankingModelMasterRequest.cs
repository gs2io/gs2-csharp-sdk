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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Ranking2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateSubscribeRankingModelMasterRequest : Gs2Request<CreateSubscribeRankingModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public long? MinimumValue { set; get; } = null!;
         public long? MaximumValue { set; get; } = null!;
         public bool? Sum { set; get; } = null!;
         public int? ScoreTtlDays { set; get; } = null!;
         public string OrderDirection { set; get; } = null!;
         public string EntryPeriodEventId { set; get; } = null!;
         public string AccessPeriodEventId { set; get; } = null!;
        public CreateSubscribeRankingModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithScoreTtlDays(int? scoreTtlDays) {
            this.ScoreTtlDays = scoreTtlDays;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public CreateSubscribeRankingModelMasterRequest WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateSubscribeRankingModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateSubscribeRankingModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)(data["minimumValue"].ToString().Contains(".") ? (long)double.Parse(data["minimumValue"].ToString()) : long.Parse(data["minimumValue"].ToString())))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)(data["maximumValue"].ToString().Contains(".") ? (long)double.Parse(data["maximumValue"].ToString()) : long.Parse(data["maximumValue"].ToString())))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()))
                .WithScoreTtlDays(!data.Keys.Contains("scoreTtlDays") || data["scoreTtlDays"] == null ? null : (int?)(data["scoreTtlDays"].ToString().Contains(".") ? (int)double.Parse(data["scoreTtlDays"].ToString()) : int.Parse(data["scoreTtlDays"].ToString())))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString());
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
                ["sum"] = Sum,
                ["scoreTtlDays"] = ScoreTtlDays,
                ["orderDirection"] = OrderDirection,
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["accessPeriodEventId"] = AccessPeriodEventId,
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
            if (AccessPeriodEventId != null) {
                writer.WritePropertyName("accessPeriodEventId");
                writer.Write(AccessPeriodEventId.ToString());
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
            key += Sum + ":";
            key += ScoreTtlDays + ":";
            key += OrderDirection + ":";
            key += EntryPeriodEventId + ":";
            key += AccessPeriodEventId + ":";
            return key;
        }
    }
}