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
using Gs2.Gs2Showcase.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Showcase.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateRandomShowcaseMasterRequest : Gs2Request<CreateRandomShowcaseMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public int? MaximumNumberOfChoice { set; get; } = null!;
         public Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] DisplayItems { set; get; } = null!;
         public long? BaseTimestamp { set; get; } = null!;
         public int? ResetIntervalHours { set; get; } = null!;
         public string SalesPeriodEventId { set; get; } = null!;
        public CreateRandomShowcaseMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithMaximumNumberOfChoice(int? maximumNumberOfChoice) {
            this.MaximumNumberOfChoice = maximumNumberOfChoice;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithDisplayItems(Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] displayItems) {
            this.DisplayItems = displayItems;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithBaseTimestamp(long? baseTimestamp) {
            this.BaseTimestamp = baseTimestamp;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithResetIntervalHours(int? resetIntervalHours) {
            this.ResetIntervalHours = resetIntervalHours;
            return this;
        }
        public CreateRandomShowcaseMasterRequest WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateRandomShowcaseMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateRandomShowcaseMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMaximumNumberOfChoice(!data.Keys.Contains("maximumNumberOfChoice") || data["maximumNumberOfChoice"] == null ? null : (int?)(data["maximumNumberOfChoice"].ToString().Contains(".") ? (int)double.Parse(data["maximumNumberOfChoice"].ToString()) : int.Parse(data["maximumNumberOfChoice"].ToString())))
                .WithDisplayItems(!data.Keys.Contains("displayItems") || data["displayItems"] == null || !data["displayItems"].IsArray ? null : data["displayItems"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.RandomDisplayItemModel.FromJson(v);
                }).ToArray())
                .WithBaseTimestamp(!data.Keys.Contains("baseTimestamp") || data["baseTimestamp"] == null ? null : (long?)(data["baseTimestamp"].ToString().Contains(".") ? (long)double.Parse(data["baseTimestamp"].ToString()) : long.Parse(data["baseTimestamp"].ToString())))
                .WithResetIntervalHours(!data.Keys.Contains("resetIntervalHours") || data["resetIntervalHours"] == null ? null : (int?)(data["resetIntervalHours"].ToString().Contains(".") ? (int)double.Parse(data["resetIntervalHours"].ToString()) : int.Parse(data["resetIntervalHours"].ToString())))
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData displayItemsJsonData = null;
            if (DisplayItems != null && DisplayItems.Length > 0)
            {
                displayItemsJsonData = new JsonData();
                foreach (var displayItem in DisplayItems)
                {
                    displayItemsJsonData.Add(displayItem.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["maximumNumberOfChoice"] = MaximumNumberOfChoice,
                ["displayItems"] = displayItemsJsonData,
                ["baseTimestamp"] = BaseTimestamp,
                ["resetIntervalHours"] = ResetIntervalHours,
                ["salesPeriodEventId"] = SalesPeriodEventId,
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
            if (MaximumNumberOfChoice != null) {
                writer.WritePropertyName("maximumNumberOfChoice");
                writer.Write((MaximumNumberOfChoice.ToString().Contains(".") ? (int)double.Parse(MaximumNumberOfChoice.ToString()) : int.Parse(MaximumNumberOfChoice.ToString())));
            }
            if (DisplayItems != null) {
                writer.WritePropertyName("displayItems");
                writer.WriteArrayStart();
                foreach (var displayItem in DisplayItems)
                {
                    if (displayItem != null) {
                        displayItem.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (BaseTimestamp != null) {
                writer.WritePropertyName("baseTimestamp");
                writer.Write((BaseTimestamp.ToString().Contains(".") ? (long)double.Parse(BaseTimestamp.ToString()) : long.Parse(BaseTimestamp.ToString())));
            }
            if (ResetIntervalHours != null) {
                writer.WritePropertyName("resetIntervalHours");
                writer.Write((ResetIntervalHours.ToString().Contains(".") ? (int)double.Parse(ResetIntervalHours.ToString()) : int.Parse(ResetIntervalHours.ToString())));
            }
            if (SalesPeriodEventId != null) {
                writer.WritePropertyName("salesPeriodEventId");
                writer.Write(SalesPeriodEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += MaximumNumberOfChoice + ":";
            key += DisplayItems + ":";
            key += BaseTimestamp + ":";
            key += ResetIntervalHours + ":";
            key += SalesPeriodEventId + ":";
            return key;
        }
    }
}