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

namespace Gs2.Gs2Showcase.Model
{

	[Preserve]
	public class SalesItemGroup : IComparable
	{
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Showcase.Model.SalesItem[] SalesItems { set; get; }

        public SalesItemGroup WithName(string name) {
            this.Name = name;
            return this;
        }

        public SalesItemGroup WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public SalesItemGroup WithSalesItems(Gs2.Gs2Showcase.Model.SalesItem[] salesItems) {
            this.SalesItems = salesItems;
            return this;
        }

    	[Preserve]
        public static SalesItemGroup FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SalesItemGroup()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSalesItems(!data.Keys.Contains("salesItems") || data["salesItems"] == null ? new Gs2.Gs2Showcase.Model.SalesItem[]{} : data["salesItems"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Showcase.Model.SalesItem.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["salesItems"] = new JsonData(SalesItems == null ? new JsonData[]{} :
                        SalesItems.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
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
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (SalesItems != null) {
                writer.WritePropertyName("salesItems");
                writer.WriteArrayStart();
                foreach (var salesItem in SalesItems)
                {
                    if (salesItem != null) {
                        salesItem.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SalesItemGroup;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (SalesItems == null && SalesItems == other.SalesItems)
            {
                // null and null
            }
            else
            {
                diff += SalesItems.Length - other.SalesItems.Length;
                for (var i = 0; i < SalesItems.Length; i++)
                {
                    diff += SalesItems[i].CompareTo(other.SalesItems[i]);
                }
            }
            return diff;
        }
    }
}