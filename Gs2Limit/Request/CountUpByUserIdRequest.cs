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
         public string NamespaceName { set; get; } = null!;
         public string LimitName { set; get; } = null!;
         public string CounterName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? CountUpValue { set; get; } = null!;
         public int? MaxValue { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
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
        public CountUpByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
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
                .WithMaxValue(!data.Keys.Contains("maxValue") || data["maxValue"] == null ? null : (int?)(data["maxValue"].ToString().Contains(".") ? (int)double.Parse(data["maxValue"].ToString()) : int.Parse(data["maxValue"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
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
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
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
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}