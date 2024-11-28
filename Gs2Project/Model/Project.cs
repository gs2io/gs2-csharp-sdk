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
        public string ProjectId { set; get; } = null!;
        public string AccountName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Plan { set; get; } = null!;
        public Gs2.Gs2Project.Model.Gs2Region[] Regions { set; get; } = null!;
        public string BillingMethodName { set; get; } = null!;
        public string EnableEventBridge { set; get; } = null!;
        public string Currency { set; get; } = null!;
        public string EventBridgeAwsAccountId { set; get; } = null!;
        public string EventBridgeAwsRegion { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
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
        public Project WithRegions(Gs2.Gs2Project.Model.Gs2Region[] regions) {
            this.Regions = regions;
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
        public Project WithCurrency(string currency) {
            this.Currency = currency;
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

        private static System.Text.RegularExpressions.Regex _accountNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAccountNameFromGrn(
            string grn
        )
        {
            var match = _accountNameRegex.Match(grn);
            if (!match.Success || !match.Groups["accountName"].Success)
            {
                return null;
            }
            return match.Groups["accountName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _projectNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetProjectNameFromGrn(
            string grn
        )
        {
            var match = _projectNameRegex.Match(grn);
            if (!match.Success || !match.Groups["projectName"].Success)
            {
                return null;
            }
            return match.Groups["projectName"].Value;
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
                .WithRegions(!data.Keys.Contains("regions") || data["regions"] == null || !data["regions"].IsArray ? null : data["regions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Project.Model.Gs2Region.FromJson(v);
                }).ToArray())
                .WithBillingMethodName(!data.Keys.Contains("billingMethodName") || data["billingMethodName"] == null ? null : data["billingMethodName"].ToString())
                .WithEnableEventBridge(!data.Keys.Contains("enableEventBridge") || data["enableEventBridge"] == null ? null : data["enableEventBridge"].ToString())
                .WithCurrency(!data.Keys.Contains("currency") || data["currency"] == null ? null : data["currency"].ToString())
                .WithEventBridgeAwsAccountId(!data.Keys.Contains("eventBridgeAwsAccountId") || data["eventBridgeAwsAccountId"] == null ? null : data["eventBridgeAwsAccountId"].ToString())
                .WithEventBridgeAwsRegion(!data.Keys.Contains("eventBridgeAwsRegion") || data["eventBridgeAwsRegion"] == null ? null : data["eventBridgeAwsRegion"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData regionsJsonData = null;
            if (Regions != null && Regions.Length > 0)
            {
                regionsJsonData = new JsonData();
                foreach (var region in Regions)
                {
                    regionsJsonData.Add(region.ToJson());
                }
            }
            return new JsonData {
                ["projectId"] = ProjectId,
                ["accountName"] = AccountName,
                ["name"] = Name,
                ["description"] = Description,
                ["plan"] = Plan,
                ["regions"] = regionsJsonData,
                ["billingMethodName"] = BillingMethodName,
                ["enableEventBridge"] = EnableEventBridge,
                ["currency"] = Currency,
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
            if (Regions != null) {
                writer.WritePropertyName("regions");
                writer.WriteArrayStart();
                foreach (var region in Regions)
                {
                    if (region != null) {
                        region.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (BillingMethodName != null) {
                writer.WritePropertyName("billingMethodName");
                writer.Write(BillingMethodName.ToString());
            }
            if (EnableEventBridge != null) {
                writer.WritePropertyName("enableEventBridge");
                writer.Write(EnableEventBridge.ToString());
            }
            if (Currency != null) {
                writer.WritePropertyName("currency");
                writer.Write(Currency.ToString());
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
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
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
            if (Regions == null && Regions == other.Regions)
            {
                // null and null
            }
            else
            {
                diff += Regions.Length - other.Regions.Length;
                for (var i = 0; i < Regions.Length; i++)
                {
                    diff += Regions[i].CompareTo(other.Regions[i]);
                }
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
            if (Currency == null && Currency == other.Currency)
            {
                // null and null
            }
            else
            {
                diff += Currency.CompareTo(other.Currency);
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

        public void Validate() {
            {
                if (ProjectId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.projectId.error.tooLong"),
                    });
                }
            }
            {
                if (AccountName.Length < 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.accountName.error.tooShort"),
                    });
                }
                if (AccountName.Length > 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.accountName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.description.error.tooLong"),
                    });
                }
            }
            {
                switch (Plan) {
                    case "free":
                    case "unlimited":
                    case "fixed":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("project", "project.project.plan.error.invalid"),
                        });
                }
            }
            {
                if (Regions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.regions.error.tooMany"),
                    });
                }
            }
            if ((Plan =="fixed" || Plan == "unlimited")) {
                if (BillingMethodName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.billingMethodName.error.tooLong"),
                    });
                }
            }
            {
                switch (EnableEventBridge) {
                    case "enable":
                    case "disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("project", "project.project.enableEventBridge.error.invalid"),
                        });
                }
            }
            {
                switch (Currency) {
                    case "USD":
                    case "JPY":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("project", "project.project.currency.error.invalid"),
                        });
                }
            }
            if (EnableEventBridge == "enable") {
                if (EventBridgeAwsAccountId.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.eventBridgeAwsAccountId.error.tooLong"),
                    });
                }
            }
            if (EnableEventBridge == "enable") {
                switch (EventBridgeAwsRegion) {
                    case "us-east-1":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("project", "project.project.eventBridgeAwsRegion.error.invalid"),
                        });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("project", "project.project.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Project {
                ProjectId = ProjectId,
                AccountName = AccountName,
                Name = Name,
                Description = Description,
                Plan = Plan,
                Regions = Regions.Clone() as Gs2.Gs2Project.Model.Gs2Region[],
                BillingMethodName = BillingMethodName,
                EnableEventBridge = EnableEventBridge,
                Currency = Currency,
                EventBridgeAwsAccountId = EventBridgeAwsAccountId,
                EventBridgeAwsRegion = EventBridgeAwsRegion,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}