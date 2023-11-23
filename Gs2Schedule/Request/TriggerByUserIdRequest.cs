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
using Gs2.Gs2Schedule.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Schedule.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class TriggerByUserIdRequest : Gs2Request<TriggerByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string TriggerName { set; get; }
         public string UserId { set; get; }
         public string TriggerStrategy { set; get; }
         public int? Ttl { set; get; }
        public string DuplicationAvoider { set; get; }
        public TriggerByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public TriggerByUserIdRequest WithTriggerName(string triggerName) {
            this.TriggerName = triggerName;
            return this;
        }
        public TriggerByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public TriggerByUserIdRequest WithTriggerStrategy(string triggerStrategy) {
            this.TriggerStrategy = triggerStrategy;
            return this;
        }
        public TriggerByUserIdRequest WithTtl(int? ttl) {
            this.Ttl = ttl;
            return this;
        }

        public TriggerByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TriggerByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TriggerByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithTriggerName(!data.Keys.Contains("triggerName") || data["triggerName"] == null ? null : data["triggerName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTriggerStrategy(!data.Keys.Contains("triggerStrategy") || data["triggerStrategy"] == null ? null : data["triggerStrategy"].ToString())
                .WithTtl(!data.Keys.Contains("ttl") || data["ttl"] == null ? null : (int?)(data["ttl"].ToString().Contains(".") ? (int)double.Parse(data["ttl"].ToString()) : int.Parse(data["ttl"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["triggerName"] = TriggerName,
                ["userId"] = UserId,
                ["triggerStrategy"] = TriggerStrategy,
                ["ttl"] = Ttl,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (TriggerName != null) {
                writer.WritePropertyName("triggerName");
                writer.Write(TriggerName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TriggerStrategy != null) {
                writer.WritePropertyName("triggerStrategy");
                writer.Write(TriggerStrategy.ToString());
            }
            if (Ttl != null) {
                writer.WritePropertyName("ttl");
                writer.Write((Ttl.ToString().Contains(".") ? (int)double.Parse(Ttl.ToString()) : int.Parse(Ttl.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += TriggerName + ":";
            key += UserId + ":";
            key += TriggerStrategy + ":";
            key += Ttl + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new TriggerByUserIdRequest {
                NamespaceName = NamespaceName,
                TriggerName = TriggerName,
                UserId = UserId,
                TriggerStrategy = TriggerStrategy,
                Ttl = Ttl,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (TriggerByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values TriggerByUserIdRequest::namespaceName");
            }
            if (TriggerName != y.TriggerName) {
                throw new ArithmeticException("mismatch parameter values TriggerByUserIdRequest::triggerName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values TriggerByUserIdRequest::userId");
            }
            if (TriggerStrategy != y.TriggerStrategy) {
                throw new ArithmeticException("mismatch parameter values TriggerByUserIdRequest::triggerStrategy");
            }
            if (Ttl != y.Ttl) {
                throw new ArithmeticException("mismatch parameter values TriggerByUserIdRequest::ttl");
            }
            return new TriggerByUserIdRequest {
                NamespaceName = NamespaceName,
                TriggerName = TriggerName,
                UserId = UserId,
                TriggerStrategy = TriggerStrategy,
                Ttl = Ttl,
            };
        }
    }
}