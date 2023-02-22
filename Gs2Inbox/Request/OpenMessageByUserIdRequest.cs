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
using Gs2.Gs2Inbox.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class OpenMessageByUserIdRequest : Gs2Request<OpenMessageByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string MessageName { set; get; }
        public string DuplicationAvoider { set; get; }
        public OpenMessageByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public OpenMessageByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public OpenMessageByUserIdRequest WithMessageName(string messageName) {
            this.MessageName = messageName;
            return this;
        }

        public OpenMessageByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static OpenMessageByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new OpenMessageByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMessageName(!data.Keys.Contains("messageName") || data["messageName"] == null ? null : data["messageName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["messageName"] = MessageName,
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
            if (MessageName != null) {
                writer.WritePropertyName("messageName");
                writer.Write(MessageName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += MessageName + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new OpenMessageByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                MessageName = MessageName,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (OpenMessageByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values OpenMessageByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values OpenMessageByUserIdRequest::userId");
            }
            if (MessageName != y.MessageName) {
                throw new ArithmeticException("mismatch parameter values OpenMessageByUserIdRequest::messageName");
            }
            return new OpenMessageByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                MessageName = MessageName,
            };
        }
    }
}