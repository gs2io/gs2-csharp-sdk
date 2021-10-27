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

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Watch.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DescribeBillingActivitiesRequest : Gs2Request<DescribeBillingActivitiesRequest>
	{
        public int? Year { set; get; }
        public int? Month { set; get; }
        public string Service { set; get; }
        public string PageToken { set; get; }
        public int? Limit { set; get; }

        public DescribeBillingActivitiesRequest WithYear(int? year) {
            this.Year = year;
            return this;
        }

        public DescribeBillingActivitiesRequest WithMonth(int? month) {
            this.Month = month;
            return this;
        }

        public DescribeBillingActivitiesRequest WithService(string service) {
            this.Service = service;
            return this;
        }

        public DescribeBillingActivitiesRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }

        public DescribeBillingActivitiesRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeBillingActivitiesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeBillingActivitiesRequest()
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)int.Parse(data["year"].ToString()))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)int.Parse(data["month"].ToString()))
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)int.Parse(data["limit"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["year"] = Year,
                ["month"] = Month,
                ["service"] = Service,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
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
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write(int.Parse(Limit.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}