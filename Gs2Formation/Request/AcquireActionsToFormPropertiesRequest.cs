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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AcquireActionsToFormPropertiesRequest : Gs2Request<AcquireActionsToFormPropertiesRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string MoldName { set; get; }
        public int? Index { set; get; }
        public Gs2.Gs2Formation.Model.AcquireAction AcquireAction { set; get; }
        public string QueueNamespaceId { set; get; }
        public string KeyId { set; get; }
        public Gs2.Gs2Formation.Model.AcquireActionConfig[] Config { set; get; }
        public string DuplicationAvoider { set; get; }

        public AcquireActionsToFormPropertiesRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithMoldName(string moldName) {
            this.MoldName = moldName;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithIndex(int? index) {
            this.Index = index;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithAcquireAction(Gs2.Gs2Formation.Model.AcquireAction acquireAction) {
            this.AcquireAction = acquireAction;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithConfig(Gs2.Gs2Formation.Model.AcquireActionConfig[] config) {
            this.Config = config;
            return this;
        }

        public AcquireActionsToFormPropertiesRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireActionsToFormPropertiesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireActionsToFormPropertiesRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMoldName(!data.Keys.Contains("moldName") || data["moldName"] == null ? null : data["moldName"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : (int?)int.Parse(data["index"].ToString()))
                .WithAcquireAction(!data.Keys.Contains("acquireAction") || data["acquireAction"] == null ? null : Gs2.Gs2Formation.Model.AcquireAction.FromJson(data["acquireAction"]))
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2Formation.Model.AcquireActionConfig[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Formation.Model.AcquireActionConfig.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["moldName"] = MoldName,
                ["index"] = Index,
                ["acquireAction"] = AcquireAction?.ToJson(),
                ["queueNamespaceId"] = QueueNamespaceId,
                ["keyId"] = KeyId,
                ["config"] = new JsonData(Config == null ? new JsonData[]{} :
                        Config.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (MoldName != null) {
                writer.WritePropertyName("moldName");
                writer.Write(MoldName.ToString());
            }
            if (Index != null) {
                writer.WritePropertyName("index");
                writer.Write(int.Parse(Index.ToString()));
            }
            if (AcquireAction != null) {
                AcquireAction.WriteJson(writer);
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var confi in Config)
            {
                if (confi != null) {
                    confi.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}