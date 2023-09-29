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
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string MoldModelName { set; get; }
        public int? Capacity { set; get; }
        public string DuplicationAvoider { set; get; }
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
                .WithCapacity(!data.Keys.Contains("capacity") || data["capacity"] == null ? null : (int?)int.Parse(data["capacity"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["moldModelName"] = MoldModelName,
                ["capacity"] = Capacity,
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
                writer.Write(int.Parse(Capacity.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += MoldModelName + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new SubMoldCapacityByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                MoldModelName = MoldModelName,
                Capacity = Capacity * x,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (SubMoldCapacityByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values SubMoldCapacityByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values SubMoldCapacityByUserIdRequest::userId");
            }
            if (MoldModelName != y.MoldModelName) {
                throw new ArithmeticException("mismatch parameter values SubMoldCapacityByUserIdRequest::moldModelName");
            }
            return new SubMoldCapacityByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                MoldModelName = MoldModelName,
                Capacity = Capacity + y.Capacity,
            };
        }
    }
}