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
	public class PutPositionByUserIdRequest : Gs2Request<PutPositionByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string AreaModelName { set; get; } = null!;
         public string LayerModelName { set; get; } = null!;
         public Gs2.Gs2MegaField.Model.Position Position { set; get; } = null!;
         public Gs2.Gs2MegaField.Model.Vector Vector { set; get; } = null!;
         public float? R { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public PutPositionByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PutPositionByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public PutPositionByUserIdRequest WithAreaModelName(string areaModelName) {
            this.AreaModelName = areaModelName;
            return this;
        }
        public PutPositionByUserIdRequest WithLayerModelName(string layerModelName) {
            this.LayerModelName = layerModelName;
            return this;
        }
        public PutPositionByUserIdRequest WithPosition(Gs2.Gs2MegaField.Model.Position position) {
            this.Position = position;
            return this;
        }
        public PutPositionByUserIdRequest WithVector(Gs2.Gs2MegaField.Model.Vector vector) {
            this.Vector = vector;
            return this;
        }
        public PutPositionByUserIdRequest WithR(float? r) {
            this.R = r;
            return this;
        }
        public PutPositionByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public PutPositionByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PutPositionByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PutPositionByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAreaModelName(!data.Keys.Contains("areaModelName") || data["areaModelName"] == null ? null : data["areaModelName"].ToString())
                .WithLayerModelName(!data.Keys.Contains("layerModelName") || data["layerModelName"] == null ? null : data["layerModelName"].ToString())
                .WithPosition(!data.Keys.Contains("position") || data["position"] == null ? null : Gs2.Gs2MegaField.Model.Position.FromJson(data["position"]))
                .WithVector(!data.Keys.Contains("vector") || data["vector"] == null ? null : Gs2.Gs2MegaField.Model.Vector.FromJson(data["vector"]))
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : (float?)float.Parse(data["r"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["areaModelName"] = AreaModelName,
                ["layerModelName"] = LayerModelName,
                ["position"] = Position?.ToJson(),
                ["vector"] = Vector?.ToJson(),
                ["r"] = R,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
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
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += AreaModelName + ":";
            key += LayerModelName + ":";
            key += Position + ":";
            key += Vector + ":";
            key += R + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}