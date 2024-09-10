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
 *
 * deny overwrite
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2JobQueue.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class JobResult : IComparable
	{
        public string JobResultId { set; get; } = null!;
        public string JobId { set; get; } = null!;
        public string ScriptId { set; get; } = null!;
        public string Args { set; get; } = null!;
        public int? TryNumber { set; get; } = null!;
        public int? StatusCode { set; get; } = null!;
        public string Result { set; get; } = null!;
        public long? TryAt { set; get; } = null!;
        public JobResult WithJobResultId(string jobResultId) {
            this.JobResultId = jobResultId;
            return this;
        }
        public JobResult WithJobId(string jobId) {
            this.JobId = jobId;
            return this;
        }
        public JobResult WithScriptId(string scriptId) {
            this.ScriptId = scriptId;
            return this;
        }
        public JobResult WithArgs(string args) {
            this.Args = args;
            return this;
        }
        public JobResult WithTryNumber(int? tryNumber) {
            this.TryNumber = tryNumber;
            return this;
        }
        public JobResult WithStatusCode(int? statusCode) {
            this.StatusCode = statusCode;
            return this;
        }
        public JobResult WithResult(string result) {
            this.Result = result;
            return this;
        }
        public JobResult WithTryAt(long? tryAt) {
            this.TryAt = tryAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+):jobResult:(?<tryNumber>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+):jobResult:(?<tryNumber>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+):jobResult:(?<tryNumber>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+):jobResult:(?<tryNumber>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+):jobResult:(?<tryNumber>.+)",
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

        private static System.Text.RegularExpressions.Regex _tryNumberRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):queue:(?<namespaceName>.+):user:(?<userId>.+):job:(?<jobName>.+):jobResult:(?<tryNumber>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTryNumberFromGrn(
            string grn
        )
        {
            var match = _tryNumberRegex.Match(grn);
            if (!match.Success || !match.Groups["tryNumber"].Success)
            {
                return null;
            }
            return match.Groups["tryNumber"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static JobResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            var result = new JobResult()
                .WithJobResultId(!data.Keys.Contains("jobResultId") || data["jobResultId"] == null ? null : data["jobResultId"].ToString())
                .WithJobId(!data.Keys.Contains("jobId") || data["jobId"] == null ? null : data["jobId"].ToString())
                .WithScriptId(!data.Keys.Contains("scriptId") || data["scriptId"] == null ? null : data["scriptId"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithTryNumber(!data.Keys.Contains("tryNumber") || data["tryNumber"] == null ? null : (int?)(data["tryNumber"].ToString().Contains(".") ? (int)double.Parse(data["tryNumber"].ToString()) : int.Parse(data["tryNumber"].ToString())))
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)(data["statusCode"].ToString().Contains(".") ? (int)double.Parse(data["statusCode"].ToString()) : int.Parse(data["statusCode"].ToString())))
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithTryAt(!data.Keys.Contains("tryAt") || data["tryAt"] == null ? null : (long?)(data["tryAt"].ToString().Contains(".") ? (long)double.Parse(data["tryAt"].ToString()) : long.Parse(data["tryAt"].ToString())));

            if (result != null) {
                Telemetry.HandleJob(Job.GetJobNameFromGrn(result.JobId), result);
            }

            return result;
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["jobResultId"] = JobResultId,
                ["jobId"] = JobId,
                ["scriptId"] = ScriptId,
                ["args"] = Args,
                ["tryNumber"] = TryNumber,
                ["statusCode"] = StatusCode,
                ["result"] = Result,
                ["tryAt"] = TryAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (JobResultId != null) {
                writer.WritePropertyName("jobResultId");
                writer.Write(JobResultId.ToString());
            }
            if (JobId != null) {
                writer.WritePropertyName("jobId");
                writer.Write(JobId.ToString());
            }
            if (ScriptId != null) {
                writer.WritePropertyName("scriptId");
                writer.Write(ScriptId.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            if (TryNumber != null) {
                writer.WritePropertyName("tryNumber");
                writer.Write((TryNumber.ToString().Contains(".") ? (int)double.Parse(TryNumber.ToString()) : int.Parse(TryNumber.ToString())));
            }
            if (StatusCode != null) {
                writer.WritePropertyName("statusCode");
                writer.Write((StatusCode.ToString().Contains(".") ? (int)double.Parse(StatusCode.ToString()) : int.Parse(StatusCode.ToString())));
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            if (TryAt != null) {
                writer.WritePropertyName("tryAt");
                writer.Write((TryAt.ToString().Contains(".") ? (long)double.Parse(TryAt.ToString()) : long.Parse(TryAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as JobResult;
            var diff = 0;
            if (JobResultId == null && JobResultId == other.JobResultId)
            {
                // null and null
            }
            else
            {
                diff += JobResultId.CompareTo(other.JobResultId);
            }
            if (JobId == null && JobId == other.JobId)
            {
                // null and null
            }
            else
            {
                diff += JobId.CompareTo(other.JobId);
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
            if (TryNumber == null && TryNumber == other.TryNumber)
            {
                // null and null
            }
            else
            {
                diff += (int)(TryNumber - other.TryNumber);
            }
            if (StatusCode == null && StatusCode == other.StatusCode)
            {
                // null and null
            }
            else
            {
                diff += (int)(StatusCode - other.StatusCode);
            }
            if (Result == null && Result == other.Result)
            {
                // null and null
            }
            else
            {
                diff += Result.CompareTo(other.Result);
            }
            if (TryAt == null && TryAt == other.TryAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(TryAt - other.TryAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (JobResultId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.jobResultId.error.tooLong"),
                    });
                }
            }
            {
                if (JobId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.jobId.error.tooLong"),
                    });
                }
            }
            {
                if (ScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.scriptId.error.tooLong"),
                    });
                }
            }
            {
                if (Args.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.args.error.tooLong"),
                    });
                }
            }
            {
                if (TryNumber < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.tryNumber.error.invalid"),
                    });
                }
                if (TryNumber > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.tryNumber.error.invalid"),
                    });
                }
            }
            {
                if (StatusCode < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.statusCode.error.invalid"),
                    });
                }
                if (StatusCode > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.statusCode.error.invalid"),
                    });
                }
            }
            {
                if (Result.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.result.error.tooLong"),
                    });
                }
            }
            {
                if (TryAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.tryAt.error.invalid"),
                    });
                }
                if (TryAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResult", "jobQueue.jobResult.tryAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new JobResult {
                JobResultId = JobResultId,
                JobId = JobId,
                ScriptId = ScriptId,
                Args = Args,
                TryNumber = TryNumber,
                StatusCode = StatusCode,
                Result = Result,
                TryAt = TryAt,
            };
        }
    }
}