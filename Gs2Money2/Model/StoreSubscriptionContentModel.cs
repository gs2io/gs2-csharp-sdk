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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class StoreSubscriptionContentModel : IComparable
	{
        public string StoreSubscriptionContentModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string ScheduleNamespaceId { set; get; }
        public string TriggerName { set; get; }
        public string TriggerExtendMode { set; get; }
        public int? RollupHour { set; get; }
        public int? ReallocateSpanDays { set; get; }
        public Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent AppleAppStore { set; get; }
        public Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent GooglePlay { set; get; }
        public StoreSubscriptionContentModel WithStoreSubscriptionContentModelId(string storeSubscriptionContentModelId) {
            this.StoreSubscriptionContentModelId = storeSubscriptionContentModelId;
            return this;
        }
        public StoreSubscriptionContentModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public StoreSubscriptionContentModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public StoreSubscriptionContentModel WithScheduleNamespaceId(string scheduleNamespaceId) {
            this.ScheduleNamespaceId = scheduleNamespaceId;
            return this;
        }
        public StoreSubscriptionContentModel WithTriggerName(string triggerName) {
            this.TriggerName = triggerName;
            return this;
        }
        public StoreSubscriptionContentModel WithTriggerExtendMode(string triggerExtendMode) {
            this.TriggerExtendMode = triggerExtendMode;
            return this;
        }
        public StoreSubscriptionContentModel WithRollupHour(int? rollupHour) {
            this.RollupHour = rollupHour;
            return this;
        }
        public StoreSubscriptionContentModel WithReallocateSpanDays(int? reallocateSpanDays) {
            this.ReallocateSpanDays = reallocateSpanDays;
            return this;
        }
        public StoreSubscriptionContentModel WithAppleAppStore(Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent appleAppStore) {
            this.AppleAppStore = appleAppStore;
            return this;
        }
        public StoreSubscriptionContentModel WithGooglePlay(Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent googlePlay) {
            this.GooglePlay = googlePlay;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:subscription:content:(?<contentName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:subscription:content:(?<contentName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:subscription:content:(?<contentName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _contentNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:subscription:content:(?<contentName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetContentNameFromGrn(
            string grn
        )
        {
            var match = _contentNameRegex.Match(grn);
            if (!match.Success || !match.Groups["contentName"].Success)
            {
                return null;
            }
            return match.Groups["contentName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StoreSubscriptionContentModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StoreSubscriptionContentModel()
                .WithStoreSubscriptionContentModelId(!data.Keys.Contains("storeSubscriptionContentModelId") || data["storeSubscriptionContentModelId"] == null ? null : data["storeSubscriptionContentModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScheduleNamespaceId(!data.Keys.Contains("scheduleNamespaceId") || data["scheduleNamespaceId"] == null ? null : data["scheduleNamespaceId"].ToString())
                .WithTriggerName(!data.Keys.Contains("triggerName") || data["triggerName"] == null ? null : data["triggerName"].ToString())
                .WithTriggerExtendMode(!data.Keys.Contains("triggerExtendMode") || data["triggerExtendMode"] == null ? null : data["triggerExtendMode"].ToString())
                .WithRollupHour(!data.Keys.Contains("rollupHour") || data["rollupHour"] == null ? null : (int?)(data["rollupHour"].ToString().Contains(".") ? (int)double.Parse(data["rollupHour"].ToString()) : int.Parse(data["rollupHour"].ToString())))
                .WithReallocateSpanDays(!data.Keys.Contains("reallocateSpanDays") || data["reallocateSpanDays"] == null ? null : (int?)(data["reallocateSpanDays"].ToString().Contains(".") ? (int)double.Parse(data["reallocateSpanDays"].ToString()) : int.Parse(data["reallocateSpanDays"].ToString())))
                .WithAppleAppStore(!data.Keys.Contains("appleAppStore") || data["appleAppStore"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent.FromJson(data["appleAppStore"]))
                .WithGooglePlay(!data.Keys.Contains("googlePlay") || data["googlePlay"] == null ? null : Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent.FromJson(data["googlePlay"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["storeSubscriptionContentModelId"] = StoreSubscriptionContentModelId,
                ["name"] = Name,
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
            if (StoreSubscriptionContentModelId != null) {
                writer.WritePropertyName("storeSubscriptionContentModelId");
                writer.Write(StoreSubscriptionContentModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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
                writer.WritePropertyName("appleAppStore");
                AppleAppStore.WriteJson(writer);
            }
            if (GooglePlay != null) {
                writer.WritePropertyName("googlePlay");
                GooglePlay.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as StoreSubscriptionContentModel;
            var diff = 0;
            if (StoreSubscriptionContentModelId == null && StoreSubscriptionContentModelId == other.StoreSubscriptionContentModelId)
            {
                // null and null
            }
            else
            {
                diff += StoreSubscriptionContentModelId.CompareTo(other.StoreSubscriptionContentModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ScheduleNamespaceId == null && ScheduleNamespaceId == other.ScheduleNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += ScheduleNamespaceId.CompareTo(other.ScheduleNamespaceId);
            }
            if (TriggerName == null && TriggerName == other.TriggerName)
            {
                // null and null
            }
            else
            {
                diff += TriggerName.CompareTo(other.TriggerName);
            }
            if (TriggerExtendMode == null && TriggerExtendMode == other.TriggerExtendMode)
            {
                // null and null
            }
            else
            {
                diff += TriggerExtendMode.CompareTo(other.TriggerExtendMode);
            }
            if (RollupHour == null && RollupHour == other.RollupHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(RollupHour - other.RollupHour);
            }
            if (ReallocateSpanDays == null && ReallocateSpanDays == other.ReallocateSpanDays)
            {
                // null and null
            }
            else
            {
                diff += (int)(ReallocateSpanDays - other.ReallocateSpanDays);
            }
            if (AppleAppStore == null && AppleAppStore == other.AppleAppStore)
            {
                // null and null
            }
            else
            {
                diff += AppleAppStore.CompareTo(other.AppleAppStore);
            }
            if (GooglePlay == null && GooglePlay == other.GooglePlay)
            {
                // null and null
            }
            else
            {
                diff += GooglePlay.CompareTo(other.GooglePlay);
            }
            return diff;
        }

        public void Validate() {
            {
                if (StoreSubscriptionContentModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.storeSubscriptionContentModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (ScheduleNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.scheduleNamespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (TriggerName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.triggerName.error.tooLong"),
                    });
                }
            }
            {
                switch (TriggerExtendMode) {
                    case "just":
                    case "rollupHour":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.triggerExtendMode.error.invalid"),
                        });
                }
            }
            if (TriggerExtendMode == "rollupHour") {
                if (RollupHour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.rollupHour.error.invalid"),
                    });
                }
                if (RollupHour > 23) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.rollupHour.error.invalid"),
                    });
                }
            }
            {
                if (ReallocateSpanDays < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.reallocateSpanDays.error.invalid"),
                    });
                }
                if (ReallocateSpanDays > 365) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeSubscriptionContentModel", "money2.storeSubscriptionContentModel.reallocateSpanDays.error.invalid"),
                    });
                }
            }
            {
            }
            {
            }
        }

        public object Clone() {
            return new StoreSubscriptionContentModel {
                StoreSubscriptionContentModelId = StoreSubscriptionContentModelId,
                Name = Name,
                Metadata = Metadata,
                ScheduleNamespaceId = ScheduleNamespaceId,
                TriggerName = TriggerName,
                TriggerExtendMode = TriggerExtendMode,
                RollupHour = RollupHour,
                ReallocateSpanDays = ReallocateSpanDays,
                AppleAppStore = AppleAppStore?.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreSubscriptionContent,
                GooglePlay = GooglePlay?.Clone() as Gs2.Gs2Money2.Model.GooglePlaySubscriptionContent,
            };
        }
    }
}