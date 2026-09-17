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
	public partial class MyPosition : IComparable
	{
        public Gs2.Gs2MegaField.Model.Position Position { set; get; }
        public Gs2.Gs2MegaField.Model.Vector Vector { set; get; }
        public float? R { set; get; }
        public MyPosition WithPosition(Gs2.Gs2MegaField.Model.Position position) {
            this.Position = position;
            return this;
        }
        public MyPosition WithVector(Gs2.Gs2MegaField.Model.Vector vector) {
            this.Vector = vector;
            return this;
        }
        public MyPosition WithR(float? r) {
            this.R = r;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MyPosition FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MyPosition()
                .WithPosition(!data.Keys.Contains("position") || data["position"] == null ? null : Gs2.Gs2MegaField.Model.Position.FromJson(data["position"]))
                .WithVector(!data.Keys.Contains("vector") || data["vector"] == null ? null : Gs2.Gs2MegaField.Model.Vector.FromJson(data["vector"]))
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableFloat(data["r"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["position"] = Position?.ToJson(),
                ["vector"] = Vector?.ToJson(),
                ["r"] = R,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Position != null) {
                writer.WritePropertyName("position");
                Position.WriteJson(writer);
            }
            if (Vector != null) {
                writer.WritePropertyName("vector");
                Vector.WriteJson(writer);
            }
            if (R != null) {
                writer.WritePropertyName("r");
                writer.Write(float.Parse(R.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as MyPosition;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type MyPosition.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Position, other.Position);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Vector, other.Vector);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(R, other.R);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
            }
            {
            }
            {
                if (R < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("myPosition", "megaField.myPosition.r.error.invalid"),
                    });
                }
                if (R > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("myPosition", "megaField.myPosition.r.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new MyPosition {
                Position = Position?.Clone() as Gs2.Gs2MegaField.Model.Position,
                Vector = Vector?.Clone() as Gs2.Gs2MegaField.Model.Vector,
                R = R,
            };
        }
    }
}