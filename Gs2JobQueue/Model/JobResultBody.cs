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
	public class JobResultBody : IComparable
	{
        public int? TryNumber { set; get; } = null!;
        public int? StatusCode { set; get; } = null!;
        public string Result { set; get; } = null!;
        public long? TryAt { set; get; } = null!;
        public JobResultBody WithTryNumber(int? tryNumber) {
            this.TryNumber = tryNumber;
            return this;
        }
        public JobResultBody WithStatusCode(int? statusCode) {
            this.StatusCode = statusCode;
            return this;
        }
        public JobResultBody WithResult(string result) {
            this.Result = result;
            return this;
        }
        public JobResultBody WithTryAt(long? tryAt) {
            this.TryAt = tryAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static JobResultBody FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new JobResultBody()
                .WithTryNumber(!data.Keys.Contains("tryNumber") || data["tryNumber"] == null ? null : (int?)(data["tryNumber"].ToString().Contains(".") ? (int)double.Parse(data["tryNumber"].ToString()) : int.Parse(data["tryNumber"].ToString())))
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)(data["statusCode"].ToString().Contains(".") ? (int)double.Parse(data["statusCode"].ToString()) : int.Parse(data["statusCode"].ToString())))
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithTryAt(!data.Keys.Contains("tryAt") || data["tryAt"] == null ? null : (long?)(data["tryAt"].ToString().Contains(".") ? (long)double.Parse(data["tryAt"].ToString()) : long.Parse(data["tryAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["tryNumber"] = TryNumber,
                ["statusCode"] = StatusCode,
                ["result"] = Result,
                ["tryAt"] = TryAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            var other = obj as JobResultBody;
            var diff = 0;
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
                if (TryNumber < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResultBody", "jobQueue.jobResultBody.tryNumber.error.invalid"),
                    });
                }
                if (TryNumber > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResultBody", "jobQueue.jobResultBody.tryNumber.error.invalid"),
                    });
                }
            }
            {
                if (StatusCode < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResultBody", "jobQueue.jobResultBody.statusCode.error.invalid"),
                    });
                }
                if (StatusCode > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResultBody", "jobQueue.jobResultBody.statusCode.error.invalid"),
                    });
                }
            }
            {
                if (Result.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResultBody", "jobQueue.jobResultBody.result.error.tooLong"),
                    });
                }
            }
            {
                if (TryAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResultBody", "jobQueue.jobResultBody.tryAt.error.invalid"),
                    });
                }
                if (TryAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("jobResultBody", "jobQueue.jobResultBody.tryAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new JobResultBody {
                TryNumber = TryNumber,
                StatusCode = StatusCode,
                Result = Result,
                TryAt = TryAt,
            };
        }
    }
}