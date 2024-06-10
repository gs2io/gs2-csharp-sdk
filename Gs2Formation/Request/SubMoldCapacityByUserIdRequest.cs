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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SubMoldCapacityByUserIdRequest : Gs2Request<SubMoldCapacityByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string MoldModelName { set; get; } = null!;
         public int? Capacity { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SubMoldCapacityByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SubMoldCapacityByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SubMoldCapacityByUserIdRequest WithMoldModelName(string moldModelName) {
            this.MoldModelName = moldModelName;
            return this;
        }
        public SubMoldCapacityByUserIdRequest WithCapacity(int? capacity) {
            this.Capacity = capacity;
            return this;
        }
        public SubMoldCapacityByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public SubMoldCapacityByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SubMoldCapacityByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubMoldCapacityByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMoldModelName(!data.Keys.Contains("moldModelName") || data["moldModelName"] == null ? null : data["moldModelName"].ToString())
                .WithCapacity(!data.Keys.Contains("capacity") || data["capacity"] == null ? null : (int?)(data["capacity"].ToString().Contains(".") ? (int)double.Parse(data["capacity"].ToString()) : int.Parse(data["capacity"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["moldModelName"] = MoldModelName,
                ["capacity"] = Capacity,
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
            if (MoldModelName != null) {
                writer.WritePropertyName("moldModelName");
                writer.Write(MoldModelName.ToString());
            }
            if (Capacity != null) {
                writer.WritePropertyName("capacity");
                writer.Write((Capacity.ToString().Contains(".") ? (int)double.Parse(Capacity.ToString()) : int.Parse(Capacity.ToString())));
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
            key += MoldModelName + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}