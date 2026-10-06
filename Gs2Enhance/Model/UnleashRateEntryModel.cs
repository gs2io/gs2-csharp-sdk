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
	public partial class UnleashRateEntryModel : IComparable
	{
        public long? GradeValue { set; get; }
        public string Type { set; get; }
        public int? NeedCount { set; get; }
        public Gs2.Gs2Enhance.Model.UnleashRecipe[] Recipes { set; get; }
        public UnleashRateEntryModel WithGradeValue(long? gradeValue) {
            this.GradeValue = gradeValue;
            return this;
        }
        public UnleashRateEntryModel WithType(string type) {
            this.Type = type;
            return this;
        }
        public UnleashRateEntryModel WithNeedCount(int? needCount) {
            this.NeedCount = needCount;
            return this;
        }
        public UnleashRateEntryModel WithRecipes(Gs2.Gs2Enhance.Model.UnleashRecipe[] recipes) {
            this.Recipes = recipes;
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
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new UnleashRateEntryModel()
                .WithGradeValue(!data.Keys.Contains("gradeValue") || data["gradeValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["gradeValue"].ToString()))
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithNeedCount(!data.Keys.Contains("needCount") || data["needCount"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["needCount"].ToString()))
                .WithRecipes(!data.Keys.Contains("recipes") || data["recipes"] == null || !data["recipes"].IsArray ? null : data["recipes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.UnleashRecipe.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData recipesJsonData = null;
            if (Recipes != null && Recipes.Length > 0)
            {
                recipesJsonData = new JsonData();
                foreach (var recipe in Recipes)
                {
                    recipesJsonData.Add(recipe.ToJson());
                }
            }
            return new JsonData {
                ["gradeValue"] = GradeValue,
                ["type"] = Type,
                ["needCount"] = NeedCount,
                ["recipes"] = recipesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GradeValue != null) {
                writer.WritePropertyName("gradeValue");
                writer.Write((GradeValue.ToString().Contains(".") ? (long)double.Parse(GradeValue.ToString()) : long.Parse(GradeValue.ToString())));
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (NeedCount != null) {
                writer.WritePropertyName("needCount");
                writer.Write((NeedCount.ToString().Contains(".") ? (int)double.Parse(NeedCount.ToString()) : int.Parse(NeedCount.ToString())));
            }
            if (Recipes != null) {
                writer.WritePropertyName("recipes");
                writer.WriteArrayStart();
                foreach (var recipe in Recipes)
                {
                    if (recipe != null) {
                        recipe.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashRateEntryModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UnleashRateEntryModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(GradeValue, other.GradeValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Type, other.Type);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(NeedCount, other.NeedCount);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Recipes, other.Recipes);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
                switch (Type) {
                    case "simple":
                    case "recipe":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("unleashRateEntryModel", "enhance.unleashRateEntryModel.type.error.invalid"),
                        });
                }
            }
            if (Type == "simple") {
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
            if (Type == "recipe") {
                if (Recipes.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateEntryModel", "enhance.unleashRateEntryModel.recipes.error.tooFew"),
                    });
                }
                if (Recipes.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateEntryModel", "enhance.unleashRateEntryModel.recipes.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnleashRateEntryModel {
                GradeValue = GradeValue,
                Type = Type,
                NeedCount = NeedCount,
                Recipes = Recipes?.Clone() as Gs2.Gs2Enhance.Model.UnleashRecipe[],
            };
        }
    }
}