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

namespace Gs2.Gs2MegaField.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Scope : IComparable
	{
        public string LayerName { set; get; } = null!;
        public float? R { set; get; } = null!;
        public int? Limit { set; get; } = null!;
        public Scope WithLayerName(string layerName) {
            this.LayerName = layerName;
            return this;
        }
        public Scope WithR(float? r) {
            this.R = r;
            return this;
        }
        public Scope WithLimit(int? limit) {
            this.Limit = limit;
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
                .WithLayerName(!data.Keys.Contains("layerName") || data["layerName"] == null ? null : data["layerName"].ToString())
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : (float?)float.Parse(data["r"].ToString()))
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["layerName"] = LayerName,
                ["r"] = R,
                ["limit"] = Limit,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (LayerName != null) {
                writer.WritePropertyName("layerName");
                writer.Write(LayerName.ToString());
            }
            if (R != null) {
                writer.WritePropertyName("r");
                writer.Write(float.Parse(R.ToString()));
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Scope;
            var diff = 0;
            if (LayerName == null && LayerName == other.LayerName)
            {
                // null and null
            }
            else
            {
                diff += LayerName.CompareTo(other.LayerName);
            }
            if (R == null && R == other.R)
            {
                // null and null
            }
            else
            {
                diff += (int)(R - other.R);
            }
            if (Limit == null && Limit == other.Limit)
            {
                // null and null
            }
            else
            {
                diff += (int)(Limit - other.Limit);
            }
            return diff;
        }

        public void Validate() {
            {
                if (LayerName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "megaField.scope.layerName.error.tooLong"),
                    });
                }
            }
            {
                if (R < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "megaField.scope.r.error.invalid"),
                    });
                }
                if (R > 16777214) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "megaField.scope.r.error.invalid"),
                    });
                }
            }
            {
                if (Limit < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "megaField.scope.limit.error.invalid"),
                    });
                }
                if (Limit > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scope", "megaField.scope.limit.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Scope {
                LayerName = LayerName,
                R = R,
                Limit = Limit,
            };
        }
    }
}