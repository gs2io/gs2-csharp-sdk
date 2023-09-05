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
using Gs2.Gs2SerialKey.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SerialKey.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class RevertUseByUserIdRequest : Gs2Request<RevertUseByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string Code { set; get; }
        public string DuplicationAvoider { set; get; }
        public RevertUseByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RevertUseByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public RevertUseByUserIdRequest WithCode(string code) {
            this.Code = code;
            return this;
        }

        public RevertUseByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RevertUseByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RevertUseByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCode(!data.Keys.Contains("code") || data["code"] == null ? null : data["code"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["code"] = Code,
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
            if (Code != null) {
                writer.WritePropertyName("code");
                writer.Write(Code.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += Code + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new RevertUseByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Code = Code,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (RevertUseByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values RevertUseByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values RevertUseByUserIdRequest::userId");
            }
            if (Code != y.Code) {
                throw new ArithmeticException("mismatch parameter values RevertUseByUserIdRequest::code");
            }
            return new RevertUseByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Code = Code,
            };
        }
    }
}