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
	public class NearUserIdsFromSystemRequest : Gs2Request<NearUserIdsFromSystemRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AreaModelName { set; get; } = null!;
         public string LayerModelName { set; get; } = null!;
         public Gs2.Gs2MegaField.Model.Position Point { set; get; } = null!;
         public float? R { set; get; } = null!;
         public int? Limit { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public NearUserIdsFromSystemRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public NearUserIdsFromSystemRequest WithAreaModelName(string areaModelName) {
            this.AreaModelName = areaModelName;
            return this;
        }
        public NearUserIdsFromSystemRequest WithLayerModelName(string layerModelName) {
            this.LayerModelName = layerModelName;
            return this;
        }
        public NearUserIdsFromSystemRequest WithPoint(Gs2.Gs2MegaField.Model.Position point) {
            this.Point = point;
            return this;
        }
        public NearUserIdsFromSystemRequest WithR(float? r) {
            this.R = r;
            return this;
        }
        public NearUserIdsFromSystemRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

        public NearUserIdsFromSystemRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static NearUserIdsFromSystemRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new NearUserIdsFromSystemRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAreaModelName(!data.Keys.Contains("areaModelName") || data["areaModelName"] == null ? null : data["areaModelName"].ToString())
                .WithLayerModelName(!data.Keys.Contains("layerModelName") || data["layerModelName"] == null ? null : data["layerModelName"].ToString())
                .WithPoint(!data.Keys.Contains("point") || data["point"] == null ? null : Gs2.Gs2MegaField.Model.Position.FromJson(data["point"]))
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : (float?)float.Parse(data["r"].ToString()))
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["areaModelName"] = AreaModelName,
                ["layerModelName"] = LayerModelName,
                ["point"] = Point?.ToJson(),
                ["r"] = R,
                ["limit"] = Limit,
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
            if (Point != null) {
                Point.WriteJson(writer);
            }
            if (R != null) {
                writer.WritePropertyName("r");
                writer.Write(float.Parse(R.ToString()));
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AreaModelName + ":";
            key += LayerModelName + ":";
            key += Point + ":";
            key += R + ":";
            key += Limit + ":";
            return key;
        }
    }
}