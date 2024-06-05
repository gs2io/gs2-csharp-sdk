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
using Gs2.Gs2Mission.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Mission.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VerifyCounterValueRequest : Gs2Request<VerifyCounterValueRequest>
	{
         public string NamespaceName { set; get; }
         public string AccessToken { set; get; }
         public string CounterName { set; get; }
         public string VerifyType { set; get; }
         public string ResetType { set; get; }
         public long? Value { set; get; }
         public bool? MultiplyValueSpecifyingQuantity { set; get; }
        public string DuplicationAvoider { set; get; }
        public VerifyCounterValueRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyCounterValueRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyCounterValueRequest WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        public VerifyCounterValueRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyCounterValueRequest WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }
        public VerifyCounterValueRequest WithValue(long? value) {
            this.Value = value;
            return this;
        }
        public VerifyCounterValueRequest WithMultiplyValueSpecifyingQuantity(bool? multiplyValueSpecifyingQuantity) {
            this.MultiplyValueSpecifyingQuantity = multiplyValueSpecifyingQuantity;
            return this;
        }

        public VerifyCounterValueRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyCounterValueRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyCounterValueRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (long?)(data["value"].ToString().Contains(".") ? (long)double.Parse(data["value"].ToString()) : long.Parse(data["value"].ToString())))
                .WithMultiplyValueSpecifyingQuantity(!data.Keys.Contains("multiplyValueSpecifyingQuantity") || data["multiplyValueSpecifyingQuantity"] == null ? null : (bool?)bool.Parse(data["multiplyValueSpecifyingQuantity"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["counterName"] = CounterName,
                ["verifyType"] = VerifyType,
                ["resetType"] = ResetType,
                ["value"] = Value,
                ["multiplyValueSpecifyingQuantity"] = MultiplyValueSpecifyingQuantity,
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
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (ResetType != null) {
                writer.WritePropertyName("resetType");
                writer.Write(ResetType.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (long)double.Parse(Value.ToString()) : long.Parse(Value.ToString())));
            }
            if (MultiplyValueSpecifyingQuantity != null) {
                writer.WritePropertyName("multiplyValueSpecifyingQuantity");
                writer.Write(bool.Parse(MultiplyValueSpecifyingQuantity.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += CounterName + ":";
            key += VerifyType + ":";
            key += ResetType + ":";
            key += Value + ":";
            key += MultiplyValueSpecifyingQuantity + ":";
            return key;
        }
    }
}