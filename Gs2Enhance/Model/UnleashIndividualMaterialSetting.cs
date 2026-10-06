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
	public partial class UnleashIndividualMaterialSetting : IComparable
	{
        public string MatchType { set; get; }
        public string GradeCondition { set; get; }
        public long? GradeValue { set; get; }
        public int? Count { set; get; }
        public UnleashIndividualMaterialSetting WithMatchType(string matchType) {
            this.MatchType = matchType;
            return this;
        }
        public UnleashIndividualMaterialSetting WithGradeCondition(string gradeCondition) {
            this.GradeCondition = gradeCondition;
            return this;
        }
        public UnleashIndividualMaterialSetting WithGradeValue(long? gradeValue) {
            this.GradeValue = gradeValue;
            return this;
        }
        public UnleashIndividualMaterialSetting WithCount(int? count) {
            this.Count = count;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnleashIndividualMaterialSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new UnleashIndividualMaterialSetting()
                .WithMatchType(!data.Keys.Contains("matchType") || data["matchType"] == null ? null : data["matchType"].ToString())
                .WithGradeCondition(!data.Keys.Contains("gradeCondition") || data["gradeCondition"] == null ? null : data["gradeCondition"].ToString())
                .WithGradeValue(!data.Keys.Contains("gradeValue") || data["gradeValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["gradeValue"].ToString()))
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["count"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["matchType"] = MatchType,
                ["gradeCondition"] = GradeCondition,
                ["gradeValue"] = GradeValue,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MatchType != null) {
                writer.WritePropertyName("matchType");
                writer.Write(MatchType.ToString());
            }
            if (GradeCondition != null) {
                writer.WritePropertyName("gradeCondition");
                writer.Write(GradeCondition.ToString());
            }
            if (GradeValue != null) {
                writer.WritePropertyName("gradeValue");
                writer.Write((GradeValue.ToString().Contains(".") ? (long)double.Parse(GradeValue.ToString()) : long.Parse(GradeValue.ToString())));
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashIndividualMaterialSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UnleashIndividualMaterialSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(MatchType, other.MatchType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(GradeCondition, other.GradeCondition);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(GradeValue, other.GradeValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Count, other.Count);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                switch (MatchType) {
                    case "sameItem":
                    case "sameGroup":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("unleashIndividualMaterialSetting", "enhance.unleashIndividualMaterialSetting.matchType.error.invalid"),
                        });
                }
            }
            {
                switch (GradeCondition) {
                    case "any":
                    case "sameAsTarget":
                    case "equal":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("unleashIndividualMaterialSetting", "enhance.unleashIndividualMaterialSetting.gradeCondition.error.invalid"),
                        });
                }
            }
            if (GradeCondition == "equal") {
                if (GradeValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashIndividualMaterialSetting", "enhance.unleashIndividualMaterialSetting.gradeValue.error.invalid"),
                    });
                }
                if (GradeValue > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashIndividualMaterialSetting", "enhance.unleashIndividualMaterialSetting.gradeValue.error.invalid"),
                    });
                }
            }
            {
                if (Count < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashIndividualMaterialSetting", "enhance.unleashIndividualMaterialSetting.count.error.invalid"),
                    });
                }
                if (Count > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashIndividualMaterialSetting", "enhance.unleashIndividualMaterialSetting.count.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnleashIndividualMaterialSetting {
                MatchType = MatchType,
                GradeCondition = GradeCondition,
                GradeValue = GradeValue,
                Count = Count,
            };
        }
    }
}