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
	public class Showcase : IComparable
	{
        public string ShowcaseId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string SalesPeriodEventId { set; get; } = null!;
        public Gs2.Gs2Showcase.Model.DisplayItem[] DisplayItems { set; get; } = null!;
        public Showcase WithShowcaseId(string showcaseId) {
            this.ShowcaseId = showcaseId;
            return this;
        }
        public Showcase WithName(string name) {
            this.Name = name;
            return this;
        }
        public Showcase WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public Showcase WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }
        public Showcase WithDisplayItems(Gs2.Gs2Showcase.Model.DisplayItem[] displayItems) {
            this.DisplayItems = displayItems;
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
        public static Showcase FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Showcase()
                .WithShowcaseId(!data.Keys.Contains("showcaseId") || data["showcaseId"] == null ? null : data["showcaseId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString())
                .WithDisplayItems(!data.Keys.Contains("displayItems") || data["displayItems"] == null || !data["displayItems"].IsArray ? null : data["displayItems"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.DisplayItem.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
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
                ["showcaseId"] = ShowcaseId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["salesPeriodEventId"] = SalesPeriodEventId,
                ["displayItems"] = displayItemsJsonData,
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Showcase;
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
            return diff;
        }

        public void Validate() {
            {
                if (ShowcaseId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("showcase", "showcase.showcase.showcaseId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("showcase", "showcase.showcase.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("showcase", "showcase.showcase.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (SalesPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("showcase", "showcase.showcase.salesPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (DisplayItems.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("showcase", "showcase.showcase.displayItems.error.tooFew"),
                    });
                }
                if (DisplayItems.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("showcase", "showcase.showcase.displayItems.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new Showcase {
                ShowcaseId = ShowcaseId,
                Name = Name,
                Metadata = Metadata,
                SalesPeriodEventId = SalesPeriodEventId,
                DisplayItems = DisplayItems.Clone() as Gs2.Gs2Showcase.Model.DisplayItem[],
            };
        }
    }
}