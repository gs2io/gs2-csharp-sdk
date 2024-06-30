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
	public class SignTargetVersion : IComparable
	{
        public string Region { set; get; } = null!;
        public string NamespaceName { set; get; } = null!;
        public string VersionName { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ Version { set; get; } = null!;
        public SignTargetVersion WithRegion(string region) {
            this.Region = region;
            return this;
        }
        public SignTargetVersion WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SignTargetVersion WithVersionName(string versionName) {
            this.VersionName = versionName;
            return this;
        }
        public SignTargetVersion WithVersion(Gs2.Gs2Version.Model.Version_ version) {
            this.Version = version;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SignTargetVersion FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SignTargetVersion()
                .WithRegion(!data.Keys.Contains("region") || data["region"] == null ? null : data["region"].ToString())
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithVersionName(!data.Keys.Contains("versionName") || data["versionName"] == null ? null : data["versionName"].ToString())
                .WithVersion(!data.Keys.Contains("version") || data["version"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["version"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["region"] = Region,
                ["namespaceName"] = NamespaceName,
                ["versionName"] = VersionName,
                ["version"] = Version?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Region != null) {
                writer.WritePropertyName("region");
                writer.Write(Region.ToString());
            }
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (VersionName != null) {
                writer.WritePropertyName("versionName");
                writer.Write(VersionName.ToString());
            }
            if (Version != null) {
                writer.WritePropertyName("version");
                Version.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SignTargetVersion;
            var diff = 0;
            if (Region == null && Region == other.Region)
            {
                // null and null
            }
            else
            {
                diff += Region.CompareTo(other.Region);
            }
            if (NamespaceName == null && NamespaceName == other.NamespaceName)
            {
                // null and null
            }
            else
            {
                diff += NamespaceName.CompareTo(other.NamespaceName);
            }
            if (VersionName == null && VersionName == other.VersionName)
            {
                // null and null
            }
            else
            {
                diff += VersionName.CompareTo(other.VersionName);
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
                if (Region.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("signTargetVersion", "version.signTargetVersion.region.error.tooLong"),
                    });
                }
            }
            {
                if (NamespaceName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("signTargetVersion", "version.signTargetVersion.namespaceName.error.tooLong"),
                    });
                }
            }
            {
                if (VersionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("signTargetVersion", "version.signTargetVersion.versionName.error.tooLong"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new SignTargetVersion {
                Region = Region,
                NamespaceName = NamespaceName,
                VersionName = VersionName,
                Version = Version.Clone() as Gs2.Gs2Version.Model.Version_,
            };
        }
    }
}