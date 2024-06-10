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
	public class GetLayerModelRequest : Gs2Request<GetLayerModelRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AreaModelName { set; get; } = null!;
         public string LayerModelName { set; get; } = null!;
        public GetLayerModelRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetLayerModelRequest WithAreaModelName(string areaModelName) {
            this.AreaModelName = areaModelName;
            return this;
        }
        public GetLayerModelRequest WithLayerModelName(string layerModelName) {
            this.LayerModelName = layerModelName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetLayerModelRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetLayerModelRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAreaModelName(!data.Keys.Contains("areaModelName") || data["areaModelName"] == null ? null : data["areaModelName"].ToString())
                .WithLayerModelName(!data.Keys.Contains("layerModelName") || data["layerModelName"] == null ? null : data["layerModelName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["areaModelName"] = AreaModelName,
                ["layerModelName"] = LayerModelName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AreaModelName != null) {
                writer.WritePropertyName("areaModelName");
                writer.Write(AreaModelName.ToString());
            }
            if (LayerModelName != null) {
                writer.WritePropertyName("layerModelName");
                writer.Write(LayerModelName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AreaModelName + ":";
            key += LayerModelName + ":";
            return key;
        }
    }
}