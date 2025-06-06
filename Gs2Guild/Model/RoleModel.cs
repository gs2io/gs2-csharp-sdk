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

namespace Gs2.Gs2Guild.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class RoleModel : IComparable
	{
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string PolicyDocument { set; get; }
        public RoleModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public RoleModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RoleModel WithPolicyDocument(string policyDocument) {
            this.PolicyDocument = policyDocument;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RoleModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RoleModel()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPolicyDocument(!data.Keys.Contains("policyDocument") || data["policyDocument"] == null ? null : data["policyDocument"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["policyDocument"] = PolicyDocument,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (PolicyDocument != null) {
                writer.WritePropertyName("policyDocument");
                writer.Write(PolicyDocument.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RoleModel;
            var diff = 0;
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
            if (PolicyDocument == null && PolicyDocument == other.PolicyDocument)
            {
                // null and null
            }
            else
            {
                diff += PolicyDocument.CompareTo(other.PolicyDocument);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("roleModel", "guild.roleModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("roleModel", "guild.roleModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (PolicyDocument.Length > 10240) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("roleModel", "guild.roleModel.policyDocument.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new RoleModel {
                Name = Name,
                Metadata = Metadata,
                PolicyDocument = PolicyDocument,
            };
        }
    }
}