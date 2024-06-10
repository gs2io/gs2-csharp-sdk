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
	public class DisplayItemMaster : IComparable
	{
        public string DisplayItemId { set; get; } = null!;
        public string Type { set; get; } = null!;
        public string SalesItemName { set; get; } = null!;
        public string SalesItemGroupName { set; get; } = null!;
        public string SalesPeriodEventId { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public DisplayItemMaster WithDisplayItemId(string displayItemId) {
            this.DisplayItemId = displayItemId;
            return this;
        }
        public DisplayItemMaster WithType(string type) {
            this.Type = type;
            return this;
        }
        public DisplayItemMaster WithSalesItemName(string salesItemName) {
            this.SalesItemName = salesItemName;
            return this;
        }
        public DisplayItemMaster WithSalesItemGroupName(string salesItemGroupName) {
            this.SalesItemGroupName = salesItemGroupName;
            return this;
        }
        public DisplayItemMaster WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }
        public DisplayItemMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DisplayItemMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DisplayItemMaster()
                .WithDisplayItemId(!data.Keys.Contains("displayItemId") || data["displayItemId"] == null ? null : data["displayItemId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithSalesItemName(!data.Keys.Contains("salesItemName") || data["salesItemName"] == null ? null : data["salesItemName"].ToString())
                .WithSalesItemGroupName(!data.Keys.Contains("salesItemGroupName") || data["salesItemGroupName"] == null ? null : data["salesItemGroupName"].ToString())
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString())
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["displayItemId"] = DisplayItemId,
                ["type"] = Type,
                ["salesItemName"] = SalesItemName,
                ["salesItemGroupName"] = SalesItemGroupName,
                ["salesPeriodEventId"] = SalesPeriodEventId,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DisplayItemId != null) {
                writer.WritePropertyName("displayItemId");
                writer.Write(DisplayItemId.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (SalesItemName != null) {
                writer.WritePropertyName("salesItemName");
                writer.Write(SalesItemName.ToString());
            }
            if (SalesItemGroupName != null) {
                writer.WritePropertyName("salesItemGroupName");
                writer.Write(SalesItemGroupName.ToString());
            }
            if (SalesPeriodEventId != null) {
                writer.WritePropertyName("salesPeriodEventId");
                writer.Write(SalesPeriodEventId.ToString());
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DisplayItemMaster;
            var diff = 0;
            if (DisplayItemId == null && DisplayItemId == other.DisplayItemId)
            {
                // null and null
            }
            else
            {
                diff += DisplayItemId.CompareTo(other.DisplayItemId);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (SalesItemName == null && SalesItemName == other.SalesItemName)
            {
                // null and null
            }
            else
            {
                diff += SalesItemName.CompareTo(other.SalesItemName);
            }
            if (SalesItemGroupName == null && SalesItemGroupName == other.SalesItemGroupName)
            {
                // null and null
            }
            else
            {
                diff += SalesItemGroupName.CompareTo(other.SalesItemGroupName);
            }
            if (SalesPeriodEventId == null && SalesPeriodEventId == other.SalesPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += SalesPeriodEventId.CompareTo(other.SalesPeriodEventId);
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
                if (DisplayItemId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItemMaster", "showcase.displayItemMaster.displayItemId.error.tooLong"),
                    });
                }
            }
            {
                switch (Type) {
                    case "salesItem":
                    case "salesItemGroup":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("displayItemMaster", "showcase.displayItemMaster.type.error.invalid"),
                        });
                }
            }
            if (Type == "salesItem") {
                if (SalesItemName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItemMaster", "showcase.displayItemMaster.salesItemName.error.tooLong"),
                    });
                }
            }
            if (Type == "salesItemGroup") {
                if (SalesItemGroupName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItemMaster", "showcase.displayItemMaster.salesItemGroupName.error.tooLong"),
                    });
                }
            }
            {
                if (SalesPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItemMaster", "showcase.displayItemMaster.salesPeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItemMaster", "showcase.displayItemMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItemMaster", "showcase.displayItemMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new DisplayItemMaster {
                DisplayItemId = DisplayItemId,
                Type = Type,
                SalesItemName = SalesItemName,
                SalesItemGroupName = SalesItemGroupName,
                SalesPeriodEventId = SalesPeriodEventId,
                Revision = Revision,
            };
        }
    }
}