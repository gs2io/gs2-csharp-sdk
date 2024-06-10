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
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateBillingMethodRequest : Gs2Request<CreateBillingMethodRequest>
	{
         public string AccountToken { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string MethodType { set; get; } = null!;
         public string CardCustomerId { set; get; } = null!;
         public string PartnerId { set; get; } = null!;
        public CreateBillingMethodRequest WithAccountToken(string accountToken) {
            this.AccountToken = accountToken;
            return this;
        }
        public CreateBillingMethodRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateBillingMethodRequest WithMethodType(string methodType) {
            this.MethodType = methodType;
            return this;
        }
        public CreateBillingMethodRequest WithCardCustomerId(string cardCustomerId) {
            this.CardCustomerId = cardCustomerId;
            return this;
        }
        public CreateBillingMethodRequest WithPartnerId(string partnerId) {
            this.PartnerId = partnerId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateBillingMethodRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateBillingMethodRequest()
                .WithAccountToken(!data.Keys.Contains("accountToken") || data["accountToken"] == null ? null : data["accountToken"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMethodType(!data.Keys.Contains("methodType") || data["methodType"] == null ? null : data["methodType"].ToString())
                .WithCardCustomerId(!data.Keys.Contains("cardCustomerId") || data["cardCustomerId"] == null ? null : data["cardCustomerId"].ToString())
                .WithPartnerId(!data.Keys.Contains("partnerId") || data["partnerId"] == null ? null : data["partnerId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["accountToken"] = AccountToken,
                ["description"] = Description,
                ["methodType"] = MethodType,
                ["cardCustomerId"] = CardCustomerId,
                ["partnerId"] = PartnerId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccountToken != null) {
                writer.WritePropertyName("accountToken");
                writer.Write(AccountToken.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (MethodType != null) {
                writer.WritePropertyName("methodType");
                writer.Write(MethodType.ToString());
            }
            if (CardCustomerId != null) {
                writer.WritePropertyName("cardCustomerId");
                writer.Write(CardCustomerId.ToString());
            }
            if (PartnerId != null) {
                writer.WritePropertyName("partnerId");
                writer.Write(PartnerId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += AccountToken + ":";
            key += Description + ":";
            key += MethodType + ":";
            key += CardCustomerId + ":";
            key += PartnerId + ":";
            return key;
        }
    }
}