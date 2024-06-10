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

namespace Gs2.Gs2SerialKey.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SerialKey : IComparable
	{
        public string SerialKeyId { set; get; } = null!;
        public string CampaignModelName { set; get; } = null!;
        public string Code { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Status { set; get; } = null!;
        public string UsedUserId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UsedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public SerialKey WithSerialKeyId(string serialKeyId) {
            this.SerialKeyId = serialKeyId;
            return this;
        }
        public SerialKey WithCampaignModelName(string campaignModelName) {
            this.CampaignModelName = campaignModelName;
            return this;
        }
        public SerialKey WithCode(string code) {
            this.Code = code;
            return this;
        }
        public SerialKey WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SerialKey WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public SerialKey WithUsedUserId(string usedUserId) {
            this.UsedUserId = usedUserId;
            return this;
        }
        public SerialKey WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SerialKey WithUsedAt(long? usedAt) {
            this.UsedAt = usedAt;
            return this;
        }
        public SerialKey WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public SerialKey WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):serialKey:(?<serialKeyCode>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):serialKey:(?<serialKeyCode>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):serialKey:(?<serialKeyCode>.+)",
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

        private static System.Text.RegularExpressions.Regex _serialKeyCodeRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):serialKey:(?<serialKeyCode>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSerialKeyCodeFromGrn(
            string grn
        )
        {
            var match = _serialKeyCodeRegex.Match(grn);
            if (!match.Success || !match.Groups["serialKeyCode"].Success)
            {
                return null;
            }
            return match.Groups["serialKeyCode"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SerialKey FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SerialKey()
                .WithSerialKeyId(!data.Keys.Contains("serialKeyId") || data["serialKeyId"] == null ? null : data["serialKeyId"].ToString())
                .WithCampaignModelName(!data.Keys.Contains("campaignModelName") || data["campaignModelName"] == null ? null : data["campaignModelName"].ToString())
                .WithCode(!data.Keys.Contains("code") || data["code"] == null ? null : data["code"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithUsedUserId(!data.Keys.Contains("usedUserId") || data["usedUserId"] == null ? null : data["usedUserId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUsedAt(!data.Keys.Contains("usedAt") || data["usedAt"] == null ? null : (long?)(data["usedAt"].ToString().Contains(".") ? (long)double.Parse(data["usedAt"].ToString()) : long.Parse(data["usedAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["serialKeyId"] = SerialKeyId,
                ["campaignModelName"] = CampaignModelName,
                ["code"] = Code,
                ["metadata"] = Metadata,
                ["status"] = Status,
                ["usedUserId"] = UsedUserId,
                ["createdAt"] = CreatedAt,
                ["usedAt"] = UsedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SerialKeyId != null) {
                writer.WritePropertyName("serialKeyId");
                writer.Write(SerialKeyId.ToString());
            }
            if (CampaignModelName != null) {
                writer.WritePropertyName("campaignModelName");
                writer.Write(CampaignModelName.ToString());
            }
            if (Code != null) {
                writer.WritePropertyName("code");
                writer.Write(Code.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (UsedUserId != null) {
                writer.WritePropertyName("usedUserId");
                writer.Write(UsedUserId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UsedAt != null) {
                writer.WritePropertyName("usedAt");
                writer.Write((UsedAt.ToString().Contains(".") ? (long)double.Parse(UsedAt.ToString()) : long.Parse(UsedAt.ToString())));
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
            var other = obj as SerialKey;
            var diff = 0;
            if (SerialKeyId == null && SerialKeyId == other.SerialKeyId)
            {
                // null and null
            }
            else
            {
                diff += SerialKeyId.CompareTo(other.SerialKeyId);
            }
            if (CampaignModelName == null && CampaignModelName == other.CampaignModelName)
            {
                // null and null
            }
            else
            {
                diff += CampaignModelName.CompareTo(other.CampaignModelName);
            }
            if (Code == null && Code == other.Code)
            {
                // null and null
            }
            else
            {
                diff += Code.CompareTo(other.Code);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            if (UsedUserId == null && UsedUserId == other.UsedUserId)
            {
                // null and null
            }
            else
            {
                diff += UsedUserId.CompareTo(other.UsedUserId);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UsedAt == null && UsedAt == other.UsedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UsedAt - other.UsedAt);
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
                if (SerialKeyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.serialKeyId.error.tooLong"),
                    });
                }
            }
            {
                if (CampaignModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.campaignModelName.error.tooLong"),
                    });
                }
            }
            {
                if (Code.Length > 48) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.code.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.metadata.error.tooLong"),
                    });
                }
            }
            {
                switch (Status) {
                    case "ACTIVE":
                    case "USED":
                    case "INACTIVE":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("serialKey", "serialKey.serialKey.status.error.invalid"),
                        });
                }
            }
            if (Status == "USED") {
                if (UsedUserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.usedUserId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UsedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.usedAt.error.invalid"),
                    });
                }
                if (UsedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.usedAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("serialKey", "serialKey.serialKey.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SerialKey {
                SerialKeyId = SerialKeyId,
                CampaignModelName = CampaignModelName,
                Code = Code,
                Metadata = Metadata,
                Status = Status,
                UsedUserId = UsedUserId,
                CreatedAt = CreatedAt,
                UsedAt = UsedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}