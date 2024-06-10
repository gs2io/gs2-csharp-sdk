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
using Gs2.Gs2JobQueue.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2JobQueue.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public bool? EnableAutoRun { set; get; } = null!;
         public Gs2.Gs2JobQueue.Model.NotificationSetting PushNotification { set; get; } = null!;
         public Gs2.Gs2JobQueue.Model.NotificationSetting RunNotification { set; get; } = null!;
         public Gs2.Gs2JobQueue.Model.LogSetting LogSetting { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithEnableAutoRun(bool? enableAutoRun) {
            this.EnableAutoRun = enableAutoRun;
            return this;
        }
        public CreateNamespaceRequest WithPushNotification(Gs2.Gs2JobQueue.Model.NotificationSetting pushNotification) {
            this.PushNotification = pushNotification;
            return this;
        }
        public CreateNamespaceRequest WithRunNotification(Gs2.Gs2JobQueue.Model.NotificationSetting runNotification) {
            this.RunNotification = runNotification;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2JobQueue.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateNamespaceRequest()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithEnableAutoRun(!data.Keys.Contains("enableAutoRun") || data["enableAutoRun"] == null ? null : (bool?)bool.Parse(data["enableAutoRun"].ToString()))
                .WithPushNotification(!data.Keys.Contains("pushNotification") || data["pushNotification"] == null ? null : Gs2.Gs2JobQueue.Model.NotificationSetting.FromJson(data["pushNotification"]))
                .WithRunNotification(!data.Keys.Contains("runNotification") || data["runNotification"] == null ? null : Gs2.Gs2JobQueue.Model.NotificationSetting.FromJson(data["runNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2JobQueue.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["enableAutoRun"] = EnableAutoRun,
                ["pushNotification"] = PushNotification?.ToJson(),
                ["runNotification"] = RunNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (EnableAutoRun != null) {
                writer.WritePropertyName("enableAutoRun");
                writer.Write(bool.Parse(EnableAutoRun.ToString()));
            }
            if (PushNotification != null) {
                PushNotification.WriteJson(writer);
            }
            if (RunNotification != null) {
                RunNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Name + ":";
            key += Description + ":";
            key += EnableAutoRun + ":";
            key += PushNotification + ":";
            key += RunNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}