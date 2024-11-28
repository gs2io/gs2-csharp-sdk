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
using Gs2.Gs2Idle.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Idle.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateCategoryModelMasterRequest : Gs2Request<CreateCategoryModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public int? RewardIntervalMinutes { set; get; } = null!;
         public int? DefaultMaximumIdleMinutes { set; get; } = null!;
         public Gs2.Gs2Idle.Model.AcquireActionList[] AcquireActions { set; get; } = null!;
         public string IdlePeriodScheduleId { set; get; } = null!;
         public string ReceivePeriodScheduleId { set; get; } = null!;
        public CreateCategoryModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateCategoryModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateCategoryModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateCategoryModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateCategoryModelMasterRequest WithRewardIntervalMinutes(int? rewardIntervalMinutes) {
            this.RewardIntervalMinutes = rewardIntervalMinutes;
            return this;
        }
        public CreateCategoryModelMasterRequest WithDefaultMaximumIdleMinutes(int? defaultMaximumIdleMinutes) {
            this.DefaultMaximumIdleMinutes = defaultMaximumIdleMinutes;
            return this;
        }
        public CreateCategoryModelMasterRequest WithAcquireActions(Gs2.Gs2Idle.Model.AcquireActionList[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }
        public CreateCategoryModelMasterRequest WithIdlePeriodScheduleId(string idlePeriodScheduleId) {
            this.IdlePeriodScheduleId = idlePeriodScheduleId;
            return this;
        }
        public CreateCategoryModelMasterRequest WithReceivePeriodScheduleId(string receivePeriodScheduleId) {
            this.ReceivePeriodScheduleId = receivePeriodScheduleId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateCategoryModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateCategoryModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithRewardIntervalMinutes(!data.Keys.Contains("rewardIntervalMinutes") || data["rewardIntervalMinutes"] == null ? null : (int?)(data["rewardIntervalMinutes"].ToString().Contains(".") ? (int)double.Parse(data["rewardIntervalMinutes"].ToString()) : int.Parse(data["rewardIntervalMinutes"].ToString())))
                .WithDefaultMaximumIdleMinutes(!data.Keys.Contains("defaultMaximumIdleMinutes") || data["defaultMaximumIdleMinutes"] == null ? null : (int?)(data["defaultMaximumIdleMinutes"].ToString().Contains(".") ? (int)double.Parse(data["defaultMaximumIdleMinutes"].ToString()) : int.Parse(data["defaultMaximumIdleMinutes"].ToString())))
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Idle.Model.AcquireActionList.FromJson(v);
                }).ToArray())
                .WithIdlePeriodScheduleId(!data.Keys.Contains("idlePeriodScheduleId") || data["idlePeriodScheduleId"] == null ? null : data["idlePeriodScheduleId"].ToString())
                .WithReceivePeriodScheduleId(!data.Keys.Contains("receivePeriodScheduleId") || data["receivePeriodScheduleId"] == null ? null : data["receivePeriodScheduleId"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["rewardIntervalMinutes"] = RewardIntervalMinutes,
                ["defaultMaximumIdleMinutes"] = DefaultMaximumIdleMinutes,
                ["acquireActions"] = acquireActionsJsonData,
                ["idlePeriodScheduleId"] = IdlePeriodScheduleId,
                ["receivePeriodScheduleId"] = ReceivePeriodScheduleId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (RewardIntervalMinutes != null) {
                writer.WritePropertyName("rewardIntervalMinutes");
                writer.Write((RewardIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(RewardIntervalMinutes.ToString()) : int.Parse(RewardIntervalMinutes.ToString())));
            }
            if (DefaultMaximumIdleMinutes != null) {
                writer.WritePropertyName("defaultMaximumIdleMinutes");
                writer.Write((DefaultMaximumIdleMinutes.ToString().Contains(".") ? (int)double.Parse(DefaultMaximumIdleMinutes.ToString()) : int.Parse(DefaultMaximumIdleMinutes.ToString())));
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (IdlePeriodScheduleId != null) {
                writer.WritePropertyName("idlePeriodScheduleId");
                writer.Write(IdlePeriodScheduleId.ToString());
            }
            if (ReceivePeriodScheduleId != null) {
                writer.WritePropertyName("receivePeriodScheduleId");
                writer.Write(ReceivePeriodScheduleId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += RewardIntervalMinutes + ":";
            key += DefaultMaximumIdleMinutes + ":";
            key += AcquireActions + ":";
            key += IdlePeriodScheduleId + ":";
            key += ReceivePeriodScheduleId + ":";
            return key;
        }
    }
}