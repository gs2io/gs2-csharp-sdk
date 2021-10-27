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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Project : IComparable
	{
        public string ProjectId { set; get; }
        public string AccountName { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Plan { set; get; }
        public string BillingMethodName { set; get; }
        public string EnableEventBridge { set; get; }
        public string EventBridgeAwsAccountId { set; get; }
        public string EventBridgeAwsRegion { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Project WithProjectId(string projectId) {
            this.ProjectId = projectId;
            return this;
        }

        public Project WithAccountName(string accountName) {
            this.AccountName = accountName;
            return this;
        }

        public Project WithName(string name) {
            this.Name = name;
            return this;
        }

        public Project WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public Project WithPlan(string plan) {
            this.Plan = plan;
            return this;
        }

        public Project WithBillingMethodName(string billingMethodName) {
            this.BillingMethodName = billingMethodName;
            return this;
        }

        public Project WithEnableEventBridge(string enableEventBridge) {
            this.EnableEventBridge = enableEventBridge;
            return this;
        }

        public Project WithEventBridgeAwsAccountId(string eventBridgeAwsAccountId) {
            this.EventBridgeAwsAccountId = eventBridgeAwsAccountId;
            return this;
        }

        public Project WithEventBridgeAwsRegion(string eventBridgeAwsRegion) {
            this.EventBridgeAwsRegion = eventBridgeAwsRegion;
            return this;
        }

        public Project WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Project WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Project FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Project()
                .WithProjectId(!data.Keys.Contains("projectId") || data["projectId"] == null ? null : data["projectId"].ToString())
                .WithAccountName(!data.Keys.Contains("accountName") || data["accountName"] == null ? null : data["accountName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithPlan(!data.Keys.Contains("plan") || data["plan"] == null ? null : data["plan"].ToString())
                .WithBillingMethodName(!data.Keys.Contains("billingMethodName") || data["billingMethodName"] == null ? null : data["billingMethodName"].ToString())
                .WithEnableEventBridge(!data.Keys.Contains("enableEventBridge") || data["enableEventBridge"] == null ? null : data["enableEventBridge"].ToString())
                .WithEventBridgeAwsAccountId(!data.Keys.Contains("eventBridgeAwsAccountId") || data["eventBridgeAwsAccountId"] == null ? null : data["eventBridgeAwsAccountId"].ToString())
                .WithEventBridgeAwsRegion(!data.Keys.Contains("eventBridgeAwsRegion") || data["eventBridgeAwsRegion"] == null ? null : data["eventBridgeAwsRegion"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["projectId"] = ProjectId,
                ["accountName"] = AccountName,
                ["name"] = Name,
                ["description"] = Description,
                ["plan"] = Plan,
                ["billingMethodName"] = BillingMethodName,
                ["enableEventBridge"] = EnableEventBridge,
                ["eventBridgeAwsAccountId"] = EventBridgeAwsAccountId,
                ["eventBridgeAwsRegion"] = EventBridgeAwsRegion,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ProjectId != null) {
                writer.WritePropertyName("projectId");
                writer.Write(ProjectId.ToString());
            }
            if (AccountName != null) {
                writer.WritePropertyName("accountName");
                writer.Write(AccountName.ToString());
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Project;
            var diff = 0;
            if (ProjectId == null && ProjectId == other.ProjectId)
            {
                // null and null
            }
            else
            {
                diff += ProjectId.CompareTo(other.ProjectId);
            }
            if (AccountName == null && AccountName == other.AccountName)
            {
                // null and null
            }
            else
            {
                diff += AccountName.CompareTo(other.AccountName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Plan == null && Plan == other.Plan)
            {
                // null and null
            }
            else
            {
                diff += Plan.CompareTo(other.Plan);
            }
            if (BillingMethodName == null && BillingMethodName == other.BillingMethodName)
            {
                // null and null
            }
            else
            {
                diff += BillingMethodName.CompareTo(other.BillingMethodName);
            }
            if (EnableEventBridge == null && EnableEventBridge == other.EnableEventBridge)
            {
                // null and null
            }
            else
            {
                diff += EnableEventBridge.CompareTo(other.EnableEventBridge);
            }
            if (EventBridgeAwsAccountId == null && EventBridgeAwsAccountId == other.EventBridgeAwsAccountId)
            {
                // null and null
            }
            else
            {
                diff += EventBridgeAwsAccountId.CompareTo(other.EventBridgeAwsAccountId);
            }
            if (EventBridgeAwsRegion == null && EventBridgeAwsRegion == other.EventBridgeAwsRegion)
            {
                // null and null
            }
            else
            {
                diff += EventBridgeAwsRegion.CompareTo(other.EventBridgeAwsRegion);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}