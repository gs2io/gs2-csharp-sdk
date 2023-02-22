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
using Gs2.Gs2Exchange.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Exchange.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ExchangeByUserIdRequest : Gs2Request<ExchangeByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string RateName { set; get; }
        public string UserId { set; get; }
        public int? Count { set; get; }
        public Gs2.Gs2Exchange.Model.Config[] Config { set; get; }
        public string DuplicationAvoider { set; get; }
        public ExchangeByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ExchangeByUserIdRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public ExchangeByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ExchangeByUserIdRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }
        public ExchangeByUserIdRequest WithConfig(Gs2.Gs2Exchange.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public ExchangeByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ExchangeByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ExchangeByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)int.Parse(data["count"].ToString()))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2Exchange.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Exchange.Model.Config.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["rateName"] = RateName,
                ["userId"] = UserId,
                ["count"] = Count,
                ["config"] = Config == null ? null : new JsonData(
                        Config.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(int.Parse(Count.ToString()));
            }
            writer.WriteArrayStart();
            foreach (var confi in Config)
            {
                if (confi != null) {
                    confi.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RateName + ":";
            key += UserId + ":";
            key += Config + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new ExchangeByUserIdRequest {
                NamespaceName = NamespaceName,
                RateName = RateName,
                UserId = UserId,
                Count = Count * x,
                Config = Config,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ExchangeByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values ExchangeByUserIdRequest::namespaceName");
            }
            if (RateName != y.RateName) {
                throw new ArithmeticException("mismatch parameter values ExchangeByUserIdRequest::rateName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values ExchangeByUserIdRequest::userId");
            }
            if (Config != y.Config) {
                throw new ArithmeticException("mismatch parameter values ExchangeByUserIdRequest::config");
            }
            return new ExchangeByUserIdRequest {
                NamespaceName = NamespaceName,
                RateName = RateName,
                UserId = UserId,
                Count = Count + y.Count,
                Config = Config,
            };
        }
    }
}