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

namespace Gs2.Gs2Watch.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BillingActivity : IComparable
	{
        public string BillingActivityId { set; get; }
        public int? Year { set; get; }
        public int? Month { set; get; }
        public string Service { set; get; }
        public string ActivityType { set; get; }
        public long? Value { set; get; }

        public BillingActivity WithBillingActivityId(string billingActivityId) {
            this.BillingActivityId = billingActivityId;
            return this;
        }

        public BillingActivity WithYear(int? year) {
            this.Year = year;
            return this;
        }

        public BillingActivity WithMonth(int? month) {
            this.Month = month;
            return this;
        }

        public BillingActivity WithService(string service) {
            this.Service = service;
            return this;
        }

        public BillingActivity WithActivityType(string activityType) {
            this.ActivityType = activityType;
            return this;
        }

        public BillingActivity WithValue(long? value) {
            this.Value = value;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):watch:(?<year>.+):(?<month>.+):(?<service>.+):(?<activityType>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):watch:(?<year>.+):(?<month>.+):(?<service>.+):(?<activityType>.+)",
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

        private static System.Text.RegularExpressions.Regex _yearRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):watch:(?<year>.+):(?<month>.+):(?<service>.+):(?<activityType>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):watch:(?<year>.+):(?<month>.+):(?<service>.+):(?<activityType>.+)",
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

        private static System.Text.RegularExpressions.Regex _serviceRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):watch:(?<year>.+):(?<month>.+):(?<service>.+):(?<activityType>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetServiceFromGrn(
            string grn
        )
        {
            var match = _serviceRegex.Match(grn);
            if (!match.Success || !match.Groups["service"].Success)
            {
                return null;
            }
            return match.Groups["service"].Value;
        }

        private static System.Text.RegularExpressions.Regex _activityTypeRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):watch:(?<year>.+):(?<month>.+):(?<service>.+):(?<activityType>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetActivityTypeFromGrn(
            string grn
        )
        {
            var match = _activityTypeRegex.Match(grn);
            if (!match.Success || !match.Groups["activityType"].Success)
            {
                return null;
            }
            return match.Groups["activityType"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BillingActivity FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BillingActivity()
                .WithBillingActivityId(!data.Keys.Contains("billingActivityId") || data["billingActivityId"] == null ? null : data["billingActivityId"].ToString())
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)int.Parse(data["year"].ToString()))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)int.Parse(data["month"].ToString()))
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithActivityType(!data.Keys.Contains("activityType") || data["activityType"] == null ? null : data["activityType"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (long?)long.Parse(data["value"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["billingActivityId"] = BillingActivityId,
                ["year"] = Year,
                ["month"] = Month,
                ["service"] = Service,
                ["activityType"] = ActivityType,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BillingActivityId != null) {
                writer.WritePropertyName("billingActivityId");
                writer.Write(BillingActivityId.ToString());
            }
            if (Year != null) {
                writer.WritePropertyName("year");
                writer.Write(int.Parse(Year.ToString()));
            }
            if (Month != null) {
                writer.WritePropertyName("month");
                writer.Write(int.Parse(Month.ToString()));
            }
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            if (ActivityType != null) {
                writer.WritePropertyName("activityType");
                writer.Write(ActivityType.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(long.Parse(Value.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BillingActivity;
            var diff = 0;
            if (BillingActivityId == null && BillingActivityId == other.BillingActivityId)
            {
                // null and null
            }
            else
            {
                diff += BillingActivityId.CompareTo(other.BillingActivityId);
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
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            return diff;
        }
    }
}