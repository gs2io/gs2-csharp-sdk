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
using UnityEngine.Scripting;

namespace Gs2.Gs2Project.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateProjectRequest : Gs2Request<CreateProjectRequest>
	{
        public string AccountToken { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Plan { set; get; }
        public string BillingMethodName { set; get; }
        public string EnableEventBridge { set; get; }
        public string EventBridgeAwsAccountId { set; get; }
        public string EventBridgeAwsRegion { set; get; }

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

    	[Preserve]
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
                .WithBillingMethodName(!data.Keys.Contains("billingMethodName") || data["billingMethodName"] == null ? null : data["billingMethodName"].ToString())
                .WithEnableEventBridge(!data.Keys.Contains("enableEventBridge") || data["enableEventBridge"] == null ? null : data["enableEventBridge"].ToString())
                .WithEventBridgeAwsAccountId(!data.Keys.Contains("eventBridgeAwsAccountId") || data["eventBridgeAwsAccountId"] == null ? null : data["eventBridgeAwsAccountId"].ToString())
                .WithEventBridgeAwsRegion(!data.Keys.Contains("eventBridgeAwsRegion") || data["eventBridgeAwsRegion"] == null ? null : data["eventBridgeAwsRegion"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["accountToken"] = AccountToken,
                ["name"] = Name,
                ["description"] = Description,
                ["plan"] = Plan,
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
    }
}