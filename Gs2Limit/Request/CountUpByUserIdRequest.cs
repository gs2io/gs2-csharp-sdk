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
	public class CountUpByUserIdRequest : Gs2Request<CountUpByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string LimitName { set; get; }
         public string CounterName { set; get; }
         public string UserId { set; get; }
         public int? CountUpValue { set; get; }
         public int? MaxValue { set; get; }
        public string DuplicationAvoider { set; get; }
        public CountUpByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CountUpByUserIdRequest WithLimitName(string limitName) {
            this.LimitName = limitName;
            return this;
        }
        public CountUpByUserIdRequest WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        public CountUpByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public CountUpByUserIdRequest WithCountUpValue(int? countUpValue) {
            this.CountUpValue = countUpValue;
            return this;
        }
        public CountUpByUserIdRequest WithMaxValue(int? maxValue) {
            this.MaxValue = maxValue;
            return this;
        }

        public CountUpByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CountUpByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CountUpByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithLimitName(!data.Keys.Contains("limitName") || data["limitName"] == null ? null : data["limitName"].ToString())
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCountUpValue(!data.Keys.Contains("countUpValue") || data["countUpValue"] == null ? null : (int?)(data["countUpValue"].ToString().Contains(".") ? (int)double.Parse(data["countUpValue"].ToString()) : int.Parse(data["countUpValue"].ToString())))
                .WithMaxValue(!data.Keys.Contains("maxValue") || data["maxValue"] == null ? null : (int?)(data["maxValue"].ToString().Contains(".") ? (int)double.Parse(data["maxValue"].ToString()) : int.Parse(data["maxValue"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["limitName"] = LimitName,
                ["counterName"] = CounterName,
                ["userId"] = UserId,
                ["countUpValue"] = CountUpValue,
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
            if (CountUpValue != null) {
                writer.WritePropertyName("countUpValue");
                writer.Write((CountUpValue.ToString().Contains(".") ? (int)double.Parse(CountUpValue.ToString()) : int.Parse(CountUpValue.ToString())));
            }
            if (MaxValue != null) {
                writer.WritePropertyName("maxValue");
                writer.Write((MaxValue.ToString().Contains(".") ? (int)double.Parse(MaxValue.ToString()) : int.Parse(MaxValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += LimitName + ":";
            key += CounterName + ":";
            key += UserId + ":";
            key += MaxValue + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new CountUpByUserIdRequest {
                NamespaceName = NamespaceName,
                LimitName = LimitName,
                CounterName = CounterName,
                UserId = UserId,
                CountUpValue = CountUpValue * x,
                MaxValue = MaxValue,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CountUpByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values CountUpByUserIdRequest::namespaceName");
            }
            if (LimitName != y.LimitName) {
                throw new ArithmeticException("mismatch parameter values CountUpByUserIdRequest::limitName");
            }
            if (CounterName != y.CounterName) {
                throw new ArithmeticException("mismatch parameter values CountUpByUserIdRequest::counterName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values CountUpByUserIdRequest::userId");
            }
            if (MaxValue != y.MaxValue) {
                throw new ArithmeticException("mismatch parameter values CountUpByUserIdRequest::maxValue");
            }
            return new CountUpByUserIdRequest {
                NamespaceName = NamespaceName,
                LimitName = LimitName,
                CounterName = CounterName,
                UserId = UserId,
                CountUpValue = CountUpValue + y.CountUpValue,
                MaxValue = MaxValue,
            };
        }
    }
}