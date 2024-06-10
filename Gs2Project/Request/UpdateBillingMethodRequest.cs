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
	public class UpdateBillingMethodRequest : Gs2Request<UpdateBillingMethodRequest>
	{
         public string AccountToken { set; get; } = null!;
         public string BillingMethodName { set; get; } = null!;
         public string Description { set; get; } = null!;
        public UpdateBillingMethodRequest WithAccountToken(string accountToken) {
            this.AccountToken = accountToken;
            return this;
        }
        public UpdateBillingMethodRequest WithBillingMethodName(string billingMethodName) {
            this.BillingMethodName = billingMethodName;
            return this;
        }
        public UpdateBillingMethodRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateBillingMethodRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateBillingMethodRequest()
                .WithAccountToken(!data.Keys.Contains("accountToken") || data["accountToken"] == null ? null : data["accountToken"].ToString())
                .WithBillingMethodName(!data.Keys.Contains("billingMethodName") || data["billingMethodName"] == null ? null : data["billingMethodName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["accountToken"] = AccountToken,
                ["billingMethodName"] = BillingMethodName,
                ["description"] = Description,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccountToken != null) {
                writer.WritePropertyName("accountToken");
                writer.Write(AccountToken.ToString());
            }
            if (BillingMethodName != null) {
                writer.WritePropertyName("billingMethodName");
                writer.Write(BillingMethodName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += AccountToken + ":";
            key += BillingMethodName + ":";
            key += Description + ":";
            return key;
        }
    }
}