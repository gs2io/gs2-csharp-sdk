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
using Gs2.Core.Control;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Core.Model
{
    public class ConsumeAction : IModel, ICloneable
    {
        public string Action { set; get; }
        public string Request { set; get; }
        public ConsumeAction WithAction(string action) {
            this.Action = action;
            return this;
        }
        public ConsumeAction WithRequest(string request) {
            this.Request = request;
            return this;
        }

        public Gs2Request ToRequest()
        {
            if (Action.StartsWith("Gs2Account:")) {
                return Gs2.Gs2Account.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2AdReward:")) {
                return Gs2.Gs2AdReward.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Auth:")) {
                return Gs2.Gs2Auth.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Buff:")) {
                return Gs2.Gs2Buff.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Chat:")) {
                return Gs2.Gs2Chat.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Datastore:")) {
                return Gs2.Gs2Datastore.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Deploy:")) {
                return Gs2.Gs2Deploy.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Dictionary:")) {
                return Gs2.Gs2Dictionary.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Distributor:")) {
                return Gs2.Gs2Distributor.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Enchant:")) {
                return Gs2.Gs2Enchant.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Enhance:")) {
                return Gs2.Gs2Enhance.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Exchange:")) {
                return Gs2.Gs2Exchange.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Experience:")) {
                return Gs2.Gs2Experience.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Formation:")) {
                return Gs2.Gs2Formation.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Freeze:")) {
                return Gs2.Gs2Freeze.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Friend:")) {
                return Gs2.Gs2Friend.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Gateway:")) {
                return Gs2.Gs2Gateway.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Grade:")) {
                return Gs2.Gs2Grade.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Guard:")) {
                return Gs2.Gs2Guard.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Guild:")) {
                return Gs2.Gs2Guild.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Identifier:")) {
                return Gs2.Gs2Identifier.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Idle:")) {
                return Gs2.Gs2Idle.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Inbox:")) {
                return Gs2.Gs2Inbox.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Inventory:")) {
                return Gs2.Gs2Inventory.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2JobQueue:")) {
                return Gs2.Gs2JobQueue.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Key:")) {
                return Gs2.Gs2Key.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Limit:")) {
                return Gs2.Gs2Limit.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Lock:")) {
                return Gs2.Gs2Lock.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Log:")) {
                return Gs2.Gs2Log.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2LoginReward:")) {
                return Gs2.Gs2LoginReward.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Lottery:")) {
                return Gs2.Gs2Lottery.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Matchmaking:")) {
                return Gs2.Gs2Matchmaking.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2MegaField:")) {
                return Gs2.Gs2MegaField.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Mission:")) {
                return Gs2.Gs2Mission.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Money:")) {
                return Gs2.Gs2Money.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Money2:")) {
                return Gs2.Gs2Money2.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2News:")) {
                return Gs2.Gs2News.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Project:")) {
                return Gs2.Gs2Project.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Quest:")) {
                return Gs2.Gs2Quest.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Ranking:")) {
                return Gs2.Gs2Ranking.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Ranking2:")) {
                return Gs2.Gs2Ranking2.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Realtime:")) {
                return Gs2.Gs2Realtime.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Schedule:")) {
                return Gs2.Gs2Schedule.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Script:")) {
                return Gs2.Gs2Script.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2SeasonRating:")) {
                return Gs2.Gs2SeasonRating.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2SerialKey:")) {
                return Gs2.Gs2SerialKey.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Showcase:")) {
                return Gs2.Gs2Showcase.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2SkillTree:")) {
                return Gs2.Gs2SkillTree.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Stamina:")) {
                return Gs2.Gs2Stamina.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2StateMachine:")) {
                return Gs2.Gs2StateMachine.Model.StampAction.ToRequest(this);
            }
            if (Action.StartsWith("Gs2Version:")) {
                return Gs2.Gs2Version.Model.StampAction.ToRequest(this);
            }
            throw new ArgumentException($"unknown service {Action}");
        }
        
#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ConsumeAction FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ConsumeAction()
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithRequest(!data.Keys.Contains("request") || data["request"] == null ? null : data["request"].IsObject ? data["request"].ToJson() : data["request"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["action"] = Action,
                ["request"] = Request,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (Request != null) {
                writer.WritePropertyName("request");
                writer.Write(Request.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ConsumeAction;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ConsumeAction.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Action, other.Action);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Request, other.Request);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public object Clone() {
            return new ConsumeAction {
                Action = Action,
                Request = Request,
            };
        }
    }
}
