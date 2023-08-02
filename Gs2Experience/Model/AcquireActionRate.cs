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
        public string Name { set; get; }
        public double[] Rates { set; get; }
        public AcquireActionRate WithName(string name) {
            this.Name = name;
            return this;
        }
        public AcquireActionRate WithRates(double[] rates) {
            this.Rates = rates;
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
                .WithRates(!data.Keys.Contains("rates") || data["rates"] == null ? new double[]{} : data["rates"].Cast<JsonData>().Select(v => {
                    return double.Parse(v.ToString());
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["rates"] = Rates == null ? null : new JsonData(
                        Rates.Select(v => {
                            return new JsonData((double?)double.Parse(v.ToString()));
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Rates != null) {
                writer.WritePropertyName("rates");
                writer.WriteArrayStart();
                foreach (var rate in Rates)
                {
                    if (rate != null) {
                        writer.Write(double.Parse(rate.ToString()));
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
            return diff;
        }
    }
}