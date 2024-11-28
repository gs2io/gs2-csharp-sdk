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
	public class Resource : IComparable
	{
        public string ResourceId { set; get; } = null!;
        public string Type { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Request { set; get; } = null!;
        public string Response { set; get; } = null!;
        public string RollbackContext { set; get; } = null!;
        public string RollbackRequest { set; get; } = null!;
        public string[] RollbackAfter { set; get; } = null!;
        public Gs2.Gs2Deploy.Model.OutputField[] OutputFields { set; get; } = null!;
        public string WorkId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
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
            var diff = 0;
            if (ResourceId == null && ResourceId == other.ResourceId)
            {
                // null and null
            }
            else
            {
                diff += ResourceId.CompareTo(other.ResourceId);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Request == null && Request == other.Request)
            {
                // null and null
            }
            else
            {
                diff += Request.CompareTo(other.Request);
            }
            if (Response == null && Response == other.Response)
            {
                // null and null
            }
            else
            {
                diff += Response.CompareTo(other.Response);
            }
            if (RollbackContext == null && RollbackContext == other.RollbackContext)
            {
                // null and null
            }
            else
            {
                diff += RollbackContext.CompareTo(other.RollbackContext);
            }
            if (RollbackRequest == null && RollbackRequest == other.RollbackRequest)
            {
                // null and null
            }
            else
            {
                diff += RollbackRequest.CompareTo(other.RollbackRequest);
            }
            if (RollbackAfter == null && RollbackAfter == other.RollbackAfter)
            {
                // null and null
            }
            else
            {
                diff += RollbackAfter.Length - other.RollbackAfter.Length;
                for (var i = 0; i < RollbackAfter.Length; i++)
                {
                    diff += RollbackAfter[i].CompareTo(other.RollbackAfter[i]);
                }
            }
            if (OutputFields == null && OutputFields == other.OutputFields)
            {
                // null and null
            }
            else
            {
                diff += OutputFields.Length - other.OutputFields.Length;
                for (var i = 0; i < OutputFields.Length; i++)
                {
                    diff += OutputFields[i].CompareTo(other.OutputFields[i]);
                }
            }
            if (WorkId == null && WorkId == other.WorkId)
            {
                // null and null
            }
            else
            {
                diff += WorkId.CompareTo(other.WorkId);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
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
                RollbackAfter = RollbackAfter.Clone() as string[],
                OutputFields = OutputFields.Clone() as Gs2.Gs2Deploy.Model.OutputField[],
                WorkId = WorkId,
                CreatedAt = CreatedAt,
            };
        }
    }
}