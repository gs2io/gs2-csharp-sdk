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
using Gs2.Gs2Matchmaking.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Matchmaking.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
        public string NamespaceName { set; get; }
        public string Description { set; get; }
        public bool? EnableRating { set; get; }
        public string CreateGatheringTriggerType { set; get; }
        public string CreateGatheringTriggerRealtimeNamespaceId { set; get; }
        public string CreateGatheringTriggerScriptId { set; get; }
        public string CompleteMatchmakingTriggerType { set; get; }
        public string CompleteMatchmakingTriggerRealtimeNamespaceId { set; get; }
        public string CompleteMatchmakingTriggerScriptId { set; get; }
        public Gs2.Gs2Matchmaking.Model.ScriptSetting ChangeRatingScript { set; get; }
        public Gs2.Gs2Matchmaking.Model.NotificationSetting JoinNotification { set; get; }
        public Gs2.Gs2Matchmaking.Model.NotificationSetting LeaveNotification { set; get; }
        public Gs2.Gs2Matchmaking.Model.NotificationSetting CompleteNotification { set; get; }
        public Gs2.Gs2Matchmaking.Model.LogSetting LogSetting { set; get; }
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithEnableRating(bool? enableRating) {
            this.EnableRating = enableRating;
            return this;
        }
        public UpdateNamespaceRequest WithCreateGatheringTriggerType(string createGatheringTriggerType) {
            this.CreateGatheringTriggerType = createGatheringTriggerType;
            return this;
        }
        public UpdateNamespaceRequest WithCreateGatheringTriggerRealtimeNamespaceId(string createGatheringTriggerRealtimeNamespaceId) {
            this.CreateGatheringTriggerRealtimeNamespaceId = createGatheringTriggerRealtimeNamespaceId;
            return this;
        }
        public UpdateNamespaceRequest WithCreateGatheringTriggerScriptId(string createGatheringTriggerScriptId) {
            this.CreateGatheringTriggerScriptId = createGatheringTriggerScriptId;
            return this;
        }
        public UpdateNamespaceRequest WithCompleteMatchmakingTriggerType(string completeMatchmakingTriggerType) {
            this.CompleteMatchmakingTriggerType = completeMatchmakingTriggerType;
            return this;
        }
        public UpdateNamespaceRequest WithCompleteMatchmakingTriggerRealtimeNamespaceId(string completeMatchmakingTriggerRealtimeNamespaceId) {
            this.CompleteMatchmakingTriggerRealtimeNamespaceId = completeMatchmakingTriggerRealtimeNamespaceId;
            return this;
        }
        public UpdateNamespaceRequest WithCompleteMatchmakingTriggerScriptId(string completeMatchmakingTriggerScriptId) {
            this.CompleteMatchmakingTriggerScriptId = completeMatchmakingTriggerScriptId;
            return this;
        }
        public UpdateNamespaceRequest WithChangeRatingScript(Gs2.Gs2Matchmaking.Model.ScriptSetting changeRatingScript) {
            this.ChangeRatingScript = changeRatingScript;
            return this;
        }
        public UpdateNamespaceRequest WithJoinNotification(Gs2.Gs2Matchmaking.Model.NotificationSetting joinNotification) {
            this.JoinNotification = joinNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLeaveNotification(Gs2.Gs2Matchmaking.Model.NotificationSetting leaveNotification) {
            this.LeaveNotification = leaveNotification;
            return this;
        }
        public UpdateNamespaceRequest WithCompleteNotification(Gs2.Gs2Matchmaking.Model.NotificationSetting completeNotification) {
            this.CompleteNotification = completeNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Matchmaking.Model.LogSetting logSetting) {
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
                .WithEnableRating(!data.Keys.Contains("enableRating") || data["enableRating"] == null ? null : (bool?)bool.Parse(data["enableRating"].ToString()))
                .WithCreateGatheringTriggerType(!data.Keys.Contains("createGatheringTriggerType") || data["createGatheringTriggerType"] == null ? null : data["createGatheringTriggerType"].ToString())
                .WithCreateGatheringTriggerRealtimeNamespaceId(!data.Keys.Contains("createGatheringTriggerRealtimeNamespaceId") || data["createGatheringTriggerRealtimeNamespaceId"] == null ? null : data["createGatheringTriggerRealtimeNamespaceId"].ToString())
                .WithCreateGatheringTriggerScriptId(!data.Keys.Contains("createGatheringTriggerScriptId") || data["createGatheringTriggerScriptId"] == null ? null : data["createGatheringTriggerScriptId"].ToString())
                .WithCompleteMatchmakingTriggerType(!data.Keys.Contains("completeMatchmakingTriggerType") || data["completeMatchmakingTriggerType"] == null ? null : data["completeMatchmakingTriggerType"].ToString())
                .WithCompleteMatchmakingTriggerRealtimeNamespaceId(!data.Keys.Contains("completeMatchmakingTriggerRealtimeNamespaceId") || data["completeMatchmakingTriggerRealtimeNamespaceId"] == null ? null : data["completeMatchmakingTriggerRealtimeNamespaceId"].ToString())
                .WithCompleteMatchmakingTriggerScriptId(!data.Keys.Contains("completeMatchmakingTriggerScriptId") || data["completeMatchmakingTriggerScriptId"] == null ? null : data["completeMatchmakingTriggerScriptId"].ToString())
                .WithChangeRatingScript(!data.Keys.Contains("changeRatingScript") || data["changeRatingScript"] == null ? null : Gs2.Gs2Matchmaking.Model.ScriptSetting.FromJson(data["changeRatingScript"]))
                .WithJoinNotification(!data.Keys.Contains("joinNotification") || data["joinNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["joinNotification"]))
                .WithLeaveNotification(!data.Keys.Contains("leaveNotification") || data["leaveNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["leaveNotification"]))
                .WithCompleteNotification(!data.Keys.Contains("completeNotification") || data["completeNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["completeNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Matchmaking.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["enableRating"] = EnableRating,
                ["createGatheringTriggerType"] = CreateGatheringTriggerType,
                ["createGatheringTriggerRealtimeNamespaceId"] = CreateGatheringTriggerRealtimeNamespaceId,
                ["createGatheringTriggerScriptId"] = CreateGatheringTriggerScriptId,
                ["completeMatchmakingTriggerType"] = CompleteMatchmakingTriggerType,
                ["completeMatchmakingTriggerRealtimeNamespaceId"] = CompleteMatchmakingTriggerRealtimeNamespaceId,
                ["completeMatchmakingTriggerScriptId"] = CompleteMatchmakingTriggerScriptId,
                ["changeRatingScript"] = ChangeRatingScript?.ToJson(),
                ["joinNotification"] = JoinNotification?.ToJson(),
                ["leaveNotification"] = LeaveNotification?.ToJson(),
                ["completeNotification"] = CompleteNotification?.ToJson(),
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
            if (EnableRating != null) {
                writer.WritePropertyName("enableRating");
                writer.Write(bool.Parse(EnableRating.ToString()));
            }
            if (CreateGatheringTriggerType != null) {
                writer.WritePropertyName("createGatheringTriggerType");
                writer.Write(CreateGatheringTriggerType.ToString());
            }
            if (CreateGatheringTriggerRealtimeNamespaceId != null) {
                writer.WritePropertyName("createGatheringTriggerRealtimeNamespaceId");
                writer.Write(CreateGatheringTriggerRealtimeNamespaceId.ToString());
            }
            if (CreateGatheringTriggerScriptId != null) {
                writer.WritePropertyName("createGatheringTriggerScriptId");
                writer.Write(CreateGatheringTriggerScriptId.ToString());
            }
            if (CompleteMatchmakingTriggerType != null) {
                writer.WritePropertyName("completeMatchmakingTriggerType");
                writer.Write(CompleteMatchmakingTriggerType.ToString());
            }
            if (CompleteMatchmakingTriggerRealtimeNamespaceId != null) {
                writer.WritePropertyName("completeMatchmakingTriggerRealtimeNamespaceId");
                writer.Write(CompleteMatchmakingTriggerRealtimeNamespaceId.ToString());
            }
            if (CompleteMatchmakingTriggerScriptId != null) {
                writer.WritePropertyName("completeMatchmakingTriggerScriptId");
                writer.Write(CompleteMatchmakingTriggerScriptId.ToString());
            }
            if (ChangeRatingScript != null) {
                ChangeRatingScript.WriteJson(writer);
            }
            if (JoinNotification != null) {
                JoinNotification.WriteJson(writer);
            }
            if (LeaveNotification != null) {
                LeaveNotification.WriteJson(writer);
            }
            if (CompleteNotification != null) {
                CompleteNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}