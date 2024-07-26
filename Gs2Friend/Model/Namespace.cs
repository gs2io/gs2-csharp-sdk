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

namespace Gs2.Gs2Friend.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting FollowScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting UnfollowScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting SendRequestScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting CancelRequestScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting AcceptRequestScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting RejectRequestScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting DeleteFriendScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.ScriptSetting UpdateProfileScript { set; get; } = null!;
        public Gs2.Gs2Friend.Model.NotificationSetting FollowNotification { set; get; } = null!;
        public Gs2.Gs2Friend.Model.NotificationSetting ReceiveRequestNotification { set; get; } = null!;
        public Gs2.Gs2Friend.Model.NotificationSetting CancelRequestNotification { set; get; } = null!;
        public Gs2.Gs2Friend.Model.NotificationSetting AcceptRequestNotification { set; get; } = null!;
        public Gs2.Gs2Friend.Model.NotificationSetting RejectRequestNotification { set; get; } = null!;
        public Gs2.Gs2Friend.Model.NotificationSetting DeleteFriendNotification { set; get; } = null!;
        public Gs2.Gs2Friend.Model.LogSetting LogSetting { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Namespace WithFollowScript(Gs2.Gs2Friend.Model.ScriptSetting followScript) {
            this.FollowScript = followScript;
            return this;
        }
        public Namespace WithUnfollowScript(Gs2.Gs2Friend.Model.ScriptSetting unfollowScript) {
            this.UnfollowScript = unfollowScript;
            return this;
        }
        public Namespace WithSendRequestScript(Gs2.Gs2Friend.Model.ScriptSetting sendRequestScript) {
            this.SendRequestScript = sendRequestScript;
            return this;
        }
        public Namespace WithCancelRequestScript(Gs2.Gs2Friend.Model.ScriptSetting cancelRequestScript) {
            this.CancelRequestScript = cancelRequestScript;
            return this;
        }
        public Namespace WithAcceptRequestScript(Gs2.Gs2Friend.Model.ScriptSetting acceptRequestScript) {
            this.AcceptRequestScript = acceptRequestScript;
            return this;
        }
        public Namespace WithRejectRequestScript(Gs2.Gs2Friend.Model.ScriptSetting rejectRequestScript) {
            this.RejectRequestScript = rejectRequestScript;
            return this;
        }
        public Namespace WithDeleteFriendScript(Gs2.Gs2Friend.Model.ScriptSetting deleteFriendScript) {
            this.DeleteFriendScript = deleteFriendScript;
            return this;
        }
        public Namespace WithUpdateProfileScript(Gs2.Gs2Friend.Model.ScriptSetting updateProfileScript) {
            this.UpdateProfileScript = updateProfileScript;
            return this;
        }
        public Namespace WithFollowNotification(Gs2.Gs2Friend.Model.NotificationSetting followNotification) {
            this.FollowNotification = followNotification;
            return this;
        }
        public Namespace WithReceiveRequestNotification(Gs2.Gs2Friend.Model.NotificationSetting receiveRequestNotification) {
            this.ReceiveRequestNotification = receiveRequestNotification;
            return this;
        }
        public Namespace WithCancelRequestNotification(Gs2.Gs2Friend.Model.NotificationSetting cancelRequestNotification) {
            this.CancelRequestNotification = cancelRequestNotification;
            return this;
        }
        public Namespace WithAcceptRequestNotification(Gs2.Gs2Friend.Model.NotificationSetting acceptRequestNotification) {
            this.AcceptRequestNotification = acceptRequestNotification;
            return this;
        }
        public Namespace WithRejectRequestNotification(Gs2.Gs2Friend.Model.NotificationSetting rejectRequestNotification) {
            this.RejectRequestNotification = rejectRequestNotification;
            return this;
        }
        public Namespace WithDeleteFriendNotification(Gs2.Gs2Friend.Model.NotificationSetting deleteFriendNotification) {
            this.DeleteFriendNotification = deleteFriendNotification;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2Friend.Model.LogSetting logSetting) {
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):friend:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):friend:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):friend:(?<namespaceName>.+)",
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
                .WithFollowScript(!data.Keys.Contains("followScript") || data["followScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["followScript"]))
                .WithUnfollowScript(!data.Keys.Contains("unfollowScript") || data["unfollowScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["unfollowScript"]))
                .WithSendRequestScript(!data.Keys.Contains("sendRequestScript") || data["sendRequestScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["sendRequestScript"]))
                .WithCancelRequestScript(!data.Keys.Contains("cancelRequestScript") || data["cancelRequestScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["cancelRequestScript"]))
                .WithAcceptRequestScript(!data.Keys.Contains("acceptRequestScript") || data["acceptRequestScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["acceptRequestScript"]))
                .WithRejectRequestScript(!data.Keys.Contains("rejectRequestScript") || data["rejectRequestScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["rejectRequestScript"]))
                .WithDeleteFriendScript(!data.Keys.Contains("deleteFriendScript") || data["deleteFriendScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["deleteFriendScript"]))
                .WithUpdateProfileScript(!data.Keys.Contains("updateProfileScript") || data["updateProfileScript"] == null ? null : Gs2.Gs2Friend.Model.ScriptSetting.FromJson(data["updateProfileScript"]))
                .WithFollowNotification(!data.Keys.Contains("followNotification") || data["followNotification"] == null ? null : Gs2.Gs2Friend.Model.NotificationSetting.FromJson(data["followNotification"]))
                .WithReceiveRequestNotification(!data.Keys.Contains("receiveRequestNotification") || data["receiveRequestNotification"] == null ? null : Gs2.Gs2Friend.Model.NotificationSetting.FromJson(data["receiveRequestNotification"]))
                .WithCancelRequestNotification(!data.Keys.Contains("cancelRequestNotification") || data["cancelRequestNotification"] == null ? null : Gs2.Gs2Friend.Model.NotificationSetting.FromJson(data["cancelRequestNotification"]))
                .WithAcceptRequestNotification(!data.Keys.Contains("acceptRequestNotification") || data["acceptRequestNotification"] == null ? null : Gs2.Gs2Friend.Model.NotificationSetting.FromJson(data["acceptRequestNotification"]))
                .WithRejectRequestNotification(!data.Keys.Contains("rejectRequestNotification") || data["rejectRequestNotification"] == null ? null : Gs2.Gs2Friend.Model.NotificationSetting.FromJson(data["rejectRequestNotification"]))
                .WithDeleteFriendNotification(!data.Keys.Contains("deleteFriendNotification") || data["deleteFriendNotification"] == null ? null : Gs2.Gs2Friend.Model.NotificationSetting.FromJson(data["deleteFriendNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Friend.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["followScript"] = FollowScript?.ToJson(),
                ["unfollowScript"] = UnfollowScript?.ToJson(),
                ["sendRequestScript"] = SendRequestScript?.ToJson(),
                ["cancelRequestScript"] = CancelRequestScript?.ToJson(),
                ["acceptRequestScript"] = AcceptRequestScript?.ToJson(),
                ["rejectRequestScript"] = RejectRequestScript?.ToJson(),
                ["deleteFriendScript"] = DeleteFriendScript?.ToJson(),
                ["updateProfileScript"] = UpdateProfileScript?.ToJson(),
                ["followNotification"] = FollowNotification?.ToJson(),
                ["receiveRequestNotification"] = ReceiveRequestNotification?.ToJson(),
                ["cancelRequestNotification"] = CancelRequestNotification?.ToJson(),
                ["acceptRequestNotification"] = AcceptRequestNotification?.ToJson(),
                ["rejectRequestNotification"] = RejectRequestNotification?.ToJson(),
                ["deleteFriendNotification"] = DeleteFriendNotification?.ToJson(),
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
            if (FollowScript != null) {
                writer.WritePropertyName("followScript");
                FollowScript.WriteJson(writer);
            }
            if (UnfollowScript != null) {
                writer.WritePropertyName("unfollowScript");
                UnfollowScript.WriteJson(writer);
            }
            if (SendRequestScript != null) {
                writer.WritePropertyName("sendRequestScript");
                SendRequestScript.WriteJson(writer);
            }
            if (CancelRequestScript != null) {
                writer.WritePropertyName("cancelRequestScript");
                CancelRequestScript.WriteJson(writer);
            }
            if (AcceptRequestScript != null) {
                writer.WritePropertyName("acceptRequestScript");
                AcceptRequestScript.WriteJson(writer);
            }
            if (RejectRequestScript != null) {
                writer.WritePropertyName("rejectRequestScript");
                RejectRequestScript.WriteJson(writer);
            }
            if (DeleteFriendScript != null) {
                writer.WritePropertyName("deleteFriendScript");
                DeleteFriendScript.WriteJson(writer);
            }
            if (UpdateProfileScript != null) {
                writer.WritePropertyName("updateProfileScript");
                UpdateProfileScript.WriteJson(writer);
            }
            if (FollowNotification != null) {
                writer.WritePropertyName("followNotification");
                FollowNotification.WriteJson(writer);
            }
            if (ReceiveRequestNotification != null) {
                writer.WritePropertyName("receiveRequestNotification");
                ReceiveRequestNotification.WriteJson(writer);
            }
            if (CancelRequestNotification != null) {
                writer.WritePropertyName("cancelRequestNotification");
                CancelRequestNotification.WriteJson(writer);
            }
            if (AcceptRequestNotification != null) {
                writer.WritePropertyName("acceptRequestNotification");
                AcceptRequestNotification.WriteJson(writer);
            }
            if (RejectRequestNotification != null) {
                writer.WritePropertyName("rejectRequestNotification");
                RejectRequestNotification.WriteJson(writer);
            }
            if (DeleteFriendNotification != null) {
                writer.WritePropertyName("deleteFriendNotification");
                DeleteFriendNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                writer.WritePropertyName("logSetting");
                LogSetting.WriteJson(writer);
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
            if (FollowScript == null && FollowScript == other.FollowScript)
            {
                // null and null
            }
            else
            {
                diff += FollowScript.CompareTo(other.FollowScript);
            }
            if (UnfollowScript == null && UnfollowScript == other.UnfollowScript)
            {
                // null and null
            }
            else
            {
                diff += UnfollowScript.CompareTo(other.UnfollowScript);
            }
            if (SendRequestScript == null && SendRequestScript == other.SendRequestScript)
            {
                // null and null
            }
            else
            {
                diff += SendRequestScript.CompareTo(other.SendRequestScript);
            }
            if (CancelRequestScript == null && CancelRequestScript == other.CancelRequestScript)
            {
                // null and null
            }
            else
            {
                diff += CancelRequestScript.CompareTo(other.CancelRequestScript);
            }
            if (AcceptRequestScript == null && AcceptRequestScript == other.AcceptRequestScript)
            {
                // null and null
            }
            else
            {
                diff += AcceptRequestScript.CompareTo(other.AcceptRequestScript);
            }
            if (RejectRequestScript == null && RejectRequestScript == other.RejectRequestScript)
            {
                // null and null
            }
            else
            {
                diff += RejectRequestScript.CompareTo(other.RejectRequestScript);
            }
            if (DeleteFriendScript == null && DeleteFriendScript == other.DeleteFriendScript)
            {
                // null and null
            }
            else
            {
                diff += DeleteFriendScript.CompareTo(other.DeleteFriendScript);
            }
            if (UpdateProfileScript == null && UpdateProfileScript == other.UpdateProfileScript)
            {
                // null and null
            }
            else
            {
                diff += UpdateProfileScript.CompareTo(other.UpdateProfileScript);
            }
            if (FollowNotification == null && FollowNotification == other.FollowNotification)
            {
                // null and null
            }
            else
            {
                diff += FollowNotification.CompareTo(other.FollowNotification);
            }
            if (ReceiveRequestNotification == null && ReceiveRequestNotification == other.ReceiveRequestNotification)
            {
                // null and null
            }
            else
            {
                diff += ReceiveRequestNotification.CompareTo(other.ReceiveRequestNotification);
            }
            if (CancelRequestNotification == null && CancelRequestNotification == other.CancelRequestNotification)
            {
                // null and null
            }
            else
            {
                diff += CancelRequestNotification.CompareTo(other.CancelRequestNotification);
            }
            if (AcceptRequestNotification == null && AcceptRequestNotification == other.AcceptRequestNotification)
            {
                // null and null
            }
            else
            {
                diff += AcceptRequestNotification.CompareTo(other.AcceptRequestNotification);
            }
            if (RejectRequestNotification == null && RejectRequestNotification == other.RejectRequestNotification)
            {
                // null and null
            }
            else
            {
                diff += RejectRequestNotification.CompareTo(other.RejectRequestNotification);
            }
            if (DeleteFriendNotification == null && DeleteFriendNotification == other.DeleteFriendNotification)
            {
                // null and null
            }
            else
            {
                diff += DeleteFriendNotification.CompareTo(other.DeleteFriendNotification);
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

        public void Validate() {
            {
                if (NamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.description.error.tooLong"),
                    });
                }
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "friend.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                FollowScript = FollowScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                UnfollowScript = UnfollowScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                SendRequestScript = SendRequestScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                CancelRequestScript = CancelRequestScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                AcceptRequestScript = AcceptRequestScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                RejectRequestScript = RejectRequestScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                DeleteFriendScript = DeleteFriendScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                UpdateProfileScript = UpdateProfileScript.Clone() as Gs2.Gs2Friend.Model.ScriptSetting,
                FollowNotification = FollowNotification.Clone() as Gs2.Gs2Friend.Model.NotificationSetting,
                ReceiveRequestNotification = ReceiveRequestNotification.Clone() as Gs2.Gs2Friend.Model.NotificationSetting,
                CancelRequestNotification = CancelRequestNotification.Clone() as Gs2.Gs2Friend.Model.NotificationSetting,
                AcceptRequestNotification = AcceptRequestNotification.Clone() as Gs2.Gs2Friend.Model.NotificationSetting,
                RejectRequestNotification = RejectRequestNotification.Clone() as Gs2.Gs2Friend.Model.NotificationSetting,
                DeleteFriendNotification = DeleteFriendNotification.Clone() as Gs2.Gs2Friend.Model.NotificationSetting,
                LogSetting = LogSetting.Clone() as Gs2.Gs2Friend.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}