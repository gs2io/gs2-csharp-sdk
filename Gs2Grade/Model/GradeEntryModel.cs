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

namespace Gs2.Gs2Grade.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class GradeEntryModel : IComparable
	{
        public string Metadata { set; get; }
        public long? RankCapValue { set; get; }
        public string PropertyIdRegex { set; get; }
        public string GradeUpPropertyIdRegex { set; get; }
        public GradeEntryModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public GradeEntryModel WithRankCapValue(long? rankCapValue) {
            this.RankCapValue = rankCapValue;
            return this;
        }
        public GradeEntryModel WithPropertyIdRegex(string propertyIdRegex) {
            this.PropertyIdRegex = propertyIdRegex;
            return this;
        }
        public GradeEntryModel WithGradeUpPropertyIdRegex(string gradeUpPropertyIdRegex) {
            this.GradeUpPropertyIdRegex = gradeUpPropertyIdRegex;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GradeEntryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GradeEntryModel()
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithRankCapValue(!data.Keys.Contains("rankCapValue") || data["rankCapValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["rankCapValue"].ToString()))
                .WithPropertyIdRegex(!data.Keys.Contains("propertyIdRegex") || data["propertyIdRegex"] == null ? null : data["propertyIdRegex"].ToString())
                .WithGradeUpPropertyIdRegex(!data.Keys.Contains("gradeUpPropertyIdRegex") || data["gradeUpPropertyIdRegex"] == null ? null : data["gradeUpPropertyIdRegex"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["metadata"] = Metadata,
                ["rankCapValue"] = RankCapValue,
                ["propertyIdRegex"] = PropertyIdRegex,
                ["gradeUpPropertyIdRegex"] = GradeUpPropertyIdRegex,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (RankCapValue != null) {
                writer.WritePropertyName("rankCapValue");
                writer.Write((RankCapValue.ToString().Contains(".") ? (long)double.Parse(RankCapValue.ToString()) : long.Parse(RankCapValue.ToString())));
            }
            if (PropertyIdRegex != null) {
                writer.WritePropertyName("propertyIdRegex");
                writer.Write(PropertyIdRegex.ToString());
            }
            if (GradeUpPropertyIdRegex != null) {
                writer.WritePropertyName("gradeUpPropertyIdRegex");
                writer.Write(GradeUpPropertyIdRegex.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GradeEntryModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type GradeEntryModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RankCapValue, other.RankCapValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(PropertyIdRegex, other.PropertyIdRegex);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(GradeUpPropertyIdRegex, other.GradeUpPropertyIdRegex);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeEntryModel", "grade.gradeEntryModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (RankCapValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeEntryModel", "grade.gradeEntryModel.rankCapValue.error.invalid"),
                    });
                }
                if (RankCapValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeEntryModel", "grade.gradeEntryModel.rankCapValue.error.invalid"),
                    });
                }
            }
            {
                if (PropertyIdRegex.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeEntryModel", "grade.gradeEntryModel.propertyIdRegex.error.tooLong"),
                    });
                }
            }
            {
                if (GradeUpPropertyIdRegex.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeEntryModel", "grade.gradeEntryModel.gradeUpPropertyIdRegex.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GradeEntryModel {
                Metadata = Metadata,
                RankCapValue = RankCapValue,
                PropertyIdRegex = PropertyIdRegex,
                GradeUpPropertyIdRegex = GradeUpPropertyIdRegex,
            };
        }
    }
}