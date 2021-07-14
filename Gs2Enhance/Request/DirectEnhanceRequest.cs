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
using UnityEngine.Scripting;

namespace Gs2.Gs2Enhance.Request
{
	[Preserve]
	[System.Serializable]
	public class DirectEnhanceRequest : Gs2Request<DirectEnhanceRequest>
	{
        public string NamespaceName { set; get; }
        public string RateName { set; get; }
        public string AccessToken { set; get; }
        public string TargetItemSetId { set; get; }
        public Gs2.Gs2Enhance.Model.Material[] Materials { set; get; }
        public Gs2.Gs2Enhance.Model.Config[] Config { set; get; }

        public DirectEnhanceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DirectEnhanceRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }

        public DirectEnhanceRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public DirectEnhanceRequest WithTargetItemSetId(string targetItemSetId) {
            this.TargetItemSetId = targetItemSetId;
            return this;
        }

        public DirectEnhanceRequest WithMaterials(Gs2.Gs2Enhance.Model.Material[] materials) {
            this.Materials = materials;
            return this;
        }

        public DirectEnhanceRequest WithConfig(Gs2.Gs2Enhance.Model.Config[] config) {
            this.Config = config;
            return this;
        }

    	[Preserve]
        public static DirectEnhanceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DirectEnhanceRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTargetItemSetId(!data.Keys.Contains("targetItemSetId") || data["targetItemSetId"] == null ? null : data["targetItemSetId"].ToString())
                .WithMaterials(!data.Keys.Contains("materials") || data["materials"] == null ? new Gs2.Gs2Enhance.Model.Material[]{} : data["materials"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.Material.FromJson(v);
                }).ToArray())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2Enhance.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.Config.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["rateName"] = RateName,
                ["accessToken"] = AccessToken,
                ["targetItemSetId"] = TargetItemSetId,
                ["materials"] = new JsonData(Materials == null ? new JsonData[]{} :
                        Materials.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["config"] = new JsonData(Config == null ? new JsonData[]{} :
                        Config.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (TargetItemSetId != null) {
                writer.WritePropertyName("targetItemSetId");
                writer.Write(TargetItemSetId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var material in Materials)
            {
                if (material != null) {
                    material.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteArrayStart();
            foreach (var confi in Config)
            {
                if (confi != null) {
                    confi.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}