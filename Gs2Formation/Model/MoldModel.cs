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
	public class MoldModel : IComparable
	{
        public string MoldModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public int? InitialMaxCapacity { set; get; } = null!;
        public int? MaxCapacity { set; get; } = null!;
        public Gs2.Gs2Formation.Model.FormModel FormModel { set; get; } = null!;
        public MoldModel WithMoldModelId(string moldModelId) {
            this.MoldModelId = moldModelId;
            return this;
        }
        public MoldModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public MoldModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public MoldModel WithInitialMaxCapacity(int? initialMaxCapacity) {
            this.InitialMaxCapacity = initialMaxCapacity;
            return this;
        }
        public MoldModel WithMaxCapacity(int? maxCapacity) {
            this.MaxCapacity = maxCapacity;
            return this;
        }
        public MoldModel WithFormModel(Gs2.Gs2Formation.Model.FormModel formModel) {
            this.FormModel = formModel;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:mold:(?<moldModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:mold:(?<moldModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:mold:(?<moldModelName>.+)",
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

        private static System.Text.RegularExpressions.Regex _moldModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):model:mold:(?<moldModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMoldModelNameFromGrn(
            string grn
        )
        {
            var match = _moldModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["moldModelName"].Success)
            {
                return null;
            }
            return match.Groups["moldModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MoldModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MoldModel()
                .WithMoldModelId(!data.Keys.Contains("moldModelId") || data["moldModelId"] == null ? null : data["moldModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInitialMaxCapacity(!data.Keys.Contains("initialMaxCapacity") || data["initialMaxCapacity"] == null ? null : (int?)(data["initialMaxCapacity"].ToString().Contains(".") ? (int)double.Parse(data["initialMaxCapacity"].ToString()) : int.Parse(data["initialMaxCapacity"].ToString())))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : (int?)(data["maxCapacity"].ToString().Contains(".") ? (int)double.Parse(data["maxCapacity"].ToString()) : int.Parse(data["maxCapacity"].ToString())))
                .WithFormModel(!data.Keys.Contains("formModel") || data["formModel"] == null ? null : Gs2.Gs2Formation.Model.FormModel.FromJson(data["formModel"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["moldModelId"] = MoldModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["initialMaxCapacity"] = InitialMaxCapacity,
                ["maxCapacity"] = MaxCapacity,
                ["formModel"] = FormModel?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MoldModelId != null) {
                writer.WritePropertyName("moldModelId");
                writer.Write(MoldModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InitialMaxCapacity != null) {
                writer.WritePropertyName("initialMaxCapacity");
                writer.Write((InitialMaxCapacity.ToString().Contains(".") ? (int)double.Parse(InitialMaxCapacity.ToString()) : int.Parse(InitialMaxCapacity.ToString())));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write((MaxCapacity.ToString().Contains(".") ? (int)double.Parse(MaxCapacity.ToString()) : int.Parse(MaxCapacity.ToString())));
            }
            if (FormModel != null) {
                writer.WritePropertyName("formModel");
                FormModel.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as MoldModel;
            var diff = 0;
            if (MoldModelId == null && MoldModelId == other.MoldModelId)
            {
                // null and null
            }
            else
            {
                diff += MoldModelId.CompareTo(other.MoldModelId);
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
            if (InitialMaxCapacity == null && InitialMaxCapacity == other.InitialMaxCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(InitialMaxCapacity - other.InitialMaxCapacity);
            }
            if (MaxCapacity == null && MaxCapacity == other.MaxCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxCapacity - other.MaxCapacity);
            }
            if (FormModel == null && FormModel == other.FormModel)
            {
                // null and null
            }
            else
            {
                diff += FormModel.CompareTo(other.FormModel);
            }
            return diff;
        }

        public void Validate() {
            {
                if (MoldModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("moldModel", "formation.moldModel.moldModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("moldModel", "formation.moldModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("moldModel", "formation.moldModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (InitialMaxCapacity < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("moldModel", "formation.moldModel.initialMaxCapacity.error.invalid"),
                    });
                }
                if (InitialMaxCapacity > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("moldModel", "formation.moldModel.initialMaxCapacity.error.invalid"),
                    });
                }
            }
            {
                if (MaxCapacity < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("moldModel", "formation.moldModel.maxCapacity.error.invalid"),
                    });
                }
                if (MaxCapacity > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("moldModel", "formation.moldModel.maxCapacity.error.invalid"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new MoldModel {
                MoldModelId = MoldModelId,
                Name = Name,
                Metadata = Metadata,
                InitialMaxCapacity = InitialMaxCapacity,
                MaxCapacity = MaxCapacity,
                FormModel = FormModel.Clone() as Gs2.Gs2Formation.Model.FormModel,
            };
        }
    }
}