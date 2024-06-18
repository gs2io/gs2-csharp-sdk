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
using Gs2.Gs2Buff.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Buff.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateBuffEntryModelMasterRequest : Gs2Request<CreateBuffEntryModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string Expression { set; get; } = null!;
         public string TargetType { set; get; } = null!;
         public Gs2.Gs2Buff.Model.BuffTargetModel TargetModel { set; get; } = null!;
         public Gs2.Gs2Buff.Model.BuffTargetAction TargetAction { set; get; } = null!;
         public int? Priority { set; get; } = null!;
         public string ApplyPeriodScheduleEventId { set; get; } = null!;
        public CreateBuffEntryModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithExpression(string expression) {
            this.Expression = expression;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithTargetType(string targetType) {
            this.TargetType = targetType;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithTargetModel(Gs2.Gs2Buff.Model.BuffTargetModel targetModel) {
            this.TargetModel = targetModel;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithTargetAction(Gs2.Gs2Buff.Model.BuffTargetAction targetAction) {
            this.TargetAction = targetAction;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithPriority(int? priority) {
            this.Priority = priority;
            return this;
        }
        public CreateBuffEntryModelMasterRequest WithApplyPeriodScheduleEventId(string applyPeriodScheduleEventId) {
            this.ApplyPeriodScheduleEventId = applyPeriodScheduleEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateBuffEntryModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateBuffEntryModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithExpression(!data.Keys.Contains("expression") || data["expression"] == null ? null : data["expression"].ToString())
                .WithTargetType(!data.Keys.Contains("targetType") || data["targetType"] == null ? null : data["targetType"].ToString())
                .WithTargetModel(!data.Keys.Contains("targetModel") || data["targetModel"] == null ? null : Gs2.Gs2Buff.Model.BuffTargetModel.FromJson(data["targetModel"]))
                .WithTargetAction(!data.Keys.Contains("targetAction") || data["targetAction"] == null ? null : Gs2.Gs2Buff.Model.BuffTargetAction.FromJson(data["targetAction"]))
                .WithPriority(!data.Keys.Contains("priority") || data["priority"] == null ? null : (int?)(data["priority"].ToString().Contains(".") ? (int)double.Parse(data["priority"].ToString()) : int.Parse(data["priority"].ToString())))
                .WithApplyPeriodScheduleEventId(!data.Keys.Contains("applyPeriodScheduleEventId") || data["applyPeriodScheduleEventId"] == null ? null : data["applyPeriodScheduleEventId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["expression"] = Expression,
                ["targetType"] = TargetType,
                ["targetModel"] = TargetModel?.ToJson(),
                ["targetAction"] = TargetAction?.ToJson(),
                ["priority"] = Priority,
                ["applyPeriodScheduleEventId"] = ApplyPeriodScheduleEventId,
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
            if (Expression != null) {
                writer.WritePropertyName("expression");
                writer.Write(Expression.ToString());
            }
            if (TargetType != null) {
                writer.WritePropertyName("targetType");
                writer.Write(TargetType.ToString());
            }
            if (TargetModel != null) {
                TargetModel.WriteJson(writer);
            }
            if (TargetAction != null) {
                TargetAction.WriteJson(writer);
            }
            if (Priority != null) {
                writer.WritePropertyName("priority");
                writer.Write((Priority.ToString().Contains(".") ? (int)double.Parse(Priority.ToString()) : int.Parse(Priority.ToString())));
            }
            if (ApplyPeriodScheduleEventId != null) {
                writer.WritePropertyName("applyPeriodScheduleEventId");
                writer.Write(ApplyPeriodScheduleEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Expression + ":";
            key += TargetType + ":";
            key += TargetModel + ":";
            key += TargetAction + ":";
            key += Priority + ":";
            key += ApplyPeriodScheduleEventId + ":";
            return key;
        }
    }
}