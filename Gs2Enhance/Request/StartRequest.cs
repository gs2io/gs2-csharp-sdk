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
using Gs2.Gs2Enhance.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Enhance.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class StartRequest : Gs2Request<StartRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RateName { set; get; } = null!;
         public string TargetItemSetId { set; get; } = null!;
         public Gs2.Gs2Enhance.Model.Material[] Materials { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public bool? Force { set; get; } = null!;
         public Gs2.Gs2Enhance.Model.Config[] Config { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public StartRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public StartRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public StartRequest WithTargetItemSetId(string targetItemSetId) {
            this.TargetItemSetId = targetItemSetId;
            return this;
        }
        public StartRequest WithMaterials(Gs2.Gs2Enhance.Model.Material[] materials) {
            this.Materials = materials;
            return this;
        }
        public StartRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public StartRequest WithForce(bool? force) {
            this.Force = force;
            return this;
        }
        public StartRequest WithConfig(Gs2.Gs2Enhance.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public StartRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StartRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StartRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithTargetItemSetId(!data.Keys.Contains("targetItemSetId") || data["targetItemSetId"] == null ? null : data["targetItemSetId"].ToString())
                .WithMaterials(!data.Keys.Contains("materials") || data["materials"] == null || !data["materials"].IsArray ? null : data["materials"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.Material.FromJson(v);
                }).ToArray())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithForce(!data.Keys.Contains("force") || data["force"] == null ? null : (bool?)bool.Parse(data["force"].ToString()))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? null : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.Config.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData materialsJsonData = null;
            if (Materials != null && Materials.Length > 0)
            {
                materialsJsonData = new JsonData();
                foreach (var material in Materials)
                {
                    materialsJsonData.Add(material.ToJson());
                }
            }
            JsonData configJsonData = null;
            if (Config != null && Config.Length > 0)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["rateName"] = RateName,
                ["targetItemSetId"] = TargetItemSetId,
                ["materials"] = materialsJsonData,
                ["accessToken"] = AccessToken,
                ["force"] = Force,
                ["config"] = configJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (TargetItemSetId != null) {
                writer.WritePropertyName("targetItemSetId");
                writer.Write(TargetItemSetId.ToString());
            }
            if (Materials != null) {
                writer.WritePropertyName("materials");
                writer.WriteArrayStart();
                foreach (var material in Materials)
                {
                    if (material != null) {
                        material.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (Force != null) {
                writer.WritePropertyName("force");
                writer.Write(bool.Parse(Force.ToString()));
            }
            if (Config != null) {
                writer.WritePropertyName("config");
                writer.WriteArrayStart();
                foreach (var confi in Config)
                {
                    if (confi != null) {
                        confi.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RateName + ":";
            key += TargetItemSetId + ":";
            key += Materials + ":";
            key += AccessToken + ":";
            key += Force + ":";
            key += Config + ":";
            return key;
        }
    }
}