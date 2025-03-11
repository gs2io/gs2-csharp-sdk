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

namespace Gs2.Gs2Distributor.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class BatchResultPayload : IComparable
	{
        public string RequestId { set; get; }
        public int? StatusCode { set; get; }
        public string ResultPayload { set; get; }
        public BatchResultPayload WithRequestId(string requestId) {
            this.RequestId = requestId;
            return this;
        }
        public BatchResultPayload WithStatusCode(int? statusCode) {
            this.StatusCode = statusCode;
            return this;
        }
        public BatchResultPayload WithResultPayload(string resultPayload) {
            this.ResultPayload = resultPayload;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BatchResultPayload FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BatchResultPayload()
                .WithRequestId(!data.Keys.Contains("requestId") || data["requestId"] == null ? null : data["requestId"].ToString())
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)(data["statusCode"].ToString().Contains(".") ? (int)double.Parse(data["statusCode"].ToString()) : int.Parse(data["statusCode"].ToString())))
                .WithResultPayload(!data.Keys.Contains("resultPayload") || data["resultPayload"] == null ? null : data["resultPayload"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["requestId"] = RequestId,
                ["statusCode"] = StatusCode,
                ["resultPayload"] = ResultPayload,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RequestId != null) {
                writer.WritePropertyName("requestId");
                writer.Write(RequestId.ToString());
            }
            if (StatusCode != null) {
                writer.WritePropertyName("statusCode");
                writer.Write((StatusCode.ToString().Contains(".") ? (int)double.Parse(StatusCode.ToString()) : int.Parse(StatusCode.ToString())));
            }
            if (ResultPayload != null) {
                writer.WritePropertyName("resultPayload");
                writer.Write(ResultPayload.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BatchResultPayload;
            var diff = 0;
            if (RequestId == null && RequestId == other.RequestId)
            {
                // null and null
            }
            else
            {
                diff += RequestId.CompareTo(other.RequestId);
            }
            if (StatusCode == null && StatusCode == other.StatusCode)
            {
                // null and null
            }
            else
            {
                diff += (int)(StatusCode - other.StatusCode);
            }
            if (ResultPayload == null && ResultPayload == other.ResultPayload)
            {
                // null and null
            }
            else
            {
                diff += ResultPayload.CompareTo(other.ResultPayload);
            }
            return diff;
        }

        public void Validate() {
            {
                if (RequestId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("batchResultPayload", "distributor.batchResultPayload.requestId.error.tooLong"),
                    });
                }
            }
            {
                if (StatusCode < 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("batchResultPayload", "distributor.batchResultPayload.statusCode.error.invalid"),
                    });
                }
                if (StatusCode > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("batchResultPayload", "distributor.batchResultPayload.statusCode.error.invalid"),
                    });
                }
            }
            {
                if (ResultPayload.Length > 10240) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("batchResultPayload", "distributor.batchResultPayload.resultPayload.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new BatchResultPayload {
                RequestId = RequestId,
                StatusCode = StatusCode,
                ResultPayload = ResultPayload,
            };
        }
    }
}