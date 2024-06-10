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
	public class DisplayItem : IComparable
	{
        public string DisplayItemId { set; get; } = null!;
        public string Type { set; get; } = null!;
        public Gs2.Gs2Showcase.Model.SalesItem SalesItem { set; get; } = null!;
        public Gs2.Gs2Showcase.Model.SalesItemGroup SalesItemGroup { set; get; } = null!;
        public string SalesPeriodEventId { set; get; } = null!;
        public DisplayItem WithDisplayItemId(string displayItemId) {
            this.DisplayItemId = displayItemId;
            return this;
        }
        public DisplayItem WithType(string type) {
            this.Type = type;
            return this;
        }
        public DisplayItem WithSalesItem(Gs2.Gs2Showcase.Model.SalesItem salesItem) {
            this.SalesItem = salesItem;
            return this;
        }
        public DisplayItem WithSalesItemGroup(Gs2.Gs2Showcase.Model.SalesItemGroup salesItemGroup) {
            this.SalesItemGroup = salesItemGroup;
            return this;
        }
        public DisplayItem WithSalesPeriodEventId(string salesPeriodEventId) {
            this.SalesPeriodEventId = salesPeriodEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DisplayItem FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DisplayItem()
                .WithDisplayItemId(!data.Keys.Contains("displayItemId") || data["displayItemId"] == null ? null : data["displayItemId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithSalesItem(!data.Keys.Contains("salesItem") || data["salesItem"] == null ? null : Gs2.Gs2Showcase.Model.SalesItem.FromJson(data["salesItem"]))
                .WithSalesItemGroup(!data.Keys.Contains("salesItemGroup") || data["salesItemGroup"] == null ? null : Gs2.Gs2Showcase.Model.SalesItemGroup.FromJson(data["salesItemGroup"]))
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["displayItemId"] = DisplayItemId,
                ["type"] = Type,
                ["salesItem"] = SalesItem?.ToJson(),
                ["salesItemGroup"] = SalesItemGroup?.ToJson(),
                ["salesPeriodEventId"] = SalesPeriodEventId,
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
            if (SalesItem != null) {
                writer.WritePropertyName("salesItem");
                SalesItem.WriteJson(writer);
            }
            if (SalesItemGroup != null) {
                writer.WritePropertyName("salesItemGroup");
                SalesItemGroup.WriteJson(writer);
            }
            if (SalesPeriodEventId != null) {
                writer.WritePropertyName("salesPeriodEventId");
                writer.Write(SalesPeriodEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DisplayItem;
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
            if (SalesItem == null && SalesItem == other.SalesItem)
            {
                // null and null
            }
            else
            {
                diff += SalesItem.CompareTo(other.SalesItem);
            }
            if (SalesItemGroup == null && SalesItemGroup == other.SalesItemGroup)
            {
                // null and null
            }
            else
            {
                diff += SalesItemGroup.CompareTo(other.SalesItemGroup);
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

        public void Validate() {
            {
                if (DisplayItemId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItem", "showcase.displayItem.displayItemId.error.tooLong"),
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
                            new RequestError("displayItem", "showcase.displayItem.type.error.invalid"),
                        });
                }
            }
            if (Type == "salesItem") {
            }
            if (Type == "salesItemGroup") {
            }
            {
                if (SalesPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("displayItem", "showcase.displayItem.salesPeriodEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new DisplayItem {
                DisplayItemId = DisplayItemId,
                Type = Type,
                SalesItem = SalesItem.Clone() as Gs2.Gs2Showcase.Model.SalesItem,
                SalesItemGroup = SalesItemGroup.Clone() as Gs2.Gs2Showcase.Model.SalesItemGroup,
                SalesPeriodEventId = SalesPeriodEventId,
            };
        }
    }
}