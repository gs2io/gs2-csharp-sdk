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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetFormRequest : Gs2Request<GetFormRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string MoldModelName { set; get; } = null!;
         public int? Index { set; get; } = null!;
        public GetFormRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetFormRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public GetFormRequest WithMoldModelName(string moldModelName) {
            this.MoldModelName = moldModelName;
            return this;
        }
        public GetFormRequest WithIndex(int? index) {
            this.Index = index;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetFormRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetFormRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithMoldModelName(!data.Keys.Contains("moldModelName") || data["moldModelName"] == null ? null : data["moldModelName"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : (int?)(data["index"].ToString().Contains(".") ? (int)double.Parse(data["index"].ToString()) : int.Parse(data["index"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["moldModelName"] = MoldModelName,
                ["index"] = Index,
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
            if (MoldModelName != null) {
                writer.WritePropertyName("moldModelName");
                writer.Write(MoldModelName.ToString());
            }
            if (Index != null) {
                writer.WritePropertyName("index");
                writer.Write((Index.ToString().Contains(".") ? (int)double.Parse(Index.ToString()) : int.Parse(Index.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += MoldModelName + ":";
            key += Index + ":";
            return key;
        }
    }
}