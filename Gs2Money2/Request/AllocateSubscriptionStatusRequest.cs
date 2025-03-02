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
using Gs2.Gs2Money2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AllocateSubscriptionStatusRequest : Gs2Request<AllocateSubscriptionStatusRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string Receipt { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public AllocateSubscriptionStatusRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public AllocateSubscriptionStatusRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public AllocateSubscriptionStatusRequest WithReceipt(string receipt) {
            this.Receipt = receipt;
            return this;
        }

        public AllocateSubscriptionStatusRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AllocateSubscriptionStatusRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AllocateSubscriptionStatusRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithReceipt(!data.Keys.Contains("receipt") || data["receipt"] == null ? null : data["receipt"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["receipt"] = Receipt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (Receipt != null) {
                writer.WritePropertyName("receipt");
                writer.Write(Receipt.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += Receipt + ":";
            return key;
        }
    }
}