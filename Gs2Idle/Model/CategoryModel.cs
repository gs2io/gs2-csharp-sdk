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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Idle.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class CategoryModel : IComparable
	{
        public string CategoryModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public int? RewardIntervalMinutes { set; get; }
        public int? DefaultMaximumIdleMinutes { set; get; }
        public Gs2.Gs2Idle.Model.AcquireActionList[] AcquireActions { set; get; }
        public string IdlePeriodScheduleId { set; get; }
        public string ReceivePeriodScheduleId { set; get; }
        public CategoryModel WithCategoryModelId(string categoryModelId) {
            this.CategoryModelId = categoryModelId;
            return this;
        }
        public CategoryModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public CategoryModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CategoryModel WithRewardIntervalMinutes(int? rewardIntervalMinutes) {
            this.RewardIntervalMinutes = rewardIntervalMinutes;
            return this;
        }
        public CategoryModel WithDefaultMaximumIdleMinutes(int? defaultMaximumIdleMinutes) {
            this.DefaultMaximumIdleMinutes = defaultMaximumIdleMinutes;
            return this;
        }
        public CategoryModel WithAcquireActions(Gs2.Gs2Idle.Model.AcquireActionList[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }
        public CategoryModel WithIdlePeriodScheduleId(string idlePeriodScheduleId) {
            this.IdlePeriodScheduleId = idlePeriodScheduleId;
            return this;
        }
        public CategoryModel WithReceivePeriodScheduleId(string receivePeriodScheduleId) {
            this.ReceivePeriodScheduleId = receivePeriodScheduleId;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
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

        private static System.Text.RegularExpressions.Regex _categoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):idle:(?<namespaceName>.+):model:(?<categoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCategoryNameFromGrn(
            string grn
        )
        {
            var match = _categoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["categoryName"].Success)
            {
                return null;
            }
            return match.Groups["categoryName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CategoryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CategoryModel()
                .WithCategoryModelId(!data.Keys.Contains("categoryModelId") || data["categoryModelId"] == null ? null : data["categoryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithRewardIntervalMinutes(!data.Keys.Contains("rewardIntervalMinutes") || data["rewardIntervalMinutes"] == null ? null : (int?)int.Parse(data["rewardIntervalMinutes"].ToString()))
                .WithDefaultMaximumIdleMinutes(!data.Keys.Contains("defaultMaximumIdleMinutes") || data["defaultMaximumIdleMinutes"] == null ? null : (int?)int.Parse(data["defaultMaximumIdleMinutes"].ToString()))
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null ? new Gs2.Gs2Idle.Model.AcquireActionList[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Idle.Model.AcquireActionList.FromJson(v);
                }).ToArray())
                .WithIdlePeriodScheduleId(!data.Keys.Contains("idlePeriodScheduleId") || data["idlePeriodScheduleId"] == null ? null : data["idlePeriodScheduleId"].ToString())
                .WithReceivePeriodScheduleId(!data.Keys.Contains("receivePeriodScheduleId") || data["receivePeriodScheduleId"] == null ? null : data["receivePeriodScheduleId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["categoryModelId"] = CategoryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["rewardIntervalMinutes"] = RewardIntervalMinutes,
                ["defaultMaximumIdleMinutes"] = DefaultMaximumIdleMinutes,
                ["acquireActions"] = AcquireActions == null ? null : new JsonData(
                        AcquireActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["idlePeriodScheduleId"] = IdlePeriodScheduleId,
                ["receivePeriodScheduleId"] = ReceivePeriodScheduleId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CategoryModelId != null) {
                writer.WritePropertyName("categoryModelId");
                writer.Write(CategoryModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (RewardIntervalMinutes != null) {
                writer.WritePropertyName("rewardIntervalMinutes");
                writer.Write(int.Parse(RewardIntervalMinutes.ToString()));
            }
            if (DefaultMaximumIdleMinutes != null) {
                writer.WritePropertyName("defaultMaximumIdleMinutes");
                writer.Write(int.Parse(DefaultMaximumIdleMinutes.ToString()));
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (IdlePeriodScheduleId != null) {
                writer.WritePropertyName("idlePeriodScheduleId");
                writer.Write(IdlePeriodScheduleId.ToString());
            }
            if (ReceivePeriodScheduleId != null) {
                writer.WritePropertyName("receivePeriodScheduleId");
                writer.Write(ReceivePeriodScheduleId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CategoryModel;
            var diff = 0;
            if (CategoryModelId == null && CategoryModelId == other.CategoryModelId)
            {
                // null and null
            }
            else
            {
                diff += CategoryModelId.CompareTo(other.CategoryModelId);
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
            if (RewardIntervalMinutes == null && RewardIntervalMinutes == other.RewardIntervalMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(RewardIntervalMinutes - other.RewardIntervalMinutes);
            }
            if (DefaultMaximumIdleMinutes == null && DefaultMaximumIdleMinutes == other.DefaultMaximumIdleMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultMaximumIdleMinutes - other.DefaultMaximumIdleMinutes);
            }
            if (AcquireActions == null && AcquireActions == other.AcquireActions)
            {
                // null and null
            }
            else
            {
                diff += AcquireActions.Length - other.AcquireActions.Length;
                for (var i = 0; i < AcquireActions.Length; i++)
                {
                    diff += AcquireActions[i].CompareTo(other.AcquireActions[i]);
                }
            }
            if (IdlePeriodScheduleId == null && IdlePeriodScheduleId == other.IdlePeriodScheduleId)
            {
                // null and null
            }
            else
            {
                diff += IdlePeriodScheduleId.CompareTo(other.IdlePeriodScheduleId);
            }
            if (ReceivePeriodScheduleId == null && ReceivePeriodScheduleId == other.ReceivePeriodScheduleId)
            {
                // null and null
            }
            else
            {
                diff += ReceivePeriodScheduleId.CompareTo(other.ReceivePeriodScheduleId);
            }
            return diff;
        }
    }
}