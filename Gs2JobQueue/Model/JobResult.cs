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
	public class JobResult : IComparable
	{
        public string JobResultId { set; get; }
        public string JobId { set; get; }
        public int? TryNumber { set; get; }
        public int? StatusCode { set; get; }
        public string Result { set; get; }
        public long? TryAt { set; get; }

        public JobResult WithJobResultId(string jobResultId) {
            this.JobResultId = jobResultId;
            return this;
        }

        public JobResult WithJobId(string jobId) {
            this.JobId = jobId;
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static JobResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new JobResult()
                .WithJobResultId(!data.Keys.Contains("jobResultId") || data["jobResultId"] == null ? null : data["jobResultId"].ToString())
                .WithJobId(!data.Keys.Contains("jobId") || data["jobId"] == null ? null : data["jobId"].ToString())
                .WithTryNumber(!data.Keys.Contains("tryNumber") || data["tryNumber"] == null ? null : (int?)int.Parse(data["tryNumber"].ToString()))
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)int.Parse(data["statusCode"].ToString()))
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithTryAt(!data.Keys.Contains("tryAt") || data["tryAt"] == null ? null : (long?)long.Parse(data["tryAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["jobResultId"] = JobResultId,
                ["jobId"] = JobId,
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
            if (TryNumber != null) {
                writer.WritePropertyName("tryNumber");
                writer.Write(int.Parse(TryNumber.ToString()));
            }
            if (StatusCode != null) {
                writer.WritePropertyName("statusCode");
                writer.Write(int.Parse(StatusCode.ToString()));
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            if (TryAt != null) {
                writer.WritePropertyName("tryAt");
                writer.Write(long.Parse(TryAt.ToString()));
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
    }
}