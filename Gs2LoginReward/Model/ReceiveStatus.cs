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

namespace Gs2.Gs2LoginReward.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ReceiveStatus : IComparable
	{
        public string ReceiveStatusId { set; get; } = null!;
        public string BonusModelName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public bool[] ReceivedSteps { set; get; } = null!;
        public long? LastReceivedAt { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public ReceiveStatus WithReceiveStatusId(string receiveStatusId) {
            this.ReceiveStatusId = receiveStatusId;
            return this;
        }
        public ReceiveStatus WithBonusModelName(string bonusModelName) {
            this.BonusModelName = bonusModelName;
            return this;
        }
        public ReceiveStatus WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ReceiveStatus WithReceivedSteps(bool[] receivedSteps) {
            this.ReceivedSteps = receivedSteps;
            return this;
        }
        public ReceiveStatus WithLastReceivedAt(long? lastReceivedAt) {
            this.LastReceivedAt = lastReceivedAt;
            return this;
        }
        public ReceiveStatus WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public ReceiveStatus WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public ReceiveStatus WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):user:(?<userId>.+):status:(?<bonusModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):user:(?<userId>.+):status:(?<bonusModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):user:(?<userId>.+):status:(?<bonusModelName>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):user:(?<userId>.+):status:(?<bonusModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _bonusModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):loginReward:(?<namespaceName>.+):user:(?<userId>.+):status:(?<bonusModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetBonusModelNameFromGrn(
            string grn
        )
        {
            var match = _bonusModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["bonusModelName"].Success)
            {
                return null;
            }
            return match.Groups["bonusModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReceiveStatus FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReceiveStatus()
                .WithReceiveStatusId(!data.Keys.Contains("receiveStatusId") || data["receiveStatusId"] == null ? null : data["receiveStatusId"].ToString())
                .WithBonusModelName(!data.Keys.Contains("bonusModelName") || data["bonusModelName"] == null ? null : data["bonusModelName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithReceivedSteps(!data.Keys.Contains("receivedSteps") || data["receivedSteps"] == null || !data["receivedSteps"].IsArray ? null : data["receivedSteps"].Cast<JsonData>().Select(v => {
                    return bool.Parse(v.ToString());
                }).ToArray())
                .WithLastReceivedAt(!data.Keys.Contains("lastReceivedAt") || data["lastReceivedAt"] == null ? null : (long?)(data["lastReceivedAt"].ToString().Contains(".") ? (long)double.Parse(data["lastReceivedAt"].ToString()) : long.Parse(data["lastReceivedAt"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData receivedStepsJsonData = null;
            if (ReceivedSteps != null && ReceivedSteps.Length > 0)
            {
                receivedStepsJsonData = new JsonData();
                foreach (var receivedStep in ReceivedSteps)
                {
                    receivedStepsJsonData.Add(receivedStep);
                }
            }
            return new JsonData {
                ["receiveStatusId"] = ReceiveStatusId,
                ["bonusModelName"] = BonusModelName,
                ["userId"] = UserId,
                ["receivedSteps"] = receivedStepsJsonData,
                ["lastReceivedAt"] = LastReceivedAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ReceiveStatusId != null) {
                writer.WritePropertyName("receiveStatusId");
                writer.Write(ReceiveStatusId.ToString());
            }
            if (BonusModelName != null) {
                writer.WritePropertyName("bonusModelName");
                writer.Write(BonusModelName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ReceivedSteps != null) {
                writer.WritePropertyName("receivedSteps");
                writer.WriteArrayStart();
                foreach (var receivedStep in ReceivedSteps)
                {
                    writer.Write(bool.Parse(receivedStep.ToString()));
                }
                writer.WriteArrayEnd();
            }
            if (LastReceivedAt != null) {
                writer.WritePropertyName("lastReceivedAt");
                writer.Write((LastReceivedAt.ToString().Contains(".") ? (long)double.Parse(LastReceivedAt.ToString()) : long.Parse(LastReceivedAt.ToString())));
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
            var other = obj as ReceiveStatus;
            var diff = 0;
            if (ReceiveStatusId == null && ReceiveStatusId == other.ReceiveStatusId)
            {
                // null and null
            }
            else
            {
                diff += ReceiveStatusId.CompareTo(other.ReceiveStatusId);
            }
            if (BonusModelName == null && BonusModelName == other.BonusModelName)
            {
                // null and null
            }
            else
            {
                diff += BonusModelName.CompareTo(other.BonusModelName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (ReceivedSteps == null && ReceivedSteps == other.ReceivedSteps)
            {
                // null and null
            }
            else
            {
                diff += ReceivedSteps.Length - other.ReceivedSteps.Length;
                for (var i = 0; i < ReceivedSteps.Length; i++)
                {
                    diff += ReceivedSteps[i] == other.ReceivedSteps[i] ? 0 : 1;
                }
            }
            if (LastReceivedAt == null && LastReceivedAt == other.LastReceivedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(LastReceivedAt - other.LastReceivedAt);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ReceiveStatusId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.receiveStatusId.error.tooLong"),
                    });
                }
            }
            {
                if (BonusModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.bonusModelName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.userId.error.tooLong"),
                    });
                }
            }
            {
                if (ReceivedSteps.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.receivedSteps.error.tooMany"),
                    });
                }
            }
            {
                if (LastReceivedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.lastReceivedAt.error.invalid"),
                    });
                }
                if (LastReceivedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.lastReceivedAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveStatus", "loginReward.receiveStatus.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ReceiveStatus {
                ReceiveStatusId = ReceiveStatusId,
                BonusModelName = BonusModelName,
                UserId = UserId,
                ReceivedSteps = ReceivedSteps.Clone() as bool[],
                LastReceivedAt = LastReceivedAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}