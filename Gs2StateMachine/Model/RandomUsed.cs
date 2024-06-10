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

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RandomUsed : IComparable
	{
        public long? Category { set; get; } = null!;
        public long? Used { set; get; } = null!;
        public RandomUsed WithCategory(long? category) {
            this.Category = category;
            return this;
        }
        public RandomUsed WithUsed(long? used) {
            this.Used = used;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RandomUsed FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RandomUsed()
                .WithCategory(!data.Keys.Contains("category") || data["category"] == null ? null : (long?)(data["category"].ToString().Contains(".") ? (long)double.Parse(data["category"].ToString()) : long.Parse(data["category"].ToString())))
                .WithUsed(!data.Keys.Contains("used") || data["used"] == null ? null : (long?)(data["used"].ToString().Contains(".") ? (long)double.Parse(data["used"].ToString()) : long.Parse(data["used"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["category"] = Category,
                ["used"] = Used,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Category != null) {
                writer.WritePropertyName("category");
                writer.Write((Category.ToString().Contains(".") ? (long)double.Parse(Category.ToString()) : long.Parse(Category.ToString())));
            }
            if (Used != null) {
                writer.WritePropertyName("used");
                writer.Write((Used.ToString().Contains(".") ? (long)double.Parse(Used.ToString()) : long.Parse(Used.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RandomUsed;
            var diff = 0;
            if (Category == null && Category == other.Category)
            {
                // null and null
            }
            else
            {
                diff += (int)(Category - other.Category);
            }
            if (Used == null && Used == other.Used)
            {
                // null and null
            }
            else
            {
                diff += (int)(Used - other.Used);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Category < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomUsed", "stateMachine.randomUsed.category.error.invalid"),
                    });
                }
                if (Category > 4294967294) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomUsed", "stateMachine.randomUsed.category.error.invalid"),
                    });
                }
            }
            {
                if (Used < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomUsed", "stateMachine.randomUsed.used.error.invalid"),
                    });
                }
                if (Used > 4294967294) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomUsed", "stateMachine.randomUsed.used.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RandomUsed {
                Category = Category,
                Used = Used,
            };
        }
    }
}