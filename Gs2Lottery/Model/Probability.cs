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

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Probability : IComparable
	{
        public Gs2.Gs2Lottery.Model.DrawnPrize Prize { set; get; } = null!;
        public float? Rate { set; get; } = null!;
        public Probability WithPrize(Gs2.Gs2Lottery.Model.DrawnPrize prize) {
            this.Prize = prize;
            return this;
        }
        public Probability WithRate(float? rate) {
            this.Rate = rate;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Probability FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Probability()
                .WithPrize(!data.Keys.Contains("prize") || data["prize"] == null ? null : Gs2.Gs2Lottery.Model.DrawnPrize.FromJson(data["prize"]))
                .WithRate(!data.Keys.Contains("rate") || data["rate"] == null ? null : (float?)float.Parse(data["rate"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["prize"] = Prize?.ToJson(),
                ["rate"] = Rate,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Prize != null) {
                writer.WritePropertyName("prize");
                Prize.WriteJson(writer);
            }
            if (Rate != null) {
                writer.WritePropertyName("rate");
                writer.Write(float.Parse(Rate.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Probability;
            var diff = 0;
            if (Prize == null && Prize == other.Prize)
            {
                // null and null
            }
            else
            {
                diff += Prize.CompareTo(other.Prize);
            }
            if (Rate == null && Rate == other.Rate)
            {
                // null and null
            }
            else
            {
                diff += (int)(Rate - other.Rate);
            }
            return diff;
        }

        public void Validate() {
            {
            }
            {
                if (Rate < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("probability", "lottery.probability.rate.error.invalid"),
                    });
                }
                if (Rate > 1.0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("probability", "lottery.probability.rate.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Probability {
                Prize = Prize.Clone() as Gs2.Gs2Lottery.Model.DrawnPrize,
                Rate = Rate,
            };
        }
    }
}