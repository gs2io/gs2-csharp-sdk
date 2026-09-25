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
	public partial class Position : IComparable
	{
        public float? X { set; get; }
        public float? Y { set; get; }
        public float? Z { set; get; }
        public Position WithX(float? x) {
            this.X = x;
            return this;
        }
        public Position WithY(float? y) {
            this.Y = y;
            return this;
        }
        public Position WithZ(float? z) {
            this.Z = z;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Position FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Position()
                .WithX(!data.Keys.Contains("x") || data["x"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableFloat(data["x"].ToString()))
                .WithY(!data.Keys.Contains("y") || data["y"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableFloat(data["y"].ToString()))
                .WithZ(!data.Keys.Contains("z") || data["z"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableFloat(data["z"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["x"] = X,
                ["y"] = Y,
                ["z"] = Z,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (X != null) {
                writer.WritePropertyName("x");
                writer.Write(float.Parse(X.ToString()));
            }
            if (Y != null) {
                writer.WritePropertyName("y");
                writer.Write(float.Parse(Y.ToString()));
            }
            if (Z != null) {
                writer.WritePropertyName("z");
                writer.Write(float.Parse(Z.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Position;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Position.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(X, other.X);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Y, other.Y);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Z, other.Z);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (X < -1048574) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("position", "megaField.position.x.error.invalid"),
                    });
                }
                if (X > 1048574) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("position", "megaField.position.x.error.invalid"),
                    });
                }
            }
            {
                if (Y < -1048574) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("position", "megaField.position.y.error.invalid"),
                    });
                }
                if (Y > 1048574) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("position", "megaField.position.y.error.invalid"),
                    });
                }
            }
            {
                if (Z < -1048574) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("position", "megaField.position.z.error.invalid"),
                    });
                }
                if (Z > 1048574) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("position", "megaField.position.z.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Position {
                X = X,
                Y = Y,
                Z = Z,
            };
        }
    }
}