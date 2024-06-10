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

namespace Gs2.Gs2Identifier.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SecurityPolicy : IComparable
	{
        public string SecurityPolicyId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Policy { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public SecurityPolicy WithSecurityPolicyId(string securityPolicyId) {
            this.SecurityPolicyId = securityPolicyId;
            return this;
        }
        public SecurityPolicy WithName(string name) {
            this.Name = name;
            return this;
        }
        public SecurityPolicy WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public SecurityPolicy WithPolicy(string policy) {
            this.Policy = policy;
            return this;
        }
        public SecurityPolicy WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SecurityPolicy WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2::(?<ownerId>.+):identifier:securityPolicy:(?<securityPolicyName>.+)",
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

        private static System.Text.RegularExpressions.Regex _securityPolicyNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2::(?<ownerId>.+):identifier:securityPolicy:(?<securityPolicyName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSecurityPolicyNameFromGrn(
            string grn
        )
        {
            var match = _securityPolicyNameRegex.Match(grn);
            if (!match.Success || !match.Groups["securityPolicyName"].Success)
            {
                return null;
            }
            return match.Groups["securityPolicyName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SecurityPolicy FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SecurityPolicy()
                .WithSecurityPolicyId(!data.Keys.Contains("securityPolicyId") || data["securityPolicyId"] == null ? null : data["securityPolicyId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithPolicy(!data.Keys.Contains("policy") || data["policy"] == null ? null : data["policy"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["securityPolicyId"] = SecurityPolicyId,
                ["name"] = Name,
                ["description"] = Description,
                ["policy"] = Policy,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SecurityPolicyId != null) {
                writer.WritePropertyName("securityPolicyId");
                writer.Write(SecurityPolicyId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Policy != null) {
                writer.WritePropertyName("policy");
                writer.Write(Policy.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SecurityPolicy;
            var diff = 0;
            if (SecurityPolicyId == null && SecurityPolicyId == other.SecurityPolicyId)
            {
                // null and null
            }
            else
            {
                diff += SecurityPolicyId.CompareTo(other.SecurityPolicyId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Policy == null && Policy == other.Policy)
            {
                // null and null
            }
            else
            {
                diff += Policy.CompareTo(other.Policy);
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
            return diff;
        }

        public void Validate() {
            {
                if (SecurityPolicyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.securityPolicyId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.description.error.tooLong"),
                    });
                }
            }
            {
                if (Policy.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.policy.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("securityPolicy", "identifier.securityPolicy.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SecurityPolicy {
                SecurityPolicyId = SecurityPolicyId,
                Name = Name,
                Description = Description,
                Policy = Policy,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}