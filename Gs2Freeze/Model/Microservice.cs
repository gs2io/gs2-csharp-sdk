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

namespace Gs2.Gs2Freeze.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Microservice : IComparable
	{
        public string Name { set; get; }
        public string Version { set; get; }
        public Microservice WithName(string name) {
            this.Name = name;
            return this;
        }
        public Microservice WithVersion(string version) {
            this.Version = version;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Microservice FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Microservice()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithVersion(!data.Keys.Contains("version") || data["version"] == null ? null : data["version"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["version"] = Version,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Version != null) {
                writer.WritePropertyName("version");
                writer.Write(Version.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Microservice;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Version == null && Version == other.Version)
            {
                // null and null
            }
            else
            {
                diff += Version.CompareTo(other.Version);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("microservice", "freeze.microservice.name.error.tooLong"),
                    });
                }
            }
            {
                if (Version.Length > 32) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("microservice", "freeze.microservice.version.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new Microservice {
                Name = Name,
                Version = Version,
            };
        }
    }
}