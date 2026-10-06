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
	public partial class UnleashQuantityMaterialSetting : IComparable
	{
        public string MatchType { set; get; }
        public string MaterialInventoryModelId { set; get; }
        public string ItemModelId { set; get; }
        public int? Count { set; get; }
        public UnleashQuantityMaterialSetting WithMatchType(string matchType) {
            this.MatchType = matchType;
            return this;
        }
        public UnleashQuantityMaterialSetting WithMaterialInventoryModelId(string materialInventoryModelId) {
            this.MaterialInventoryModelId = materialInventoryModelId;
            return this;
        }
        public UnleashQuantityMaterialSetting WithItemModelId(string itemModelId) {
            this.ItemModelId = itemModelId;
            return this;
        }
        public UnleashQuantityMaterialSetting WithCount(int? count) {
            this.Count = count;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnleashQuantityMaterialSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new UnleashQuantityMaterialSetting()
                .WithMatchType(!data.Keys.Contains("matchType") || data["matchType"] == null ? null : data["matchType"].ToString())
                .WithMaterialInventoryModelId(!data.Keys.Contains("materialInventoryModelId") || data["materialInventoryModelId"] == null ? null : data["materialInventoryModelId"].ToString())
                .WithItemModelId(!data.Keys.Contains("itemModelId") || data["itemModelId"] == null ? null : data["itemModelId"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["count"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["matchType"] = MatchType,
                ["materialInventoryModelId"] = MaterialInventoryModelId,
                ["itemModelId"] = ItemModelId,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MatchType != null) {
                writer.WritePropertyName("matchType");
                writer.Write(MatchType.ToString());
            }
            if (MaterialInventoryModelId != null) {
                writer.WritePropertyName("materialInventoryModelId");
                writer.Write(MaterialInventoryModelId.ToString());
            }
            if (ItemModelId != null) {
                writer.WritePropertyName("itemModelId");
                writer.Write(ItemModelId.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashQuantityMaterialSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UnleashQuantityMaterialSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(MatchType, other.MatchType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaterialInventoryModelId, other.MaterialInventoryModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ItemModelId, other.ItemModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Count, other.Count);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                switch (MatchType) {
                    case "sameGroup":
                    case "specified":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("unleashQuantityMaterialSetting", "enhance.unleashQuantityMaterialSetting.matchType.error.invalid"),
                        });
                }
            }
            if (MatchType == "sameGroup") {
                if (MaterialInventoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashQuantityMaterialSetting", "enhance.unleashQuantityMaterialSetting.materialInventoryModelId.error.tooLong"),
                    });
                }
            }
            if (MatchType == "specified") {
                if (ItemModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashQuantityMaterialSetting", "enhance.unleashQuantityMaterialSetting.itemModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashQuantityMaterialSetting", "enhance.unleashQuantityMaterialSetting.count.error.invalid"),
                    });
                }
                if (Count > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashQuantityMaterialSetting", "enhance.unleashQuantityMaterialSetting.count.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnleashQuantityMaterialSetting {
                MatchType = MatchType,
                MaterialInventoryModelId = MaterialInventoryModelId,
                ItemModelId = ItemModelId,
                Count = Count,
            };
        }
    }
}