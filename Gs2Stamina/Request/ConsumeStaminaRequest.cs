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
	public class ConsumeStaminaRequest : Gs2Request<ConsumeStaminaRequest>
	{
         public string NamespaceName { set; get; }
         public string StaminaName { set; get; }
         public string AccessToken { set; get; }
         public int? ConsumeValue { set; get; }
        public string DuplicationAvoider { set; get; }
        public ConsumeStaminaRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ConsumeStaminaRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }
        public ConsumeStaminaRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public ConsumeStaminaRequest WithConsumeValue(int? consumeValue) {
            this.ConsumeValue = consumeValue;
            return this;
        }

        public ConsumeStaminaRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ConsumeStaminaRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ConsumeStaminaRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithConsumeValue(!data.Keys.Contains("consumeValue") || data["consumeValue"] == null ? null : (int?)(data["consumeValue"].ToString().Contains(".") ? (int)double.Parse(data["consumeValue"].ToString()) : int.Parse(data["consumeValue"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["staminaName"] = StaminaName,
                ["accessToken"] = AccessToken,
                ["consumeValue"] = ConsumeValue,
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
            if (ConsumeValue != null) {
                writer.WritePropertyName("consumeValue");
                writer.Write((ConsumeValue.ToString().Contains(".") ? (int)double.Parse(ConsumeValue.ToString()) : int.Parse(ConsumeValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += StaminaName + ":";
            key += AccessToken + ":";
            key += ConsumeValue + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply ConsumeStaminaRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ConsumeStaminaRequest)x;
            return this;
        }
    }
}