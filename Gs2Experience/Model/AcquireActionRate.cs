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

namespace Gs2.Gs2Experience.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AcquireActionRate : IComparable
	{
        public string Name { set; get; } = null!;
        public string Mode { set; get; } = null!;
        public double[] Rates { set; get; } = null!;
        public string[] BigRates { set; get; } = null!;
        public AcquireActionRate WithName(string name) {
            this.Name = name;
            return this;
        }
        public AcquireActionRate WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public AcquireActionRate WithRates(double[] rates) {
            this.Rates = rates;
            return this;
        }
        public AcquireActionRate WithBigRates(string[] bigRates) {
            this.BigRates = bigRates;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireActionRate FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireActionRate()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithRates(!data.Keys.Contains("rates") || data["rates"] == null || !data["rates"].IsArray ? null : data["rates"].Cast<JsonData>().Select(v => {
                    return double.Parse(v.ToString());
                }).ToArray())
                .WithBigRates(!data.Keys.Contains("bigRates") || data["bigRates"] == null || !data["bigRates"].IsArray ? null : data["bigRates"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData ratesJsonData = null;
            if (Rates != null && Rates.Length > 0)
            {
                ratesJsonData = new JsonData();
                foreach (var rate in Rates)
                {
                    ratesJsonData.Add(rate);
                }
            }
            JsonData bigRatesJsonData = null;
            if (BigRates != null && BigRates.Length > 0)
            {
                bigRatesJsonData = new JsonData();
                foreach (var bigRate in BigRates)
                {
                    bigRatesJsonData.Add(bigRate);
                }
            }
            return new JsonData {
                ["name"] = Name,
                ["mode"] = Mode,
                ["rates"] = ratesJsonData,
                ["bigRates"] = bigRatesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Mode != null) {
                writer.WritePropertyName("mode");
                writer.Write(Mode.ToString());
            }
            if (Rates != null) {
                writer.WritePropertyName("rates");
                writer.WriteArrayStart();
                foreach (var rate in Rates)
                {
                    writer.Write(double.Parse(rate.ToString()));
                }
                writer.WriteArrayEnd();
            }
            if (BigRates != null) {
                writer.WritePropertyName("bigRates");
                writer.WriteArrayStart();
                foreach (var bigRate in BigRates)
                {
                    if (bigRate != null) {
                        writer.Write(bigRate.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AcquireActionRate;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Mode == null && Mode == other.Mode)
            {
                // null and null
            }
            else
            {
                diff += Mode.CompareTo(other.Mode);
            }
            if (Rates == null && Rates == other.Rates)
            {
                // null and null
            }
            else
            {
                diff += Rates.Length - other.Rates.Length;
                for (var i = 0; i < Rates.Length; i++)
                {
                    diff += (int)(Rates[i] - other.Rates[i]);
                }
            }
            if (BigRates == null && BigRates == other.BigRates)
            {
                // null and null
            }
            else
            {
                diff += BigRates.Length - other.BigRates.Length;
                for (var i = 0; i < BigRates.Length; i++)
                {
                    diff += BigRates[i].CompareTo(other.BigRates[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionRate", "experience.acquireActionRate.name.error.tooLong"),
                    });
                }
            }
            {
                switch (Mode) {
                    case "double":
                    case "big":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("acquireActionRate", "experience.acquireActionRate.mode.error.invalid"),
                        });
                }
            }
            if (Mode == "double") {
                if (Rates.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionRate", "experience.acquireActionRate.rates.error.tooFew"),
                    });
                }
                if (Rates.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionRate", "experience.acquireActionRate.rates.error.tooMany"),
                    });
                }
            }
            if (Mode == "big") {
                if (BigRates.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionRate", "experience.acquireActionRate.bigRates.error.tooFew"),
                    });
                }
                if (BigRates.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionRate", "experience.acquireActionRate.bigRates.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new AcquireActionRate {
                Name = Name,
                Mode = Mode,
                Rates = Rates.Clone() as double[],
                BigRates = BigRates.Clone() as string[],
            };
        }
    }
}