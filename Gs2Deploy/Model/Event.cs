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

namespace Gs2.Gs2Deploy.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Event : IComparable
	{
        public string EventId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string ResourceName { set; get; } = null!;
        public string Type { set; get; } = null!;
        public string Message { set; get; } = null!;
        public long? EventAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Event WithEventId(string eventId) {
            this.EventId = eventId;
            return this;
        }
        public Event WithName(string name) {
            this.Name = name;
            return this;
        }
        public Event WithResourceName(string resourceName) {
            this.ResourceName = resourceName;
            return this;
        }
        public Event WithType(string type) {
            this.Type = type;
            return this;
        }
        public Event WithMessage(string message) {
            this.Message = message;
            return this;
        }
        public Event WithEventAt(long? eventAt) {
            this.EventAt = eventAt;
            return this;
        }
        public Event WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):event:(?<eventName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):event:(?<eventName>.+)",
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

        private static System.Text.RegularExpressions.Regex _stackNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):event:(?<eventName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetStackNameFromGrn(
            string grn
        )
        {
            var match = _stackNameRegex.Match(grn);
            if (!match.Success || !match.Groups["stackName"].Success)
            {
                return null;
            }
            return match.Groups["stackName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _eventNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):event:(?<eventName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetEventNameFromGrn(
            string grn
        )
        {
            var match = _eventNameRegex.Match(grn);
            if (!match.Success || !match.Groups["eventName"].Success)
            {
                return null;
            }
            return match.Groups["eventName"].Value;
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
                .WithEventId(!data.Keys.Contains("eventId") || data["eventId"] == null ? null : data["eventId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithResourceName(!data.Keys.Contains("resourceName") || data["resourceName"] == null ? null : data["resourceName"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithMessage(!data.Keys.Contains("message") || data["message"] == null ? null : data["message"].ToString())
                .WithEventAt(!data.Keys.Contains("eventAt") || data["eventAt"] == null ? null : (long?)(data["eventAt"].ToString().Contains(".") ? (long)double.Parse(data["eventAt"].ToString()) : long.Parse(data["eventAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["eventId"] = EventId,
                ["name"] = Name,
                ["resourceName"] = ResourceName,
                ["type"] = Type,
                ["message"] = Message,
                ["eventAt"] = EventAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (EventId != null) {
                writer.WritePropertyName("eventId");
                writer.Write(EventId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (ResourceName != null) {
                writer.WritePropertyName("resourceName");
                writer.Write(ResourceName.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (Message != null) {
                writer.WritePropertyName("message");
                writer.Write(Message.ToString());
            }
            if (EventAt != null) {
                writer.WritePropertyName("eventAt");
                writer.Write((EventAt.ToString().Contains(".") ? (long)double.Parse(EventAt.ToString()) : long.Parse(EventAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Event;
            var diff = 0;
            if (EventId == null && EventId == other.EventId)
            {
                // null and null
            }
            else
            {
                diff += EventId.CompareTo(other.EventId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (ResourceName == null && ResourceName == other.ResourceName)
            {
                // null and null
            }
            else
            {
                diff += ResourceName.CompareTo(other.ResourceName);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (Message == null && Message == other.Message)
            {
                // null and null
            }
            else
            {
                diff += Message.CompareTo(other.Message);
            }
            if (EventAt == null && EventAt == other.EventAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(EventAt - other.EventAt);
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
                if (EventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.eventId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.name.error.tooLong"),
                    });
                }
            }
            {
                if (ResourceName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.resourceName.error.tooLong"),
                    });
                }
            }
            {
                switch (Type) {
                    case "CREATE_IN_PROGRESS":
                    case "CREATE_COMPLETE":
                    case "CREATE_FAILED":
                    case "UPDATE_IN_PROGRESS":
                    case "UPDATE_COMPLETE":
                    case "UPDATE_FAILED":
                    case "CLEAN_IN_PROGRESS":
                    case "CLEAN_COMPLETE":
                    case "CLEAN_FAILED":
                    case "DELETE_IN_PROGRESS":
                    case "DELETE_COMPLETE":
                    case "DELETE_FAILED":
                    case "ROLLBACK_IN_PROGRESS":
                    case "ROLLBACK_COMPLETE":
                    case "ROLLBACK_FAILED":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("event", "deploy.event.type.error.invalid"),
                        });
                }
            }
            {
                if (Message.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.message.error.tooLong"),
                    });
                }
            }
            {
                if (EventAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.eventAt.error.invalid"),
                    });
                }
                if (EventAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.eventAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "deploy.event.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Event {
                EventId = EventId,
                Name = Name,
                ResourceName = ResourceName,
                Type = Type,
                Message = Message,
                EventAt = EventAt,
                Revision = Revision,
            };
        }
    }
}