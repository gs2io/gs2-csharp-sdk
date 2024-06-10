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

namespace Gs2.Gs2Grade.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class GradeEntryModel : IComparable
	{
        public string Metadata { set; get; } = null!;
        public long? RankCapValue { set; get; } = null!;
        public string PropertyIdRegex { set; get; } = null!;
        public string GradeUpPropertyIdRegex { set; get; } = null!;
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
                .WithRankCapValue(!data.Keys.Contains("rankCapValue") || data["rankCapValue"] == null ? null : (long?)(data["rankCapValue"].ToString().Contains(".") ? (long)double.Parse(data["rankCapValue"].ToString()) : long.Parse(data["rankCapValue"].ToString())))
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
            var diff = 0;
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (RankCapValue == null && RankCapValue == other.RankCapValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(RankCapValue - other.RankCapValue);
            }
            if (PropertyIdRegex == null && PropertyIdRegex == other.PropertyIdRegex)
            {
                // null and null
            }
            else
            {
                diff += PropertyIdRegex.CompareTo(other.PropertyIdRegex);
            }
            if (GradeUpPropertyIdRegex == null && GradeUpPropertyIdRegex == other.GradeUpPropertyIdRegex)
            {
                // null and null
            }
            else
            {
                diff += GradeUpPropertyIdRegex.CompareTo(other.GradeUpPropertyIdRegex);
            }
            return diff;
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