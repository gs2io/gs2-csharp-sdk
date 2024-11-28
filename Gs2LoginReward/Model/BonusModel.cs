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
	public class BonusModel : IComparable
	{
        public string BonusModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Mode { set; get; } = null!;
        public string PeriodEventId { set; get; } = null!;
        public int? ResetHour { set; get; } = null!;
        public string Repeat { set; get; } = null!;
        public Gs2.Gs2LoginReward.Model.Reward[] Rewards { set; get; } = null!;
        public string MissedReceiveRelief { set; get; } = null!;
        public Gs2.Core.Model.VerifyAction[] MissedReceiveReliefVerifyActions { set; get; } = null!;
        public Gs2.Core.Model.ConsumeAction[] MissedReceiveReliefConsumeActions { set; get; } = null!;
        public BonusModel WithBonusModelId(string bonusModelId) {
            this.BonusModelId = bonusModelId;
            return this;
        }
        public BonusModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public BonusModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public BonusModel WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public BonusModel WithPeriodEventId(string periodEventId) {
            this.PeriodEventId = periodEventId;
            return this;
        }
        public BonusModel WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }
        public BonusModel WithRepeat(string repeat) {
            this.Repeat = repeat;
            return this;
        }
        public BonusModel WithRewards(Gs2.Gs2LoginReward.Model.Reward[] rewards) {
            this.Rewards = rewards;
            return this;
        }
        public BonusModel WithMissedReceiveRelief(string missedReceiveRelief) {
            this.MissedReceiveRelief = missedReceiveRelief;
            return this;
        }
        public BonusModel WithMissedReceiveReliefVerifyActions(Gs2.Core.Model.VerifyAction[] missedReceiveReliefVerifyActions) {
            this.MissedReceiveReliefVerifyActions = missedReceiveReliefVerifyActions;
            return this;
        }
        public BonusModel WithMissedReceiveReliefConsumeActions(Gs2.Core.Model.ConsumeAction[] missedReceiveReliefConsumeActions) {
            this.MissedReceiveReliefConsumeActions = missedReceiveReliefConsumeActions;
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
        public static BonusModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BonusModel()
                .WithBonusModelId(!data.Keys.Contains("bonusModelId") || data["bonusModelId"] == null ? null : data["bonusModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
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
                }).ToArray());
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
                ["metadata"] = Metadata,
                ["mode"] = Mode,
                ["periodEventId"] = PeriodEventId,
                ["resetHour"] = ResetHour,
                ["repeat"] = Repeat,
                ["rewards"] = rewardsJsonData,
                ["missedReceiveRelief"] = MissedReceiveRelief,
                ["missedReceiveReliefVerifyActions"] = missedReceiveReliefVerifyActionsJsonData,
                ["missedReceiveReliefConsumeActions"] = missedReceiveReliefConsumeActionsJsonData,
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BonusModel;
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
            return diff;
        }

        public void Validate() {
            {
                if (BonusModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.bonusModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.metadata.error.tooLong"),
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
                            new RequestError("bonusModel", "loginReward.bonusModel.mode.error.invalid"),
                        });
                }
            }
            {
                if (PeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.periodEventId.error.tooLong"),
                    });
                }
            }
            if (PeriodEventId == "") {
                if (ResetHour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.resetHour.error.invalid"),
                    });
                }
                if (ResetHour > 23) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.resetHour.error.invalid"),
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
                            new RequestError("bonusModel", "loginReward.bonusModel.repeat.error.invalid"),
                        });
                }
            }
            {
                if (Rewards.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.rewards.error.tooMany"),
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
                            new RequestError("bonusModel", "loginReward.bonusModel.missedReceiveRelief.error.invalid"),
                        });
                }
            }
            {
                if (MissedReceiveReliefVerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.missedReceiveReliefVerifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (MissedReceiveReliefConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("bonusModel", "loginReward.bonusModel.missedReceiveReliefConsumeActions.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new BonusModel {
                BonusModelId = BonusModelId,
                Name = Name,
                Metadata = Metadata,
                Mode = Mode,
                PeriodEventId = PeriodEventId,
                ResetHour = ResetHour,
                Repeat = Repeat,
                Rewards = Rewards.Clone() as Gs2.Gs2LoginReward.Model.Reward[],
                MissedReceiveRelief = MissedReceiveRelief,
                MissedReceiveReliefVerifyActions = MissedReceiveReliefVerifyActions.Clone() as Gs2.Core.Model.VerifyAction[],
                MissedReceiveReliefConsumeActions = MissedReceiveReliefConsumeActions.Clone() as Gs2.Core.Model.ConsumeAction[],
            };
        }
    }
}