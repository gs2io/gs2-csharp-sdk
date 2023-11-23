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
	public class Position : IComparable
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
            return new Position()
                .WithX(!data.Keys.Contains("x") || data["x"] == null ? null : (float?)float.Parse(data["x"].ToString()))
                .WithY(!data.Keys.Contains("y") || data["y"] == null ? null : (float?)float.Parse(data["y"].ToString()))
                .WithZ(!data.Keys.Contains("z") || data["z"] == null ? null : (float?)float.Parse(data["z"].ToString()));
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
            var diff = 0;
            if (X == null && X == other.X)
            {
                // null and null
            }
            else
            {
                diff += (int)(X - other.X);
            }
            if (Y == null && Y == other.Y)
            {
                // null and null
            }
            else
            {
                diff += (int)(Y - other.Y);
            }
            if (Z == null && Z == other.Z)
            {
                // null and null
            }
            else
            {
                diff += (int)(Z - other.Z);
            }
            return diff;
        }
    }
}