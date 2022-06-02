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

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Billing : IComparable
	{
        public string BillingId { set; get; }
        public string ProjectName { set; get; }
        public int? Year { set; get; }
        public int? Month { set; get; }
        public string Region { set; get; }
        public string Service { set; get; }
        public string ActivityType { set; get; }
        public double? Unit { set; get; }
        public string UnitName { set; get; }
        public double? Price { set; get; }
        public string Currency { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Billing WithBillingId(string billingId) {
            this.BillingId = billingId;
            return this;
        }

        public Billing WithProjectName(string projectName) {
            this.ProjectName = projectName;
            return this;
        }

        public Billing WithYear(int? year) {
            this.Year = year;
            return this;
        }

        public Billing WithMonth(int? month) {
            this.Month = month;
            return this;
        }

        public Billing WithRegion(string region) {
            this.Region = region;
            return this;
        }

        public Billing WithService(string service) {
            this.Service = service;
            return this;
        }

        public Billing WithActivityType(string activityType) {
            this.ActivityType = activityType;
            return this;
        }

        public Billing WithUnit(double? unit) {
            this.Unit = unit;
            return this;
        }

        public Billing WithUnitName(string unitName) {
            this.UnitName = unitName;
            return this;
        }

        public Billing WithPrice(double? price) {
            this.Price = price;
            return this;
        }

        public Billing WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }

        public Billing WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Billing WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _accountNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):billing:(?<year>.+):(?<month>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAccountNameFromGrn(
            string grn
        )
        {
            var match = _accountNameRegex.Match(grn);
            if (!match.Success || !match.Groups["accountName"].Success)
            {
                return null;
            }
            return match.Groups["accountName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _projectNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):billing:(?<year>.+):(?<month>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetProjectNameFromGrn(
            string grn
        )
        {
            var match = _projectNameRegex.Match(grn);
            if (!match.Success || !match.Groups["projectName"].Success)
            {
                return null;
            }
            return match.Groups["projectName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _yearRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):billing:(?<year>.+):(?<month>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetYearFromGrn(
            string grn
        )
        {
            var match = _yearRegex.Match(grn);
            if (!match.Success || !match.Groups["year"].Success)
            {
                return null;
            }
            return match.Groups["year"].Value;
        }

        private static System.Text.RegularExpressions.Regex _monthRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):billing:(?<year>.+):(?<month>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMonthFromGrn(
            string grn
        )
        {
            var match = _monthRegex.Match(grn);
            if (!match.Success || !match.Groups["month"].Success)
            {
                return null;
            }
            return match.Groups["month"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Billing FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Billing()
                .WithBillingId(!data.Keys.Contains("billingId") || data["billingId"] == null ? null : data["billingId"].ToString())
                .WithProjectName(!data.Keys.Contains("projectName") || data["projectName"] == null ? null : data["projectName"].ToString())
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)int.Parse(data["year"].ToString()))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)int.Parse(data["month"].ToString()))
                .WithRegion(!data.Keys.Contains("region") || data["region"] == null ? null : data["region"].ToString())
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithActivityType(!data.Keys.Contains("activityType") || data["activityType"] == null ? null : data["activityType"].ToString())
                .WithUnit(!data.Keys.Contains("unit") || data["unit"] == null ? null : (double?)double.Parse(data["unit"].ToString()))
                .WithUnitName(!data.Keys.Contains("unitName") || data["unitName"] == null ? null : data["unitName"].ToString())
                .WithPrice(!data.Keys.Contains("price") || data["price"] == null ? null : (double?)double.Parse(data["price"].ToString()))
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["billingId"] = BillingId,
                ["projectName"] = ProjectName,
                ["year"] = Year,
                ["month"] = Month,
                ["region"] = Region,
                ["service"] = Service,
                ["activityType"] = ActivityType,
                ["unit"] = Unit,
                ["unitName"] = UnitName,
                ["price"] = Price,
                ["currency"] = Currency,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BillingId != null) {
                writer.WritePropertyName("billingId");
                writer.Write(BillingId.ToString());
            }
            if (ProjectName != null) {
                writer.WritePropertyName("projectName");
                writer.Write(ProjectName.ToString());
            }
            if (Year != null) {
                writer.WritePropertyName("year");
                writer.Write(int.Parse(Year.ToString()));
            }
            if (Month != null) {
                writer.WritePropertyName("month");
                writer.Write(int.Parse(Month.ToString()));
            }
            if (Region != null) {
                writer.WritePropertyName("region");
                writer.Write(Region.ToString());
            }
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            if (ActivityType != null) {
                writer.WritePropertyName("activityType");
                writer.Write(ActivityType.ToString());
            }
            if (Unit != null) {
                writer.WritePropertyName("unit");
                writer.Write(double.Parse(Unit.ToString()));
            }
            if (UnitName != null) {
                writer.WritePropertyName("unitName");
                writer.Write(UnitName.ToString());
            }
            if (Price != null) {
                writer.WritePropertyName("price");
                writer.Write(double.Parse(Price.ToString()));
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Billing;
            var diff = 0;
            if (BillingId == null && BillingId == other.BillingId)
            {
                // null and null
            }
            else
            {
                diff += BillingId.CompareTo(other.BillingId);
            }
            if (ProjectName == null && ProjectName == other.ProjectName)
            {
                // null and null
            }
            else
            {
                diff += ProjectName.CompareTo(other.ProjectName);
            }
            if (Year == null && Year == other.Year)
            {
                // null and null
            }
            else
            {
                diff += (int)(Year - other.Year);
            }
            if (Month == null && Month == other.Month)
            {
                // null and null
            }
            else
            {
                diff += (int)(Month - other.Month);
            }
            if (Region == null && Region == other.Region)
            {
                // null and null
            }
            else
            {
                diff += Region.CompareTo(other.Region);
            }
            if (Service == null && Service == other.Service)
            {
                // null and null
            }
            else
            {
                diff += Service.CompareTo(other.Service);
            }
            if (ActivityType == null && ActivityType == other.ActivityType)
            {
                // null and null
            }
            else
            {
                diff += ActivityType.CompareTo(other.ActivityType);
            }
            if (Unit == null && Unit == other.Unit)
            {
                // null and null
            }
            else
            {
                diff += (int)(Unit - other.Unit);
            }
            if (UnitName == null && UnitName == other.UnitName)
            {
                // null and null
            }
            else
            {
                diff += UnitName.CompareTo(other.UnitName);
            }
            if (Price == null && Price == other.Price)
            {
                // null and null
            }
            else
            {
                diff += (int)(Price - other.Price);
            }
            if (Currency == null && Currency == other.Currency)
            {
                // null and null
            }
            else
            {
                diff += Currency.CompareTo(other.Currency);
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
            return diff;
        }
    }
}