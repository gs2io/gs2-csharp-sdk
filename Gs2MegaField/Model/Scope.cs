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
        public double? R { set; get; }
        public int? Limit { set; get; }
        public Scope WithR(double? r) {
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
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : (double?)double.Parse(data["r"].ToString()))
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)int.Parse(data["limit"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["r"] = R,
                ["limit"] = Limit,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (R != null) {
                writer.WritePropertyName("r");
                writer.Write(double.Parse(R.ToString()));
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write(int.Parse(Limit.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Scope;
            var diff = 0;
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
    }
}