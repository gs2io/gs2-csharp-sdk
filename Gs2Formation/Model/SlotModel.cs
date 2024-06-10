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

namespace Gs2.Gs2Formation.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SlotModel : IComparable
	{
        public string Name { set; get; } = null!;
        public string PropertyRegex { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public SlotModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public SlotModel WithPropertyRegex(string propertyRegex) {
            this.PropertyRegex = propertyRegex;
            return this;
        }
        public SlotModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SlotModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SlotModel()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithPropertyRegex(!data.Keys.Contains("propertyRegex") || data["propertyRegex"] == null ? null : data["propertyRegex"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["propertyRegex"] = PropertyRegex,
                ["metadata"] = Metadata,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (PropertyRegex != null) {
                writer.WritePropertyName("propertyRegex");
                writer.Write(PropertyRegex.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SlotModel;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (PropertyRegex == null && PropertyRegex == other.PropertyRegex)
            {
                // null and null
            }
            else
            {
                diff += PropertyRegex.CompareTo(other.PropertyRegex);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("slotModel", "formation.slotModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (PropertyRegex.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("slotModel", "formation.slotModel.propertyRegex.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("slotModel", "formation.slotModel.metadata.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SlotModel {
                Name = Name,
                PropertyRegex = PropertyRegex,
                Metadata = Metadata,
            };
        }
    }
}