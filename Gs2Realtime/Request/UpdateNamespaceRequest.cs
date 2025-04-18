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
using Gs2.Gs2Realtime.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Realtime.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string ServerType { set; get; } = null!;
         public string ServerSpec { set; get; } = null!;
         public Gs2.Gs2Realtime.Model.NotificationSetting CreateNotification { set; get; } = null!;
         public Gs2.Gs2Realtime.Model.LogSetting LogSetting { set; get; } = null!;
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithServerType(string serverType) {
            this.ServerType = serverType;
            return this;
        }
        public UpdateNamespaceRequest WithServerSpec(string serverSpec) {
            this.ServerSpec = serverSpec;
            return this;
        }
        public UpdateNamespaceRequest WithCreateNotification(Gs2.Gs2Realtime.Model.NotificationSetting createNotification) {
            this.CreateNotification = createNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Realtime.Model.LogSetting logSetting) {
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
                .WithServerType(!data.Keys.Contains("serverType") || data["serverType"] == null ? null : data["serverType"].ToString())
                .WithServerSpec(!data.Keys.Contains("serverSpec") || data["serverSpec"] == null ? null : data["serverSpec"].ToString())
                .WithCreateNotification(!data.Keys.Contains("createNotification") || data["createNotification"] == null ? null : Gs2.Gs2Realtime.Model.NotificationSetting.FromJson(data["createNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Realtime.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["serverType"] = ServerType,
                ["serverSpec"] = ServerSpec,
                ["createNotification"] = CreateNotification?.ToJson(),
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
            if (ServerType != null) {
                writer.WritePropertyName("serverType");
                writer.Write(ServerType.ToString());
            }
            if (ServerSpec != null) {
                writer.WritePropertyName("serverSpec");
                writer.Write(ServerSpec.ToString());
            }
            if (CreateNotification != null) {
                CreateNotification.WriteJson(writer);
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
            key += ServerType + ":";
            key += ServerSpec + ":";
            key += CreateNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}