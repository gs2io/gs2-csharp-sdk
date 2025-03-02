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
using Gs2.Gs2Money2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateStoreSubscriptionContentModelMasterRequest : Gs2Request<UpdateStoreSubscriptionContentModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ContentName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string ScheduleNamespaceId { set; get; } = null!;
         public string TriggerName { set; get; } = null!;
         public Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent AppleAppStore { set; get; } = null!;
         public Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent GooglePlay { set; get; } = null!;
        public UpdateStoreSubscriptionContentModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateStoreSubscriptionContentModelMasterRequest WithContentName(string contentName) {
            this.ContentName = contentName;
            return this;
        }
        public UpdateStoreSubscriptionContentModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateStoreSubscriptionContentModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateStoreSubscriptionContentModelMasterRequest WithScheduleNamespaceId(string scheduleNamespaceId) {
            this.ScheduleNamespaceId = scheduleNamespaceId;
            return this;
        }
        public UpdateStoreSubscriptionContentModelMasterRequest WithTriggerName(string triggerName) {
            this.TriggerName = triggerName;
            return this;
        }
        public UpdateStoreSubscriptionContentModelMasterRequest WithAppleAppStore(Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent appleAppStore) {
            this.AppleAppStore = appleAppStore;
            return this;
        }
        public UpdateStoreSubscriptionContentModelMasterRequest WithGooglePlay(Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent googlePlay) {
            this.GooglePlay = googlePlay;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateStoreSubscriptionContentModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateStoreSubscriptionContentModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithContentName(!data.Keys.Contains("contentName") || data["contentName"] == null ? null : data["contentName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleNamespaceId(!data.Keys.Contains("scheduleNamespaceId") || data["scheduleNamespaceId"] == null ? null : data["scheduleNamespaceId"].ToString())
                .WithTriggerName(!data.Keys.Contains("triggerName") || data["triggerName"] == null ? null : data["triggerName"].ToString())
                .WithAppleAppStore(!data.Keys.Contains("appleAppStore") || data["appleAppStore"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent.FromJson(data["appleAppStore"]))
                .WithGooglePlay(!data.Keys.Contains("googlePlay") || data["googlePlay"] == null ? null : Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent.FromJson(data["googlePlay"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["contentName"] = ContentName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["scheduleNamespaceId"] = ScheduleNamespaceId,
                ["triggerName"] = TriggerName,
                ["appleAppStore"] = AppleAppStore?.ToJson(),
                ["googlePlay"] = GooglePlay?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (ContentName != null) {
                writer.WritePropertyName("contentName");
                writer.Write(ContentName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ScheduleNamespaceId != null) {
                writer.WritePropertyName("scheduleNamespaceId");
                writer.Write(ScheduleNamespaceId.ToString());
            }
            if (TriggerName != null) {
                writer.WritePropertyName("triggerName");
                writer.Write(TriggerName.ToString());
            }
            if (AppleAppStore != null) {
                AppleAppStore.WriteJson(writer);
            }
            if (GooglePlay != null) {
                GooglePlay.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ContentName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ScheduleNamespaceId + ":";
            key += TriggerName + ":";
            key += AppleAppStore + ":";
            key += GooglePlay + ":";
            return key;
        }
    }
}