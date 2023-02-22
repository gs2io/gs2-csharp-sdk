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
using Gs2.Gs2Stamina.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Stamina.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SetMaxValueByUserIdRequest : Gs2Request<SetMaxValueByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string StaminaName { set; get; }
        public string UserId { set; get; }
        public int? MaxValue { set; get; }
        public string DuplicationAvoider { set; get; }
        public SetMaxValueByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SetMaxValueByUserIdRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }
        public SetMaxValueByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SetMaxValueByUserIdRequest WithMaxValue(int? maxValue) {
            this.MaxValue = maxValue;
            return this;
        }

        public SetMaxValueByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetMaxValueByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetMaxValueByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMaxValue(!data.Keys.Contains("maxValue") || data["maxValue"] == null ? null : (int?)int.Parse(data["maxValue"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["staminaName"] = StaminaName,
                ["userId"] = UserId,
                ["maxValue"] = MaxValue,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (StaminaName != null) {
                writer.WritePropertyName("staminaName");
                writer.Write(StaminaName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (MaxValue != null) {
                writer.WritePropertyName("maxValue");
                writer.Write(int.Parse(MaxValue.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += StaminaName + ":";
            key += UserId + ":";
            key += MaxValue + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new SetMaxValueByUserIdRequest {
                NamespaceName = NamespaceName,
                StaminaName = StaminaName,
                UserId = UserId,
                MaxValue = MaxValue,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (SetMaxValueByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values SetMaxValueByUserIdRequest::namespaceName");
            }
            if (StaminaName != y.StaminaName) {
                throw new ArithmeticException("mismatch parameter values SetMaxValueByUserIdRequest::staminaName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values SetMaxValueByUserIdRequest::userId");
            }
            if (MaxValue != y.MaxValue) {
                throw new ArithmeticException("mismatch parameter values SetMaxValueByUserIdRequest::maxValue");
            }
            return new SetMaxValueByUserIdRequest {
                NamespaceName = NamespaceName,
                StaminaName = StaminaName,
                UserId = UserId,
                MaxValue = MaxValue,
            };
        }
    }
}