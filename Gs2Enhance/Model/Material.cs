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
using UnityEngine.Scripting;

namespace Gs2.Gs2Enhance.Model
{

	[Preserve]
	public class Material : IComparable
	{
        public string MaterialItemSetId { set; get; }
        public int? Count { set; get; }

        public Material WithMaterialItemSetId(string materialItemSetId) {
            this.MaterialItemSetId = materialItemSetId;
            return this;
        }

        public Material WithCount(int? count) {
            this.Count = count;
            return this;
        }

    	[Preserve]
        public static Material FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Material()
                .WithMaterialItemSetId(!data.Keys.Contains("materialItemSetId") || data["materialItemSetId"] == null ? null : data["materialItemSetId"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)int.Parse(data["count"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["materialItemSetId"] = MaterialItemSetId,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MaterialItemSetId != null) {
                writer.WritePropertyName("materialItemSetId");
                writer.Write(MaterialItemSetId.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(int.Parse(Count.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Material;
            var diff = 0;
            if (MaterialItemSetId == null && MaterialItemSetId == other.MaterialItemSetId)
            {
                // null and null
            }
            else
            {
                diff += MaterialItemSetId.CompareTo(other.MaterialItemSetId);
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
    }
}