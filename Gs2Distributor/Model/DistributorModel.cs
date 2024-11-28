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

namespace Gs2.Gs2Distributor.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class DistributorModel : IComparable
	{
        public string DistributorModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string InboxNamespaceId { set; get; } = null!;
        public string[] WhiteListTargetIds { set; get; } = null!;
        public DistributorModel WithDistributorModelId(string distributorModelId) {
            this.DistributorModelId = distributorModelId;
            return this;
        }
        public DistributorModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public DistributorModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public DistributorModel WithInboxNamespaceId(string inboxNamespaceId) {
            this.InboxNamespaceId = inboxNamespaceId;
            return this;
        }
        public DistributorModel WithWhiteListTargetIds(string[] whiteListTargetIds) {
            this.WhiteListTargetIds = whiteListTargetIds;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):model:(?<distributorName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):model:(?<distributorName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):model:(?<distributorName>.+)",
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

        private static System.Text.RegularExpressions.Regex _distributorNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):model:(?<distributorName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetDistributorNameFromGrn(
            string grn
        )
        {
            var match = _distributorNameRegex.Match(grn);
            if (!match.Success || !match.Groups["distributorName"].Success)
            {
                return null;
            }
            return match.Groups["distributorName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DistributorModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DistributorModel()
                .WithDistributorModelId(!data.Keys.Contains("distributorModelId") || data["distributorModelId"] == null ? null : data["distributorModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInboxNamespaceId(!data.Keys.Contains("inboxNamespaceId") || data["inboxNamespaceId"] == null ? null : data["inboxNamespaceId"].ToString())
                .WithWhiteListTargetIds(!data.Keys.Contains("whiteListTargetIds") || data["whiteListTargetIds"] == null || !data["whiteListTargetIds"].IsArray ? null : data["whiteListTargetIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData whiteListTargetIdsJsonData = null;
            if (WhiteListTargetIds != null && WhiteListTargetIds.Length > 0)
            {
                whiteListTargetIdsJsonData = new JsonData();
                foreach (var whiteListTargetId in WhiteListTargetIds)
                {
                    whiteListTargetIdsJsonData.Add(whiteListTargetId);
                }
            }
            return new JsonData {
                ["distributorModelId"] = DistributorModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["inboxNamespaceId"] = InboxNamespaceId,
                ["whiteListTargetIds"] = whiteListTargetIdsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DistributorModelId != null) {
                writer.WritePropertyName("distributorModelId");
                writer.Write(DistributorModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InboxNamespaceId != null) {
                writer.WritePropertyName("inboxNamespaceId");
                writer.Write(InboxNamespaceId.ToString());
            }
            if (WhiteListTargetIds != null) {
                writer.WritePropertyName("whiteListTargetIds");
                writer.WriteArrayStart();
                foreach (var whiteListTargetId in WhiteListTargetIds)
                {
                    if (whiteListTargetId != null) {
                        writer.Write(whiteListTargetId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DistributorModel;
            var diff = 0;
            if (DistributorModelId == null && DistributorModelId == other.DistributorModelId)
            {
                // null and null
            }
            else
            {
                diff += DistributorModelId.CompareTo(other.DistributorModelId);
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
            if (InboxNamespaceId == null && InboxNamespaceId == other.InboxNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += InboxNamespaceId.CompareTo(other.InboxNamespaceId);
            }
            if (WhiteListTargetIds == null && WhiteListTargetIds == other.WhiteListTargetIds)
            {
                // null and null
            }
            else
            {
                diff += WhiteListTargetIds.Length - other.WhiteListTargetIds.Length;
                for (var i = 0; i < WhiteListTargetIds.Length; i++)
                {
                    diff += WhiteListTargetIds[i].CompareTo(other.WhiteListTargetIds[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (DistributorModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("distributorModel", "distributor.distributorModel.distributorModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("distributorModel", "distributor.distributorModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("distributorModel", "distributor.distributorModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (InboxNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("distributorModel", "distributor.distributorModel.inboxNamespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (WhiteListTargetIds.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("distributorModel", "distributor.distributorModel.whiteListTargetIds.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new DistributorModel {
                DistributorModelId = DistributorModelId,
                Name = Name,
                Metadata = Metadata,
                InboxNamespaceId = InboxNamespaceId,
                WhiteListTargetIds = WhiteListTargetIds.Clone() as string[],
            };
        }
    }
}