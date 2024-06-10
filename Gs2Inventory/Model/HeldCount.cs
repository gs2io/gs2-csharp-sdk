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

namespace Gs2.Gs2Inventory.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class HeldCount : IComparable
	{
        public string ItemName { set; get; } = null!;
        public long? Count { set; get; } = null!;
        public HeldCount WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public HeldCount WithCount(long? count) {
            this.Count = count;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static HeldCount FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new HeldCount()
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (long?)(data["count"].ToString().Contains(".") ? (long)double.Parse(data["count"].ToString()) : long.Parse(data["count"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["itemName"] = ItemName,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ItemName != null) {
                writer.WritePropertyName("itemName");
                writer.Write(ItemName.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (long)double.Parse(Count.ToString()) : long.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as HeldCount;
            var diff = 0;
            if (ItemName == null && ItemName == other.ItemName)
            {
                // null and null
            }
            else
            {
                diff += ItemName.CompareTo(other.ItemName);
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
                if (ItemName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("heldCount", "inventory.heldCount.itemName.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("heldCount", "inventory.heldCount.count.error.invalid"),
                    });
                }
                if (Count > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("heldCount", "inventory.heldCount.count.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new HeldCount {
                ItemName = ItemName,
                Count = Count,
            };
        }
    }
}