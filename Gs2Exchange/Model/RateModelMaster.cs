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

namespace Gs2.Gs2Exchange.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class RateModelMaster : IComparable
	{
        public string RateModelId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public string TimingType { set; get; }
        public int? LockTime { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public RateModelMaster WithRateModelId(string rateModelId) {
            this.RateModelId = rateModelId;
            return this;
        }
        public RateModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public RateModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public RateModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RateModelMaster WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
            return this;
        }
        public RateModelMaster WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }
        public RateModelMaster WithTimingType(string timingType) {
            this.TimingType = timingType;
            return this;
        }
        public RateModelMaster WithLockTime(int? lockTime) {
            this.LockTime = lockTime;
            return this;
        }
        public RateModelMaster WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }
        public RateModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public RateModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public RateModelMaster WithRevision(long? revision) {
            this.Revision = revision;
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
        public static RateModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new RateModelMaster()
                .WithRateModelId(!data.Keys.Contains("rateModelId") || data["rateModelId"] == null ? null : data["rateModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithVerifyActions(!data.Keys.Contains("verifyActions") || data["verifyActions"] == null || !data["verifyActions"].IsArray ? null : data["verifyActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? null : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithTimingType(!data.Keys.Contains("timingType") || data["timingType"] == null ? null : data["timingType"].ToString())
                .WithLockTime(!data.Keys.Contains("lockTime") || data["lockTime"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["lockTime"].ToString()))
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData verifyActionsJsonData = null;
            if (VerifyActions != null && VerifyActions.Length > 0)
            {
                verifyActionsJsonData = new JsonData();
                foreach (var verifyAction in VerifyActions)
                {
                    verifyActionsJsonData.Add(verifyAction.ToJson());
                }
            }
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null && ConsumeActions.Length > 0)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
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
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["verifyActions"] = verifyActionsJsonData,
                ["consumeActions"] = consumeActionsJsonData,
                ["timingType"] = TimingType,
                ["lockTime"] = LockTime,
                ["acquireActions"] = acquireActionsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (VerifyActions != null) {
                writer.WritePropertyName("verifyActions");
                writer.WriteArrayStart();
                foreach (var verifyAction in VerifyActions)
                {
                    if (verifyAction != null) {
                        verifyAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
                writer.Write((LockTime.ToString().Contains(".") ? (int)double.Parse(LockTime.ToString()) : int.Parse(LockTime.ToString())));
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RateModelMaster;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type RateModelMaster.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(RateModelId, other.RateModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(VerifyActions, other.VerifyActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(ConsumeActions, other.ConsumeActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TimingType, other.TimingType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(LockTime, other.LockTime);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(AcquireActions, other.AcquireActions);
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
            return 0;
        }

        public void Validate() {
            {
                if (RateModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.rateModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.verifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.consumeActions.error.tooMany"),
                    });
                }
            }
            {
                switch (TimingType) {
                    case "immediate":
                    case "await":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("rateModelMaster", "exchange.rateModelMaster.timingType.error.invalid"),
                        });
                }
            }
            if (TimingType == "await") {
                if (LockTime < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.lockTime.error.invalid"),
                    });
                }
                if (LockTime > 538214400) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.lockTime.error.invalid"),
                    });
                }
            }
            {
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.acquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModelMaster", "exchange.rateModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RateModelMaster {
                RateModelId = RateModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                VerifyActions = VerifyActions?.Clone() as Gs2.Core.Model.VerifyAction[],
                ConsumeActions = ConsumeActions?.Clone() as Gs2.Core.Model.ConsumeAction[],
                TimingType = TimingType,
                LockTime = LockTime,
                AcquireActions = AcquireActions?.Clone() as Gs2.Core.Model.AcquireAction[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}