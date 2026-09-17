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

namespace Gs2.Gs2Mission.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class MissionTaskModelMaster : IComparable
	{
        public string MissionTaskId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string Description { set; get; }
        public string VerifyCompleteType { set; get; }
        public Gs2.Gs2Mission.Model.TargetCounterModel TargetCounter { set; get; }
        public Gs2.Core.Model.VerifyAction[] VerifyCompleteConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] CompleteAcquireActions { set; get; }
        public string ChallengePeriodEventId { set; get; }
        public string PremiseMissionTaskName { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        [Obsolete("This method is deprecated")]
        public string CounterName { set; get; }
        [Obsolete("This method is deprecated")]
        public string TargetResetType { set; get; }
        [Obsolete("This method is deprecated")]
        public long? TargetValue { set; get; }
        public MissionTaskModelMaster WithMissionTaskId(string missionTaskId) {
            this.MissionTaskId = missionTaskId;
            return this;
        }
        public MissionTaskModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public MissionTaskModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public MissionTaskModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public MissionTaskModelMaster WithVerifyCompleteType(string verifyCompleteType) {
            this.VerifyCompleteType = verifyCompleteType;
            return this;
        }
        public MissionTaskModelMaster WithTargetCounter(Gs2.Gs2Mission.Model.TargetCounterModel targetCounter) {
            this.TargetCounter = targetCounter;
            return this;
        }
        public MissionTaskModelMaster WithVerifyCompleteConsumeActions(Gs2.Core.Model.VerifyAction[] verifyCompleteConsumeActions) {
            this.VerifyCompleteConsumeActions = verifyCompleteConsumeActions;
            return this;
        }
        public MissionTaskModelMaster WithCompleteAcquireActions(Gs2.Core.Model.AcquireAction[] completeAcquireActions) {
            this.CompleteAcquireActions = completeAcquireActions;
            return this;
        }
        public MissionTaskModelMaster WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }
        public MissionTaskModelMaster WithPremiseMissionTaskName(string premiseMissionTaskName) {
            this.PremiseMissionTaskName = premiseMissionTaskName;
            return this;
        }
        public MissionTaskModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public MissionTaskModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public MissionTaskModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public MissionTaskModelMaster WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public MissionTaskModelMaster WithTargetResetType(string targetResetType) {
            this.TargetResetType = targetResetType;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public MissionTaskModelMaster WithTargetValue(long? targetValue) {
            this.TargetValue = targetValue;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+):missionTaskModelMaster:(?<missionTaskName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+):missionTaskModelMaster:(?<missionTaskName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+):missionTaskModelMaster:(?<missionTaskName>.+)",
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

        private static System.Text.RegularExpressions.Regex _missionGroupNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+):missionTaskModelMaster:(?<missionTaskName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMissionGroupNameFromGrn(
            string grn
        )
        {
            var match = _missionGroupNameRegex.Match(grn);
            if (!match.Success || !match.Groups["missionGroupName"].Success)
            {
                return null;
            }
            return match.Groups["missionGroupName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _missionTaskNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+):missionTaskModelMaster:(?<missionTaskName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMissionTaskNameFromGrn(
            string grn
        )
        {
            var match = _missionTaskNameRegex.Match(grn);
            if (!match.Success || !match.Groups["missionTaskName"].Success)
            {
                return null;
            }
            return match.Groups["missionTaskName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MissionTaskModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new MissionTaskModelMaster()
                .WithMissionTaskId(!data.Keys.Contains("missionTaskId") || data["missionTaskId"] == null ? null : data["missionTaskId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithVerifyCompleteType(!data.Keys.Contains("verifyCompleteType") || data["verifyCompleteType"] == null ? null : data["verifyCompleteType"].ToString())
                .WithTargetCounter(!data.Keys.Contains("targetCounter") || data["targetCounter"] == null ? null : Gs2.Gs2Mission.Model.TargetCounterModel.FromJson(data["targetCounter"]))
                .WithVerifyCompleteConsumeActions(!data.Keys.Contains("verifyCompleteConsumeActions") || data["verifyCompleteConsumeActions"] == null || !data["verifyCompleteConsumeActions"].IsArray ? null : data["verifyCompleteConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithCompleteAcquireActions(!data.Keys.Contains("completeAcquireActions") || data["completeAcquireActions"] == null || !data["completeAcquireActions"].IsArray ? null : data["completeAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithPremiseMissionTaskName(!data.Keys.Contains("premiseMissionTaskName") || data["premiseMissionTaskName"] == null ? null : data["premiseMissionTaskName"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()))
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithTargetResetType(!data.Keys.Contains("targetResetType") || data["targetResetType"] == null ? null : data["targetResetType"].ToString())
                .WithTargetValue(!data.Keys.Contains("targetValue") || data["targetValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["targetValue"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData verifyCompleteConsumeActionsJsonData = null;
            if (VerifyCompleteConsumeActions != null && VerifyCompleteConsumeActions.Length > 0)
            {
                verifyCompleteConsumeActionsJsonData = new JsonData();
                foreach (var verifyCompleteConsumeAction in VerifyCompleteConsumeActions)
                {
                    verifyCompleteConsumeActionsJsonData.Add(verifyCompleteConsumeAction.ToJson());
                }
            }
            JsonData completeAcquireActionsJsonData = null;
            if (CompleteAcquireActions != null && CompleteAcquireActions.Length > 0)
            {
                completeAcquireActionsJsonData = new JsonData();
                foreach (var completeAcquireAction in CompleteAcquireActions)
                {
                    completeAcquireActionsJsonData.Add(completeAcquireAction.ToJson());
                }
            }
            return new JsonData {
                ["missionTaskId"] = MissionTaskId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["description"] = Description,
                ["verifyCompleteType"] = VerifyCompleteType,
                ["targetCounter"] = TargetCounter?.ToJson(),
                ["verifyCompleteConsumeActions"] = verifyCompleteConsumeActionsJsonData,
                ["completeAcquireActions"] = completeAcquireActionsJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["premiseMissionTaskName"] = PremiseMissionTaskName,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MissionTaskId != null) {
                writer.WritePropertyName("missionTaskId");
                writer.Write(MissionTaskId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (VerifyCompleteType != null) {
                writer.WritePropertyName("verifyCompleteType");
                writer.Write(VerifyCompleteType.ToString());
            }
            if (TargetCounter != null) {
                writer.WritePropertyName("targetCounter");
                TargetCounter.WriteJson(writer);
            }
            if (VerifyCompleteConsumeActions != null) {
                writer.WritePropertyName("verifyCompleteConsumeActions");
                writer.WriteArrayStart();
                foreach (var verifyCompleteConsumeAction in VerifyCompleteConsumeActions)
                {
                    if (verifyCompleteConsumeAction != null) {
                        verifyCompleteConsumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CompleteAcquireActions != null) {
                writer.WritePropertyName("completeAcquireActions");
                writer.WriteArrayStart();
                foreach (var completeAcquireAction in CompleteAcquireActions)
                {
                    if (completeAcquireAction != null) {
                        completeAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            if (PremiseMissionTaskName != null) {
                writer.WritePropertyName("premiseMissionTaskName");
                writer.Write(PremiseMissionTaskName.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
            if (TargetResetType != null) {
                writer.WritePropertyName("targetResetType");
                writer.Write(TargetResetType.ToString());
            }
            if (TargetValue != null) {
                writer.WritePropertyName("targetValue");
                writer.Write((TargetValue.ToString().Contains(".") ? (long)double.Parse(TargetValue.ToString()) : long.Parse(TargetValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as MissionTaskModelMaster;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type MissionTaskModelMaster.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(MissionTaskId, other.MissionTaskId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(VerifyCompleteType, other.VerifyCompleteType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TargetCounter, other.TargetCounter);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(VerifyCompleteConsumeActions, other.VerifyCompleteConsumeActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(CompleteAcquireActions, other.CompleteAcquireActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ChallengePeriodEventId, other.ChallengePeriodEventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(PremiseMissionTaskName, other.PremiseMissionTaskName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CounterName, other.CounterName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TargetResetType, other.TargetResetType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TargetValue, other.TargetValue);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (MissionTaskId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.missionTaskId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                switch (VerifyCompleteType) {
                    case "counter":
                    case "verifyActions":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.verifyCompleteType.error.invalid"),
                        });
                }
            }
            if (VerifyCompleteType == "counter") {
            }
            {
                if (VerifyCompleteConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.verifyCompleteConsumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (CompleteAcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.completeAcquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (PremiseMissionTaskName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.premiseMissionTaskName.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionTaskModelMaster", "mission.missionTaskModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new MissionTaskModelMaster {
                MissionTaskId = MissionTaskId,
                Name = Name,
                Metadata = Metadata,
                Description = Description,
                VerifyCompleteType = VerifyCompleteType,
                TargetCounter = TargetCounter?.Clone() as Gs2.Gs2Mission.Model.TargetCounterModel,
                VerifyCompleteConsumeActions = VerifyCompleteConsumeActions?.Clone() as Gs2.Core.Model.VerifyAction[],
                CompleteAcquireActions = CompleteAcquireActions?.Clone() as Gs2.Core.Model.AcquireAction[],
                ChallengePeriodEventId = ChallengePeriodEventId,
                PremiseMissionTaskName = PremiseMissionTaskName,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
                CounterName = CounterName,
                TargetResetType = TargetResetType,
                TargetValue = TargetValue,
            };
        }
    }
}