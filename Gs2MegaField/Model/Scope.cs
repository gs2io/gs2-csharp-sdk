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

namespace Gs2.Gs2MegaField.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Scope : IComparable
	{
        public string LayerName { set; get; }
        public float? R { set; get; }
        public int? Limit { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Scope()
                .WithLayerName(!data.Keys.Contains("layerName") || data["layerName"] == null ? null : data["layerName"].ToString())
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableFloat(data["r"].ToString()))
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["limit"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Scope.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(LayerName, other.LayerName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(R, other.R);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Limit, other.Limit);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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