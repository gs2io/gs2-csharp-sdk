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
using Gs2.Gs2MegaField.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2MegaField.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ActionRequest : Gs2Request<ActionRequest>
	{
        public string NamespaceName { set; get; }
        public string AccessToken { set; get; }
        public string AreaModelName { set; get; }
        public string LayerModelName { set; get; }
        public Gs2.Gs2MegaField.Model.MyPosition Position { set; get; }
        public Gs2.Gs2MegaField.Model.Scope Scope { set; get; }
        public ActionRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ActionRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public ActionRequest WithAreaModelName(string areaModelName) {
            this.AreaModelName = areaModelName;
            return this;
        }
        public ActionRequest WithLayerModelName(string layerModelName) {
            this.LayerModelName = layerModelName;
            return this;
        }
        public ActionRequest WithPosition(Gs2.Gs2MegaField.Model.MyPosition position) {
            this.Position = position;
            return this;
        }
        public ActionRequest WithScope(Gs2.Gs2MegaField.Model.Scope scope) {
            this.Scope = scope;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ActionRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ActionRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithAreaModelName(!data.Keys.Contains("areaModelName") || data["areaModelName"] == null ? null : data["areaModelName"].ToString())
                .WithLayerModelName(!data.Keys.Contains("layerModelName") || data["layerModelName"] == null ? null : data["layerModelName"].ToString())
                .WithPosition(!data.Keys.Contains("position") || data["position"] == null ? null : Gs2.Gs2MegaField.Model.MyPosition.FromJson(data["position"]))
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : Gs2.Gs2MegaField.Model.Scope.FromJson(data["scope"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["areaModelName"] = AreaModelName,
                ["layerModelName"] = LayerModelName,
                ["position"] = Position?.ToJson(),
                ["scope"] = Scope?.ToJson(),
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
            if (AreaModelName != null) {
                writer.WritePropertyName("areaModelName");
                writer.Write(AreaModelName.ToString());
            }
            if (LayerModelName != null) {
                writer.WritePropertyName("layerModelName");
                writer.Write(LayerModelName.ToString());
            }
            if (Position != null) {
                Position.WriteJson(writer);
            }
            if (Scope != null) {
                Scope.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}