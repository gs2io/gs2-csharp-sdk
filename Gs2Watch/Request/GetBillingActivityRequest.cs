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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Watch.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Watch.Request
{
	[Preserve]
	[System.Serializable]
	public class GetBillingActivityRequest : Gs2Request<GetBillingActivityRequest>
	{
        public int? Year { set; get; }
        public int? Month { set; get; }
        public string Service { set; get; }
        public string ActivityType { set; get; }

        public GetBillingActivityRequest WithYear(int? year) {
            this.Year = year;
            return this;
        }

        public GetBillingActivityRequest WithMonth(int? month) {
            this.Month = month;
            return this;
        }

        public GetBillingActivityRequest WithService(string service) {
            this.Service = service;
            return this;
        }

        public GetBillingActivityRequest WithActivityType(string activityType) {
            this.ActivityType = activityType;
            return this;
        }

    	[Preserve]
        public static GetBillingActivityRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetBillingActivityRequest()
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)int.Parse(data["year"].ToString()))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)int.Parse(data["month"].ToString()))
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithActivityType(!data.Keys.Contains("activityType") || data["activityType"] == null ? null : data["activityType"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["year"] = Year,
                ["month"] = Month,
                ["service"] = Service,
                ["activityType"] = ActivityType,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            writer.WriteObjectEnd();
        }
    }
}