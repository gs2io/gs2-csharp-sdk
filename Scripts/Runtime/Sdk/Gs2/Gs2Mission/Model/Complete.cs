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
using UnityEngine.Scripting;

namespace Gs2.Gs2Mission.Model
{
	[Preserve]
	public class Complete : IComparable
	{

        /** 達成状況 */
        public string completeId { set; get; }

        /**
         * 達成状況を設定
         *
         * @param completeId 達成状況
         * @return this
         */
        public Complete WithCompleteId(string completeId) {
            this.completeId = completeId;
            return this;
        }

        /** ユーザーID */
        public string userId { set; get; }

        /**
         * ユーザーIDを設定
         *
         * @param userId ユーザーID
         * @return this
         */
        public Complete WithUserId(string userId) {
            this.userId = userId;
            return this;
        }

        /** ミッショングループ名 */
        public string missionGroupName { set; get; }

        /**
         * ミッショングループ名を設定
         *
         * @param missionGroupName ミッショングループ名
         * @return this
         */
        public Complete WithMissionGroupName(string missionGroupName) {
            this.missionGroupName = missionGroupName;
            return this;
        }

        /** 達成済みのタスク名リスト */
        public List<string> completedMissionTaskNames { set; get; }

        /**
         * 達成済みのタスク名リストを設定
         *
         * @param completedMissionTaskNames 達成済みのタスク名リスト
         * @return this
         */
        public Complete WithCompletedMissionTaskNames(List<string> completedMissionTaskNames) {
            this.completedMissionTaskNames = completedMissionTaskNames;
            return this;
        }

        /** 報酬の受け取り済みのタスク名リスト */
        public List<string> receivedMissionTaskNames { set; get; }

        /**
         * 報酬の受け取り済みのタスク名リストを設定
         *
         * @param receivedMissionTaskNames 報酬の受け取り済みのタスク名リスト
         * @return this
         */
        public Complete WithReceivedMissionTaskNames(List<string> receivedMissionTaskNames) {
            this.receivedMissionTaskNames = receivedMissionTaskNames;
            return this;
        }

        /** 作成日時 */
        public long? createdAt { set; get; }

        /**
         * 作成日時を設定
         *
         * @param createdAt 作成日時
         * @return this
         */
        public Complete WithCreatedAt(long? createdAt) {
            this.createdAt = createdAt;
            return this;
        }

        /** 最終更新日時 */
        public long? updatedAt { set; get; }

        /**
         * 最終更新日時を設定
         *
         * @param updatedAt 最終更新日時
         * @return this
         */
        public Complete WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.completeId != null)
            {
                writer.WritePropertyName("completeId");
                writer.Write(this.completeId);
            }
            if(this.userId != null)
            {
                writer.WritePropertyName("userId");
                writer.Write(this.userId);
            }
            if(this.missionGroupName != null)
            {
                writer.WritePropertyName("missionGroupName");
                writer.Write(this.missionGroupName);
            }
            if(this.completedMissionTaskNames != null)
            {
                writer.WritePropertyName("completedMissionTaskNames");
                writer.WriteArrayStart();
                foreach(var item in this.completedMissionTaskNames)
                {
                    writer.Write(item);
                }
                writer.WriteArrayEnd();
            }
            if(this.receivedMissionTaskNames != null)
            {
                writer.WritePropertyName("receivedMissionTaskNames");
                writer.WriteArrayStart();
                foreach(var item in this.receivedMissionTaskNames)
                {
                    writer.Write(item);
                }
                writer.WriteArrayEnd();
            }
            if(this.createdAt.HasValue)
            {
                writer.WritePropertyName("createdAt");
                writer.Write(this.createdAt.Value);
            }
            if(this.updatedAt.HasValue)
            {
                writer.WritePropertyName("updatedAt");
                writer.Write(this.updatedAt.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetMissionGroupNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):mission:(?<namespaceName>.*):user:(?<userId>.*):group:(?<missionGroupName>.*):complete");
        if (!match.Groups["missionGroupName"].Success)
        {
            return null;
        }
        return match.Groups["missionGroupName"].Value;
    }

    public static string GetUserIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):mission:(?<namespaceName>.*):user:(?<userId>.*):group:(?<missionGroupName>.*):complete");
        if (!match.Groups["userId"].Success)
        {
            return null;
        }
        return match.Groups["userId"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):mission:(?<namespaceName>.*):user:(?<userId>.*):group:(?<missionGroupName>.*):complete");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):mission:(?<namespaceName>.*):user:(?<userId>.*):group:(?<missionGroupName>.*):complete");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):mission:(?<namespaceName>.*):user:(?<userId>.*):group:(?<missionGroupName>.*):complete");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static Complete FromDict(JsonData data)
        {
            return new Complete()
                .WithCompleteId(data.Keys.Contains("completeId") && data["completeId"] != null ? data["completeId"].ToString() : null)
                .WithUserId(data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString() : null)
                .WithMissionGroupName(data.Keys.Contains("missionGroupName") && data["missionGroupName"] != null ? data["missionGroupName"].ToString() : null)
                .WithCompletedMissionTaskNames(data.Keys.Contains("completedMissionTaskNames") && data["completedMissionTaskNames"] != null ? data["completedMissionTaskNames"].Cast<JsonData>().Select(value =>
                    {
                        return value.ToString();
                    }
                ).ToList() : null)
                .WithReceivedMissionTaskNames(data.Keys.Contains("receivedMissionTaskNames") && data["receivedMissionTaskNames"] != null ? data["receivedMissionTaskNames"].Cast<JsonData>().Select(value =>
                    {
                        return value.ToString();
                    }
                ).ToList() : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Complete;
            var diff = 0;
            if (completeId == null && completeId == other.completeId)
            {
                // null and null
            }
            else
            {
                diff += completeId.CompareTo(other.completeId);
            }
            if (userId == null && userId == other.userId)
            {
                // null and null
            }
            else
            {
                diff += userId.CompareTo(other.userId);
            }
            if (missionGroupName == null && missionGroupName == other.missionGroupName)
            {
                // null and null
            }
            else
            {
                diff += missionGroupName.CompareTo(other.missionGroupName);
            }
            if (completedMissionTaskNames == null && completedMissionTaskNames == other.completedMissionTaskNames)
            {
                // null and null
            }
            else
            {
                diff += completedMissionTaskNames.Count - other.completedMissionTaskNames.Count;
                for (var i = 0; i < completedMissionTaskNames.Count; i++)
                {
                    diff += completedMissionTaskNames[i].CompareTo(other.completedMissionTaskNames[i]);
                }
            }
            if (receivedMissionTaskNames == null && receivedMissionTaskNames == other.receivedMissionTaskNames)
            {
                // null and null
            }
            else
            {
                diff += receivedMissionTaskNames.Count - other.receivedMissionTaskNames.Count;
                for (var i = 0; i < receivedMissionTaskNames.Count; i++)
                {
                    diff += receivedMissionTaskNames[i].CompareTo(other.receivedMissionTaskNames[i]);
                }
            }
            if (createdAt == null && createdAt == other.createdAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(createdAt - other.createdAt);
            }
            if (updatedAt == null && updatedAt == other.updatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(updatedAt - other.updatedAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["completeId"] = completeId;
            data["userId"] = userId;
            data["missionGroupName"] = missionGroupName;
            data["completedMissionTaskNames"] = new JsonData(completedMissionTaskNames);
            data["receivedMissionTaskNames"] = new JsonData(receivedMissionTaskNames);
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}