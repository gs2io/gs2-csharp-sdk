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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Enhance.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class BonusRate : IComparable
	{
        public float? Rate { set; get; }
        public int? Weight { set; get; }
        public BonusRate WithRate(float? rate) {
            this.Rate = rate;
            return this;
        }
        public BonusRate WithWeight(int? weight) {
            this.Weight = weight;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BonusRate FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BonusRate()
                .WithRate(!data.Keys.Contains("rate") || data["rate"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableFloat(data["rate"].ToString()))
                .WithWeight(!data.Keys.Contains("weight") || data["weight"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["weight"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["rate"] = Rate,
                ["weight"] = Weight,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Rate != null) {
                writer.WritePropertyName("rate");
                writer.Write(float.Parse(Rate.ToString()));
            }
            if (Weight != null) {
                writer.WritePropertyName("weight");
                writer.Write((Weight.ToString().Contains(".") ? (int)double.Parse(Weight.ToString()) : int.Parse(Weight.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BonusRate;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type BonusRate.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Rate, other.Rate);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Weight, other.Weight);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Rate < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusRate", "enhance.bonusRate.rate.error.invalid"),
                    });
                }
                if (Rate > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusRate", "enhance.bonusRate.rate.error.invalid"),
                    });
                }
            }
            {
                if (Weight < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusRate", "enhance.bonusRate.weight.error.invalid"),
                    });
                }
                if (Weight > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusRate", "enhance.bonusRate.weight.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BonusRate {
                Rate = Rate,
                Weight = Weight,
            };
        }
    }
}