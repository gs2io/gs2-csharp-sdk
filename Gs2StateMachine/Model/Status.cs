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

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Status : IComparable
	{
        public string StatusId { set; get; }
        public string UserId { set; get; }
        public string Name { set; get; }
        public long? StateMachineVersion { set; get; }
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
                .WithStateMachineVersion(!data.Keys.Contains("stateMachineVersion") || data["stateMachineVersion"] == null ? null : (long?)long.Parse(data["stateMachineVersion"].ToString()))
                .WithStacks(!data.Keys.Contains("stacks") || data["stacks"] == null ? new Gs2.Gs2StateMachine.Model.StackEntry[]{} : data["stacks"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2StateMachine.Model.StackEntry.FromJson(v);
                }).ToArray())
                .WithVariables(!data.Keys.Contains("variables") || data["variables"] == null ? new Gs2.Gs2StateMachine.Model.Variable[]{} : data["variables"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2StateMachine.Model.Variable.FromJson(v);
                }).ToArray())
                .WithValue(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithLastError(!data.Keys.Contains("lastError") || data["lastError"] == null ? null : data["lastError"].ToString())
                .WithTransitionCount(!data.Keys.Contains("transitionCount") || data["transitionCount"] == null ? null : (int?)int.Parse(data["transitionCount"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData stacksJsonData = null;
            if (Stacks != null)
            {
                stacksJsonData = new JsonData();
                foreach (var stack in Stacks)
                {
                    stacksJsonData.Add(stack.ToJson());
                }
            }
            JsonData variablesJsonData = null;
            if (Variables != null)
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
                writer.Write(long.Parse(StateMachineVersion.ToString()));
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
                writer.Write(int.Parse(TransitionCount.ToString()));
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
            var other = obj as Status;
            var diff = 0;
            if (StatusId == null && StatusId == other.StatusId)
            {
                // null and null
            }
            else
            {
                diff += StatusId.CompareTo(other.StatusId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (StateMachineVersion == null && StateMachineVersion == other.StateMachineVersion)
            {
                // null and null
            }
            else
            {
                diff += (int)(StateMachineVersion - other.StateMachineVersion);
            }
            if (Stacks == null && Stacks == other.Stacks)
            {
                // null and null
            }
            else
            {
                diff += Stacks.Length - other.Stacks.Length;
                for (var i = 0; i < Stacks.Length; i++)
                {
                    diff += Stacks[i].CompareTo(other.Stacks[i]);
                }
            }
            if (Variables == null && Variables == other.Variables)
            {
                // null and null
            }
            else
            {
                diff += Variables.Length - other.Variables.Length;
                for (var i = 0; i < Variables.Length; i++)
                {
                    diff += Variables[i].CompareTo(other.Variables[i]);
                }
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += Value.CompareTo(other.Value);
            }
            if (LastError == null && LastError == other.LastError)
            {
                // null and null
            }
            else
            {
                diff += LastError.CompareTo(other.LastError);
            }
            if (TransitionCount == null && TransitionCount == other.TransitionCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(TransitionCount - other.TransitionCount);
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