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

namespace Gs2.Gs2Buff.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class OverrideBuffRate : IComparable
	{
        public string Name { set; get; } = null!;
        public float? Rate { set; get; } = null!;
        public OverrideBuffRate WithName(string name) {
            this.Name = name;
            return this;
        }
        public OverrideBuffRate WithRate(float? rate) {
            this.Rate = rate;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static OverrideBuffRate FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new OverrideBuffRate()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithRate(!data.Keys.Contains("rate") || data["rate"] == null ? null : (float?)float.Parse(data["rate"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["rate"] = Rate,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Rate != null) {
                writer.WritePropertyName("rate");
                writer.Write(float.Parse(Rate.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as OverrideBuffRate;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
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
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("overrideBuffRate", "buff.overrideBuffRate.name.error.tooLong"),
                    });
                }
            }
            {
                if (Rate < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("overrideBuffRate", "buff.overrideBuffRate.rate.error.invalid"),
                    });
                }
                if (Rate > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("overrideBuffRate", "buff.overrideBuffRate.rate.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new OverrideBuffRate {
                Name = Name,
                Rate = Rate,
            };
        }
    }
}