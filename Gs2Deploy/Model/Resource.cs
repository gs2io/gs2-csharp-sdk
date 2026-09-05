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

namespace Gs2.Gs2Deploy.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Resource : IComparable
	{
        public string ResourceId { set; get; }
        public string Type { set; get; }
        public string Name { set; get; }
        public string Request { set; get; }
        public string Response { set; get; }
        public string RollbackContext { set; get; }
        public string RollbackRequest { set; get; }
        public string[] RollbackAfter { set; get; }
        public Gs2.Gs2Deploy.Model.OutputField[] OutputFields { set; get; }
        public string WorkId { set; get; }
        public long? CreatedAt { set; get; }
        public Resource WithResourceId(string resourceId) {
            this.ResourceId = resourceId;
            return this;
        }
        public Resource WithType(string type) {
            this.Type = type;
            return this;
        }
        public Resource WithName(string name) {
            this.Name = name;
            return this;
        }
        public Resource WithRequest(string request) {
            this.Request = request;
            return this;
        }
        public Resource WithResponse(string response) {
            this.Response = response;
            return this;
        }
        public Resource WithRollbackContext(string rollbackContext) {
            this.RollbackContext = rollbackContext;
            return this;
        }
        public Resource WithRollbackRequest(string rollbackRequest) {
            this.RollbackRequest = rollbackRequest;
            return this;
        }
        public Resource WithRollbackAfter(string[] rollbackAfter) {
            this.RollbackAfter = rollbackAfter;
            return this;
        }
        public Resource WithOutputFields(Gs2.Gs2Deploy.Model.OutputField[] outputFields) {
            this.OutputFields = outputFields;
            return this;
        }
        public Resource WithWorkId(string workId) {
            this.WorkId = workId;
            return this;
        }
        public Resource WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):resource:(?<resourceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):resource:(?<resourceName>.+)",
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

        private static System.Text.RegularExpressions.Regex _stackNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):resource:(?<resourceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetStackNameFromGrn(
            string grn
        )
        {
            var match = _stackNameRegex.Match(grn);
            if (!match.Success || !match.Groups["stackName"].Success)
            {
                return null;
            }
            return match.Groups["stackName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _resourceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):resource:(?<resourceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetResourceNameFromGrn(
            string grn
        )
        {
            var match = _resourceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["resourceName"].Success)
            {
                return null;
            }
            return match.Groups["resourceName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Resource FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Resource()
                .WithResourceId(!data.Keys.Contains("resourceId") || data["resourceId"] == null ? null : data["resourceId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithRequest(!data.Keys.Contains("request") || data["request"] == null ? null : data["request"].ToString())
                .WithResponse(!data.Keys.Contains("response") || data["response"] == null ? null : data["response"].ToString())
                .WithRollbackContext(!data.Keys.Contains("rollbackContext") || data["rollbackContext"] == null ? null : data["rollbackContext"].ToString())
                .WithRollbackRequest(!data.Keys.Contains("rollbackRequest") || data["rollbackRequest"] == null ? null : data["rollbackRequest"].ToString())
                .WithRollbackAfter(!data.Keys.Contains("rollbackAfter") || data["rollbackAfter"] == null || !data["rollbackAfter"].IsArray ? null : data["rollbackAfter"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithOutputFields(!data.Keys.Contains("outputFields") || data["outputFields"] == null || !data["outputFields"].IsArray ? null : data["outputFields"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Deploy.Model.OutputField.FromJson(v);
                }).ToArray())
                .WithWorkId(!data.Keys.Contains("workId") || data["workId"] == null ? null : data["workId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData rollbackAfterJsonData = null;
            if (RollbackAfter != null && RollbackAfter.Length > 0)
            {
                rollbackAfterJsonData = new JsonData();
                foreach (var rollbackAfter in RollbackAfter)
                {
                    rollbackAfterJsonData.Add(rollbackAfter);
                }
            }
            JsonData outputFieldsJsonData = null;
            if (OutputFields != null && OutputFields.Length > 0)
            {
                outputFieldsJsonData = new JsonData();
                foreach (var outputField in OutputFields)
                {
                    outputFieldsJsonData.Add(outputField.ToJson());
                }
            }
            return new JsonData {
                ["resourceId"] = ResourceId,
                ["type"] = Type,
                ["name"] = Name,
                ["request"] = Request,
                ["response"] = Response,
                ["rollbackContext"] = RollbackContext,
                ["rollbackRequest"] = RollbackRequest,
                ["rollbackAfter"] = rollbackAfterJsonData,
                ["outputFields"] = outputFieldsJsonData,
                ["workId"] = WorkId,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ResourceId != null) {
                writer.WritePropertyName("resourceId");
                writer.Write(ResourceId.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Request != null) {
                writer.WritePropertyName("request");
                writer.Write(Request.ToString());
            }
            if (Response != null) {
                writer.WritePropertyName("response");
                writer.Write(Response.ToString());
            }
            if (RollbackContext != null) {
                writer.WritePropertyName("rollbackContext");
                writer.Write(RollbackContext.ToString());
            }
            if (RollbackRequest != null) {
                writer.WritePropertyName("rollbackRequest");
                writer.Write(RollbackRequest.ToString());
            }
            if (RollbackAfter != null) {
                writer.WritePropertyName("rollbackAfter");
                writer.WriteArrayStart();
                foreach (var rollbackAfter in RollbackAfter)
                {
                    if (rollbackAfter != null) {
                        writer.Write(rollbackAfter.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (OutputFields != null) {
                writer.WritePropertyName("outputFields");
                writer.WriteArrayStart();
                foreach (var outputField in OutputFields)
                {
                    if (outputField != null) {
                        outputField.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (WorkId != null) {
                writer.WritePropertyName("workId");
                writer.Write(WorkId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Resource;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Resource.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(ResourceId, other.ResourceId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Type, other.Type);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Request, other.Request);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Response, other.Response);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RollbackContext, other.RollbackContext);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RollbackRequest, other.RollbackRequest);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(RollbackAfter, other.RollbackAfter);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(OutputFields, other.OutputFields);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(WorkId, other.WorkId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (ResourceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.resourceId.error.tooLong"),
                    });
                }
            }
            {
                if (Type.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.type.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.name.error.tooLong"),
                    });
                }
            }
            {
                if (Request.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.request.error.tooLong"),
                    });
                }
            }
            {
                if (Response.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.response.error.tooLong"),
                    });
                }
            }
            {
                switch (RollbackContext) {
                    case "create":
                    case "update":
                    case "delete":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("resource", "deploy.resource.rollbackContext.error.invalid"),
                        });
                }
            }
            {
                if (RollbackRequest.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.rollbackRequest.error.tooLong"),
                    });
                }
            }
            {
                if (RollbackAfter.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.rollbackAfter.error.tooMany"),
                    });
                }
            }
            {
                if (OutputFields.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.outputFields.error.tooMany"),
                    });
                }
            }
            {
                if (WorkId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.workId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("resource", "deploy.resource.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Resource {
                ResourceId = ResourceId,
                Type = Type,
                Name = Name,
                Request = Request,
                Response = Response,
                RollbackContext = RollbackContext,
                RollbackRequest = RollbackRequest,
                RollbackAfter = RollbackAfter?.Clone() as string[],
                OutputFields = OutputFields?.Clone() as Gs2.Gs2Deploy.Model.OutputField[],
                WorkId = WorkId,
                CreatedAt = CreatedAt,
            };
        }
    }
}