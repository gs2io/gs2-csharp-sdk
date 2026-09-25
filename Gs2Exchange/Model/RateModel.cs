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
	public partial class RateModel : IComparable
	{
        public string RateModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public string TimingType { set; get; }
        public int? LockTime { set; get; }
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
        public RateModel WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
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
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new RateModel()
                .WithRateModelId(!data.Keys.Contains("rateModelId") || data["rateModelId"] == null ? null : data["rateModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
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
                }).ToArray());
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
                ["metadata"] = Metadata,
                ["verifyActions"] = verifyActionsJsonData,
                ["consumeActions"] = consumeActionsJsonData,
                ["timingType"] = TimingType,
                ["lockTime"] = LockTime,
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RateModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type RateModel.", nameof(obj));
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
            return 0;
        }

        public void Validate() {
            {
                if (RateModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.rateModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.verifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.consumeActions.error.tooMany"),
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
                            new RequestError("rateModel", "exchange.rateModel.timingType.error.invalid"),
                        });
                }
            }
            if (TimingType == "await") {
                if (LockTime < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.lockTime.error.invalid"),
                    });
                }
                if (LockTime > 538214400) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.lockTime.error.invalid"),
                    });
                }
            }
            {
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "exchange.rateModel.acquireActions.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new RateModel {
                RateModelId = RateModelId,
                Name = Name,
                Metadata = Metadata,
                VerifyActions = VerifyActions?.Clone() as Gs2.Core.Model.VerifyAction[],
                ConsumeActions = ConsumeActions?.Clone() as Gs2.Core.Model.ConsumeAction[],
                TimingType = TimingType,
                LockTime = LockTime,
                AcquireActions = AcquireActions?.Clone() as Gs2.Core.Model.AcquireAction[],
            };
        }
    }
}