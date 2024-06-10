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

namespace Gs2.Gs2Enchant.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RarityParameterCountModel : IComparable
	{
        public int? Count { set; get; } = null!;
        public int? Weight { set; get; } = null!;
        public RarityParameterCountModel WithCount(int? count) {
            this.Count = count;
            return this;
        }
        public RarityParameterCountModel WithWeight(int? weight) {
            this.Weight = weight;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RarityParameterCountModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RarityParameterCountModel()
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())))
                .WithWeight(!data.Keys.Contains("weight") || data["weight"] == null ? null : (int?)(data["weight"].ToString().Contains(".") ? (int)double.Parse(data["weight"].ToString()) : int.Parse(data["weight"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["count"] = Count,
                ["weight"] = Weight,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            if (Weight != null) {
                writer.WritePropertyName("weight");
                writer.Write((Weight.ToString().Contains(".") ? (int)double.Parse(Weight.ToString()) : int.Parse(Weight.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RarityParameterCountModel;
            var diff = 0;
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            if (Weight == null && Weight == other.Weight)
            {
                // null and null
            }
            else
            {
                diff += (int)(Weight - other.Weight);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Count < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterCountModel", "enchant.rarityParameterCountModel.count.error.invalid"),
                    });
                }
                if (Count > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterCountModel", "enchant.rarityParameterCountModel.count.error.invalid"),
                    });
                }
            }
            {
                if (Weight < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterCountModel", "enchant.rarityParameterCountModel.weight.error.invalid"),
                    });
                }
                if (Weight > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterCountModel", "enchant.rarityParameterCountModel.weight.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RarityParameterCountModel {
                Count = Count,
                Weight = Weight,
            };
        }
    }
}