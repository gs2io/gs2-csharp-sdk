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

namespace Gs2.Gs2LoginReward.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BonusModelMaster : IComparable
	{
        public string BonusModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Mode { set; get; } = null!;
        public string PeriodEventId { set; get; } = null!;
        public int? ResetHour { set; get; } = null!;
        public string Repeat { set; get; } = null!;
        public Gs2.Gs2LoginReward.Model.Reward[] Rewards { set; get; } = null!;
        public string MissedReceiveRelief { set; get; } = null!;
        public Gs2.Core.Model.VerifyAction[] MissedReceiveReliefVerifyActions { set; get; } = null!;
        public Gs2.Core.Model.ConsumeAction[] MissedReceiveReliefConsumeActions { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public BonusModelMaster WithBonusModelId(string bonusModelId) {
            this.BonusModelId = bonusModelId;
            return this;
        }
        public BonusModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public BonusModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public BonusModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public BonusModelMaster WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public BonusModelMaster WithPeriodEventId(string periodEventId) {
            this.PeriodEventId = periodEventId;
            return this;
        }
        public BonusModelMaster WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }
        public BonusModelMaster WithRepeat(string repeat) {
            this.Repeat = repeat;
            return this;
        }
        public BonusModelMaster WithRewards(Gs2.Gs2LoginReward.Model.Reward[] rewards) {
            this.Rewards = rewards;
            return this;
        }
        public BonusModelMaster WithMissedReceiveRelief(string missedReceiveRelief) {
            this.MissedReceiveRelief = missedReceiveRelief;
            return this;
        }
        public BonusModelMaster WithMissedReceiveReliefVerifyActions(Gs2.Core.Model.VerifyAction[] missedReceiveReliefVerifyActions) {
            this.MissedReceiveReliefVerifyActions = missedReceiveReliefVerifyActions;
            return this;
        }
        public BonusModelMaster WithMissedReceiveReliefConsumeActions(Gs2.Core.Model.ConsumeAction[] missedReceiveReliefConsumeActions) {
            this.MissedReceiveReliefConsumeActions = missedReceiveReliefConsumeActions;
            return this;
        }
        public BonusModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public BonusModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public BonusModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):bonusModel:(?<bonusModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):bonusModel:(?<bonusModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):bonusModel:(?<bonusModelName>.+)",
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

        private static System.Text.RegularExpressions.Regex _bonusModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):bonusModel:(?<bonusModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetBonusModelNameFromGrn(
            string grn
        )
        {
            var match = _bonusModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["bonusModelName"].Success)
            {
                return null;
            }
            return match.Groups["bonusModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BonusModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BonusModelMaster()
                .WithBonusModelId(!data.Keys.Contains("bonusModelId") || data["bonusModelId"] == null ? null : data["bonusModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithPeriodEventId(!data.Keys.Contains("periodEventId") || data["periodEventId"] == null ? null : data["periodEventId"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)(data["resetHour"].ToString().Contains(".") ? (int)double.Parse(data["resetHour"].ToString()) : int.Parse(data["resetHour"].ToString())))
                .WithRepeat(!data.Keys.Contains("repeat") || data["repeat"] == null ? null : data["repeat"].ToString())
                .WithRewards(!data.Keys.Contains("rewards") || data["rewards"] == null || !data["rewards"].IsArray ? null : data["rewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2LoginReward.Model.Reward.FromJson(v);
                }).ToArray())
                .WithMissedReceiveRelief(!data.Keys.Contains("missedReceiveRelief") || data["missedReceiveRelief"] == null ? null : data["missedReceiveRelief"].ToString())
                .WithMissedReceiveReliefVerifyActions(!data.Keys.Contains("missedReceiveReliefVerifyActions") || data["missedReceiveReliefVerifyActions"] == null || !data["missedReceiveReliefVerifyActions"].IsArray ? null : data["missedReceiveReliefVerifyActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithMissedReceiveReliefConsumeActions(!data.Keys.Contains("missedReceiveReliefConsumeActions") || data["missedReceiveReliefConsumeActions"] == null || !data["missedReceiveReliefConsumeActions"].IsArray ? null : data["missedReceiveReliefConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData rewardsJsonData = null;
            if (Rewards != null && Rewards.Length > 0)
            {
                rewardsJsonData = new JsonData();
                foreach (var reward in Rewards)
                {
                    rewardsJsonData.Add(reward.ToJson());
                }
            }
            JsonData missedReceiveReliefVerifyActionsJsonData = null;
            if (MissedReceiveReliefVerifyActions != null && MissedReceiveReliefVerifyActions.Length > 0)
            {
                missedReceiveReliefVerifyActionsJsonData = new JsonData();
                foreach (var missedReceiveReliefVerifyAction in MissedReceiveReliefVerifyActions)
                {
                    missedReceiveReliefVerifyActionsJsonData.Add(missedReceiveReliefVerifyAction.ToJson());
                }
            }
            JsonData missedReceiveReliefConsumeActionsJsonData = null;
            if (MissedReceiveReliefConsumeActions != null && MissedReceiveReliefConsumeActions.Length > 0)
            {
                missedReceiveReliefConsumeActionsJsonData = new JsonData();
                foreach (var missedReceiveReliefConsumeAction in MissedReceiveReliefConsumeActions)
                {
                    missedReceiveReliefConsumeActionsJsonData.Add(missedReceiveReliefConsumeAction.ToJson());
                }
            }
            return new JsonData {
                ["bonusModelId"] = BonusModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["mode"] = Mode,
                ["periodEventId"] = PeriodEventId,
                ["resetHour"] = ResetHour,
                ["repeat"] = Repeat,
                ["rewards"] = rewardsJsonData,
                ["missedReceiveRelief"] = MissedReceiveRelief,
                ["missedReceiveReliefVerifyActions"] = missedReceiveReliefVerifyActionsJsonData,
                ["missedReceiveReliefConsumeActions"] = missedReceiveReliefConsumeActionsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BonusModelId != null) {
                writer.WritePropertyName("bonusModelId");
                writer.Write(BonusModelId.ToString());
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
            if (Mode != null) {
                writer.WritePropertyName("mode");
                writer.Write(Mode.ToString());
            }
            if (PeriodEventId != null) {
                writer.WritePropertyName("periodEventId");
                writer.Write(PeriodEventId.ToString());
            }
            if (ResetHour != null) {
                writer.WritePropertyName("resetHour");
                writer.Write((ResetHour.ToString().Contains(".") ? (int)double.Parse(ResetHour.ToString()) : int.Parse(ResetHour.ToString())));
            }
            if (Repeat != null) {
                writer.WritePropertyName("repeat");
                writer.Write(Repeat.ToString());
            }
            if (Rewards != null) {
                writer.WritePropertyName("rewards");
                writer.WriteArrayStart();
                foreach (var reward in Rewards)
                {
                    if (reward != null) {
                        reward.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (MissedReceiveRelief != null) {
                writer.WritePropertyName("missedReceiveRelief");
                writer.Write(MissedReceiveRelief.ToString());
            }
            if (MissedReceiveReliefVerifyActions != null) {
                writer.WritePropertyName("missedReceiveReliefVerifyActions");
                writer.WriteArrayStart();
                foreach (var missedReceiveReliefVerifyAction in MissedReceiveReliefVerifyActions)
                {
                    if (missedReceiveReliefVerifyAction != null) {
                        missedReceiveReliefVerifyAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (MissedReceiveReliefConsumeActions != null) {
                writer.WritePropertyName("missedReceiveReliefConsumeActions");
                writer.WriteArrayStart();
                foreach (var missedReceiveReliefConsumeAction in MissedReceiveReliefConsumeActions)
                {
                    if (missedReceiveReliefConsumeAction != null) {
                        missedReceiveReliefConsumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            var other = obj as BonusModelMaster;
            var diff = 0;
            if (BonusModelId == null && BonusModelId == other.BonusModelId)
            {
                // null and null
            }
            else
            {
                diff += BonusModelId.CompareTo(other.BonusModelId);
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
            if (Mode == null && Mode == other.Mode)
            {
                // null and null
            }
            else
            {
                diff += Mode.CompareTo(other.Mode);
            }
            if (PeriodEventId == null && PeriodEventId == other.PeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += PeriodEventId.CompareTo(other.PeriodEventId);
            }
            if (ResetHour == null && ResetHour == other.ResetHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetHour - other.ResetHour);
            }
            if (Repeat == null && Repeat == other.Repeat)
            {
                // null and null
            }
            else
            {
                diff += Repeat.CompareTo(other.Repeat);
            }
            if (Rewards == null && Rewards == other.Rewards)
            {
                // null and null
            }
            else
            {
                diff += Rewards.Length - other.Rewards.Length;
                for (var i = 0; i < Rewards.Length; i++)
                {
                    diff += Rewards[i].CompareTo(other.Rewards[i]);
                }
            }
            if (MissedReceiveRelief == null && MissedReceiveRelief == other.MissedReceiveRelief)
            {
                // null and null
            }
            else
            {
                diff += MissedReceiveRelief.CompareTo(other.MissedReceiveRelief);
            }
            if (MissedReceiveReliefVerifyActions == null && MissedReceiveReliefVerifyActions == other.MissedReceiveReliefVerifyActions)
            {
                // null and null
            }
            else
            {
                diff += MissedReceiveReliefVerifyActions.Length - other.MissedReceiveReliefVerifyActions.Length;
                for (var i = 0; i < MissedReceiveReliefVerifyActions.Length; i++)
                {
                    diff += MissedReceiveReliefVerifyActions[i].CompareTo(other.MissedReceiveReliefVerifyActions[i]);
                }
            }
            if (MissedReceiveReliefConsumeActions == null && MissedReceiveReliefConsumeActions == other.MissedReceiveReliefConsumeActions)
            {
                // null and null
            }
            else
            {
                diff += MissedReceiveReliefConsumeActions.Length - other.MissedReceiveReliefConsumeActions.Length;
                for (var i = 0; i < MissedReceiveReliefConsumeActions.Length; i++)
                {
                    diff += MissedReceiveReliefConsumeActions[i].CompareTo(other.MissedReceiveReliefConsumeActions[i]);
                }
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
                if (BonusModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.bonusModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                switch (Mode) {
                    case "schedule":
                    case "streaming":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.mode.error.invalid"),
                        });
                }
            }
            {
                if (PeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.periodEventId.error.tooLong"),
                    });
                }
            }
            if (PeriodEventId == "") {
                if (ResetHour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.resetHour.error.invalid"),
                    });
                }
                if (ResetHour > 23) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.resetHour.error.invalid"),
                    });
                }
            }
            if (Mode == "streaming") {
                switch (Repeat) {
                    case "enabled":
                    case "disabled":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.repeat.error.invalid"),
                        });
                }
            }
            {
                if (Rewards.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.rewards.error.tooMany"),
                    });
                }
            }
            {
                switch (MissedReceiveRelief) {
                    case "enabled":
                    case "disabled":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.missedReceiveRelief.error.invalid"),
                        });
                }
            }
            {
                if (MissedReceiveReliefVerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.missedReceiveReliefVerifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (MissedReceiveReliefConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.missedReceiveReliefConsumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModelMaster", "loginReward.bonusModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BonusModelMaster {
                BonusModelId = BonusModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                Mode = Mode,
                PeriodEventId = PeriodEventId,
                ResetHour = ResetHour,
                Repeat = Repeat,
                Rewards = Rewards.Clone() as Gs2.Gs2LoginReward.Model.Reward[],
                MissedReceiveRelief = MissedReceiveRelief,
                MissedReceiveReliefVerifyActions = MissedReceiveReliefVerifyActions.Clone() as Gs2.Core.Model.VerifyAction[],
                MissedReceiveReliefConsumeActions = MissedReceiveReliefConsumeActions.Clone() as Gs2.Core.Model.ConsumeAction[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}