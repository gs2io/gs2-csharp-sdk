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
	public class ShowcaseMaster : IComparable
	{
        public string ShowcaseId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string SalesPeriodEventId { set; get; }
        public Gs2.Gs2Showcase.Model.DisplayItemMaster[] DisplayItems { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }

        public ShowcaseMaster WithShowcaseId(string showcaseId) {
            this.ShowcaseId = showcaseId;
            return this;
        }

        public ShowcaseMaster WithName(string name) {
            this.Name = name;
            return this;
        }

        public ShowcaseMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public ShowcaseMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public ShowcaseMaster WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }

        public ShowcaseMaster WithDisplayItems(Gs2.Gs2Showcase.Model.DisplayItemMaster[] displayItems) {
            this.DisplayItems = displayItems;
            return this;
        }

        public ShowcaseMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public ShowcaseMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        public ShowcaseMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):showcase:(?<showcaseName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):showcase:(?<showcaseName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):showcase:(?<showcaseName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):showcase:(?<showcaseName>.+)",
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
        public static ShowcaseMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ShowcaseMaster()
                .WithShowcaseId(!data.Keys.Contains("showcaseId") || data["showcaseId"] == null ? null : data["showcaseId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString())
                .WithDisplayItems(!data.Keys.Contains("displayItems") || data["displayItems"] == null || !data["displayItems"].IsArray ? new Gs2.Gs2Showcase.Model.DisplayItemMaster[]{} : data["displayItems"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.DisplayItemMaster.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData displayItemsJsonData = null;
            if (DisplayItems != null)
            {
                displayItemsJsonData = new JsonData();
                foreach (var displayItem in DisplayItems)
                {
                    displayItemsJsonData.Add(displayItem.ToJson());
                }
            }
            return new JsonData {
                ["showcaseId"] = ShowcaseId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["salesPeriodEventId"] = SalesPeriodEventId,
                ["displayItems"] = displayItemsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ShowcaseId != null) {
                writer.WritePropertyName("showcaseId");
                writer.Write(ShowcaseId.ToString());
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
            if (SalesPeriodEventId != null) {
                writer.WritePropertyName("salesPeriodEventId");
                writer.Write(SalesPeriodEventId.ToString());
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write(long.Parse(Revision.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ShowcaseMaster;
            var diff = 0;
            if (ShowcaseId == null && ShowcaseId == other.ShowcaseId)
            {
                // null and null
            }
            else
            {
                diff += ShowcaseId.CompareTo(other.ShowcaseId);
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
            if (SalesPeriodEventId == null && SalesPeriodEventId == other.SalesPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += SalesPeriodEventId.CompareTo(other.SalesPeriodEventId);
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
    }
}