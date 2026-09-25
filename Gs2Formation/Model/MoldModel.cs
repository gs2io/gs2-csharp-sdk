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

namespace Gs2.Gs2Formation.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class MoldModel : IComparable
	{
        public string MoldModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public int? InitialMaxCapacity { set; get; }
        public int? MaxCapacity { set; get; }
        public Gs2.Gs2Formation.Model.FormModel FormModel { set; get; }
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
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new MoldModel()
                .WithMoldModelId(!data.Keys.Contains("moldModelId") || data["moldModelId"] == null ? null : data["moldModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInitialMaxCapacity(!data.Keys.Contains("initialMaxCapacity") || data["initialMaxCapacity"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["initialMaxCapacity"].ToString()))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["maxCapacity"].ToString()))
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type MoldModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(MoldModelId, other.MoldModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(InitialMaxCapacity, other.InitialMaxCapacity);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaxCapacity, other.MaxCapacity);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(FormModel, other.FormModel);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
                FormModel = FormModel?.Clone() as Gs2.Gs2Formation.Model.FormModel,
            };
        }
    }
}