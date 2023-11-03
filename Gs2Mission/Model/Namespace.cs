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
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2Mission.Model.TransactionSetting TransactionSetting { set; get; }
        public Gs2.Gs2Mission.Model.ScriptSetting MissionCompleteScript { set; get; }
        public Gs2.Gs2Mission.Model.ScriptSetting CounterIncrementScript { set; get; }
        public Gs2.Gs2Mission.Model.ScriptSetting ReceiveRewardsScript { set; get; }
        public Gs2.Gs2Mission.Model.NotificationSetting CompleteNotification { set; get; }
        public Gs2.Gs2Mission.Model.LogSetting LogSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }

        public Namespace WithNamespaceId(string namespaceId) {
            this.NamespaceId = namespaceId;
            return this;
        }

        public Namespace WithName(string name) {
            this.Name = name;
            return this;
        }

        public Namespace WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public Namespace WithTransactionSetting(Gs2.Gs2Mission.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }

        public Namespace WithMissionCompleteScript(Gs2.Gs2Mission.Model.ScriptSetting missionCompleteScript) {
            this.MissionCompleteScript = missionCompleteScript;
            return this;
        }

        public Namespace WithCounterIncrementScript(Gs2.Gs2Mission.Model.ScriptSetting counterIncrementScript) {
            this.CounterIncrementScript = counterIncrementScript;
            return this;
        }

        public Namespace WithReceiveRewardsScript(Gs2.Gs2Mission.Model.ScriptSetting receiveRewardsScript) {
            this.ReceiveRewardsScript = receiveRewardsScript;
            return this;
        }

        public Namespace WithCompleteNotification(Gs2.Gs2Mission.Model.NotificationSetting completeNotification) {
            this.CompleteNotification = completeNotification;
            return this;
        }

        public Namespace WithLogSetting(Gs2.Gs2Mission.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

        public Namespace WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Namespace WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        public Namespace WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+)",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Namespace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Mission.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithMissionCompleteScript(!data.Keys.Contains("missionCompleteScript") || data["missionCompleteScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["missionCompleteScript"]))
                .WithCounterIncrementScript(!data.Keys.Contains("counterIncrementScript") || data["counterIncrementScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["counterIncrementScript"]))
                .WithReceiveRewardsScript(!data.Keys.Contains("receiveRewardsScript") || data["receiveRewardsScript"] == null ? null : Gs2.Gs2Mission.Model.ScriptSetting.FromJson(data["receiveRewardsScript"]))
                .WithCompleteNotification(!data.Keys.Contains("completeNotification") || data["completeNotification"] == null ? null : Gs2.Gs2Mission.Model.NotificationSetting.FromJson(data["completeNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Mission.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["missionCompleteScript"] = MissionCompleteScript?.ToJson(),
                ["counterIncrementScript"] = CounterIncrementScript?.ToJson(),
                ["receiveRewardsScript"] = ReceiveRewardsScript?.ToJson(),
                ["completeNotification"] = CompleteNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceId != null) {
                writer.WritePropertyName("namespaceId");
                writer.Write(NamespaceId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (TransactionSetting != null) {
                writer.WritePropertyName("transactionSetting");
                TransactionSetting.WriteJson(writer);
            }
            if (MissionCompleteScript != null) {
                writer.WritePropertyName("missionCompleteScript");
                MissionCompleteScript.WriteJson(writer);
            }
            if (CounterIncrementScript != null) {
                writer.WritePropertyName("counterIncrementScript");
                CounterIncrementScript.WriteJson(writer);
            }
            if (ReceiveRewardsScript != null) {
                writer.WritePropertyName("receiveRewardsScript");
                ReceiveRewardsScript.WriteJson(writer);
            }
            if (CompleteNotification != null) {
                writer.WritePropertyName("completeNotification");
                CompleteNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                writer.WritePropertyName("logSetting");
                LogSetting.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write(long.Parse(Revision.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Namespace;
            var diff = 0;
            if (NamespaceId == null && NamespaceId == other.NamespaceId)
            {
                // null and null
            }
            else
            {
                diff += NamespaceId.CompareTo(other.NamespaceId);
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
            if (TransactionSetting == null && TransactionSetting == other.TransactionSetting)
            {
                // null and null
            }
            else
            {
                diff += TransactionSetting.CompareTo(other.TransactionSetting);
            }
            if (MissionCompleteScript == null && MissionCompleteScript == other.MissionCompleteScript)
            {
                // null and null
            }
            else
            {
                diff += MissionCompleteScript.CompareTo(other.MissionCompleteScript);
            }
            if (CounterIncrementScript == null && CounterIncrementScript == other.CounterIncrementScript)
            {
                // null and null
            }
            else
            {
                diff += CounterIncrementScript.CompareTo(other.CounterIncrementScript);
            }
            if (ReceiveRewardsScript == null && ReceiveRewardsScript == other.ReceiveRewardsScript)
            {
                // null and null
            }
            else
            {
                diff += ReceiveRewardsScript.CompareTo(other.ReceiveRewardsScript);
            }
            if (CompleteNotification == null && CompleteNotification == other.CompleteNotification)
            {
                // null and null
            }
            else
            {
                diff += CompleteNotification.CompareTo(other.CompleteNotification);
            }
            if (LogSetting == null && LogSetting == other.LogSetting)
            {
                // null and null
            }
            else
            {
                diff += LogSetting.CompareTo(other.LogSetting);
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
    }
}