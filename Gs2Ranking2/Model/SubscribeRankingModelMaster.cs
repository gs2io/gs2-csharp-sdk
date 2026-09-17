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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class SubscribeRankingModelMaster : IComparable
	{
        public string SubscribeRankingModelId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public long? MinimumValue { set; get; }
        public long? MaximumValue { set; get; }
        public bool? Sum { set; get; }
        public string OrderDirection { set; get; }
        public string EntryPeriodEventId { set; get; }
        public string AccessPeriodEventId { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public SubscribeRankingModelMaster WithSubscribeRankingModelId(string subscribeRankingModelId) {
            this.SubscribeRankingModelId = subscribeRankingModelId;
            return this;
        }
        public SubscribeRankingModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public SubscribeRankingModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public SubscribeRankingModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SubscribeRankingModelMaster WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public SubscribeRankingModelMaster WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public SubscribeRankingModelMaster WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public SubscribeRankingModelMaster WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public SubscribeRankingModelMaster WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public SubscribeRankingModelMaster WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }
        public SubscribeRankingModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SubscribeRankingModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public SubscribeRankingModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:subscribe:(?<rankingName>.+)",
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

        private static System.Text.RegularExpressions.Regex _rankingNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):master:model:subscribe:(?<rankingName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRankingNameFromGrn(
            string grn
        )
        {
            var match = _rankingNameRegex.Match(grn);
            if (!match.Success || !match.Groups["rankingName"].Success)
            {
                return null;
            }
            return match.Groups["rankingName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SubscribeRankingModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new SubscribeRankingModelMaster()
                .WithSubscribeRankingModelId(!data.Keys.Contains("subscribeRankingModelId") || data["subscribeRankingModelId"] == null ? null : data["subscribeRankingModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["minimumValue"].ToString()))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["maximumValue"].ToString()))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableBool(data["sum"].ToString()))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscribeRankingModelId"] = SubscribeRankingModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["sum"] = Sum,
                ["orderDirection"] = OrderDirection,
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["accessPeriodEventId"] = AccessPeriodEventId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SubscribeRankingModelId != null) {
                writer.WritePropertyName("subscribeRankingModelId");
                writer.Write(SubscribeRankingModelId.ToString());
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
            var other = obj as SubscribeRankingModelMaster;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type SubscribeRankingModelMaster.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(SubscribeRankingModelId, other.SubscribeRankingModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MinimumValue, other.MinimumValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaximumValue, other.MaximumValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Sum, other.Sum);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(OrderDirection, other.OrderDirection);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EntryPeriodEventId, other.EntryPeriodEventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AccessPeriodEventId, other.AccessPeriodEventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (SubscribeRankingModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.subscribeRankingModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (MinimumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.minimumValue.error.invalid"),
                    });
                }
                if (MinimumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.minimumValue.error.invalid"),
                    });
                }
            }
            {
                if (MaximumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.maximumValue.error.invalid"),
                    });
                }
                if (MaximumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.maximumValue.error.invalid"),
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
                            new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.orderDirection.error.invalid"),
                        });
                }
            }
            {
                if (EntryPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.entryPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (AccessPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.accessPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModelMaster", "ranking2.subscribeRankingModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SubscribeRankingModelMaster {
                SubscribeRankingModelId = SubscribeRankingModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                MinimumValue = MinimumValue,
                MaximumValue = MaximumValue,
                Sum = Sum,
                OrderDirection = OrderDirection,
                EntryPeriodEventId = EntryPeriodEventId,
                AccessPeriodEventId = AccessPeriodEventId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}