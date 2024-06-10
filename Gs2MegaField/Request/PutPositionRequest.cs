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
	public class PutPositionRequest : Gs2Request<PutPositionRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string AreaModelName { set; get; } = null!;
         public string LayerModelName { set; get; } = null!;
         public Gs2.Gs2MegaField.Model.Position Position { set; get; } = null!;
         public Gs2.Gs2MegaField.Model.Vector Vector { set; get; } = null!;
         public float? R { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public PutPositionRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PutPositionRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public PutPositionRequest WithAreaModelName(string areaModelName) {
            this.AreaModelName = areaModelName;
            return this;
        }
        public PutPositionRequest WithLayerModelName(string layerModelName) {
            this.LayerModelName = layerModelName;
            return this;
        }
        public PutPositionRequest WithPosition(Gs2.Gs2MegaField.Model.Position position) {
            this.Position = position;
            return this;
        }
        public PutPositionRequest WithVector(Gs2.Gs2MegaField.Model.Vector vector) {
            this.Vector = vector;
            return this;
        }
        public PutPositionRequest WithR(float? r) {
            this.R = r;
            return this;
        }

        public PutPositionRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PutPositionRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PutPositionRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithAreaModelName(!data.Keys.Contains("areaModelName") || data["areaModelName"] == null ? null : data["areaModelName"].ToString())
                .WithLayerModelName(!data.Keys.Contains("layerModelName") || data["layerModelName"] == null ? null : data["layerModelName"].ToString())
                .WithPosition(!data.Keys.Contains("position") || data["position"] == null ? null : Gs2.Gs2MegaField.Model.Position.FromJson(data["position"]))
                .WithVector(!data.Keys.Contains("vector") || data["vector"] == null ? null : Gs2.Gs2MegaField.Model.Vector.FromJson(data["vector"]))
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : (float?)float.Parse(data["r"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["areaModelName"] = AreaModelName,
                ["layerModelName"] = LayerModelName,
                ["position"] = Position?.ToJson(),
                ["vector"] = Vector?.ToJson(),
                ["r"] = R,
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
            if (Vector != null) {
                Vector.WriteJson(writer);
            }
            if (R != null) {
                writer.WritePropertyName("r");
                writer.Write(float.Parse(R.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += AreaModelName + ":";
            key += LayerModelName + ":";
            key += Position + ":";
            key += Vector + ":";
            key += R + ":";
            return key;
        }
    }
}