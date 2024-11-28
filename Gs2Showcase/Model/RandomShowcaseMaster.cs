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
	public class RandomShowcaseMaster : IComparable
	{
        public string ShowcaseId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public int? MaximumNumberOfChoice { set; get; } = null!;
        public Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] DisplayItems { set; get; } = null!;
        public long? BaseTimestamp { set; get; } = null!;
        public int? ResetIntervalHours { set; get; } = null!;
        public string SalesPeriodEventId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public RandomShowcaseMaster WithShowcaseId(string showcaseId) {
            this.ShowcaseId = showcaseId;
            return this;
        }
        public RandomShowcaseMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public RandomShowcaseMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public RandomShowcaseMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RandomShowcaseMaster WithMaximumNumberOfChoice(int? maximumNumberOfChoice) {
            this.MaximumNumberOfChoice = maximumNumberOfChoice;
            return this;
        }
        public RandomShowcaseMaster WithDisplayItems(Gs2.Gs2Showcase.Model.RandomDisplayItemModel[] displayItems) {
            this.DisplayItems = displayItems;
            return this;
        }
        public RandomShowcaseMaster WithBaseTimestamp(long? baseTimestamp) {
            this.BaseTimestamp = baseTimestamp;
            return this;
        }
        public RandomShowcaseMaster WithResetIntervalHours(int? resetIntervalHours) {
            this.ResetIntervalHours = resetIntervalHours;
            return this;
        }
        public RandomShowcaseMaster WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }
        public RandomShowcaseMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public RandomShowcaseMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public RandomShowcaseMaster WithRevision(long? revision) {
            this.Revision = revision;
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
        public static RandomShowcaseMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RandomShowcaseMaster()
                .WithShowcaseId(!data.Keys.Contains("showcaseId") || data["showcaseId"] == null ? null : data["showcaseId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMaximumNumberOfChoice(!data.Keys.Contains("maximumNumberOfChoice") || data["maximumNumberOfChoice"] == null ? null : (int?)(data["maximumNumberOfChoice"].ToString().Contains(".") ? (int)double.Parse(data["maximumNumberOfChoice"].ToString()) : int.Parse(data["maximumNumberOfChoice"].ToString())))
                .WithDisplayItems(!data.Keys.Contains("displayItems") || data["displayItems"] == null || !data["displayItems"].IsArray ? null : data["displayItems"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.RandomDisplayItemModel.FromJson(v);
                }).ToArray())
                .WithBaseTimestamp(!data.Keys.Contains("baseTimestamp") || data["baseTimestamp"] == null ? null : (long?)(data["baseTimestamp"].ToString().Contains(".") ? (long)double.Parse(data["baseTimestamp"].ToString()) : long.Parse(data["baseTimestamp"].ToString())))
                .WithResetIntervalHours(!data.Keys.Contains("resetIntervalHours") || data["resetIntervalHours"] == null ? null : (int?)(data["resetIntervalHours"].ToString().Contains(".") ? (int)double.Parse(data["resetIntervalHours"].ToString()) : int.Parse(data["resetIntervalHours"].ToString())))
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["maximumNumberOfChoice"] = MaximumNumberOfChoice,
                ["displayItems"] = displayItemsJsonData,
                ["baseTimestamp"] = BaseTimestamp,
                ["resetIntervalHours"] = ResetIntervalHours,
                ["salesPeriodEventId"] = SalesPeriodEventId,
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RandomShowcaseMaster;
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

        public void Validate() {
            {
                if (ShowcaseId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.showcaseId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (MaximumNumberOfChoice < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.maximumNumberOfChoice.error.invalid"),
                    });
                }
                if (MaximumNumberOfChoice > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.maximumNumberOfChoice.error.invalid"),
                    });
                }
            }
            {
                if (DisplayItems.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.displayItems.error.tooFew"),
                    });
                }
                if (DisplayItems.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.displayItems.error.tooMany"),
                    });
                }
            }
            {
                if (BaseTimestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.baseTimestamp.error.invalid"),
                    });
                }
                if (BaseTimestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.baseTimestamp.error.invalid"),
                    });
                }
            }
            {
                if (ResetIntervalHours < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.resetIntervalHours.error.invalid"),
                    });
                }
                if (ResetIntervalHours > 168) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.resetIntervalHours.error.invalid"),
                    });
                }
            }
            {
                if (SalesPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.salesPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomShowcaseMaster", "showcase.randomShowcaseMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RandomShowcaseMaster {
                ShowcaseId = ShowcaseId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                MaximumNumberOfChoice = MaximumNumberOfChoice,
                DisplayItems = DisplayItems.Clone() as Gs2.Gs2Showcase.Model.RandomDisplayItemModel[],
                BaseTimestamp = BaseTimestamp,
                ResetIntervalHours = ResetIntervalHours,
                SalesPeriodEventId = SalesPeriodEventId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}