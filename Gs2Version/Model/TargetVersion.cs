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
	public class TargetVersion : IComparable
	{
        public string VersionName { set; get; } = null!;
        public string Body { set; get; } = null!;
        public string Signature { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ Version { set; get; } = null!;
        public TargetVersion WithVersionName(string versionName) {
            this.VersionName = versionName;
            return this;
        }
        public TargetVersion WithBody(string body) {
            this.Body = body;
            return this;
        }
        public TargetVersion WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }
        public TargetVersion WithVersion(Gs2.Gs2Version.Model.Version_ version) {
            this.Version = version;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TargetVersion FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TargetVersion()
                .WithVersionName(!data.Keys.Contains("versionName") || data["versionName"] == null ? null : data["versionName"].ToString())
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString())
                .WithVersion(!data.Keys.Contains("version") || data["version"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["version"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["versionName"] = VersionName,
                ["body"] = Body,
                ["signature"] = Signature,
                ["version"] = Version?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (VersionName != null) {
                writer.WritePropertyName("versionName");
                writer.Write(VersionName.ToString());
            }
            if (Body != null) {
                writer.WritePropertyName("body");
                writer.Write(Body.ToString());
            }
            if (Signature != null) {
                writer.WritePropertyName("signature");
                writer.Write(Signature.ToString());
            }
            if (Version != null) {
                writer.WritePropertyName("version");
                Version.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TargetVersion;
            var diff = 0;
            if (VersionName == null && VersionName == other.VersionName)
            {
                // null and null
            }
            else
            {
                diff += VersionName.CompareTo(other.VersionName);
            }
            if (Body == null && Body == other.Body)
            {
                // null and null
            }
            else
            {
                diff += Body.CompareTo(other.Body);
            }
            if (Signature == null && Signature == other.Signature)
            {
                // null and null
            }
            else
            {
                diff += Signature.CompareTo(other.Signature);
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
                if (VersionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("targetVersion", "version.targetVersion.versionName.error.tooLong"),
                    });
                }
            }
            {
                if (Body.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("targetVersion", "version.targetVersion.body.error.tooLong"),
                    });
                }
            }
            {
                if (Signature.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("targetVersion", "version.targetVersion.signature.error.tooLong"),
                    });
                }
            }
            if (Signature == "") {
            }
        }

        public object Clone() {
            return new TargetVersion {
                VersionName = VersionName,
                Body = Body,
                Signature = Signature,
                Version = Version.Clone() as Gs2.Gs2Version.Model.Version_,
            };
        }
    }
}