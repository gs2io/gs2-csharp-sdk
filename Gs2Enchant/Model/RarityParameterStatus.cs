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

namespace Gs2.Gs2Enchant.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RarityParameterStatus : IComparable
	{
        public string RarityParameterStatusId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string ParameterName { set; get; } = null!;
        public string PropertyId { set; get; } = null!;
        public Gs2.Gs2Enchant.Model.RarityParameterValue[] ParameterValues { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public RarityParameterStatus WithRarityParameterStatusId(string rarityParameterStatusId) {
            this.RarityParameterStatusId = rarityParameterStatusId;
            return this;
        }
        public RarityParameterStatus WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public RarityParameterStatus WithParameterName(string parameterName) {
            this.ParameterName = parameterName;
            return this;
        }
        public RarityParameterStatus WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }
        public RarityParameterStatus WithParameterValues(Gs2.Gs2Enchant.Model.RarityParameterValue[] parameterValues) {
            this.ParameterValues = parameterValues;
            return this;
        }
        public RarityParameterStatus WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public RarityParameterStatus WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public RarityParameterStatus WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):user:(?<userId>.+):rarity:(?<parameterName>.+):(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):user:(?<userId>.+):rarity:(?<parameterName>.+):(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):user:(?<userId>.+):rarity:(?<parameterName>.+):(?<propertyId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):user:(?<userId>.+):rarity:(?<parameterName>.+):(?<propertyId>.+)",
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

        private static System.Text.RegularExpressions.Regex _parameterNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):user:(?<userId>.+):rarity:(?<parameterName>.+):(?<propertyId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetParameterNameFromGrn(
            string grn
        )
        {
            var match = _parameterNameRegex.Match(grn);
            if (!match.Success || !match.Groups["parameterName"].Success)
            {
                return null;
            }
            return match.Groups["parameterName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _propertyIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):user:(?<userId>.+):rarity:(?<parameterName>.+):(?<propertyId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetPropertyIdFromGrn(
            string grn
        )
        {
            var match = _propertyIdRegex.Match(grn);
            if (!match.Success || !match.Groups["propertyId"].Success)
            {
                return null;
            }
            return match.Groups["propertyId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RarityParameterStatus FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RarityParameterStatus()
                .WithRarityParameterStatusId(!data.Keys.Contains("rarityParameterStatusId") || data["rarityParameterStatusId"] == null ? null : data["rarityParameterStatusId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithParameterName(!data.Keys.Contains("parameterName") || data["parameterName"] == null ? null : data["parameterName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithParameterValues(!data.Keys.Contains("parameterValues") || data["parameterValues"] == null || !data["parameterValues"].IsArray ? null : data["parameterValues"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.RarityParameterValue.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData parameterValuesJsonData = null;
            if (ParameterValues != null && ParameterValues.Length > 0)
            {
                parameterValuesJsonData = new JsonData();
                foreach (var parameterValue in ParameterValues)
                {
                    parameterValuesJsonData.Add(parameterValue.ToJson());
                }
            }
            return new JsonData {
                ["rarityParameterStatusId"] = RarityParameterStatusId,
                ["userId"] = UserId,
                ["parameterName"] = ParameterName,
                ["propertyId"] = PropertyId,
                ["parameterValues"] = parameterValuesJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RarityParameterStatusId != null) {
                writer.WritePropertyName("rarityParameterStatusId");
                writer.Write(RarityParameterStatusId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ParameterName != null) {
                writer.WritePropertyName("parameterName");
                writer.Write(ParameterName.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (ParameterValues != null) {
                writer.WritePropertyName("parameterValues");
                writer.WriteArrayStart();
                foreach (var parameterValue in ParameterValues)
                {
                    if (parameterValue != null) {
                        parameterValue.WriteJson(writer);
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
            var other = obj as RarityParameterStatus;
            var diff = 0;
            if (RarityParameterStatusId == null && RarityParameterStatusId == other.RarityParameterStatusId)
            {
                // null and null
            }
            else
            {
                diff += RarityParameterStatusId.CompareTo(other.RarityParameterStatusId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (ParameterName == null && ParameterName == other.ParameterName)
            {
                // null and null
            }
            else
            {
                diff += ParameterName.CompareTo(other.ParameterName);
            }
            if (PropertyId == null && PropertyId == other.PropertyId)
            {
                // null and null
            }
            else
            {
                diff += PropertyId.CompareTo(other.PropertyId);
            }
            if (ParameterValues == null && ParameterValues == other.ParameterValues)
            {
                // null and null
            }
            else
            {
                diff += ParameterValues.Length - other.ParameterValues.Length;
                for (var i = 0; i < ParameterValues.Length; i++)
                {
                    diff += ParameterValues[i].CompareTo(other.ParameterValues[i]);
                }
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
                if (RarityParameterStatusId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.rarityParameterStatusId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.userId.error.tooLong"),
                    });
                }
            }
            {
                if (ParameterName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.parameterName.error.tooLong"),
                    });
                }
            }
            {
                if (PropertyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.propertyId.error.tooLong"),
                    });
                }
            }
            {
                if (ParameterValues.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.parameterValues.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rarityParameterStatus", "enchant.rarityParameterStatus.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RarityParameterStatus {
                RarityParameterStatusId = RarityParameterStatusId,
                UserId = UserId,
                ParameterName = ParameterName,
                PropertyId = PropertyId,
                ParameterValues = ParameterValues.Clone() as Gs2.Gs2Enchant.Model.RarityParameterValue[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}