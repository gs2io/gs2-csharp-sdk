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

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Insight : IComparable
	{
        public string InsightId { set; get; }
        public string Name { set; get; }
        public string TaskId { set; get; }
        public string Host { set; get; }
        public string Password { set; get; }
        public string Status { set; get; }
        public long? CreatedAt { set; get; }
        public long? Revision { set; get; }
        public Insight WithInsightId(string insightId) {
            this.InsightId = insightId;
            return this;
        }
        public Insight WithName(string name) {
            this.Name = name;
            return this;
        }
        public Insight WithTaskId(string taskId) {
            this.TaskId = taskId;
            return this;
        }
        public Insight WithHost(string host) {
            this.Host = host;
            return this;
        }
        public Insight WithPassword(string password) {
            this.Password = password;
            return this;
        }
        public Insight WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public Insight WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Insight WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):insight:(?<insightName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):insight:(?<insightName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):insight:(?<insightName>.+)",
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

        private static System.Text.RegularExpressions.Regex _insightNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):insight:(?<insightName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetInsightNameFromGrn(
            string grn
        )
        {
            var match = _insightNameRegex.Match(grn);
            if (!match.Success || !match.Groups["insightName"].Success)
            {
                return null;
            }
            return match.Groups["insightName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Insight FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Insight()
                .WithInsightId(!data.Keys.Contains("insightId") || data["insightId"] == null ? null : data["insightId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithTaskId(!data.Keys.Contains("taskId") || data["taskId"] == null ? null : data["taskId"].ToString())
                .WithHost(!data.Keys.Contains("host") || data["host"] == null ? null : data["host"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["insightId"] = InsightId,
                ["name"] = Name,
                ["taskId"] = TaskId,
                ["host"] = Host,
                ["password"] = Password,
                ["status"] = Status,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (InsightId != null) {
                writer.WritePropertyName("insightId");
                writer.Write(InsightId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (TaskId != null) {
                writer.WritePropertyName("taskId");
                writer.Write(TaskId.ToString());
            }
            if (Host != null) {
                writer.WritePropertyName("host");
                writer.Write(Host.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Insight;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Insight.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(InsightId, other.InsightId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TaskId, other.TaskId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Host, other.Host);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Password, other.Password);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Status, other.Status);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (InsightId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.insightId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.name.error.tooLong"),
                    });
                }
            }
            {
                if (TaskId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.taskId.error.tooLong"),
                    });
                }
            }
            {
                if (Host.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.host.error.tooLong"),
                    });
                }
            }
            {
                if (Password.Length > 32) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.password.error.tooLong"),
                    });
                }
            }
            {
                switch (Status) {
                    case "ALLOCATING":
                    case "LAUNCHING":
                    case "ACTIVE":
                    case "DELETED":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("insight", "log.insight.status.error.invalid"),
                        });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("insight", "log.insight.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Insight {
                InsightId = InsightId,
                Name = Name,
                TaskId = TaskId,
                Host = Host,
                Password = Password,
                Status = Status,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}