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
	public class CountUpRequest : Gs2Request<CountUpRequest>
	{
        public string NamespaceName { set; get; }
        public string LimitName { set; get; }
        public string CounterName { set; get; }
        public string AccessToken { set; get; }
        public int? CountUpValue { set; get; }
        public int? MaxValue { set; get; }
        public string DuplicationAvoider { set; get; }
        public CountUpRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CountUpRequest WithLimitName(string limitName) {
            this.LimitName = limitName;
            return this;
        }
        public CountUpRequest WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        public CountUpRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public CountUpRequest WithCountUpValue(int? countUpValue) {
            this.CountUpValue = countUpValue;
            return this;
        }
        public CountUpRequest WithMaxValue(int? maxValue) {
            this.MaxValue = maxValue;
            return this;
        }

        public CountUpRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CountUpRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CountUpRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithLimitName(!data.Keys.Contains("limitName") || data["limitName"] == null ? null : data["limitName"].ToString())
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithCountUpValue(!data.Keys.Contains("countUpValue") || data["countUpValue"] == null ? null : (int?)int.Parse(data["countUpValue"].ToString()))
                .WithMaxValue(!data.Keys.Contains("maxValue") || data["maxValue"] == null ? null : (int?)int.Parse(data["maxValue"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["limitName"] = LimitName,
                ["counterName"] = CounterName,
                ["accessToken"] = AccessToken,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (CountUpValue != null) {
                writer.WritePropertyName("countUpValue");
                writer.Write(int.Parse(CountUpValue.ToString()));
            }
            if (MaxValue != null) {
                writer.WritePropertyName("maxValue");
                writer.Write(int.Parse(MaxValue.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}