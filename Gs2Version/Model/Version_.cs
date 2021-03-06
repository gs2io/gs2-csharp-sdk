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

namespace Gs2.Gs2Version.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Version_ : IComparable
	{
        public int? Major { set; get; }
        public int? Minor { set; get; }
        public int? Micro { set; get; }
        public Version_ WithMajor(int? major) {
            this.Major = major;
            return this;
        }
        public Version_ WithMinor(int? minor) {
            this.Minor = minor;
            return this;
        }
        public Version_ WithMicro(int? micro) {
            this.Micro = micro;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Version_ FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Version_()
                .WithMajor(!data.Keys.Contains("major") || data["major"] == null ? null : (int?)int.Parse(data["major"].ToString()))
                .WithMinor(!data.Keys.Contains("minor") || data["minor"] == null ? null : (int?)int.Parse(data["minor"].ToString()))
                .WithMicro(!data.Keys.Contains("micro") || data["micro"] == null ? null : (int?)int.Parse(data["micro"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["major"] = Major,
                ["minor"] = Minor,
                ["micro"] = Micro,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Major != null) {
                writer.WritePropertyName("major");
                writer.Write(int.Parse(Major.ToString()));
            }
            if (Minor != null) {
                writer.WritePropertyName("minor");
                writer.Write(int.Parse(Minor.ToString()));
            }
            if (Micro != null) {
                writer.WritePropertyName("micro");
                writer.Write(int.Parse(Micro.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Version_;
            var diff = 0;
            if (Major == null && Major == other.Major)
            {
                // null and null
            }
            else
            {
                diff += (int)(Major - other.Major);
            }
            if (Minor == null && Minor == other.Minor)
            {
                // null and null
            }
            else
            {
                diff += (int)(Minor - other.Minor);
            }
            if (Micro == null && Micro == other.Micro)
            {
                // null and null
            }
            else
            {
                diff += (int)(Micro - other.Micro);
            }
            return diff;
        }
    }
}