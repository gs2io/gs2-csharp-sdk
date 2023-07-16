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
	public class UpdateRandomShowcaseMasterRequest : Gs2Request<UpdateRandomShowcaseMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string ShowcaseName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public int? MaximumNumberOfChoice { set; get; }
        public Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] DisplayItems { set; get; }
        public long? BaseTimestamp { set; get; }
        public int? ResetIntervalHours { set; get; }
        public string SalesPeriodEventId { set; get; }
        public UpdateRandomShowcaseMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithShowcaseName(string showcaseName) {
            this.ShowcaseName = showcaseName;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithMaximumNumberOfChoice(int? maximumNumberOfChoice) {
            this.MaximumNumberOfChoice = maximumNumberOfChoice;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithDisplayItems(Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] displayItems) {
            this.DisplayItems = displayItems;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithBaseTimestamp(long? baseTimestamp) {
            this.BaseTimestamp = baseTimestamp;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithResetIntervalHours(int? resetIntervalHours) {
            this.ResetIntervalHours = resetIntervalHours;
            return this;
        }
        public UpdateRandomShowcaseMasterRequest WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateRandomShowcaseMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateRandomShowcaseMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithShowcaseName(!data.Keys.Contains("showcaseName") || data["showcaseName"] == null ? null : data["showcaseName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMaximumNumberOfChoice(!data.Keys.Contains("maximumNumberOfChoice") || data["maximumNumberOfChoice"] == null ? null : (int?)int.Parse(data["maximumNumberOfChoice"].ToString()))
                .WithDisplayItems(!data.Keys.Contains("displayItems") || data["displayItems"] == null ? new Gs2.Gs2Showcase.Model.RandomDisplayItemModel[]{} : data["displayItems"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.RandomDisplayItemModel.FromJson(v);
                }).ToArray())
                .WithBaseTimestamp(!data.Keys.Contains("baseTimestamp") || data["baseTimestamp"] == null ? null : (long?)long.Parse(data["baseTimestamp"].ToString()))
                .WithResetIntervalHours(!data.Keys.Contains("resetIntervalHours") || data["resetIntervalHours"] == null ? null : (int?)int.Parse(data["resetIntervalHours"].ToString()))
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["showcaseName"] = ShowcaseName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["maximumNumberOfChoice"] = MaximumNumberOfChoice,
                ["displayItems"] = DisplayItems == null ? null : new JsonData(
                        DisplayItems.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
            if (ShowcaseName != null) {
                writer.WritePropertyName("showcaseName");
                writer.Write(ShowcaseName.ToString());
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
                writer.Write(int.Parse(MaximumNumberOfChoice.ToString()));
            }
            writer.WriteArrayStart();
            foreach (var displayItem in DisplayItems)
            {
                if (displayItem != null) {
                    displayItem.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (BaseTimestamp != null) {
                writer.WritePropertyName("baseTimestamp");
                writer.Write(long.Parse(BaseTimestamp.ToString()));
            }
            if (ResetIntervalHours != null) {
                writer.WritePropertyName("resetIntervalHours");
                writer.Write(int.Parse(ResetIntervalHours.ToString()));
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
            key += ShowcaseName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += MaximumNumberOfChoice + ":";
            key += DisplayItems + ":";
            key += BaseTimestamp + ":";
            key += ResetIntervalHours + ":";
            key += SalesPeriodEventId + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateRandomShowcaseMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateRandomShowcaseMasterRequest)x;
            return this;
        }
    }
}