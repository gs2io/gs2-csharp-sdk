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

namespace Gs2.Gs2Script.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class RandomStatus : IComparable
	{
        public long? Seed { set; get; }
        public Gs2.Gs2Script.Model.RandomUsed[] Used { set; get; }
        public RandomStatus WithSeed(long? seed) {
            this.Seed = seed;
            return this;
        }
        public RandomStatus WithUsed(Gs2.Gs2Script.Model.RandomUsed[] used) {
            this.Used = used;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RandomStatus FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new RandomStatus()
                .WithSeed(!data.Keys.Contains("seed") || data["seed"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["seed"].ToString()))
                .WithUsed(!data.Keys.Contains("used") || data["used"] == null || !data["used"].IsArray ? null : data["used"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Script.Model.RandomUsed.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData usedJsonData = null;
            if (Used != null && Used.Length > 0)
            {
                usedJsonData = new JsonData();
                foreach (var use in Used)
                {
                    usedJsonData.Add(use.ToJson());
                }
            }
            return new JsonData {
                ["seed"] = Seed,
                ["used"] = usedJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Seed != null) {
                writer.WritePropertyName("seed");
                writer.Write((Seed.ToString().Contains(".") ? (long)double.Parse(Seed.ToString()) : long.Parse(Seed.ToString())));
            }
            if (Used != null) {
                writer.WritePropertyName("used");
                writer.WriteArrayStart();
                foreach (var use in Used)
                {
                    if (use != null) {
                        use.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RandomStatus;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type RandomStatus.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Seed, other.Seed);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Used, other.Used);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Seed < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomStatus", "script.randomStatus.seed.error.invalid"),
                    });
                }
                if (Seed > 4294967294) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomStatus", "script.randomStatus.seed.error.invalid"),
                    });
                }
            }
            {
                if (Used.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("randomStatus", "script.randomStatus.used.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new RandomStatus {
                Seed = Seed,
                Used = Used?.Clone() as Gs2.Gs2Script.Model.RandomUsed[],
            };
        }
    }
}