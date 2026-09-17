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

namespace Gs2.Gs2Stamina.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class RecoverValueTable : IComparable
	{
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string ExperienceModelId { set; get; }
        public int[] Values { set; get; }
        public RecoverValueTable WithName(string name) {
            this.Name = name;
            return this;
        }
        public RecoverValueTable WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RecoverValueTable WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public RecoverValueTable WithValues(int[] values) {
            this.Values = values;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RecoverValueTable FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new RecoverValueTable()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithValues(!data.Keys.Contains("values") || data["values"] == null || !data["values"].IsArray ? null : data["values"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData valuesJsonData = null;
            if (Values != null && Values.Length > 0)
            {
                valuesJsonData = new JsonData();
                foreach (var value in Values)
                {
                    valuesJsonData.Add(value);
                }
            }
            return new JsonData {
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["experienceModelId"] = ExperienceModelId,
                ["values"] = valuesJsonData,
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
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (Values != null) {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach (var value in Values)
                {
                    writer.Write((value.ToString().Contains(".") ? (int)double.Parse(value.ToString()) : int.Parse(value.ToString())));
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RecoverValueTable;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type RecoverValueTable.", nameof(obj));
            }
            var diff = 0;
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
            diff = ModelComparer.Compare(ExperienceModelId, other.ExperienceModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Values, other.Values);
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
                        new RequestError("recoverValueTable", "stamina.recoverValueTable.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("recoverValueTable", "stamina.recoverValueTable.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (ExperienceModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("recoverValueTable", "stamina.recoverValueTable.experienceModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Values.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("recoverValueTable", "stamina.recoverValueTable.values.error.tooFew"),
                    });
                }
                if (Values.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("recoverValueTable", "stamina.recoverValueTable.values.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new RecoverValueTable {
                Name = Name,
                Metadata = Metadata,
                ExperienceModelId = ExperienceModelId,
                Values = Values?.Clone() as int[],
            };
        }
    }
}