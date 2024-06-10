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

namespace Gs2.Gs2Deploy.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ChangeSet : IComparable
	{
        public string ResourceName { set; get; } = null!;
        public string ResourceType { set; get; } = null!;
        public string Operation { set; get; } = null!;
        public ChangeSet WithResourceName(string resourceName) {
            this.ResourceName = resourceName;
            return this;
        }
        public ChangeSet WithResourceType(string resourceType) {
            this.ResourceType = resourceType;
            return this;
        }
        public ChangeSet WithOperation(string operation) {
            this.Operation = operation;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ChangeSet FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ChangeSet()
                .WithResourceName(!data.Keys.Contains("resourceName") || data["resourceName"] == null ? null : data["resourceName"].ToString())
                .WithResourceType(!data.Keys.Contains("resourceType") || data["resourceType"] == null ? null : data["resourceType"].ToString())
                .WithOperation(!data.Keys.Contains("operation") || data["operation"] == null ? null : data["operation"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["resourceName"] = ResourceName,
                ["resourceType"] = ResourceType,
                ["operation"] = Operation,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ResourceName != null) {
                writer.WritePropertyName("resourceName");
                writer.Write(ResourceName.ToString());
            }
            if (ResourceType != null) {
                writer.WritePropertyName("resourceType");
                writer.Write(ResourceType.ToString());
            }
            if (Operation != null) {
                writer.WritePropertyName("operation");
                writer.Write(Operation.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ChangeSet;
            var diff = 0;
            if (ResourceName == null && ResourceName == other.ResourceName)
            {
                // null and null
            }
            else
            {
                diff += ResourceName.CompareTo(other.ResourceName);
            }
            if (ResourceType == null && ResourceType == other.ResourceType)
            {
                // null and null
            }
            else
            {
                diff += ResourceType.CompareTo(other.ResourceType);
            }
            if (Operation == null && Operation == other.Operation)
            {
                // null and null
            }
            else
            {
                diff += Operation.CompareTo(other.Operation);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ResourceName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("changeSet", "deploy.changeSet.resourceName.error.tooLong"),
                    });
                }
            }
            {
                if (ResourceType.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("changeSet", "deploy.changeSet.resourceType.error.tooLong"),
                    });
                }
            }
            {
                switch (Operation) {
                    case "create":
                    case "update":
                    case "delete":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("changeSet", "deploy.changeSet.operation.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new ChangeSet {
                ResourceName = ResourceName,
                ResourceType = ResourceType,
                Operation = Operation,
            };
        }
    }
}