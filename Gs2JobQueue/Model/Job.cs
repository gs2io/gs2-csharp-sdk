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

namespace Gs2.Gs2JobQueue.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Job : IComparable
	{
        public string JobId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string ScriptId { set; get; } = null!;
        public string Args { set; get; } = null!;
        public int? CurrentRetryCount { set; get; } = null!;
        public int? MaxTryCount { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public Job WithJobId(string jobId) {
            this.JobId = jobId;
            return this;
        }
        public Job WithName(string name) {
            this.Name = name;
            return this;
        }
        public Job WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Job WithScriptId(string scriptId) {
            this.ScriptId = scriptId;
            return this;
        }
        public Job WithArgs(string args) {
            this.Args = args;
            return this;
        }
        public Job WithCurrentRetryCount(int? currentRetryCount) {
            this.CurrentRetryCount = currentRetryCount;
            return this;
        }
        public Job WithMaxTryCount(int? maxTryCount) {
            this.MaxTryCount = maxTryCount;
            return this;
        }
        public Job WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Job WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+)",
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

        private static System.Text.RegularExpressions.Regex _jobNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetJobNameFromGrn(
            string grn
        )
        {
            var match = _jobNameRegex.Match(grn);
            if (!match.Success || !match.Groups["jobName"].Success)
            {
                return null;
            }
            return match.Groups["jobName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Job FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Job()
                .WithJobId(!data.Keys.Contains("jobId") || data["jobId"] == null ? null : data["jobId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithScriptId(!data.Keys.Contains("scriptId") || data["scriptId"] == null ? null : data["scriptId"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithCurrentRetryCount(!data.Keys.Contains("currentRetryCount") || data["currentRetryCount"] == null ? null : (int?)(data["currentRetryCount"].ToString().Contains(".") ? (int)double.Parse(data["currentRetryCount"].ToString()) : int.Parse(data["currentRetryCount"].ToString())))
                .WithMaxTryCount(!data.Keys.Contains("maxTryCount") || data["maxTryCount"] == null ? null : (int?)(data["maxTryCount"].ToString().Contains(".") ? (int)double.Parse(data["maxTryCount"].ToString()) : int.Parse(data["maxTryCount"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["jobId"] = JobId,
                ["name"] = Name,
                ["userId"] = UserId,
                ["scriptId"] = ScriptId,
                ["args"] = Args,
                ["currentRetryCount"] = CurrentRetryCount,
                ["maxTryCount"] = MaxTryCount,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (JobId != null) {
                writer.WritePropertyName("jobId");
                writer.Write(JobId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ScriptId != null) {
                writer.WritePropertyName("scriptId");
                writer.Write(ScriptId.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            if (CurrentRetryCount != null) {
                writer.WritePropertyName("currentRetryCount");
                writer.Write((CurrentRetryCount.ToString().Contains(".") ? (int)double.Parse(CurrentRetryCount.ToString()) : int.Parse(CurrentRetryCount.ToString())));
            }
            if (MaxTryCount != null) {
                writer.WritePropertyName("maxTryCount");
                writer.Write((MaxTryCount.ToString().Contains(".") ? (int)double.Parse(MaxTryCount.ToString()) : int.Parse(MaxTryCount.ToString())));
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
            var other = obj as Job;
            var diff = 0;
            if (JobId == null && JobId == other.JobId)
            {
                // null and null
            }
            else
            {
                diff += JobId.CompareTo(other.JobId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (ScriptId == null && ScriptId == other.ScriptId)
            {
                // null and null
            }
            else
            {
                diff += ScriptId.CompareTo(other.ScriptId);
            }
            if (Args == null && Args == other.Args)
            {
                // null and null
            }
            else
            {
                diff += Args.CompareTo(other.Args);
            }
            if (CurrentRetryCount == null && CurrentRetryCount == other.CurrentRetryCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentRetryCount - other.CurrentRetryCount);
            }
            if (MaxTryCount == null && MaxTryCount == other.MaxTryCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxTryCount - other.MaxTryCount);
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
                if (JobId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.jobId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.name.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.userId.error.tooLong"),
                    });
                }
            }
            {
                if (ScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.scriptId.error.tooLong"),
                    });
                }
            }
            {
                if (Args.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.args.error.tooLong"),
                    });
                }
            }
            {
                if (CurrentRetryCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.currentRetryCount.error.invalid"),
                    });
                }
                if (CurrentRetryCount > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.currentRetryCount.error.invalid"),
                    });
                }
            }
            {
                if (MaxTryCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.maxTryCount.error.invalid"),
                    });
                }
                if (MaxTryCount > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.maxTryCount.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("job", "jobQueue.job.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Job {
                JobId = JobId,
                Name = Name,
                UserId = UserId,
                ScriptId = ScriptId,
                Args = Args,
                CurrentRetryCount = CurrentRetryCount,
                MaxTryCount = MaxTryCount,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}