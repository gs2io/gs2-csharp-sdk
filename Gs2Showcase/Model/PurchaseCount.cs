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

namespace Gs2.Gs2Showcase.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class PurchaseCount : IComparable
	{
        public string Name { set; get; } = null!;
        public int? Count { set; get; } = null!;
        public PurchaseCount WithName(string name) {
            this.Name = name;
            return this;
        }
        public PurchaseCount WithCount(int? count) {
            this.Count = count;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PurchaseCount FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PurchaseCount()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as PurchaseCount;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("purchaseCount", "showcase.purchaseCount.name.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("purchaseCount", "showcase.purchaseCount.count.error.invalid"),
                    });
                }
                if (Count > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("purchaseCount", "showcase.purchaseCount.count.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new PurchaseCount {
                Name = Name,
                Count = Count,
            };
        }
    }
}