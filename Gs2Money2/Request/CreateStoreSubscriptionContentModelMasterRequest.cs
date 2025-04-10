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
	public class CreateStoreSubscriptionContentModelMasterRequest : Gs2Request<CreateStoreSubscriptionContentModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string ScheduleNamespaceId { set; get; } = null!;
         public string TriggerName { set; get; } = null!;
         public string TriggerExtendMode { set; get; } = null!;
         public int? RollupHour { set; get; } = null!;
         public int? ReallocateSpanDays { set; get; } = null!;
         public Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent AppleAppStore { set; get; } = null!;
         public Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent GooglePlay { set; get; } = null!;
        public CreateStoreSubscriptionContentModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithScheduleNamespaceId(string scheduleNamespaceId) {
            this.ScheduleNamespaceId = scheduleNamespaceId;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithTriggerName(string triggerName) {
            this.TriggerName = triggerName;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithTriggerExtendMode(string triggerExtendMode) {
            this.TriggerExtendMode = triggerExtendMode;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithRollupHour(int? rollupHour) {
            this.RollupHour = rollupHour;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithReallocateSpanDays(int? reallocateSpanDays) {
            this.ReallocateSpanDays = reallocateSpanDays;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithAppleAppStore(Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent appleAppStore) {
            this.AppleAppStore = appleAppStore;
            return this;
        }
        public CreateStoreSubscriptionContentModelMasterRequest WithGooglePlay(Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent googlePlay) {
            this.GooglePlay = googlePlay;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateStoreSubscriptionContentModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateStoreSubscriptionContentModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleNamespaceId(!data.Keys.Contains("scheduleNamespaceId") || data["scheduleNamespaceId"] == null ? null : data["scheduleNamespaceId"].ToString())
                .WithTriggerName(!data.Keys.Contains("triggerName") || data["triggerName"] == null ? null : data["triggerName"].ToString())
                .WithTriggerExtendMode(!data.Keys.Contains("triggerExtendMode") || data["triggerExtendMode"] == null ? null : data["triggerExtendMode"].ToString())
                .WithRollupHour(!data.Keys.Contains("rollupHour") || data["rollupHour"] == null ? null : (int?)(data["rollupHour"].ToString().Contains(".") ? (int)double.Parse(data["rollupHour"].ToString()) : int.Parse(data["rollupHour"].ToString())))
                .WithReallocateSpanDays(!data.Keys.Contains("reallocateSpanDays") || data["reallocateSpanDays"] == null ? null : (int?)(data["reallocateSpanDays"].ToString().Contains(".") ? (int)double.Parse(data["reallocateSpanDays"].ToString()) : int.Parse(data["reallocateSpanDays"].ToString())))
                .WithAppleAppStore(!data.Keys.Contains("appleAppStore") || data["appleAppStore"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent.FromJson(data["appleAppStore"]))
                .WithGooglePlay(!data.Keys.Contains("googlePlay") || data["googlePlay"] == null ? null : Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent.FromJson(data["googlePlay"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["scheduleNamespaceId"] = ScheduleNamespaceId,
                ["triggerName"] = TriggerName,
                ["triggerExtendMode"] = TriggerExtendMode,
                ["rollupHour"] = RollupHour,
                ["reallocateSpanDays"] = ReallocateSpanDays,
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
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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
            if (TriggerExtendMode != null) {
                writer.WritePropertyName("triggerExtendMode");
                writer.Write(TriggerExtendMode.ToString());
            }
            if (RollupHour != null) {
                writer.WritePropertyName("rollupHour");
                writer.Write((RollupHour.ToString().Contains(".") ? (int)double.Parse(RollupHour.ToString()) : int.Parse(RollupHour.ToString())));
            }
            if (ReallocateSpanDays != null) {
                writer.WritePropertyName("reallocateSpanDays");
                writer.Write((ReallocateSpanDays.ToString().Contains(".") ? (int)double.Parse(ReallocateSpanDays.ToString()) : int.Parse(ReallocateSpanDays.ToString())));
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
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ScheduleNamespaceId + ":";
            key += TriggerName + ":";
            key += TriggerExtendMode + ":";
            key += RollupHour + ":";
            key += ReallocateSpanDays + ":";
            key += AppleAppStore + ":";
            key += GooglePlay + ":";
            return key;
        }
    }
}