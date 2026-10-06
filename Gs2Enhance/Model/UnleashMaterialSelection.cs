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
	public partial class UnleashMaterialSelection : IComparable
	{
        public string Name { set; get; }
        public string[] ItemSetIds { set; get; }
        public UnleashMaterialSelection WithName(string name) {
            this.Name = name;
            return this;
        }
        public UnleashMaterialSelection WithItemSetIds(string[] itemSetIds) {
            this.ItemSetIds = itemSetIds;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnleashMaterialSelection FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new UnleashMaterialSelection()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithItemSetIds(!data.Keys.Contains("itemSetIds") || data["itemSetIds"] == null || !data["itemSetIds"].IsArray ? null : data["itemSetIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData itemSetIdsJsonData = null;
            if (ItemSetIds != null && ItemSetIds.Length > 0)
            {
                itemSetIdsJsonData = new JsonData();
                foreach (var itemSetId in ItemSetIds)
                {
                    itemSetIdsJsonData.Add(itemSetId);
                }
            }
            return new JsonData {
                ["name"] = Name,
                ["itemSetIds"] = itemSetIdsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (ItemSetIds != null) {
                writer.WritePropertyName("itemSetIds");
                writer.WriteArrayStart();
                foreach (var itemSetId in ItemSetIds)
                {
                    if (itemSetId != null) {
                        writer.Write(itemSetId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashMaterialSelection;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UnleashMaterialSelection.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(ItemSetIds, other.ItemSetIds);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashMaterialSelection", "enhance.unleashMaterialSelection.name.error.tooLong"),
                    });
                }
            }
            {
                if (ItemSetIds.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashMaterialSelection", "enhance.unleashMaterialSelection.itemSetIds.error.tooFew"),
                    });
                }
                if (ItemSetIds.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashMaterialSelection", "enhance.unleashMaterialSelection.itemSetIds.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnleashMaterialSelection {
                Name = Name,
                ItemSetIds = ItemSetIds?.Clone() as string[],
            };
        }
    }
}