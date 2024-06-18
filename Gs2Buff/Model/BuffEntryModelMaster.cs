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

namespace Gs2.Gs2Buff.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BuffEntryModelMaster : IComparable
	{
        public string BuffEntryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Expression { set; get; } = null!;
        public string TargetType { set; get; } = null!;
        public Gs2.Gs2Buff.Model.BuffTargetModel TargetModel { set; get; } = null!;
        public Gs2.Gs2Buff.Model.BuffTargetAction TargetAction { set; get; } = null!;
        public int? Priority { set; get; } = null!;
        public string ApplyPeriodScheduleEventId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public BuffEntryModelMaster WithBuffEntryModelId(string buffEntryModelId) {
            this.BuffEntryModelId = buffEntryModelId;
            return this;
        }
        public BuffEntryModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public BuffEntryModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public BuffEntryModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public BuffEntryModelMaster WithExpression(string expression) {
            this.Expression = expression;
            return this;
        }
        public BuffEntryModelMaster WithTargetType(string targetType) {
            this.TargetType = targetType;
            return this;
        }
        public BuffEntryModelMaster WithTargetModel(Gs2.Gs2Buff.Model.BuffTargetModel targetModel) {
            this.TargetModel = targetModel;
            return this;
        }
        public BuffEntryModelMaster WithTargetAction(Gs2.Gs2Buff.Model.BuffTargetAction targetAction) {
            this.TargetAction = targetAction;
            return this;
        }
        public BuffEntryModelMaster WithPriority(int? priority) {
            this.Priority = priority;
            return this;
        }
        public BuffEntryModelMaster WithApplyPeriodScheduleEventId(string applyPeriodScheduleEventId) {
            this.ApplyPeriodScheduleEventId = applyPeriodScheduleEventId;
            return this;
        }
        public BuffEntryModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public BuffEntryModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public BuffEntryModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):buff:(?<namespaceName>.+):model:(?<buffEntryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):buff:(?<namespaceName>.+):model:(?<buffEntryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):buff:(?<namespaceName>.+):model:(?<buffEntryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _buffEntryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):buff:(?<namespaceName>.+):model:(?<buffEntryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetBuffEntryNameFromGrn(
            string grn
        )
        {
            var match = _buffEntryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["buffEntryName"].Success)
            {
                return null;
            }
            return match.Groups["buffEntryName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BuffEntryModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BuffEntryModelMaster()
                .WithBuffEntryModelId(!data.Keys.Contains("buffEntryModelId") || data["buffEntryModelId"] == null ? null : data["buffEntryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithExpression(!data.Keys.Contains("expression") || data["expression"] == null ? null : data["expression"].ToString())
                .WithTargetType(!data.Keys.Contains("targetType") || data["targetType"] == null ? null : data["targetType"].ToString())
                .WithTargetModel(!data.Keys.Contains("targetModel") || data["targetModel"] == null ? null : Gs2.Gs2Buff.Model.BuffTargetModel.FromJson(data["targetModel"]))
                .WithTargetAction(!data.Keys.Contains("targetAction") || data["targetAction"] == null ? null : Gs2.Gs2Buff.Model.BuffTargetAction.FromJson(data["targetAction"]))
                .WithPriority(!data.Keys.Contains("priority") || data["priority"] == null ? null : (int?)(data["priority"].ToString().Contains(".") ? (int)double.Parse(data["priority"].ToString()) : int.Parse(data["priority"].ToString())))
                .WithApplyPeriodScheduleEventId(!data.Keys.Contains("applyPeriodScheduleEventId") || data["applyPeriodScheduleEventId"] == null ? null : data["applyPeriodScheduleEventId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["buffEntryModelId"] = BuffEntryModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["expression"] = Expression,
                ["targetType"] = TargetType,
                ["targetModel"] = TargetModel?.ToJson(),
                ["targetAction"] = TargetAction?.ToJson(),
                ["priority"] = Priority,
                ["applyPeriodScheduleEventId"] = ApplyPeriodScheduleEventId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BuffEntryModelId != null) {
                writer.WritePropertyName("buffEntryModelId");
                writer.Write(BuffEntryModelId.ToString());
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
                writer.WritePropertyName("targetModel");
                TargetModel.WriteJson(writer);
            }
            if (TargetAction != null) {
                writer.WritePropertyName("targetAction");
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BuffEntryModelMaster;
            var diff = 0;
            if (BuffEntryModelId == null && BuffEntryModelId == other.BuffEntryModelId)
            {
                // null and null
            }
            else
            {
                diff += BuffEntryModelId.CompareTo(other.BuffEntryModelId);
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
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Expression == null && Expression == other.Expression)
            {
                // null and null
            }
            else
            {
                diff += Expression.CompareTo(other.Expression);
            }
            if (TargetType == null && TargetType == other.TargetType)
            {
                // null and null
            }
            else
            {
                diff += TargetType.CompareTo(other.TargetType);
            }
            if (TargetModel == null && TargetModel == other.TargetModel)
            {
                // null and null
            }
            else
            {
                diff += TargetModel.CompareTo(other.TargetModel);
            }
            if (TargetAction == null && TargetAction == other.TargetAction)
            {
                // null and null
            }
            else
            {
                diff += TargetAction.CompareTo(other.TargetAction);
            }
            if (Priority == null && Priority == other.Priority)
            {
                // null and null
            }
            else
            {
                diff += (int)(Priority - other.Priority);
            }
            if (ApplyPeriodScheduleEventId == null && ApplyPeriodScheduleEventId == other.ApplyPeriodScheduleEventId)
            {
                // null and null
            }
            else
            {
                diff += ApplyPeriodScheduleEventId.CompareTo(other.ApplyPeriodScheduleEventId);
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (BuffEntryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.buffEntryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                switch (Expression) {
                    case "rate_add":
                    case "mul":
                    case "value_add":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.expression.error.invalid"),
                        });
                }
            }
            {
                switch (TargetType) {
                    case "model":
                    case "action":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.targetType.error.invalid"),
                        });
                }
            }
            if (TargetType == "model") {
            }
            if (TargetType == "action") {
            }
            {
                if (Priority < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.priority.error.invalid"),
                    });
                }
                if (Priority > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.priority.error.invalid"),
                    });
                }
            }
            {
                if (ApplyPeriodScheduleEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.applyPeriodScheduleEventId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffEntryModelMaster", "buff.buffEntryModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BuffEntryModelMaster {
                BuffEntryModelId = BuffEntryModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                Expression = Expression,
                TargetType = TargetType,
                TargetModel = TargetModel.Clone() as Gs2.Gs2Buff.Model.BuffTargetModel,
                TargetAction = TargetAction.Clone() as Gs2.Gs2Buff.Model.BuffTargetAction,
                Priority = Priority,
                ApplyPeriodScheduleEventId = ApplyPeriodScheduleEventId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}