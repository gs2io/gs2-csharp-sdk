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
using Gs2.Gs2Distributor.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Distributor.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string AssumeUserId { set; get; } = null!;
         public Gs2.Gs2Distributor.Model.NotificationSetting AutoRunStampSheetNotification { set; get; } = null!;
         public Gs2.Gs2Distributor.Model.NotificationSetting AutoRunTransactionNotification { set; get; } = null!;
         public Gs2.Gs2Distributor.Model.LogSetting LogSetting { set; get; } = null!;
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithAssumeUserId(string assumeUserId) {
            this.AssumeUserId = assumeUserId;
            return this;
        }
        public UpdateNamespaceRequest WithAutoRunStampSheetNotification(Gs2.Gs2Distributor.Model.NotificationSetting autoRunStampSheetNotification) {
            this.AutoRunStampSheetNotification = autoRunStampSheetNotification;
            return this;
        }
        public UpdateNamespaceRequest WithAutoRunTransactionNotification(Gs2.Gs2Distributor.Model.NotificationSetting autoRunTransactionNotification) {
            this.AutoRunTransactionNotification = autoRunTransactionNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2Distributor.Model.LogSetting logSetting) {
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
                .WithAssumeUserId(!data.Keys.Contains("assumeUserId") || data["assumeUserId"] == null ? null : data["assumeUserId"].ToString())
                .WithAutoRunStampSheetNotification(!data.Keys.Contains("autoRunStampSheetNotification") || data["autoRunStampSheetNotification"] == null ? null : Gs2.Gs2Distributor.Model.NotificationSetting.FromJson(data["autoRunStampSheetNotification"]))
                .WithAutoRunTransactionNotification(!data.Keys.Contains("autoRunTransactionNotification") || data["autoRunTransactionNotification"] == null ? null : Gs2.Gs2Distributor.Model.NotificationSetting.FromJson(data["autoRunTransactionNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Distributor.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["assumeUserId"] = AssumeUserId,
                ["autoRunStampSheetNotification"] = AutoRunStampSheetNotification?.ToJson(),
                ["autoRunTransactionNotification"] = AutoRunTransactionNotification?.ToJson(),
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
            if (AssumeUserId != null) {
                writer.WritePropertyName("assumeUserId");
                writer.Write(AssumeUserId.ToString());
            }
            if (AutoRunStampSheetNotification != null) {
                AutoRunStampSheetNotification.WriteJson(writer);
            }
            if (AutoRunTransactionNotification != null) {
                AutoRunTransactionNotification.WriteJson(writer);
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
            key += AssumeUserId + ":";
            key += AutoRunStampSheetNotification + ":";
            key += AutoRunTransactionNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}