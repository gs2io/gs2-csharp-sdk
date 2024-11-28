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
	public class PropertyFormModel : IComparable
	{
        public string PropertyFormModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Formation.Model.SlotModel[] Slots { set; get; } = null!;
        public PropertyFormModel WithPropertyFormModelId(string propertyFormModelId) {
            this.PropertyFormModelId = propertyFormModelId;
            return this;
        }
        public PropertyFormModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public PropertyFormModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public PropertyFormModel WithSlots(Gs2.Gs2Formation.Model.SlotModel[] slots) {
            this.Slots = slots;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:propertyForm:(?<propertyFormModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:propertyForm:(?<propertyFormModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:propertyForm:(?<propertyFormModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _propertyFormModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:propertyForm:(?<propertyFormModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetPropertyFormModelNameFromGrn(
            string grn
        )
        {
            var match = _propertyFormModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["propertyFormModelName"].Success)
            {
                return null;
            }
            return match.Groups["propertyFormModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PropertyFormModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PropertyFormModel()
                .WithPropertyFormModelId(!data.Keys.Contains("propertyFormModelId") || data["propertyFormModelId"] == null ? null : data["propertyFormModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithSlots(!data.Keys.Contains("slots") || data["slots"] == null || !data["slots"].IsArray ? null : data["slots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Formation.Model.SlotModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData slotsJsonData = null;
            if (Slots != null && Slots.Length > 0)
            {
                slotsJsonData = new JsonData();
                foreach (var slot in Slots)
                {
                    slotsJsonData.Add(slot.ToJson());
                }
            }
            return new JsonData {
                ["propertyFormModelId"] = PropertyFormModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["slots"] = slotsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PropertyFormModelId != null) {
                writer.WritePropertyName("propertyFormModelId");
                writer.Write(PropertyFormModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Slots != null) {
                writer.WritePropertyName("slots");
                writer.WriteArrayStart();
                foreach (var slot in Slots)
                {
                    if (slot != null) {
                        slot.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as PropertyFormModel;
            var diff = 0;
            if (PropertyFormModelId == null && PropertyFormModelId == other.PropertyFormModelId)
            {
                // null and null
            }
            else
            {
                diff += PropertyFormModelId.CompareTo(other.PropertyFormModelId);
            }
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
            if (Slots == null && Slots == other.Slots)
            {
                // null and null
            }
            else
            {
                diff += Slots.Length - other.Slots.Length;
                for (var i = 0; i < Slots.Length; i++)
                {
                    diff += Slots[i].CompareTo(other.Slots[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (PropertyFormModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("propertyFormModel", "formation.propertyFormModel.propertyFormModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("propertyFormModel", "formation.propertyFormModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("propertyFormModel", "formation.propertyFormModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Slots.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("propertyFormModel", "formation.propertyFormModel.slots.error.tooFew"),
                    });
                }
                if (Slots.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("propertyFormModel", "formation.propertyFormModel.slots.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new PropertyFormModel {
                PropertyFormModelId = PropertyFormModelId,
                Name = Name,
                Metadata = Metadata,
                Slots = Slots.Clone() as Gs2.Gs2Formation.Model.SlotModel[],
            };
        }
    }
}