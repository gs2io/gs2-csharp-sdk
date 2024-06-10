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
	public class Event : IComparable
	{
        public string EventType { set; get; } = null!;
        public Gs2.Gs2StateMachine.Model.ChangeStateEvent ChangeStateEvent { set; get; } = null!;
        public Gs2.Gs2StateMachine.Model.EmitEvent EmitEvent { set; get; } = null!;
        public Event WithEventType(string eventType) {
            this.EventType = eventType;
            return this;
        }
        public Event WithChangeStateEvent(Gs2.Gs2StateMachine.Model.ChangeStateEvent changeStateEvent) {
            this.ChangeStateEvent = changeStateEvent;
            return this;
        }
        public Event WithEmitEvent(Gs2.Gs2StateMachine.Model.EmitEvent emitEvent) {
            this.EmitEvent = emitEvent;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Event FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Event()
                .WithEventType(!data.Keys.Contains("eventType") || data["eventType"] == null ? null : data["eventType"].ToString())
                .WithChangeStateEvent(!data.Keys.Contains("changeStateEvent") || data["changeStateEvent"] == null ? null : Gs2.Gs2StateMachine.Model.ChangeStateEvent.FromJson(data["changeStateEvent"]))
                .WithEmitEvent(!data.Keys.Contains("emitEvent") || data["emitEvent"] == null ? null : Gs2.Gs2StateMachine.Model.EmitEvent.FromJson(data["emitEvent"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["eventType"] = EventType,
                ["changeStateEvent"] = ChangeStateEvent?.ToJson(),
                ["emitEvent"] = EmitEvent?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (EventType != null) {
                writer.WritePropertyName("eventType");
                writer.Write(EventType.ToString());
            }
            if (ChangeStateEvent != null) {
                writer.WritePropertyName("changeStateEvent");
                ChangeStateEvent.WriteJson(writer);
            }
            if (EmitEvent != null) {
                writer.WritePropertyName("emitEvent");
                EmitEvent.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Event;
            var diff = 0;
            if (EventType == null && EventType == other.EventType)
            {
                // null and null
            }
            else
            {
                diff += EventType.CompareTo(other.EventType);
            }
            if (ChangeStateEvent == null && ChangeStateEvent == other.ChangeStateEvent)
            {
                // null and null
            }
            else
            {
                diff += ChangeStateEvent.CompareTo(other.ChangeStateEvent);
            }
            if (EmitEvent == null && EmitEvent == other.EmitEvent)
            {
                // null and null
            }
            else
            {
                diff += EmitEvent.CompareTo(other.EmitEvent);
            }
            return diff;
        }

        public void Validate() {
            {
                switch (EventType) {
                    case "change_state":
                    case "emit":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("event", "stateMachine.event.eventType.error.invalid"),
                        });
                }
            }
            if (EventType == "change_state") {
            }
            if (EventType == "emit") {
            }
        }

        public object Clone() {
            return new Event {
                EventType = EventType,
                ChangeStateEvent = ChangeStateEvent.Clone() as Gs2.Gs2StateMachine.Model.ChangeStateEvent,
                EmitEvent = EmitEvent.Clone() as Gs2.Gs2StateMachine.Model.EmitEvent,
            };
        }
    }
}