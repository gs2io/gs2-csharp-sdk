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
using Gs2.Gs2Inbox.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateGlobalMessageMasterRequest : Gs2Request<UpdateGlobalMessageMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GlobalMessageName { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public Gs2.Core.Model.AcquireAction[] ReadAcquireActions { set; get; } = null!;
         public Gs2.Gs2Inbox.Model.TimeSpan_ ExpiresTimeSpan { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public long? ExpiresAt { set; get; } = null!;
         public string MessageReceptionPeriodEventId { set; get; } = null!;
        public UpdateGlobalMessageMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateGlobalMessageMasterRequest WithGlobalMessageName(string globalMessageName) {
            this.GlobalMessageName = globalMessageName;
            return this;
        }
        public UpdateGlobalMessageMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateGlobalMessageMasterRequest WithReadAcquireActions(Gs2.Core.Model.AcquireAction[] readAcquireActions) {
            this.ReadAcquireActions = readAcquireActions;
            return this;
        }
        public UpdateGlobalMessageMasterRequest WithExpiresTimeSpan(Gs2.Gs2Inbox.Model.TimeSpan_ expiresTimeSpan) {
            this.ExpiresTimeSpan = expiresTimeSpan;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public UpdateGlobalMessageMasterRequest WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }
        public UpdateGlobalMessageMasterRequest WithMessageReceptionPeriodEventId(string messageReceptionPeriodEventId) {
            this.MessageReceptionPeriodEventId = messageReceptionPeriodEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateGlobalMessageMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateGlobalMessageMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGlobalMessageName(!data.Keys.Contains("globalMessageName") || data["globalMessageName"] == null ? null : data["globalMessageName"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithReadAcquireActions(!data.Keys.Contains("readAcquireActions") || data["readAcquireActions"] == null || !data["readAcquireActions"].IsArray ? null : data["readAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithExpiresTimeSpan(!data.Keys.Contains("expiresTimeSpan") || data["expiresTimeSpan"] == null ? null : Gs2.Gs2Inbox.Model.TimeSpan_.FromJson(data["expiresTimeSpan"]))
                .WithMessageReceptionPeriodEventId(!data.Keys.Contains("messageReceptionPeriodEventId") || data["messageReceptionPeriodEventId"] == null ? null : data["messageReceptionPeriodEventId"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData readAcquireActionsJsonData = null;
            if (ReadAcquireActions != null && ReadAcquireActions.Length > 0)
            {
                readAcquireActionsJsonData = new JsonData();
                foreach (var readAcquireAction in ReadAcquireActions)
                {
                    readAcquireActionsJsonData.Add(readAcquireAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["globalMessageName"] = GlobalMessageName,
                ["metadata"] = Metadata,
                ["readAcquireActions"] = readAcquireActionsJsonData,
                ["expiresTimeSpan"] = ExpiresTimeSpan?.ToJson(),
                ["messageReceptionPeriodEventId"] = MessageReceptionPeriodEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (GlobalMessageName != null) {
                writer.WritePropertyName("globalMessageName");
                writer.Write(GlobalMessageName.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ReadAcquireActions != null) {
                writer.WritePropertyName("readAcquireActions");
                writer.WriteArrayStart();
                foreach (var readAcquireAction in ReadAcquireActions)
                {
                    if (readAcquireAction != null) {
                        readAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ExpiresTimeSpan != null) {
                ExpiresTimeSpan.WriteJson(writer);
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (MessageReceptionPeriodEventId != null) {
                writer.WritePropertyName("messageReceptionPeriodEventId");
                writer.Write(MessageReceptionPeriodEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GlobalMessageName + ":";
            key += Metadata + ":";
            key += ReadAcquireActions + ":";
            key += ExpiresTimeSpan + ":";
            key += MessageReceptionPeriodEventId + ":";
            return key;
        }
    }
}