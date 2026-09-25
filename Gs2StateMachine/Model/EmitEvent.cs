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
	public partial class EmitEvent : IComparable
	{
        public string Event { set; get; }
        public string Parameters { set; get; }
        public long? Timestamp { set; get; }
        public EmitEvent WithEvent(string event_) {
            this.Event = event_;
            return this;
        }
        public EmitEvent WithParameters(string parameters) {
            this.Parameters = parameters;
            return this;
        }
        public EmitEvent WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static EmitEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new EmitEvent()
                .WithEvent(!data.Keys.Contains("event") || data["event"] == null ? null : data["event"].ToString())
                .WithParameters(!data.Keys.Contains("parameters") || data["parameters"] == null ? null : data["parameters"].ToString())
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["timestamp"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["event"] = Event,
                ["parameters"] = Parameters,
                ["timestamp"] = Timestamp,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Event != null) {
                writer.WritePropertyName("event");
                writer.Write(Event.ToString());
            }
            if (Parameters != null) {
                writer.WritePropertyName("parameters");
                writer.Write(Parameters.ToString());
            }
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as EmitEvent;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type EmitEvent.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Event, other.Event);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Parameters, other.Parameters);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Timestamp, other.Timestamp);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Event.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("emitEvent", "stateMachine.emitEvent.event.error.tooLong"),
                    });
                }
            }
            {
                if (Parameters.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("emitEvent", "stateMachine.emitEvent.parameters.error.tooLong"),
                    });
                }
            }
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("emitEvent", "stateMachine.emitEvent.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("emitEvent", "stateMachine.emitEvent.timestamp.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new EmitEvent {
                Event = Event,
                Parameters = Parameters,
                Timestamp = Timestamp,
            };
        }
    }
}