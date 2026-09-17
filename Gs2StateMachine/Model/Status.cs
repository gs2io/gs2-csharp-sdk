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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Status : IComparable
	{
        public string StatusId { set; get; }
        public string UserId { set; get; }
        public string Name { set; get; }
        public long? StateMachineVersion { set; get; }
        public string EnableSpeculativeExecution { set; get; }
        public string StateMachineDefinition { set; get; }
        public Gs2.Gs2StateMachine.Model.RandomStatus RandomStatus { set; get; }
        public Gs2.Gs2StateMachine.Model.StackEntry[] Stacks { set; get; }
        public Gs2.Gs2StateMachine.Model.Variable[] Variables { set; get; }
        public string Value { set; get; }
        public string LastError { set; get; }
        public int? TransitionCount { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public Status WithStatusId(string statusId) {
            this.StatusId = statusId;
            return this;
        }
        public Status WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Status WithName(string name) {
            this.Name = name;
            return this;
        }
        public Status WithStateMachineVersion(long? stateMachineVersion) {
            this.StateMachineVersion = stateMachineVersion;
            return this;
        }
        public Status WithEnableSpeculativeExecution(string enableSpeculativeExecution) {
            this.EnableSpeculativeExecution = enableSpeculativeExecution;
            return this;
        }
        public Status WithStateMachineDefinition(string stateMachineDefinition) {
            this.StateMachineDefinition = stateMachineDefinition;
            return this;
        }
        public Status WithRandomStatus(Gs2.Gs2StateMachine.Model.RandomStatus randomStatus) {
            this.RandomStatus = randomStatus;
            return this;
        }
        public Status WithStacks(Gs2.Gs2StateMachine.Model.StackEntry[] stacks) {
            this.Stacks = stacks;
            return this;
        }
        public Status WithVariables(Gs2.Gs2StateMachine.Model.Variable[] variables) {
            this.Variables = variables;
            return this;
        }
        public Status WithValue(string value) {
            this.Value = value;
            return this;
        }
        public Status WithLastError(string lastError) {
            this.LastError = lastError;
            return this;
        }
        public Status WithTransitionCount(int? transitionCount) {
            this.TransitionCount = transitionCount;
            return this;
        }
        public Status WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Status WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):user:(?<userId>.+):status:(?<statusName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):user:(?<userId>.+):status:(?<statusName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):user:(?<userId>.+):status:(?<statusName>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):user:(?<userId>.+):status:(?<statusName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _statusNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):user:(?<userId>.+):status:(?<statusName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetStatusNameFromGrn(
            string grn
        )
        {
            var match = _statusNameRegex.Match(grn);
            if (!match.Success || !match.Groups["statusName"].Success)
            {
                return null;
            }
            return match.Groups["statusName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Status FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Status()
                .WithStatusId(!data.Keys.Contains("statusId") || data["statusId"] == null ? null : data["statusId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithStateMachineVersion(!data.Keys.Contains("stateMachineVersion") || data["stateMachineVersion"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["stateMachineVersion"].ToString()))
                .WithEnableSpeculativeExecution(!data.Keys.Contains("enableSpeculativeExecution") || data["enableSpeculativeExecution"] == null ? null : data["enableSpeculativeExecution"].ToString())
                .WithStateMachineDefinition(!data.Keys.Contains("stateMachineDefinition") || data["stateMachineDefinition"] == null ? null : data["stateMachineDefinition"].ToString())
                .WithRandomStatus(!data.Keys.Contains("randomStatus") || data["randomStatus"] == null ? null : Gs2.Gs2StateMachine.Model.RandomStatus.FromJson(data["randomStatus"]))
                .WithStacks(!data.Keys.Contains("stacks") || data["stacks"] == null || !data["stacks"].IsArray ? null : data["stacks"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2StateMachine.Model.StackEntry.FromJson(v);
                }).ToArray())
                .WithVariables(!data.Keys.Contains("variables") || data["variables"] == null || !data["variables"].IsArray ? null : data["variables"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2StateMachine.Model.Variable.FromJson(v);
                }).ToArray())
                .WithValue(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithLastError(!data.Keys.Contains("lastError") || data["lastError"] == null ? null : data["lastError"].ToString())
                .WithTransitionCount(!data.Keys.Contains("transitionCount") || data["transitionCount"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["transitionCount"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData stacksJsonData = null;
            if (Stacks != null && Stacks.Length > 0)
            {
                stacksJsonData = new JsonData();
                foreach (var stack in Stacks)
                {
                    stacksJsonData.Add(stack.ToJson());
                }
            }
            JsonData variablesJsonData = null;
            if (Variables != null && Variables.Length > 0)
            {
                variablesJsonData = new JsonData();
                foreach (var variable in Variables)
                {
                    variablesJsonData.Add(variable.ToJson());
                }
            }
            return new JsonData {
                ["statusId"] = StatusId,
                ["userId"] = UserId,
                ["name"] = Name,
                ["stateMachineVersion"] = StateMachineVersion,
                ["enableSpeculativeExecution"] = EnableSpeculativeExecution,
                ["stateMachineDefinition"] = StateMachineDefinition,
                ["randomStatus"] = RandomStatus?.ToJson(),
                ["stacks"] = stacksJsonData,
                ["variables"] = variablesJsonData,
                ["status"] = Value,
                ["lastError"] = LastError,
                ["transitionCount"] = TransitionCount,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StatusId != null) {
                writer.WritePropertyName("statusId");
                writer.Write(StatusId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (StateMachineVersion != null) {
                writer.WritePropertyName("stateMachineVersion");
                writer.Write((StateMachineVersion.ToString().Contains(".") ? (long)double.Parse(StateMachineVersion.ToString()) : long.Parse(StateMachineVersion.ToString())));
            }
            if (EnableSpeculativeExecution != null) {
                writer.WritePropertyName("enableSpeculativeExecution");
                writer.Write(EnableSpeculativeExecution.ToString());
            }
            if (StateMachineDefinition != null) {
                writer.WritePropertyName("stateMachineDefinition");
                writer.Write(StateMachineDefinition.ToString());
            }
            if (RandomStatus != null) {
                writer.WritePropertyName("randomStatus");
                RandomStatus.WriteJson(writer);
            }
            if (Stacks != null) {
                writer.WritePropertyName("stacks");
                writer.WriteArrayStart();
                foreach (var stack in Stacks)
                {
                    if (stack != null) {
                        stack.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Variables != null) {
                writer.WritePropertyName("variables");
                writer.WriteArrayStart();
                foreach (var variable in Variables)
                {
                    if (variable != null) {
                        variable.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(Value.ToString());
            }
            if (LastError != null) {
                writer.WritePropertyName("lastError");
                writer.Write(LastError.ToString());
            }
            if (TransitionCount != null) {
                writer.WritePropertyName("transitionCount");
                writer.Write((TransitionCount.ToString().Contains(".") ? (int)double.Parse(TransitionCount.ToString()) : int.Parse(TransitionCount.ToString())));
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
            var other = obj as Status;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Status.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(StatusId, other.StatusId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(StateMachineVersion, other.StateMachineVersion);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableSpeculativeExecution, other.EnableSpeculativeExecution);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(StateMachineDefinition, other.StateMachineDefinition);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RandomStatus, other.RandomStatus);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Stacks, other.Stacks);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Variables, other.Variables);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Value, other.Value);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(LastError, other.LastError);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TransitionCount, other.TransitionCount);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (StatusId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.statusId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.name.error.tooLong"),
                    });
                }
            }
            {
                if (StateMachineVersion < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.stateMachineVersion.error.invalid"),
                    });
                }
                if (StateMachineVersion > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.stateMachineVersion.error.invalid"),
                    });
                }
            }
            {
                switch (EnableSpeculativeExecution) {
                    case "enable":
                    case "disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("status", "stateMachine.status.enableSpeculativeExecution.error.invalid"),
                        });
                }
            }
            {
                if (StateMachineDefinition.Length > 16777216) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.stateMachineDefinition.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (Stacks.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.stacks.error.tooMany"),
                    });
                }
            }
            {
                if (Variables.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.variables.error.tooMany"),
                    });
                }
            }
            {
                switch (Value) {
                    case "Running":
                    case "Wait":
                    case "Pass":
                    case "Error":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("status", "stateMachine.status.status.error.invalid"),
                        });
                }
            }
            {
                if (LastError.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.lastError.error.tooLong"),
                    });
                }
            }
            {
                if (TransitionCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.transitionCount.error.invalid"),
                    });
                }
                if (TransitionCount > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.transitionCount.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("status", "stateMachine.status.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Status {
                StatusId = StatusId,
                UserId = UserId,
                Name = Name,
                StateMachineVersion = StateMachineVersion,
                EnableSpeculativeExecution = EnableSpeculativeExecution,
                StateMachineDefinition = StateMachineDefinition,
                RandomStatus = RandomStatus?.Clone() as Gs2.Gs2StateMachine.Model.RandomStatus,
                Stacks = Stacks?.Clone() as Gs2.Gs2StateMachine.Model.StackEntry[],
                Variables = Variables?.Clone() as Gs2.Gs2StateMachine.Model.Variable[],
                Value = Value,
                LastError = LastError,
                TransitionCount = TransitionCount,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}