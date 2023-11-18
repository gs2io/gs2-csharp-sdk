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
using Gs2.Gs2StateMachine.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2StateMachine.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class StartStateMachineByUserIdRequest : Gs2Request<StartStateMachineByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string Args { set; get; }
        public int? Ttl { set; get; }
        public string DuplicationAvoider { set; get; }

        public StartStateMachineByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public StartStateMachineByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public StartStateMachineByUserIdRequest WithArgs(string args) {
            this.Args = args;
            return this;
        }

        public StartStateMachineByUserIdRequest WithTtl(int? ttl) {
            this.Ttl = ttl;
            return this;
        }

        public StartStateMachineByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StartStateMachineByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StartStateMachineByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithTtl(!data.Keys.Contains("ttl") || data["ttl"] == null ? null : (int?)(data["ttl"].ToString().Contains(".") ? (int)double.Parse(data["ttl"].ToString()) : int.Parse(data["ttl"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["args"] = Args,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
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
            key += UserId + ":";
            key += Args + ":";
            key += Ttl + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new StartStateMachineByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Args = Args,
                Ttl = Ttl,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (StartStateMachineByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values StartStateMachineByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values StartStateMachineByUserIdRequest::userId");
            }
            if (Args != y.Args) {
                throw new ArithmeticException("mismatch parameter values StartStateMachineByUserIdRequest::args");
            }
            if (Ttl != y.Ttl) {
                throw new ArithmeticException("mismatch parameter values StartStateMachineByUserIdRequest::ttl");
            }
            return new StartStateMachineByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Args = Args,
                Ttl = Ttl,
            };
        }
    }
}