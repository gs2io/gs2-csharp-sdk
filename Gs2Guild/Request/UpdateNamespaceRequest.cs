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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Guild.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Guild.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2Guild.Model.TransactionSetting TransactionSetting { set; get; } = null!;
         public Gs2.Gs2Guild.Model.NotificationSetting ChangeNotification { set; get; } = null!;
         public Gs2.Gs2Guild.Model.NotificationSetting JoinNotification { set; get; } = null!;
         public Gs2.Gs2Guild.Model.NotificationSetting LeaveNotification { set; get; } = null!;
         public Gs2.Gs2Guild.Model.NotificationSetting ChangeMemberNotification { set; get; } = null!;
         public bool? ChangeMemberNotificationIgnoreChangeMetadata { set; get; } = null!;
         public Gs2.Gs2Guild.Model.NotificationSetting ReceiveRequestNotification { set; get; } = null!;
         public Gs2.Gs2Guild.Model.NotificationSetting RemoveRequestNotification { set; get; } = null!;
         public Gs2.Gs2Guild.Model.ScriptSetting CreateGuildScript { set; get; } = null!;
         public Gs2.Gs2Guild.Model.ScriptSetting UpdateGuildScript { set; get; } = null!;
         public Gs2.Gs2Guild.Model.ScriptSetting JoinGuildScript { set; get; } = null!;
         public Gs2.Gs2Guild.Model.ScriptSetting ReceiveJoinRequestScript { set; get; } = null!;
         public Gs2.Gs2Guild.Model.ScriptSetting LeaveGuildScript { set; get; } = null!;
         public Gs2.Gs2Guild.Model.ScriptSetting ChangeRoleScript { set; get; } = null!;
         public Gs2.Gs2Guild.Model.ScriptSetting DeleteGuildScript { set; get; } = null!;
         public Gs2.Gs2Guild.Model.LogSetting LogSetting { set; get; } = null!;
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithTransactionSetting(Gs2.Gs2Guild.Model.TransactionSetting transactionSetting) {
            this.TransactionSetting = transactionSetting;
            return this;
        }
        public UpdateNamespaceRequest WithChangeNotification(Gs2.Gs2Guild.Model.NotificationSetting changeNotification) {
            this.ChangeNotification = changeNotification;
            return this;
        }
        public UpdateNamespaceRequest WithJoinNotification(Gs2.Gs2Guild.Model.NotificationSetting joinNotification) {
            this.JoinNotification = joinNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLeaveNotification(Gs2.Gs2Guild.Model.NotificationSetting leaveNotification) {
            this.LeaveNotification = leaveNotification;
            return this;
        }
        public UpdateNamespaceRequest WithChangeMemberNotification(Gs2.Gs2Guild.Model.NotificationSetting changeMemberNotification) {
            this.ChangeMemberNotification = changeMemberNotification;
            return this;
        }
        public UpdateNamespaceRequest WithChangeMemberNotificationIgnoreChangeMetadata(bool? changeMemberNotificationIgnoreChangeMetadata) {
            this.ChangeMemberNotificationIgnoreChangeMetadata = changeMemberNotificationIgnoreChangeMetadata;
            return this;
        }
        public UpdateNamespaceRequest WithReceiveRequestNotification(Gs2.Gs2Guild.Model.NotificationSetting receiveRequestNotification) {
            this.ReceiveRequestNotification = receiveRequestNotification;
            return this;
        }
        public UpdateNamespaceRequest WithRemoveRequestNotification(Gs2.Gs2Guild.Model.NotificationSetting removeRequestNotification) {
            this.RemoveRequestNotification = removeRequestNotification;
            return this;
        }
        public UpdateNamespaceRequest WithCreateGuildScript(Gs2.Gs2Guild.Model.ScriptSetting createGuildScript) {
            this.CreateGuildScript = createGuildScript;
            return this;
        }
        public UpdateNamespaceRequest WithUpdateGuildScript(Gs2.Gs2Guild.Model.ScriptSetting updateGuildScript) {
            this.UpdateGuildScript = updateGuildScript;
            return this;
        }
        public UpdateNamespaceRequest WithJoinGuildScript(Gs2.Gs2Guild.Model.ScriptSetting joinGuildScript) {
            this.JoinGuildScript = joinGuildScript;
            return this;
        }
        public UpdateNamespaceRequest WithReceiveJoinRequestScript(Gs2.Gs2Guild.Model.ScriptSetting receiveJoinRequestScript) {
            this.ReceiveJoinRequestScript = receiveJoinRequestScript;
            return this;
        }
        public UpdateNamespaceRequest WithLeaveGuildScript(Gs2.Gs2Guild.Model.ScriptSetting leaveGuildScript) {
            this.LeaveGuildScript = leaveGuildScript;
            return this;
        }
        public UpdateNamespaceRequest WithChangeRoleScript(Gs2.Gs2Guild.Model.ScriptSetting changeRoleScript) {
            this.ChangeRoleScript = changeRoleScript;
            return this;
        }
        public UpdateNamespaceRequest WithDeleteGuildScript(Gs2.Gs2Guild.Model.ScriptSetting deleteGuildScript) {
            this.DeleteGuildScript = deleteGuildScript;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Guild.Model.LogSetting logSetting) {
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
                .WithTransactionSetting(!data.Keys.Contains("transactionSetting") || data["transactionSetting"] == null ? null : Gs2.Gs2Guild.Model.TransactionSetting.FromJson(data["transactionSetting"]))
                .WithChangeNotification(!data.Keys.Contains("changeNotification") || data["changeNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["changeNotification"]))
                .WithJoinNotification(!data.Keys.Contains("joinNotification") || data["joinNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["joinNotification"]))
                .WithLeaveNotification(!data.Keys.Contains("leaveNotification") || data["leaveNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["leaveNotification"]))
                .WithChangeMemberNotification(!data.Keys.Contains("changeMemberNotification") || data["changeMemberNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["changeMemberNotification"]))
                .WithChangeMemberNotificationIgnoreChangeMetadata(!data.Keys.Contains("changeMemberNotificationIgnoreChangeMetadata") || data["changeMemberNotificationIgnoreChangeMetadata"] == null ? null : (bool?)bool.Parse(data["changeMemberNotificationIgnoreChangeMetadata"].ToString()))
                .WithReceiveRequestNotification(!data.Keys.Contains("receiveRequestNotification") || data["receiveRequestNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["receiveRequestNotification"]))
                .WithRemoveRequestNotification(!data.Keys.Contains("removeRequestNotification") || data["removeRequestNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["removeRequestNotification"]))
                .WithCreateGuildScript(!data.Keys.Contains("createGuildScript") || data["createGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["createGuildScript"]))
                .WithUpdateGuildScript(!data.Keys.Contains("updateGuildScript") || data["updateGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["updateGuildScript"]))
                .WithJoinGuildScript(!data.Keys.Contains("joinGuildScript") || data["joinGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["joinGuildScript"]))
                .WithReceiveJoinRequestScript(!data.Keys.Contains("receiveJoinRequestScript") || data["receiveJoinRequestScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["receiveJoinRequestScript"]))
                .WithLeaveGuildScript(!data.Keys.Contains("leaveGuildScript") || data["leaveGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["leaveGuildScript"]))
                .WithChangeRoleScript(!data.Keys.Contains("changeRoleScript") || data["changeRoleScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["changeRoleScript"]))
                .WithDeleteGuildScript(!data.Keys.Contains("deleteGuildScript") || data["deleteGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["deleteGuildScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Guild.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["transactionSetting"] = TransactionSetting?.ToJson(),
                ["changeNotification"] = ChangeNotification?.ToJson(),
                ["joinNotification"] = JoinNotification?.ToJson(),
                ["leaveNotification"] = LeaveNotification?.ToJson(),
                ["changeMemberNotification"] = ChangeMemberNotification?.ToJson(),
                ["changeMemberNotificationIgnoreChangeMetadata"] = ChangeMemberNotificationIgnoreChangeMetadata,
                ["receiveRequestNotification"] = ReceiveRequestNotification?.ToJson(),
                ["removeRequestNotification"] = RemoveRequestNotification?.ToJson(),
                ["createGuildScript"] = CreateGuildScript?.ToJson(),
                ["updateGuildScript"] = UpdateGuildScript?.ToJson(),
                ["joinGuildScript"] = JoinGuildScript?.ToJson(),
                ["receiveJoinRequestScript"] = ReceiveJoinRequestScript?.ToJson(),
                ["leaveGuildScript"] = LeaveGuildScript?.ToJson(),
                ["changeRoleScript"] = ChangeRoleScript?.ToJson(),
                ["deleteGuildScript"] = DeleteGuildScript?.ToJson(),
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
            if (TransactionSetting != null) {
                TransactionSetting.WriteJson(writer);
            }
            if (ChangeNotification != null) {
                ChangeNotification.WriteJson(writer);
            }
            if (JoinNotification != null) {
                JoinNotification.WriteJson(writer);
            }
            if (LeaveNotification != null) {
                LeaveNotification.WriteJson(writer);
            }
            if (ChangeMemberNotification != null) {
                ChangeMemberNotification.WriteJson(writer);
            }
            if (ChangeMemberNotificationIgnoreChangeMetadata != null) {
                writer.WritePropertyName("changeMemberNotificationIgnoreChangeMetadata");
                writer.Write(bool.Parse(ChangeMemberNotificationIgnoreChangeMetadata.ToString()));
            }
            if (ReceiveRequestNotification != null) {
                ReceiveRequestNotification.WriteJson(writer);
            }
            if (RemoveRequestNotification != null) {
                RemoveRequestNotification.WriteJson(writer);
            }
            if (CreateGuildScript != null) {
                CreateGuildScript.WriteJson(writer);
            }
            if (UpdateGuildScript != null) {
                UpdateGuildScript.WriteJson(writer);
            }
            if (JoinGuildScript != null) {
                JoinGuildScript.WriteJson(writer);
            }
            if (ReceiveJoinRequestScript != null) {
                ReceiveJoinRequestScript.WriteJson(writer);
            }
            if (LeaveGuildScript != null) {
                LeaveGuildScript.WriteJson(writer);
            }
            if (ChangeRoleScript != null) {
                ChangeRoleScript.WriteJson(writer);
            }
            if (DeleteGuildScript != null) {
                DeleteGuildScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Description + ":";
            key += TransactionSetting + ":";
            key += ChangeNotification + ":";
            key += JoinNotification + ":";
            key += LeaveNotification + ":";
            key += ChangeMemberNotification + ":";
            key += ChangeMemberNotificationIgnoreChangeMetadata + ":";
            key += ReceiveRequestNotification + ":";
            key += RemoveRequestNotification + ":";
            key += CreateGuildScript + ":";
            key += UpdateGuildScript + ":";
            key += JoinGuildScript + ":";
            key += ReceiveJoinRequestScript + ":";
            key += LeaveGuildScript + ":";
            key += ChangeRoleScript + ":";
            key += DeleteGuildScript + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}