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
using Gs2.Gs2Account.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Account.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateTakeOverTypeModelMasterRequest : Gs2Request<CreateTakeOverTypeModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public int? Type { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public Gs2.Gs2Account.Model.OpenIdConnectSetting OpenIdConnectSetting { set; get; } = null!;
        public CreateTakeOverTypeModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateTakeOverTypeModelMasterRequest WithType(int? type) {
            this.Type = type;
            return this;
        }
        public CreateTakeOverTypeModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateTakeOverTypeModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateTakeOverTypeModelMasterRequest WithOpenIdConnectSetting(Gs2.Gs2Account.Model.OpenIdConnectSetting openIdConnectSetting) {
            this.OpenIdConnectSetting = openIdConnectSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateTakeOverTypeModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateTakeOverTypeModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : (int?)(data["type"].ToString().Contains(".") ? (int)double.Parse(data["type"].ToString()) : int.Parse(data["type"].ToString())))
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithOpenIdConnectSetting(!data.Keys.Contains("openIdConnectSetting") || data["openIdConnectSetting"] == null ? null : Gs2.Gs2Account.Model.OpenIdConnectSetting.FromJson(data["openIdConnectSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["type"] = Type,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["openIdConnectSetting"] = OpenIdConnectSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write((Type.ToString().Contains(".") ? (int)double.Parse(Type.ToString()) : int.Parse(Type.ToString())));
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (OpenIdConnectSetting != null) {
                OpenIdConnectSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Type + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += OpenIdConnectSetting + ":";
            return key;
        }
    }
}