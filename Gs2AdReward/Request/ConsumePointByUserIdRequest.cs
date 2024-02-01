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
using Gs2.Gs2AdReward.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2AdReward.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ConsumePointByUserIdRequest : Gs2Request<ConsumePointByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public long? Point { set; get; }
        public string DuplicationAvoider { set; get; }
        public ConsumePointByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ConsumePointByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ConsumePointByUserIdRequest WithPoint(long? point) {
            this.Point = point;
            return this;
        }

        public ConsumePointByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ConsumePointByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ConsumePointByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPoint(!data.Keys.Contains("point") || data["point"] == null ? null : (long?)(data["point"].ToString().Contains(".") ? (long)double.Parse(data["point"].ToString()) : long.Parse(data["point"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["point"] = Point,
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
            if (Point != null) {
                writer.WritePropertyName("point");
                writer.Write((Point.ToString().Contains(".") ? (long)double.Parse(Point.ToString()) : long.Parse(Point.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            return key;
        }
    }
}