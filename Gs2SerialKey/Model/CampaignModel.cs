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
	public class CampaignModel : IComparable
	{
        public string CampaignId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public bool? EnableCampaignCode { set; get; } = null!;
        public CampaignModel WithCampaignId(string campaignId) {
            this.CampaignId = campaignId;
            return this;
        }
        public CampaignModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public CampaignModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CampaignModel WithEnableCampaignCode(bool? enableCampaignCode) {
            this.EnableCampaignCode = enableCampaignCode;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):model:campaign:(?<campaignModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):model:campaign:(?<campaignModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):model:campaign:(?<campaignModelName>.+)",
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

        private static System.Text.RegularExpressions.Regex _campaignModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):serialKey:(?<namespaceName>.+):model:campaign:(?<campaignModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCampaignModelNameFromGrn(
            string grn
        )
        {
            var match = _campaignModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["campaignModelName"].Success)
            {
                return null;
            }
            return match.Groups["campaignModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CampaignModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CampaignModel()
                .WithCampaignId(!data.Keys.Contains("campaignId") || data["campaignId"] == null ? null : data["campaignId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithEnableCampaignCode(!data.Keys.Contains("enableCampaignCode") || data["enableCampaignCode"] == null ? null : (bool?)bool.Parse(data["enableCampaignCode"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["campaignId"] = CampaignId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["enableCampaignCode"] = EnableCampaignCode,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CampaignId != null) {
                writer.WritePropertyName("campaignId");
                writer.Write(CampaignId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (EnableCampaignCode != null) {
                writer.WritePropertyName("enableCampaignCode");
                writer.Write(bool.Parse(EnableCampaignCode.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CampaignModel;
            var diff = 0;
            if (CampaignId == null && CampaignId == other.CampaignId)
            {
                // null and null
            }
            else
            {
                diff += CampaignId.CompareTo(other.CampaignId);
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
            if (EnableCampaignCode == null && EnableCampaignCode == other.EnableCampaignCode)
            {
                // null and null
            }
            else
            {
                diff += EnableCampaignCode == other.EnableCampaignCode ? 0 : 1;
            }
            return diff;
        }

        public void Validate() {
            {
                if (CampaignId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("campaignModel", "serialKey.campaignModel.campaignId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("campaignModel", "serialKey.campaignModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("campaignModel", "serialKey.campaignModel.metadata.error.tooLong"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new CampaignModel {
                CampaignId = CampaignId,
                Name = Name,
                Metadata = Metadata,
                EnableCampaignCode = EnableCampaignCode,
            };
        }
    }
}