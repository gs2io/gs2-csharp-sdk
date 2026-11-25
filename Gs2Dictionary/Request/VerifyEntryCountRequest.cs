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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Dictionary.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Dictionary.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VerifyEntryCountRequest : Gs2Request<VerifyEntryCountRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string[] EntryModelNames { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public int? Count { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyEntryCountRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyEntryCountRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyEntryCountRequest WithEntryModelNames(string[] entryModelNames) {
            this.EntryModelNames = entryModelNames;
            return this;
        }
        public VerifyEntryCountRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyEntryCountRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public VerifyEntryCountRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyEntryCountRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyEntryCountRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithEntryModelNames(!data.Keys.Contains("entryModelNames") || data["entryModelNames"] == null || !data["entryModelNames"].IsArray ? null : data["entryModelNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["count"].ToString()));
        }

        public override JsonData ToJson()
        {
            JsonData entryModelNamesJsonData = null;
            if (EntryModelNames != null && EntryModelNames.Length > 0)
            {
                entryModelNamesJsonData = new JsonData();
                foreach (var entryModelName in EntryModelNames)
                {
                    entryModelNamesJsonData.Add(entryModelName);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["entryModelNames"] = entryModelNamesJsonData,
                ["verifyType"] = VerifyType,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (EntryModelNames != null) {
                writer.WritePropertyName("entryModelNames");
                writer.WriteArrayStart();
                foreach (var entryModelName in EntryModelNames)
                {
                    writer.Write(entryModelName.ToString());
                }
                writer.WriteArrayEnd();
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += EntryModelNames + ":";
            key += VerifyType + ":";
            key += Count + ":";
            return key;
        }
    }
}