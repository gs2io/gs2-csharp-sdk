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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class CalculatedAt : IComparable
	{
        public string CategoryName { set; get; } = null!;
        public long? Value { set; get; } = null!;
        public CalculatedAt WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public CalculatedAt WithValue(long? value) {
            this.Value = value;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CalculatedAt FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CalculatedAt()
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithValue(!data.Keys.Contains("calculatedAt") || data["calculatedAt"] == null ? null : (long?)(data["calculatedAt"].ToString().Contains(".") ? (long)double.Parse(data["calculatedAt"].ToString()) : long.Parse(data["calculatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["categoryName"] = CategoryName,
                ["calculatedAt"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (long)double.Parse(Value.ToString()) : long.Parse(Value.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CalculatedAt;
            var diff = 0;
            if (CategoryName == null && CategoryName == other.CategoryName)
            {
                // null and null
            }
            else
            {
                diff += CategoryName.CompareTo(other.CategoryName);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            return diff;
        }

        public void Validate() {
            {
                if (CategoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("calculatedAt", "ranking.calculatedAt.categoryName.error.tooLong"),
                    });
                }
            }
            {
                if (Value < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("calculatedAt", "ranking.calculatedAt.calculatedAt.error.invalid"),
                    });
                }
                if (Value > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("calculatedAt", "ranking.calculatedAt.calculatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new CalculatedAt {
                CategoryName = CategoryName,
                Value = Value,
            };
        }
    }
}