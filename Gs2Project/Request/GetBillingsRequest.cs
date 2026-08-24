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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetBillingsRequest : Gs2Request<GetBillingsRequest>
	{
         public int? Year { set; get; } = null!;
         public int? Month { set; get; } = null!;
         public string Service { set; get; } = null!;
        public GetBillingsRequest WithYear(int? year) {
            this.Year = year;
            return this;
        }
        public GetBillingsRequest WithMonth(int? month) {
            this.Month = month;
            return this;
        }
        public GetBillingsRequest WithService(string service) {
            this.Service = service;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetBillingsRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetBillingsRequest()
                .WithYear(!data.Keys.Contains("year") || data["year"] == null ? null : (int?)(data["year"].ToString().Contains(".") ? (int)double.Parse(data["year"].ToString()) : int.Parse(data["year"].ToString())))
                .WithMonth(!data.Keys.Contains("month") || data["month"] == null ? null : (int?)(data["month"].ToString().Contains(".") ? (int)double.Parse(data["month"].ToString()) : int.Parse(data["month"].ToString())))
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["year"] = Year,
                ["month"] = Month,
                ["service"] = Service,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Year != null) {
                writer.WritePropertyName("year");
                writer.Write((Year.ToString().Contains(".") ? (int)double.Parse(Year.ToString()) : int.Parse(Year.ToString())));
            }
            if (Month != null) {
                writer.WritePropertyName("month");
                writer.Write((Month.ToString().Contains(".") ? (int)double.Parse(Month.ToString()) : int.Parse(Month.ToString())));
            }
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Year + ":";
            key += Month + ":";
            key += Service + ":";
            return key;
        }
    }
}