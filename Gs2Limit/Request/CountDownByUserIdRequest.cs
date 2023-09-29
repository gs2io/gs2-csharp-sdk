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
using Gs2.Gs2Limit.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Limit.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CountDownByUserIdRequest : Gs2Request<CountDownByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string LimitName { set; get; }
        public string CounterName { set; get; }
        public string UserId { set; get; }
        public int? CountDownValue { set; get; }
        public string DuplicationAvoider { set; get; }
        public CountDownByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CountDownByUserIdRequest WithLimitName(string limitName) {
            this.LimitName = limitName;
            return this;
        }
        public CountDownByUserIdRequest WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        public CountDownByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public CountDownByUserIdRequest WithCountDownValue(int? countDownValue) {
            this.CountDownValue = countDownValue;
            return this;
        }

        public CountDownByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CountDownByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CountDownByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithLimitName(!data.Keys.Contains("limitName") || data["limitName"] == null ? null : data["limitName"].ToString())
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCountDownValue(!data.Keys.Contains("countDownValue") || data["countDownValue"] == null ? null : (int?)int.Parse(data["countDownValue"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["limitName"] = LimitName,
                ["counterName"] = CounterName,
                ["userId"] = UserId,
                ["countDownValue"] = CountDownValue,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (LimitName != null) {
                writer.WritePropertyName("limitName");
                writer.Write(LimitName.ToString());
            }
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (CountDownValue != null) {
                writer.WritePropertyName("countDownValue");
                writer.Write(int.Parse(CountDownValue.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += LimitName + ":";
            key += CounterName + ":";
            key += UserId + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new CountDownByUserIdRequest {
                NamespaceName = NamespaceName,
                LimitName = LimitName,
                CounterName = CounterName,
                UserId = UserId,
                CountDownValue = CountDownValue * x,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CountDownByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values CountDownByUserIdRequest::namespaceName");
            }
            if (LimitName != y.LimitName) {
                throw new ArithmeticException("mismatch parameter values CountDownByUserIdRequest::limitName");
            }
            if (CounterName != y.CounterName) {
                throw new ArithmeticException("mismatch parameter values CountDownByUserIdRequest::counterName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values CountDownByUserIdRequest::userId");
            }
            return new CountDownByUserIdRequest {
                NamespaceName = NamespaceName,
                LimitName = LimitName,
                CounterName = CounterName,
                UserId = UserId,
                CountDownValue = CountDownValue + y.CountDownValue,
            };
        }
    }
}