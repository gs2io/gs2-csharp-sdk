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

namespace Gs2.Gs2Enchant.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class RarityParameterCountModel : IComparable
	{
        public int? Count { set; get; }
        public int? Weight { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new RarityParameterCountModel()
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["count"].ToString()))
                .WithWeight(!data.Keys.Contains("weight") || data["weight"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["weight"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type RarityParameterCountModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Count, other.Count);
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