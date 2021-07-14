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

namespace Gs2.Gs2Chat.Model
{

	[Preserve]
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public bool? AllowCreateRoom { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting PostMessageScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting CreateRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting DeleteRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting SubscribeRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting UnsubscribeRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.NotificationSetting PostNotification { set; get; }
        public Gs2.Gs2Chat.Model.LogSetting LogSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

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

        public Namespace WithAllowCreateRoom(bool? allowCreateRoom) {
            this.AllowCreateRoom = allowCreateRoom;
            return this;
        }

        public Namespace WithPostMessageScript(Gs2.Gs2Chat.Model.ScriptSetting postMessageScript) {
            this.PostMessageScript = postMessageScript;
            return this;
        }

        public Namespace WithCreateRoomScript(Gs2.Gs2Chat.Model.ScriptSetting createRoomScript) {
            this.CreateRoomScript = createRoomScript;
            return this;
        }

        public Namespace WithDeleteRoomScript(Gs2.Gs2Chat.Model.ScriptSetting deleteRoomScript) {
            this.DeleteRoomScript = deleteRoomScript;
            return this;
        }

        public Namespace WithSubscribeRoomScript(Gs2.Gs2Chat.Model.ScriptSetting subscribeRoomScript) {
            this.SubscribeRoomScript = subscribeRoomScript;
            return this;
        }

        public Namespace WithUnsubscribeRoomScript(Gs2.Gs2Chat.Model.ScriptSetting unsubscribeRoomScript) {
            this.UnsubscribeRoomScript = unsubscribeRoomScript;
            return this;
        }

        public Namespace WithPostNotification(Gs2.Gs2Chat.Model.NotificationSetting postNotification) {
            this.PostNotification = postNotification;
            return this;
        }

        public Namespace WithLogSetting(Gs2.Gs2Chat.Model.LogSetting logSetting) {
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

    	[Preserve]
        public static Namespace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithAllowCreateRoom(!data.Keys.Contains("allowCreateRoom") || data["allowCreateRoom"] == null ? null : (bool?)bool.Parse(data["allowCreateRoom"].ToString()))
                .WithPostMessageScript(!data.Keys.Contains("postMessageScript") || data["postMessageScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["postMessageScript"]))
                .WithCreateRoomScript(!data.Keys.Contains("createRoomScript") || data["createRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["createRoomScript"]))
                .WithDeleteRoomScript(!data.Keys.Contains("deleteRoomScript") || data["deleteRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["deleteRoomScript"]))
                .WithSubscribeRoomScript(!data.Keys.Contains("subscribeRoomScript") || data["subscribeRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["subscribeRoomScript"]))
                .WithUnsubscribeRoomScript(!data.Keys.Contains("unsubscribeRoomScript") || data["unsubscribeRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["unsubscribeRoomScript"]))
                .WithPostNotification(!data.Keys.Contains("postNotification") || data["postNotification"] == null ? null : Gs2.Gs2Chat.Model.NotificationSetting.FromJson(data["postNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Chat.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["allowCreateRoom"] = AllowCreateRoom,
                ["postMessageScript"] = PostMessageScript?.ToJson(),
                ["createRoomScript"] = CreateRoomScript?.ToJson(),
                ["deleteRoomScript"] = DeleteRoomScript?.ToJson(),
                ["subscribeRoomScript"] = SubscribeRoomScript?.ToJson(),
                ["unsubscribeRoomScript"] = UnsubscribeRoomScript?.ToJson(),
                ["postNotification"] = PostNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
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
            if (AllowCreateRoom != null) {
                writer.WritePropertyName("allowCreateRoom");
                writer.Write(bool.Parse(AllowCreateRoom.ToString()));
            }
            if (PostMessageScript != null) {
                writer.WritePropertyName("postMessageScript");
                PostMessageScript.WriteJson(writer);
            }
            if (CreateRoomScript != null) {
                writer.WritePropertyName("createRoomScript");
                CreateRoomScript.WriteJson(writer);
            }
            if (DeleteRoomScript != null) {
                writer.WritePropertyName("deleteRoomScript");
                DeleteRoomScript.WriteJson(writer);
            }
            if (SubscribeRoomScript != null) {
                writer.WritePropertyName("subscribeRoomScript");
                SubscribeRoomScript.WriteJson(writer);
            }
            if (UnsubscribeRoomScript != null) {
                writer.WritePropertyName("unsubscribeRoomScript");
                UnsubscribeRoomScript.WriteJson(writer);
            }
            if (PostNotification != null) {
                writer.WritePropertyName("postNotification");
                PostNotification.WriteJson(writer);
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
            if (AllowCreateRoom == null && AllowCreateRoom == other.AllowCreateRoom)
            {
                // null and null
            }
            else
            {
                diff += AllowCreateRoom == other.AllowCreateRoom ? 0 : 1;
            }
            if (PostMessageScript == null && PostMessageScript == other.PostMessageScript)
            {
                // null and null
            }
            else
            {
                diff += PostMessageScript.CompareTo(other.PostMessageScript);
            }
            if (CreateRoomScript == null && CreateRoomScript == other.CreateRoomScript)
            {
                // null and null
            }
            else
            {
                diff += CreateRoomScript.CompareTo(other.CreateRoomScript);
            }
            if (DeleteRoomScript == null && DeleteRoomScript == other.DeleteRoomScript)
            {
                // null and null
            }
            else
            {
                diff += DeleteRoomScript.CompareTo(other.DeleteRoomScript);
            }
            if (SubscribeRoomScript == null && SubscribeRoomScript == other.SubscribeRoomScript)
            {
                // null and null
            }
            else
            {
                diff += SubscribeRoomScript.CompareTo(other.SubscribeRoomScript);
            }
            if (UnsubscribeRoomScript == null && UnsubscribeRoomScript == other.UnsubscribeRoomScript)
            {
                // null and null
            }
            else
            {
                diff += UnsubscribeRoomScript.CompareTo(other.UnsubscribeRoomScript);
            }
            if (PostNotification == null && PostNotification == other.PostNotification)
            {
                // null and null
            }
            else
            {
                diff += PostNotification.CompareTo(other.PostNotification);
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
            return diff;
        }
    }
}