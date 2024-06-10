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
	public class UpdateProjectRequest : Gs2Request<UpdateProjectRequest>
	{
         public string AccountToken { set; get; } = null!;
         public string ProjectName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Plan { set; get; } = null!;
         public string BillingMethodName { set; get; } = null!;
         public string EnableEventBridge { set; get; } = null!;
         public string EventBridgeAwsAccountId { set; get; } = null!;
         public string EventBridgeAwsRegion { set; get; } = null!;
        public UpdateProjectRequest WithAccountToken(string accountToken) {
            this.AccountToken = accountToken;
            return this;
        }
        public UpdateProjectRequest WithProjectName(string projectName) {
            this.ProjectName = projectName;
            return this;
        }
        public UpdateProjectRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateProjectRequest WithPlan(string plan) {
            this.Plan = plan;
            return this;
        }
        public UpdateProjectRequest WithBillingMethodName(string billingMethodName) {
            this.BillingMethodName = billingMethodName;
            return this;
        }
        public UpdateProjectRequest WithEnableEventBridge(string enableEventBridge) {
            this.EnableEventBridge = enableEventBridge;
            return this;
        }
        public UpdateProjectRequest WithEventBridgeAwsAccountId(string eventBridgeAwsAccountId) {
            this.EventBridgeAwsAccountId = eventBridgeAwsAccountId;
            return this;
        }
        public UpdateProjectRequest WithEventBridgeAwsRegion(string eventBridgeAwsRegion) {
            this.EventBridgeAwsRegion = eventBridgeAwsRegion;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateProjectRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateProjectRequest()
                .WithAccountToken(!data.Keys.Contains("accountToken") || data["accountToken"] == null ? null : data["accountToken"].ToString())
                .WithProjectName(!data.Keys.Contains("projectName") || data["projectName"] == null ? null : data["projectName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithPlan(!data.Keys.Contains("plan") || data["plan"] == null ? null : data["plan"].ToString())
                .WithBillingMethodName(!data.Keys.Contains("billingMethodName") || data["billingMethodName"] == null ? null : data["billingMethodName"].ToString())
                .WithEnableEventBridge(!data.Keys.Contains("enableEventBridge") || data["enableEventBridge"] == null ? null : data["enableEventBridge"].ToString())
                .WithEventBridgeAwsAccountId(!data.Keys.Contains("eventBridgeAwsAccountId") || data["eventBridgeAwsAccountId"] == null ? null : data["eventBridgeAwsAccountId"].ToString())
                .WithEventBridgeAwsRegion(!data.Keys.Contains("eventBridgeAwsRegion") || data["eventBridgeAwsRegion"] == null ? null : data["eventBridgeAwsRegion"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["accountToken"] = AccountToken,
                ["projectName"] = ProjectName,
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
            if (ProjectName != null) {
                writer.WritePropertyName("projectName");
                writer.Write(ProjectName.ToString());
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

        public override string UniqueKey() {
            var key = "";
            key += AccountToken + ":";
            key += ProjectName + ":";
            key += Description + ":";
            key += Plan + ":";
            key += BillingMethodName + ":";
            key += EnableEventBridge + ":";
            key += EventBridgeAwsAccountId + ":";
            key += EventBridgeAwsRegion + ":";
            return key;
        }
    }
}