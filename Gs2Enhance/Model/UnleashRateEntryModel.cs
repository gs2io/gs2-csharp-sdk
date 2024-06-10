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

namespace Gs2.Gs2Enhance.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class UnleashRateEntryModel : IComparable
	{
        public long? GradeValue { set; get; } = null!;
        public int? NeedCount { set; get; } = null!;
        public UnleashRateEntryModel WithGradeValue(long? gradeValue) {
            this.GradeValue = gradeValue;
            return this;
        }
        public UnleashRateEntryModel WithNeedCount(int? needCount) {
            this.NeedCount = needCount;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnleashRateEntryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UnleashRateEntryModel()
                .WithGradeValue(!data.Keys.Contains("gradeValue") || data["gradeValue"] == null ? null : (long?)(data["gradeValue"].ToString().Contains(".") ? (long)double.Parse(data["gradeValue"].ToString()) : long.Parse(data["gradeValue"].ToString())))
                .WithNeedCount(!data.Keys.Contains("needCount") || data["needCount"] == null ? null : (int?)(data["needCount"].ToString().Contains(".") ? (int)double.Parse(data["needCount"].ToString()) : int.Parse(data["needCount"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["gradeValue"] = GradeValue,
                ["needCount"] = NeedCount,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GradeValue != null) {
                writer.WritePropertyName("gradeValue");
                writer.Write((GradeValue.ToString().Contains(".") ? (long)double.Parse(GradeValue.ToString()) : long.Parse(GradeValue.ToString())));
            }
            if (NeedCount != null) {
                writer.WritePropertyName("needCount");
                writer.Write((NeedCount.ToString().Contains(".") ? (int)double.Parse(NeedCount.ToString()) : int.Parse(NeedCount.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashRateEntryModel;
            var diff = 0;
            if (GradeValue == null && GradeValue == other.GradeValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(GradeValue - other.GradeValue);
            }
            if (NeedCount == null && NeedCount == other.NeedCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(NeedCount - other.NeedCount);
            }
            return diff;
        }

        public void Validate() {
            {
                if (GradeValue < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateEntryModel", "enhance.unleashRateEntryModel.gradeValue.error.invalid"),
                    });
                }
                if (GradeValue > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateEntryModel", "enhance.unleashRateEntryModel.gradeValue.error.invalid"),
                    });
                }
            }
            {
                if (NeedCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateEntryModel", "enhance.unleashRateEntryModel.needCount.error.invalid"),
                    });
                }
                if (NeedCount > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateEntryModel", "enhance.unleashRateEntryModel.needCount.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnleashRateEntryModel {
                GradeValue = GradeValue,
                NeedCount = NeedCount,
            };
        }
    }
}