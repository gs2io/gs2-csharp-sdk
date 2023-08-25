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
	public class StartByUserIdRequest : Gs2Request<StartByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string RateName { set; get; }
        public string TargetItemSetId { set; get; }
        public Gs2.Gs2Enhance.Model.Material[] Materials { set; get; }
        public string UserId { set; get; }
        public bool? Force { set; get; }
        public Gs2.Gs2Enhance.Model.Config[] Config { set; get; }
        public string DuplicationAvoider { set; get; }
        public StartByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public StartByUserIdRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public StartByUserIdRequest WithTargetItemSetId(string targetItemSetId) {
            this.TargetItemSetId = targetItemSetId;
            return this;
        }
        public StartByUserIdRequest WithMaterials(Gs2.Gs2Enhance.Model.Material[] materials) {
            this.Materials = materials;
            return this;
        }
        public StartByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public StartByUserIdRequest WithForce(bool? force) {
            this.Force = force;
            return this;
        }
        public StartByUserIdRequest WithConfig(Gs2.Gs2Enhance.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public StartByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StartByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StartByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithTargetItemSetId(!data.Keys.Contains("targetItemSetId") || data["targetItemSetId"] == null ? null : data["targetItemSetId"].ToString())
                .WithMaterials(!data.Keys.Contains("materials") || data["materials"] == null ? new Gs2.Gs2Enhance.Model.Material[]{} : data["materials"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.Material.FromJson(v);
                }).ToArray())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithForce(!data.Keys.Contains("force") || data["force"] == null ? null : (bool?)bool.Parse(data["force"].ToString()))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2Enhance.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.Config.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData materialsJsonData = null;
            if (Materials != null)
            {
                materialsJsonData = new JsonData();
                foreach (var material in Materials)
                {
                    materialsJsonData.Add(material.ToJson());
                }
            }
            JsonData configJsonData = null;
            if (Config != null)
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
                ["userId"] = UserId,
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
            writer.WriteArrayStart();
            foreach (var material in Materials)
            {
                if (material != null) {
                    material.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Force != null) {
                writer.WritePropertyName("force");
                writer.Write(bool.Parse(Force.ToString()));
            }
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

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RateName + ":";
            key += TargetItemSetId + ":";
            key += Materials + ":";
            key += UserId + ":";
            key += Force + ":";
            key += Config + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply StartByUserIdRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (StartByUserIdRequest)x;
            return this;
        }
    }
}