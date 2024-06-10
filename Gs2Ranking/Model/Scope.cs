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
	public class Scope : IComparable
	{
        public string Name { set; get; } = null!;
        public long? TargetDays { set; get; } = null!;
        public Scope WithName(string name) {
            this.Name = name;
            return this;
        }
        public Scope WithTargetDays(long? targetDays) {
            this.TargetDays = targetDays;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Scope FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Scope()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithTargetDays(!data.Keys.Contains("targetDays") || data["targetDays"] == null ? null : (long?)(data["targetDays"].ToString().Contains(".") ? (long)double.Parse(data["targetDays"].ToString()) : long.Parse(data["targetDays"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["targetDays"] = TargetDays,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (TargetDays != null) {
                writer.WritePropertyName("targetDays");
                writer.Write((TargetDays.ToString().Contains(".") ? (long)double.Parse(TargetDays.ToString()) : long.Parse(TargetDays.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Scope;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (TargetDays == null && TargetDays == other.TargetDays)
            {
                // null and null
            }
            else
            {
                diff += (int)(TargetDays - other.TargetDays);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "ranking.scope.name.error.tooLong"),
                    });
                }
            }
            {
                if (TargetDays < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "ranking.scope.targetDays.error.invalid"),
                    });
                }
                if (TargetDays > 365) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "ranking.scope.targetDays.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Scope {
                Name = Name,
                TargetDays = TargetDays,
            };
        }
    }
}