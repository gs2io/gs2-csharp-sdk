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
	public class CreateProjectRequest : Gs2Request<CreateProjectRequest>
	{
         public string AccountToken { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Plan { set; get; } = null!;
         public string Currency { set; get; } = null!;
         public string ActivateRegionName { set; get; } = null!;
         public string BillingMethodName { set; get; } = null!;
         public string EnableEventBridge { set; get; } = null!;
         public string EventBridgeAwsAccountId { set; get; } = null!;
         public string EventBridgeAwsRegion { set; get; } = null!;
        public CreateProjectRequest WithAccountToken(string accountToken) {
            this.AccountToken = accountToken;
            return this;
        }
        public CreateProjectRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateProjectRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateProjectRequest WithPlan(string plan) {
            this.Plan = plan;
            return this;
        }
        public CreateProjectRequest WithCurrency(string currency) {
            this.Currency = currency;
            return this;
        }
        public CreateProjectRequest WithActivateRegionName(string activateRegionName) {
            this.ActivateRegionName = activateRegionName;
            return this;
        }
        public CreateProjectRequest WithBillingMethodName(string billingMethodName) {
            this.BillingMethodName = billingMethodName;
            return this;
        }
        public CreateProjectRequest WithEnableEventBridge(string enableEventBridge) {
            this.EnableEventBridge = enableEventBridge;
            return this;
        }
        public CreateProjectRequest WithEventBridgeAwsAccountId(string eventBridgeAwsAccountId) {
            this.EventBridgeAwsAccountId = eventBridgeAwsAccountId;
            return this;
        }
        public CreateProjectRequest WithEventBridgeAwsRegion(string eventBridgeAwsRegion) {
            this.EventBridgeAwsRegion = eventBridgeAwsRegion;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateProjectRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateProjectRequest()
                .WithAccountToken(!data.Keys.Contains("accountToken") || data["accountToken"] == null ? null : data["accountToken"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithPlan(!data.Keys.Contains("plan") || data["plan"] == null ? null : data["plan"].ToString())
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithActivateRegionName(!data.Keys.Contains("activateRegionName") || data["activateRegionName"] == null ? null : data["activateRegionName"].ToString())
                .WithBillingMethodName(!data.Keys.Contains("billingMethodName") || data["billingMethodName"] == null ? null : data["billingMethodName"].ToString())
                .WithEnableEventBridge(!data.Keys.Contains("enableEventBridge") || data["enableEventBridge"] == null ? null : data["enableEventBridge"].ToString())
                .WithEventBridgeAwsAccountId(!data.Keys.Contains("eventBridgeAwsAccountId") || data["eventBridgeAwsAccountId"] == null ? null : data["eventBridgeAwsAccountId"].ToString())
                .WithEventBridgeAwsRegion(!data.Keys.Contains("eventBridgeAwsRegion") || data["eventBridgeAwsRegion"] == null ? null : data["eventBridgeAwsRegion"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["accountToken"] = AccountToken,
                ["name"] = Name,
                ["description"] = Description,
                ["plan"] = Plan,
                ["currency"] = Currency,
                ["activateRegionName"] = ActivateRegionName,
                ["billingMethodName"] = BillingMethodName,
                ["enableEventBridge"] = EnableEventBridge,
                ["eventBridgeAwsAccountId"] = EventBridgeAwsAccountId,
                ["eventBridgeAwsRegion"] = EventBridgeAwsRegion,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccountToken != null) {
                writer.WritePropertyName("accountToken");
                writer.Write(AccountToken.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Plan != null) {
                writer.WritePropertyName("plan");
                writer.Write(Plan.ToString());
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
            }
            if (ActivateRegionName != null) {
                writer.WritePropertyName("activateRegionName");
                writer.Write(ActivateRegionName.ToString());
            }
            if (BillingMethodName != null) {
                writer.WritePropertyName("billingMethodName");
                writer.Write(BillingMethodName.ToString());
            }
            if (EnableEventBridge != null) {
                writer.WritePropertyName("enableEventBridge");
                writer.Write(EnableEventBridge.ToString());
            }
            if (EventBridgeAwsAccountId != null) {
                writer.WritePropertyName("eventBridgeAwsAccountId");
                writer.Write(EventBridgeAwsAccountId.ToString());
            }
            if (EventBridgeAwsRegion != null) {
                writer.WritePropertyName("eventBridgeAwsRegion");
                writer.Write(EventBridgeAwsRegion.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += AccountToken + ":";
            key += Name + ":";
            key += Description + ":";
            key += Plan + ":";
            key += Currency + ":";
            key += ActivateRegionName + ":";
            key += BillingMethodName + ":";
            key += EnableEventBridge + ":";
            key += EventBridgeAwsAccountId + ":";
            key += EventBridgeAwsRegion + ":";
            return key;
        }
    }
}