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
	public class VerifyReceiptRequest : Gs2Request<VerifyReceiptRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string ContentName { set; get; } = null!;
         public Gs2.Gs2Money2.Model.Receipt Receipt { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyReceiptRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyReceiptRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyReceiptRequest WithContentName(string contentName) {
            this.ContentName = contentName;
            return this;
        }
        public VerifyReceiptRequest WithReceipt(Gs2.Gs2Money2.Model.Receipt receipt) {
            this.Receipt = receipt;
            return this;
        }

        public VerifyReceiptRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyReceiptRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyReceiptRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithContentName(!data.Keys.Contains("contentName") || data["contentName"] == null ? null : data["contentName"].ToString())
                .WithReceipt(!data.Keys.Contains("receipt") || data["receipt"] == null ? null : Gs2.Gs2Money2.Model.Receipt.FromJson(data["receipt"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["contentName"] = ContentName,
                ["receipt"] = Receipt?.ToJson(),
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
            if (ContentName != null) {
                writer.WritePropertyName("contentName");
                writer.Write(ContentName.ToString());
            }
            if (Receipt != null) {
                Receipt.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += ContentName + ":";
            key += Receipt + ":";
            return key;
        }
    }
}