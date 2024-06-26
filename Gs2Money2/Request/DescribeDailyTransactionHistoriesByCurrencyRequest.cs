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
	public class DescribeDailyTransactionHistoriesByCurrencyRequest : Gs2Request<DescribeDailyTransactionHistoriesByCurrencyRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Currency { set; get; } = null!;
         public int? Year { set; get; } = null!;
         public int? Month { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
        public DescribeDailyTransactionHistoriesByCurrencyRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DescribeDailyTransactionHistoriesByCurrencyRequest WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }
        public DescribeDailyTransactionHistoriesByCurrencyRequest WithYear(int? year) {
            this.Year = year;
            return this;
        }
        public DescribeDailyTransactionHistoriesByCurrencyRequest WithMonth(int? month) {
            this.Month = month;
            return this;
        }
        public DescribeDailyTransactionHistoriesByCurrencyRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public DescribeDailyTransactionHistoriesByCurrencyRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeDailyTransactionHistoriesByCurrencyRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeDailyTransactionHistoriesByCurrencyRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)(data["year"].ToString().Contains(".") ? (int)double.Parse(data["year"].ToString()) : int.Parse(data["year"].ToString())))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)(data["month"].ToString().Contains(".") ? (int)double.Parse(data["month"].ToString()) : int.Parse(data["month"].ToString())))
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["currency"] = Currency,
                ["year"] = Year,
                ["month"] = Month,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            if (Year != null) {
                writer.WritePropertyName("year");
                writer.Write((Year.ToString().Contains(".") ? (int)double.Parse(Year.ToString()) : int.Parse(Year.ToString())));
            }
            if (Month != null) {
                writer.WritePropertyName("month");
                writer.Write((Month.ToString().Contains(".") ? (int)double.Parse(Month.ToString()) : int.Parse(Month.ToString())));
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Currency + ":";
            key += Year + ":";
            key += Month + ":";
            key += PageToken + ":";
            key += Limit + ":";
            return key;
        }
    }
}