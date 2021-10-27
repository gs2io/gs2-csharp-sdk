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
using Gs2.Gs2Chat.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Chat.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
        public string NamespaceName { set; get; }
        public string Description { set; get; }
        public bool? AllowCreateRoom { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting PostMessageScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting CreateRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting DeleteRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting SubscribeRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.ScriptSetting UnsubscribeRoomScript { set; get; }
        public Gs2.Gs2Chat.Model.NotificationSetting PostNotification { set; get; }
        public Gs2.Gs2Chat.Model.LogSetting LogSetting { set; get; }

        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdateNamespaceRequest WithAllowCreateRoom(bool? allowCreateRoom) {
            this.AllowCreateRoom = allowCreateRoom;
            return this;
        }

        public UpdateNamespaceRequest WithPostMessageScript(Gs2.Gs2Chat.Model.ScriptSetting postMessageScript) {
            this.PostMessageScript = postMessageScript;
            return this;
        }

        public UpdateNamespaceRequest WithCreateRoomScript(Gs2.Gs2Chat.Model.ScriptSetting createRoomScript) {
            this.CreateRoomScript = createRoomScript;
            return this;
        }

        public UpdateNamespaceRequest WithDeleteRoomScript(Gs2.Gs2Chat.Model.ScriptSetting deleteRoomScript) {
            this.DeleteRoomScript = deleteRoomScript;
            return this;
        }

        public UpdateNamespaceRequest WithSubscribeRoomScript(Gs2.Gs2Chat.Model.ScriptSetting subscribeRoomScript) {
            this.SubscribeRoomScript = subscribeRoomScript;
            return this;
        }

        public UpdateNamespaceRequest WithUnsubscribeRoomScript(Gs2.Gs2Chat.Model.ScriptSetting unsubscribeRoomScript) {
            this.UnsubscribeRoomScript = unsubscribeRoomScript;
            return this;
        }

        public UpdateNamespaceRequest WithPostNotification(Gs2.Gs2Chat.Model.NotificationSetting postNotification) {
            this.PostNotification = postNotification;
            return this;
        }

        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Chat.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateNamespaceRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithAllowCreateRoom(!data.Keys.Contains("allowCreateRoom") || data["allowCreateRoom"] == null ? null : (bool?)bool.Parse(data["allowCreateRoom"].ToString()))
                .WithPostMessageScript(!data.Keys.Contains("postMessageScript") || data["postMessageScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["postMessageScript"]))
                .WithCreateRoomScript(!data.Keys.Contains("createRoomScript") || data["createRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["createRoomScript"]))
                .WithDeleteRoomScript(!data.Keys.Contains("deleteRoomScript") || data["deleteRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["deleteRoomScript"]))
                .WithSubscribeRoomScript(!data.Keys.Contains("subscribeRoomScript") || data["subscribeRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["subscribeRoomScript"]))
                .WithUnsubscribeRoomScript(!data.Keys.Contains("unsubscribeRoomScript") || data["unsubscribeRoomScript"] == null ? null : Gs2.Gs2Chat.Model.ScriptSetting.FromJson(data["unsubscribeRoomScript"]))
                .WithPostNotification(!data.Keys.Contains("postNotification") || data["postNotification"] == null ? null : Gs2.Gs2Chat.Model.NotificationSetting.FromJson(data["postNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Chat.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["allowCreateRoom"] = AllowCreateRoom,
                ["postMessageScript"] = PostMessageScript?.ToJson(),
                ["createRoomScript"] = CreateRoomScript?.ToJson(),
                ["deleteRoomScript"] = DeleteRoomScript?.ToJson(),
                ["subscribeRoomScript"] = SubscribeRoomScript?.ToJson(),
                ["unsubscribeRoomScript"] = UnsubscribeRoomScript?.ToJson(),
                ["postNotification"] = PostNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
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
                PostMessageScript.WriteJson(writer);
            }
            if (CreateRoomScript != null) {
                CreateRoomScript.WriteJson(writer);
            }
            if (DeleteRoomScript != null) {
                DeleteRoomScript.WriteJson(writer);
            }
            if (SubscribeRoomScript != null) {
                SubscribeRoomScript.WriteJson(writer);
            }
            if (UnsubscribeRoomScript != null) {
                UnsubscribeRoomScript.WriteJson(writer);
            }
            if (PostNotification != null) {
                PostNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}