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
	public class DefaultGradeModel : IComparable
	{
        public string PropertyIdRegex { set; get; } = null!;
        public long? DefaultGradeValue { set; get; } = null!;
        public DefaultGradeModel WithPropertyIdRegex(string propertyIdRegex) {
            this.PropertyIdRegex = propertyIdRegex;
            return this;
        }
        public DefaultGradeModel WithDefaultGradeValue(long? defaultGradeValue) {
            this.DefaultGradeValue = defaultGradeValue;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DefaultGradeModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DefaultGradeModel()
                .WithPropertyIdRegex(!data.Keys.Contains("propertyIdRegex") || data["propertyIdRegex"] == null ? null : data["propertyIdRegex"].ToString())
                .WithDefaultGradeValue(!data.Keys.Contains("defaultGradeValue") || data["defaultGradeValue"] == null ? null : (long?)(data["defaultGradeValue"].ToString().Contains(".") ? (long)double.Parse(data["defaultGradeValue"].ToString()) : long.Parse(data["defaultGradeValue"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["propertyIdRegex"] = PropertyIdRegex,
                ["defaultGradeValue"] = DefaultGradeValue,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PropertyIdRegex != null) {
                writer.WritePropertyName("propertyIdRegex");
                writer.Write(PropertyIdRegex.ToString());
            }
            if (DefaultGradeValue != null) {
                writer.WritePropertyName("defaultGradeValue");
                writer.Write((DefaultGradeValue.ToString().Contains(".") ? (long)double.Parse(DefaultGradeValue.ToString()) : long.Parse(DefaultGradeValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DefaultGradeModel;
            var diff = 0;
            if (PropertyIdRegex == null && PropertyIdRegex == other.PropertyIdRegex)
            {
                // null and null
            }
            else
            {
                diff += PropertyIdRegex.CompareTo(other.PropertyIdRegex);
            }
            if (DefaultGradeValue == null && DefaultGradeValue == other.DefaultGradeValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultGradeValue - other.DefaultGradeValue);
            }
            return diff;
        }

        public void Validate() {
            {
                if (PropertyIdRegex.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("defaultGradeModel", "grade.defaultGradeModel.propertyIdRegex.error.tooLong"),
                    });
                }
            }
            {
                if (DefaultGradeValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("defaultGradeModel", "grade.defaultGradeModel.defaultGradeValue.error.invalid"),
                    });
                }
                if (DefaultGradeValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("defaultGradeModel", "grade.defaultGradeModel.defaultGradeValue.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new DefaultGradeModel {
                PropertyIdRegex = PropertyIdRegex,
                DefaultGradeValue = DefaultGradeValue,
            };
        }
    }
}