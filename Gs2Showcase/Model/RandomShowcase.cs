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

namespace Gs2.Gs2Showcase.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RandomShowcase : IComparable
	{
        public string RandomShowcaseId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public int? MaximumNumberOfChoice { set; get; }
        public Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] DisplayItems { set; get; }
        public long? BaseTimestamp { set; get; }
        public int? ResetIntervalHours { set; get; }
        public string SalesPeriodEventId { set; get; }
        public RandomShowcase WithRandomShowcaseId(string randomShowcaseId) {
            this.RandomShowcaseId = randomShowcaseId;
            return this;
        }
        public RandomShowcase WithName(string name) {
            this.Name = name;
            return this;
        }
        public RandomShowcase WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RandomShowcase WithMaximumNumberOfChoice(int? maximumNumberOfChoice) {
            this.MaximumNumberOfChoice = maximumNumberOfChoice;
            return this;
        }
        public RandomShowcase WithDisplayItems(Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] displayItems) {
            this.DisplayItems = displayItems;
            return this;
        }
        public RandomShowcase WithBaseTimestamp(long? baseTimestamp) {
            this.BaseTimestamp = baseTimestamp;
            return this;
        }
        public RandomShowcase WithResetIntervalHours(int? resetIntervalHours) {
            this.ResetIntervalHours = resetIntervalHours;
            return this;
        }
        public RandomShowcase WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):random:showcase:(?<showcaseName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):random:showcase:(?<showcaseName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):random:showcase:(?<showcaseName>.+)",
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

        private static System.Text.RegularExpressions.Regex _showcaseNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):random:showcase:(?<showcaseName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetShowcaseNameFromGrn(
            string grn
        )
        {
            var match = _showcaseNameRegex.Match(grn);
            if (!match.Success || !match.Groups["showcaseName"].Success)
            {
                return null;
            }
            return match.Groups["showcaseName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RandomShowcase FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RandomShowcase()
                .WithRandomShowcaseId(!data.Keys.Contains("randomShowcaseId") || data["randomShowcaseId"] == null ? null : data["randomShowcaseId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMaximumNumberOfChoice(!data.Keys.Contains("maximumNumberOfChoice") || data["maximumNumberOfChoice"] == null ? null : (int?)int.Parse(data["maximumNumberOfChoice"].ToString()))
                .WithDisplayItems(!data.Keys.Contains("displayItems") || data["displayItems"] == null ? new Gs2.Gs2Showcase.Model.RandomDisplayItemModel[]{} : data["displayItems"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.RandomDisplayItemModel.FromJson(v);
                }).ToArray())
                .WithBaseTimestamp(!data.Keys.Contains("baseTimestamp") || data["baseTimestamp"] == null ? null : (long?)long.Parse(data["baseTimestamp"].ToString()))
                .WithResetIntervalHours(!data.Keys.Contains("resetIntervalHours") || data["resetIntervalHours"] == null ? null : (int?)int.Parse(data["resetIntervalHours"].ToString()))
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["randomShowcaseId"] = RandomShowcaseId,
                ["name"] = Name,
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
            if (RandomShowcaseId != null) {
                writer.WritePropertyName("randomShowcaseId");
                writer.Write(RandomShowcaseId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (MaximumNumberOfChoice != null) {
                writer.WritePropertyName("maximumNumberOfChoice");
                writer.Write(int.Parse(MaximumNumberOfChoice.ToString()));
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

        public int CompareTo(object obj)
        {
            var other = obj as RandomShowcase;
            var diff = 0;
            if (RandomShowcaseId == null && RandomShowcaseId == other.RandomShowcaseId)
            {
                // null and null
            }
            else
            {
                diff += RandomShowcaseId.CompareTo(other.RandomShowcaseId);
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
            if (MaximumNumberOfChoice == null && MaximumNumberOfChoice == other.MaximumNumberOfChoice)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumNumberOfChoice - other.MaximumNumberOfChoice);
            }
            if (DisplayItems == null && DisplayItems == other.DisplayItems)
            {
                // null and null
            }
            else
            {
                diff += DisplayItems.Length - other.DisplayItems.Length;
                for (var i = 0; i < DisplayItems.Length; i++)
                {
                    diff += DisplayItems[i].CompareTo(other.DisplayItems[i]);
                }
            }
            if (BaseTimestamp == null && BaseTimestamp == other.BaseTimestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(BaseTimestamp - other.BaseTimestamp);
            }
            if (ResetIntervalHours == null && ResetIntervalHours == other.ResetIntervalHours)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetIntervalHours - other.ResetIntervalHours);
            }
            if (SalesPeriodEventId == null && SalesPeriodEventId == other.SalesPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += SalesPeriodEventId.CompareTo(other.SalesPeriodEventId);
            }
            return diff;
        }
    }
}