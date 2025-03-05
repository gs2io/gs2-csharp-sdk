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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class GooglePlaySetting : IComparable
	{
        public string PackageName { set; get; } = null!;
        public string PublicKey { set; get; } = null!;
        public GooglePlaySetting WithPackageName(string packageName) {
            this.PackageName = packageName;
            return this;
        }
        public GooglePlaySetting WithPublicKey(string publicKey) {
            this.PublicKey = publicKey;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GooglePlaySetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GooglePlaySetting()
                .WithPackageName(!data.Keys.Contains("packageName") || data["packageName"] == null ? null : data["packageName"].ToString())
                .WithPublicKey(!data.Keys.Contains("publicKey") || data["publicKey"] == null ? null : data["publicKey"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["packageName"] = PackageName,
                ["publicKey"] = PublicKey,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PackageName != null) {
                writer.WritePropertyName("packageName");
                writer.Write(PackageName.ToString());
            }
            if (PublicKey != null) {
                writer.WritePropertyName("publicKey");
                writer.Write(PublicKey.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GooglePlaySetting;
            var diff = 0;
            if (PackageName == null && PackageName == other.PackageName)
            {
                // null and null
            }
            else
            {
                diff += PackageName.CompareTo(other.PackageName);
            }
            if (PublicKey == null && PublicKey == other.PublicKey)
            {
                // null and null
            }
            else
            {
                diff += PublicKey.CompareTo(other.PublicKey);
            }
            return diff;
        }

        public void Validate() {
            {
                if (PackageName.Length > 5120) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("googlePlaySetting", "money2.googlePlaySetting.packageName.error.tooLong"),
                    });
                }
            }
            {
                if (PublicKey.Length > 5120) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("googlePlaySetting", "money2.googlePlaySetting.publicKey.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GooglePlaySetting {
                PackageName = PackageName,
                PublicKey = PublicKey,
            };
        }
    }
}