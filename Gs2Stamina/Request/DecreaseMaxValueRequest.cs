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
	public class DecreaseMaxValueRequest : Gs2Request<DecreaseMaxValueRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string StaminaName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public int? DecreaseValue { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public DecreaseMaxValueRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DecreaseMaxValueRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }
        public DecreaseMaxValueRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public DecreaseMaxValueRequest WithDecreaseValue(int? decreaseValue) {
            this.DecreaseValue = decreaseValue;
            return this;
        }

        public DecreaseMaxValueRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DecreaseMaxValueRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DecreaseMaxValueRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithDecreaseValue(!data.Keys.Contains("decreaseValue") || data["decreaseValue"] == null ? null : (int?)(data["decreaseValue"].ToString().Contains(".") ? (int)double.Parse(data["decreaseValue"].ToString()) : int.Parse(data["decreaseValue"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["staminaName"] = StaminaName,
                ["accessToken"] = AccessToken,
                ["decreaseValue"] = DecreaseValue,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (DecreaseValue != null) {
                writer.WritePropertyName("decreaseValue");
                writer.Write((DecreaseValue.ToString().Contains(".") ? (int)double.Parse(DecreaseValue.ToString()) : int.Parse(DecreaseValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += StaminaName + ":";
            key += AccessToken + ":";
            key += DecreaseValue + ":";
            return key;
        }
    }
}