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

namespace Gs2.Gs2Mission.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Complete : IComparable
	{
        public string CompleteId { set; get; }
        public string UserId { set; get; }
        public string MissionGroupName { set; get; }
        public string[] CompletedMissionTaskNames { set; get; }
        public string[] ReceivedMissionTaskNames { set; get; }
        public long? NextResetAt { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Complete WithCompleteId(string completeId) {
            this.CompleteId = completeId;
            return this;
        }

        public Complete WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Complete WithMissionGroupName(string missionGroupName) {
            this.MissionGroupName = missionGroupName;
            return this;
        }

        public Complete WithCompletedMissionTaskNames(string[] completedMissionTaskNames) {
            this.CompletedMissionTaskNames = completedMissionTaskNames;
            return this;
        }

        public Complete WithReceivedMissionTaskNames(string[] receivedMissionTaskNames) {
            this.ReceivedMissionTaskNames = receivedMissionTaskNames;
            return this;
        }

        public Complete WithNextResetAt(long? nextResetAt) {
            this.NextResetAt = nextResetAt;
            return this;
        }

        public Complete WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Complete WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):user:(?<userId>.+):group:(?<missionGroupName>.+):complete",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):user:(?<userId>.+):group:(?<missionGroupName>.+):complete",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):user:(?<userId>.+):group:(?<missionGroupName>.+):complete",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):user:(?<userId>.+):group:(?<missionGroupName>.+):complete",
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

        private static System.Text.RegularExpressions.Regex _missionGroupNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):user:(?<userId>.+):group:(?<missionGroupName>.+):complete",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMissionGroupNameFromGrn(
            string grn
        )
        {
            var match = _missionGroupNameRegex.Match(grn);
            if (!match.Success || !match.Groups["missionGroupName"].Success)
            {
                return null;
            }
            return match.Groups["missionGroupName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Complete FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Complete()
                .WithCompleteId(!data.Keys.Contains("completeId") || data["completeId"] == null ? null : data["completeId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMissionGroupName(!data.Keys.Contains("missionGroupName") || data["missionGroupName"] == null ? null : data["missionGroupName"].ToString())
                .WithCompletedMissionTaskNames(!data.Keys.Contains("completedMissionTaskNames") || data["completedMissionTaskNames"] == null ? new string[]{} : data["completedMissionTaskNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithReceivedMissionTaskNames(!data.Keys.Contains("receivedMissionTaskNames") || data["receivedMissionTaskNames"] == null ? new string[]{} : data["receivedMissionTaskNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithNextResetAt(!data.Keys.Contains("nextResetAt") || data["nextResetAt"] == null ? null : (long?)long.Parse(data["nextResetAt"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["completeId"] = CompleteId,
                ["userId"] = UserId,
                ["missionGroupName"] = MissionGroupName,
                ["completedMissionTaskNames"] = new JsonData(CompletedMissionTaskNames == null ? new JsonData[]{} :
                        CompletedMissionTaskNames.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["receivedMissionTaskNames"] = new JsonData(ReceivedMissionTaskNames == null ? new JsonData[]{} :
                        ReceivedMissionTaskNames.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["nextResetAt"] = NextResetAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CompleteId != null) {
                writer.WritePropertyName("completeId");
                writer.Write(CompleteId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (MissionGroupName != null) {
                writer.WritePropertyName("missionGroupName");
                writer.Write(MissionGroupName.ToString());
            }
            if (CompletedMissionTaskNames != null) {
                writer.WritePropertyName("completedMissionTaskNames");
                writer.WriteArrayStart();
                foreach (var completedMissionTaskName in CompletedMissionTaskNames)
                {
                    if (completedMissionTaskName != null) {
                        writer.Write(completedMissionTaskName.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ReceivedMissionTaskNames != null) {
                writer.WritePropertyName("receivedMissionTaskNames");
                writer.WriteArrayStart();
                foreach (var receivedMissionTaskName in ReceivedMissionTaskNames)
                {
                    if (receivedMissionTaskName != null) {
                        writer.Write(receivedMissionTaskName.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (NextResetAt != null) {
                writer.WritePropertyName("nextResetAt");
                writer.Write(long.Parse(NextResetAt.ToString()));
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
            var other = obj as Complete;
            var diff = 0;
            if (CompleteId == null && CompleteId == other.CompleteId)
            {
                // null and null
            }
            else
            {
                diff += CompleteId.CompareTo(other.CompleteId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (MissionGroupName == null && MissionGroupName == other.MissionGroupName)
            {
                // null and null
            }
            else
            {
                diff += MissionGroupName.CompareTo(other.MissionGroupName);
            }
            if (CompletedMissionTaskNames == null && CompletedMissionTaskNames == other.CompletedMissionTaskNames)
            {
                // null and null
            }
            else
            {
                diff += CompletedMissionTaskNames.Length - other.CompletedMissionTaskNames.Length;
                for (var i = 0; i < CompletedMissionTaskNames.Length; i++)
                {
                    diff += CompletedMissionTaskNames[i].CompareTo(other.CompletedMissionTaskNames[i]);
                }
            }
            if (ReceivedMissionTaskNames == null && ReceivedMissionTaskNames == other.ReceivedMissionTaskNames)
            {
                // null and null
            }
            else
            {
                diff += ReceivedMissionTaskNames.Length - other.ReceivedMissionTaskNames.Length;
                for (var i = 0; i < ReceivedMissionTaskNames.Length; i++)
                {
                    diff += ReceivedMissionTaskNames[i].CompareTo(other.ReceivedMissionTaskNames[i]);
                }
            }
            if (NextResetAt == null && NextResetAt == other.NextResetAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(NextResetAt - other.NextResetAt);
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