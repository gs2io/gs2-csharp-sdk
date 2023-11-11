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

namespace Gs2.Gs2Exchange.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RateModel : IComparable
	{
        public string RateModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public string TimingType { set; get; }
        public int? LockTime { set; get; }
        public bool? EnableSkip { set; get; }
        public Gs2.Core.Model.ConsumeAction[] SkipConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }

        public RateModel WithRateModelId(string rateModelId) {
            this.RateModelId = rateModelId;
            return this;
        }

        public RateModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public RateModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public RateModel WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }

        public RateModel WithTimingType(string timingType) {
            this.TimingType = timingType;
            return this;
        }

        public RateModel WithLockTime(int? lockTime) {
            this.LockTime = lockTime;
            return this;
        }

        public RateModel WithEnableSkip(bool? enableSkip) {
            this.EnableSkip = enableSkip;
            return this;
        }

        public RateModel WithSkipConsumeActions(Gs2.Core.Model.ConsumeAction[] skipConsumeActions) {
            this.SkipConsumeActions = skipConsumeActions;
            return this;
        }

        public RateModel WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):model:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):model:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):model:(?<rateName>.+)",
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

        private static System.Text.RegularExpressions.Regex _rateNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):model:(?<rateName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRateNameFromGrn(
            string grn
        )
        {
            var match = _rateNameRegex.Match(grn);
            if (!match.Success || !match.Groups["rateName"].Success)
            {
                return null;
            }
            return match.Groups["rateName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RateModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RateModel()
                .WithRateModelId(!data.Keys.Contains("rateModelId") || data["rateModelId"] == null ? null : data["rateModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? new Gs2.Core.Model.ConsumeAction[]{} : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithTimingType(!data.Keys.Contains("timingType") || data["timingType"] == null ? null : data["timingType"].ToString())
                .WithLockTime(!data.Keys.Contains("lockTime") || data["lockTime"] == null ? null : (int?)int.Parse(data["lockTime"].ToString()))
                .WithEnableSkip(!data.Keys.Contains("enableSkip") || data["enableSkip"] == null ? null : (bool?)bool.Parse(data["enableSkip"].ToString()))
                .WithSkipConsumeActions(!data.Keys.Contains("skipConsumeActions") || data["skipConsumeActions"] == null || !data["skipConsumeActions"].IsArray ? new Gs2.Core.Model.ConsumeAction[]{} : data["skipConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? new Gs2.Core.Model.AcquireAction[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            JsonData skipConsumeActionsJsonData = null;
            if (SkipConsumeActions != null)
            {
                skipConsumeActionsJsonData = new JsonData();
                foreach (var skipConsumeAction in SkipConsumeActions)
                {
                    skipConsumeActionsJsonData.Add(skipConsumeAction.ToJson());
                }
            }
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["rateModelId"] = RateModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["consumeActions"] = consumeActionsJsonData,
                ["timingType"] = TimingType,
                ["lockTime"] = LockTime,
                ["enableSkip"] = EnableSkip,
                ["skipConsumeActions"] = skipConsumeActionsJsonData,
                ["acquireActions"] = acquireActionsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RateModelId != null) {
                writer.WritePropertyName("rateModelId");
                writer.Write(RateModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ConsumeActions != null) {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach (var consumeAction in ConsumeActions)
                {
                    if (consumeAction != null) {
                        consumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (TimingType != null) {
                writer.WritePropertyName("timingType");
                writer.Write(TimingType.ToString());
            }
            if (LockTime != null) {
                writer.WritePropertyName("lockTime");
                writer.Write(int.Parse(LockTime.ToString()));
            }
            if (EnableSkip != null) {
                writer.WritePropertyName("enableSkip");
                writer.Write(bool.Parse(EnableSkip.ToString()));
            }
            if (SkipConsumeActions != null) {
                writer.WritePropertyName("skipConsumeActions");
                writer.WriteArrayStart();
                foreach (var skipConsumeAction in SkipConsumeActions)
                {
                    if (skipConsumeAction != null) {
                        skipConsumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RateModel;
            var diff = 0;
            if (RateModelId == null && RateModelId == other.RateModelId)
            {
                // null and null
            }
            else
            {
                diff += RateModelId.CompareTo(other.RateModelId);
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
            if (ConsumeActions == null && ConsumeActions == other.ConsumeActions)
            {
                // null and null
            }
            else
            {
                diff += ConsumeActions.Length - other.ConsumeActions.Length;
                for (var i = 0; i < ConsumeActions.Length; i++)
                {
                    diff += ConsumeActions[i].CompareTo(other.ConsumeActions[i]);
                }
            }
            if (TimingType == null && TimingType == other.TimingType)
            {
                // null and null
            }
            else
            {
                diff += TimingType.CompareTo(other.TimingType);
            }
            if (LockTime == null && LockTime == other.LockTime)
            {
                // null and null
            }
            else
            {
                diff += (int)(LockTime - other.LockTime);
            }
            if (EnableSkip == null && EnableSkip == other.EnableSkip)
            {
                // null and null
            }
            else
            {
                diff += EnableSkip == other.EnableSkip ? 0 : 1;
            }
            if (SkipConsumeActions == null && SkipConsumeActions == other.SkipConsumeActions)
            {
                // null and null
            }
            else
            {
                diff += SkipConsumeActions.Length - other.SkipConsumeActions.Length;
                for (var i = 0; i < SkipConsumeActions.Length; i++)
                {
                    diff += SkipConsumeActions[i].CompareTo(other.SkipConsumeActions[i]);
                }
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
            return diff;
        }
    }
}