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

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class NumericRange : IComparable
	{
        public double? Min { set; get; }
        public double? Max { set; get; }
        public NumericRange WithMin(double? min) {
            this.Min = min;
            return this;
        }
        public NumericRange WithMax(double? max) {
            this.Max = max;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static NumericRange FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new NumericRange()
                .WithMin(!data.Keys.Contains("min") || data["min"] == null ? null : (double?)double.Parse(data["min"].ToString()))
                .WithMax(!data.Keys.Contains("max") || data["max"] == null ? null : (double?)double.Parse(data["max"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["min"] = Min,
                ["max"] = Max,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Min != null) {
                writer.WritePropertyName("min");
                writer.Write(double.Parse(Min.ToString()));
            }
            if (Max != null) {
                writer.WritePropertyName("max");
                writer.Write(double.Parse(Max.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as NumericRange;
            var diff = 0;
            if (Min == null && Min == other.Min)
            {
                // null and null
            }
            else
            {
                diff += (int)(Min - other.Min);
            }
            if (Max == null && Max == other.Max)
            {
                // null and null
            }
            else
            {
                diff += (int)(Max - other.Max);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Min < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("numericRange", "log.numericRange.min.error.invalid"),
                    });
                }
                if (Min > 281474976710654) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("numericRange", "log.numericRange.min.error.invalid"),
                    });
                }
            }
            {
                if (Max < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("numericRange", "log.numericRange.max.error.invalid"),
                    });
                }
                if (Max > 281474976710654) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("numericRange", "log.numericRange.max.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new NumericRange {
                Min = Min,
                Max = Max,
            };
        }
    }
}