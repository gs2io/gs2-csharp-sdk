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
	public class DecreaseCounterByUserIdRequest : Gs2Request<DecreaseCounterByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string CounterName { set; get; }
         public string UserId { set; get; }
         public long? Value { set; get; }
        public string DuplicationAvoider { set; get; }
        public DecreaseCounterByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DecreaseCounterByUserIdRequest WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        public DecreaseCounterByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DecreaseCounterByUserIdRequest WithValue(long? value) {
            this.Value = value;
            return this;
        }

        public DecreaseCounterByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DecreaseCounterByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DecreaseCounterByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (long?)(data["value"].ToString().Contains(".") ? (long)double.Parse(data["value"].ToString()) : long.Parse(data["value"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["counterName"] = CounterName,
                ["userId"] = UserId,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (long)double.Parse(Value.ToString()) : long.Parse(Value.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CounterName + ":";
            key += UserId + ":";
            return key;
        }
    }
}