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
using Gs2.Gs2LoginReward.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2LoginReward.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateBonusModelMasterRequest : Gs2Request<CreateBonusModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string Mode { set; get; }
        public string PeriodEventId { set; get; }
        public int? ResetHour { set; get; }
        public string Repeat { set; get; }
        public Gs2.Gs2LoginReward.Model.Reward[] Rewards { set; get; }
        public string MissedReceiveRelief { set; get; }
        public Gs2.Core.Model.ConsumeAction[] MissedReceiveReliefConsumeActions { set; get; }
        public CreateBonusModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateBonusModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateBonusModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateBonusModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateBonusModelMasterRequest WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public CreateBonusModelMasterRequest WithPeriodEventId(string periodEventId) {
            this.PeriodEventId = periodEventId;
            return this;
        }
        public CreateBonusModelMasterRequest WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }
        public CreateBonusModelMasterRequest WithRepeat(string repeat) {
            this.Repeat = repeat;
            return this;
        }
        public CreateBonusModelMasterRequest WithRewards(Gs2.Gs2LoginReward.Model.Reward[] rewards) {
            this.Rewards = rewards;
            return this;
        }
        public CreateBonusModelMasterRequest WithMissedReceiveRelief(string missedReceiveRelief) {
            this.MissedReceiveRelief = missedReceiveRelief;
            return this;
        }
        public CreateBonusModelMasterRequest WithMissedReceiveReliefConsumeActions(Gs2.Core.Model.ConsumeAction[] missedReceiveReliefConsumeActions) {
            this.MissedReceiveReliefConsumeActions = missedReceiveReliefConsumeActions;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateBonusModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateBonusModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithPeriodEventId(!data.Keys.Contains("periodEventId") || data["periodEventId"] == null ? null : data["periodEventId"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)int.Parse(data["resetHour"].ToString()))
                .WithRepeat(!data.Keys.Contains("repeat") || data["repeat"] == null ? null : data["repeat"].ToString())
                .WithRewards(!data.Keys.Contains("rewards") || data["rewards"] == null ? new Gs2.Gs2LoginReward.Model.Reward[]{} : data["rewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2LoginReward.Model.Reward.FromJson(v);
                }).ToArray())
                .WithMissedReceiveRelief(!data.Keys.Contains("missedReceiveRelief") || data["missedReceiveRelief"] == null ? null : data["missedReceiveRelief"].ToString())
                .WithMissedReceiveReliefConsumeActions(!data.Keys.Contains("missedReceiveReliefConsumeActions") || data["missedReceiveReliefConsumeActions"] == null ? new Gs2.Core.Model.ConsumeAction[]{} : data["missedReceiveReliefConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["mode"] = Mode,
                ["periodEventId"] = PeriodEventId,
                ["resetHour"] = ResetHour,
                ["repeat"] = Repeat,
                ["rewards"] = Rewards == null ? null : new JsonData(
                        Rewards.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["missedReceiveRelief"] = MissedReceiveRelief,
                ["missedReceiveReliefConsumeActions"] = MissedReceiveReliefConsumeActions == null ? null : new JsonData(
                        MissedReceiveReliefConsumeActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
                writer.Write(int.Parse(ResetHour.ToString()));
            }
            if (Repeat != null) {
                writer.WritePropertyName("repeat");
                writer.Write(Repeat.ToString());
            }
            writer.WriteArrayStart();
            foreach (var reward in Rewards)
            {
                if (reward != null) {
                    reward.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (MissedReceiveRelief != null) {
                writer.WritePropertyName("missedReceiveRelief");
                writer.Write(MissedReceiveRelief.ToString());
            }
            writer.WriteArrayStart();
            foreach (var missedReceiveReliefConsumeAction in MissedReceiveReliefConsumeActions)
            {
                if (missedReceiveReliefConsumeAction != null) {
                    missedReceiveReliefConsumeAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Mode + ":";
            key += PeriodEventId + ":";
            key += ResetHour + ":";
            key += Repeat + ":";
            key += Rewards + ":";
            key += MissedReceiveRelief + ":";
            key += MissedReceiveReliefConsumeActions + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply CreateBonusModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CreateBonusModelMasterRequest)x;
            return this;
        }
    }
}