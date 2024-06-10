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

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SubscribeRankingModel : IComparable
	{
        public string SubscribeRankingModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? MinimumValue { set; get; } = null!;
        public long? MaximumValue { set; get; } = null!;
        public bool? Sum { set; get; } = null!;
        public string OrderDirection { set; get; } = null!;
        public string EntryPeriodEventId { set; get; } = null!;
        public string AccessPeriodEventId { set; get; } = null!;
        public SubscribeRankingModel WithSubscribeRankingModelId(string subscribeRankingModelId) {
            this.SubscribeRankingModelId = subscribeRankingModelId;
            return this;
        }
        public SubscribeRankingModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public SubscribeRankingModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SubscribeRankingModel WithMinimumValue(long? minimumValue) {
            this.MinimumValue = minimumValue;
            return this;
        }
        public SubscribeRankingModel WithMaximumValue(long? maximumValue) {
            this.MaximumValue = maximumValue;
            return this;
        }
        public SubscribeRankingModel WithSum(bool? sum) {
            this.Sum = sum;
            return this;
        }
        public SubscribeRankingModel WithOrderDirection(string orderDirection) {
            this.OrderDirection = orderDirection;
            return this;
        }
        public SubscribeRankingModel WithEntryPeriodEventId(string entryPeriodEventId) {
            this.EntryPeriodEventId = entryPeriodEventId;
            return this;
        }
        public SubscribeRankingModel WithAccessPeriodEventId(string accessPeriodEventId) {
            this.AccessPeriodEventId = accessPeriodEventId;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):subscribe:(?<rankingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):subscribe:(?<rankingName>.+)",
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
        public static SubscribeRankingModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscribeRankingModel()
                .WithSubscribeRankingModelId(!data.Keys.Contains("subscribeRankingModelId") || data["subscribeRankingModelId"] == null ? null : data["subscribeRankingModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMinimumValue(!data.Keys.Contains("minimumValue") || data["minimumValue"] == null ? null : (long?)(data["minimumValue"].ToString().Contains(".") ? (long)double.Parse(data["minimumValue"].ToString()) : long.Parse(data["minimumValue"].ToString())))
                .WithMaximumValue(!data.Keys.Contains("maximumValue") || data["maximumValue"] == null ? null : (long?)(data["maximumValue"].ToString().Contains(".") ? (long)double.Parse(data["maximumValue"].ToString()) : long.Parse(data["maximumValue"].ToString())))
                .WithSum(!data.Keys.Contains("sum") || data["sum"] == null ? null : (bool?)bool.Parse(data["sum"].ToString()))
                .WithOrderDirection(!data.Keys.Contains("orderDirection") || data["orderDirection"] == null ? null : data["orderDirection"].ToString())
                .WithEntryPeriodEventId(!data.Keys.Contains("entryPeriodEventId") || data["entryPeriodEventId"] == null ? null : data["entryPeriodEventId"].ToString())
                .WithAccessPeriodEventId(!data.Keys.Contains("accessPeriodEventId") || data["accessPeriodEventId"] == null ? null : data["accessPeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscribeRankingModelId"] = SubscribeRankingModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["minimumValue"] = MinimumValue,
                ["maximumValue"] = MaximumValue,
                ["sum"] = Sum,
                ["orderDirection"] = OrderDirection,
                ["entryPeriodEventId"] = EntryPeriodEventId,
                ["accessPeriodEventId"] = AccessPeriodEventId,
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SubscribeRankingModel;
            var diff = 0;
            if (SubscribeRankingModelId == null && SubscribeRankingModelId == other.SubscribeRankingModelId)
            {
                // null and null
            }
            else
            {
                diff += SubscribeRankingModelId.CompareTo(other.SubscribeRankingModelId);
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
            if (MinimumValue == null && MinimumValue == other.MinimumValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(MinimumValue - other.MinimumValue);
            }
            if (MaximumValue == null && MaximumValue == other.MaximumValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumValue - other.MaximumValue);
            }
            if (Sum == null && Sum == other.Sum)
            {
                // null and null
            }
            else
            {
                diff += Sum == other.Sum ? 0 : 1;
            }
            if (OrderDirection == null && OrderDirection == other.OrderDirection)
            {
                // null and null
            }
            else
            {
                diff += OrderDirection.CompareTo(other.OrderDirection);
            }
            if (EntryPeriodEventId == null && EntryPeriodEventId == other.EntryPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += EntryPeriodEventId.CompareTo(other.EntryPeriodEventId);
            }
            if (AccessPeriodEventId == null && AccessPeriodEventId == other.AccessPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += AccessPeriodEventId.CompareTo(other.AccessPeriodEventId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (SubscribeRankingModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.subscribeRankingModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (MinimumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.minimumValue.error.invalid"),
                    });
                }
                if (MinimumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.minimumValue.error.invalid"),
                    });
                }
            }
            {
                if (MaximumValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.maximumValue.error.invalid"),
                    });
                }
                if (MaximumValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.maximumValue.error.invalid"),
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
                            new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.orderDirection.error.invalid"),
                        });
                }
            }
            {
                if (EntryPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.entryPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (AccessPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingModel", "ranking2.subscribeRankingModel.accessPeriodEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SubscribeRankingModel {
                SubscribeRankingModelId = SubscribeRankingModelId,
                Name = Name,
                Metadata = Metadata,
                MinimumValue = MinimumValue,
                MaximumValue = MaximumValue,
                Sum = Sum,
                OrderDirection = OrderDirection,
                EntryPeriodEventId = EntryPeriodEventId,
                AccessPeriodEventId = AccessPeriodEventId,
            };
        }
    }
}