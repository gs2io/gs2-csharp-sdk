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
using Gs2.Gs2Friend.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Friend.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
        public string NamespaceName { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting FollowScript { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting UnfollowScript { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting SendRequestScript { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting CancelRequestScript { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting AcceptRequestScript { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting RejectRequestScript { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting DeleteFriendScript { set; get; }
        public Gs2.Gs2Friend.Model.ScriptSetting UpdateProfileScript { set; get; }
        public Gs2.Gs2Friend.Model.NotificationSetting FollowNotification { set; get; }
        public Gs2.Gs2Friend.Model.NotificationSetting ReceiveRequestNotification { set; get; }
        public Gs2.Gs2Friend.Model.NotificationSetting AcceptRequestNotification { set; get; }
        public Gs2.Gs2Friend.Model.LogSetting LogSetting { set; get; }
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithFollowScript(Gs2.Gs2Friend.Model.ScriptSetting followScript) {
            this.FollowScript = followScript;
            return this;
        }
        public UpdateNamespaceRequest WithUnfollowScript(Gs2.Gs2Friend.Model.ScriptSetting unfollowScript) {
            this.UnfollowScript = unfollowScript;
            return this;
        }
        public UpdateNamespaceRequest WithSendRequestScript(Gs2.Gs2Friend.Model.ScriptSetting sendRequestScript) {
            this.SendRequestScript = sendRequestScript;
            return this;
        }
        public UpdateNamespaceRequest WithCancelRequestScript(Gs2.Gs2Friend.Model.ScriptSetting cancelRequestScript) {
            this.CancelRequestScript = cancelRequestScript;
            return this;
        }
        public UpdateNamespaceRequest WithAcceptRequestScript(Gs2.Gs2Friend.Model.ScriptSetting acceptRequestScript) {
            this.AcceptRequestScript = acceptRequestScript;
            return this;
        }
        public UpdateNamespaceRequest WithRejectRequestScript(Gs2.Gs2Friend.Model.ScriptSetting rejectRequestScript) {
            this.RejectRequestScript = rejectRequestScript;
            return this;
        }
        public UpdateNamespaceRequest WithDeleteFriendScript(Gs2.Gs2Friend.Model.ScriptSetting deleteFriendScript) {
            this.DeleteFriendScript = deleteFriendScript;
            return this;
        }
        public UpdateNamespaceRequest WithUpdateProfileScript(Gs2.Gs2Friend.Model.ScriptSetting updateProfileScript) {
            this.UpdateProfileScript = updateProfileScript;
            return this;
        }
        public UpdateNamespaceRequest WithFollowNotification(Gs2.Gs2Friend.Model.NotificationSetting followNotification) {
            this.FollowNotification = followNotification;
            return this;
        }
        public UpdateNamespaceRequest WithReceiveRequestNotification(Gs2.Gs2Friend.Model.NotificationSetting receiveRequestNotification) {
            this.ReceiveRequestNotification = receiveRequestNotification;
            return this;
        }
        public UpdateNamespaceRequest WithAcceptRequestNotification(Gs2.Gs2Friend.Model.NotificationSetting acceptRequestNotification) {
            this.AcceptRequestNotification = acceptRequestNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Friend.Model.LogSetting logSetting) {
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
                .WithAcceptRequestNotification(!data.Keys.Contains("acceptRequestNotification") || data["acceptRequestNotification"] == null ? null : Gs2.Gs2Friend.Model.NotificationSetting.FromJson(data["acceptRequestNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Friend.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
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
                ["acceptRequestNotification"] = AcceptRequestNotification?.ToJson(),
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
            if (FollowScript != null) {
                FollowScript.WriteJson(writer);
            }
            if (UnfollowScript != null) {
                UnfollowScript.WriteJson(writer);
            }
            if (SendRequestScript != null) {
                SendRequestScript.WriteJson(writer);
            }
            if (CancelRequestScript != null) {
                CancelRequestScript.WriteJson(writer);
            }
            if (AcceptRequestScript != null) {
                AcceptRequestScript.WriteJson(writer);
            }
            if (RejectRequestScript != null) {
                RejectRequestScript.WriteJson(writer);
            }
            if (DeleteFriendScript != null) {
                DeleteFriendScript.WriteJson(writer);
            }
            if (UpdateProfileScript != null) {
                UpdateProfileScript.WriteJson(writer);
            }
            if (FollowNotification != null) {
                FollowNotification.WriteJson(writer);
            }
            if (ReceiveRequestNotification != null) {
                ReceiveRequestNotification.WriteJson(writer);
            }
            if (AcceptRequestNotification != null) {
                AcceptRequestNotification.WriteJson(writer);
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
            key += FollowScript + ":";
            key += UnfollowScript + ":";
            key += SendRequestScript + ":";
            key += CancelRequestScript + ":";
            key += AcceptRequestScript + ":";
            key += RejectRequestScript + ":";
            key += DeleteFriendScript + ":";
            key += UpdateProfileScript + ":";
            key += FollowNotification + ":";
            key += ReceiveRequestNotification + ":";
            key += AcceptRequestNotification + ":";
            key += LogSetting + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateNamespaceRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateNamespaceRequest)x;
            return this;
        }
    }
}