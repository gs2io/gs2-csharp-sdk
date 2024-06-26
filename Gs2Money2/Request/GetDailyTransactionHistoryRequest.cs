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
using Gs2.Gs2Money2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetDailyTransactionHistoryRequest : Gs2Request<GetDailyTransactionHistoryRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public int? Year { set; get; } = null!;
         public int? Month { set; get; } = null!;
         public int? Day { set; get; } = null!;
         public string Currency { set; get; } = null!;
        public GetDailyTransactionHistoryRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetDailyTransactionHistoryRequest WithYear(int? year) {
            this.Year = year;
            return this;
        }
        public GetDailyTransactionHistoryRequest WithMonth(int? month) {
            this.Month = month;
            return this;
        }
        public GetDailyTransactionHistoryRequest WithDay(int? day) {
            this.Day = day;
            return this;
        }
        public GetDailyTransactionHistoryRequest WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetDailyTransactionHistoryRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetDailyTransactionHistoryRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)(data["year"].ToString().Contains(".") ? (int)double.Parse(data["year"].ToString()) : int.Parse(data["year"].ToString())))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)(data["month"].ToString().Contains(".") ? (int)double.Parse(data["month"].ToString()) : int.Parse(data["month"].ToString())))
                .WithDay(!data.Keys.Contains("day") || data["day"] == null ? null : (int?)(data["day"].ToString().Contains(".") ? (int)double.Parse(data["day"].ToString()) : int.Parse(data["day"].ToString())))
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["year"] = Year,
                ["month"] = Month,
                ["day"] = Day,
                ["currency"] = Currency,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Year != null) {
                writer.WritePropertyName("year");
                writer.Write((Year.ToString().Contains(".") ? (int)double.Parse(Year.ToString()) : int.Parse(Year.ToString())));
            }
            if (Month != null) {
                writer.WritePropertyName("month");
                writer.Write((Month.ToString().Contains(".") ? (int)double.Parse(Month.ToString()) : int.Parse(Month.ToString())));
            }
            if (Day != null) {
                writer.WritePropertyName("day");
                writer.Write((Day.ToString().Contains(".") ? (int)double.Parse(Day.ToString()) : int.Parse(Day.ToString())));
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Year + ":";
            key += Month + ":";
            key += Day + ":";
            key += Currency + ":";
            return key;
        }
    }
}