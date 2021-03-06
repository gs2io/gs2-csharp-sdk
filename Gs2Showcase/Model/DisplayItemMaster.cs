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
        public string DisplayItemId { set; get; }
        public string Type { set; get; }
        public string SalesItemName { set; get; }
        public string SalesItemGroupName { set; get; }
        public string SalesPeriodEventId { set; get; }
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
                .WithSalesPeriodEventId(!data.Keys.Contains("salesPeriodEventId") || data["salesPeriodEventId"] == null ? null : data["salesPeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["displayItemId"] = DisplayItemId,
                ["type"] = Type,
                ["salesItemName"] = SalesItemName,
                ["salesItemGroupName"] = SalesItemGroupName,
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
            return diff;
        }
    }
}