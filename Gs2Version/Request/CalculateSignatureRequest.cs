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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Version.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Version.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CalculateSignatureRequest : Gs2Request<CalculateSignatureRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string VersionName { set; get; } = null!;
         public Gs2.Gs2Version.Model.Version_ Version { set; get; } = null!;
        public CalculateSignatureRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CalculateSignatureRequest WithVersionName(string versionName) {
            this.VersionName = versionName;
            return this;
        }
        public CalculateSignatureRequest WithVersion(Gs2.Gs2Version.Model.Version_ version) {
            this.Version = version;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CalculateSignatureRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CalculateSignatureRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithVersionName(!data.Keys.Contains("versionName") || data["versionName"] == null ? null : data["versionName"].ToString())
                .WithVersion(!data.Keys.Contains("version") || data["version"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["version"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["versionName"] = VersionName,
                ["version"] = Version?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (VersionName != null) {
                writer.WritePropertyName("versionName");
                writer.Write(VersionName.ToString());
            }
            if (Version != null) {
                Version.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += VersionName + ":";
            key += Version + ":";
            return key;
        }
    }
}