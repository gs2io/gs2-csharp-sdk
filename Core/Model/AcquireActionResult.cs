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

namespace Gs2.Core.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AcquireActionResult : IComparable
	{
        public string Action { set; get; } = null!;
        public string AcquireRequest { set; get; } = null!;
        public int? StatusCode { set; get; } = null!;
        public string AcquireResult { set; get; } = null!;
        public AcquireActionResult WithAction(string action) {
            this.Action = action;
            return this;
        }
        public AcquireActionResult WithAcquireRequest(string acquireRequest) {
            this.AcquireRequest = acquireRequest;
            return this;
        }
        public AcquireActionResult WithStatusCode(int? statusCode) {
            this.StatusCode = statusCode;
            return this;
        }
        public AcquireActionResult WithAcquireResult(string acquireResult) {
            this.AcquireResult = acquireResult;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireActionResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireActionResult()
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithAcquireRequest(!data.Keys.Contains("acquireRequest") || data["acquireRequest"] == null ? null : data["acquireRequest"].ToString())
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)(data["statusCode"].ToString().Contains(".") ? (int)double.Parse(data["statusCode"].ToString()) : int.Parse(data["statusCode"].ToString())))
                .WithAcquireResult(!data.Keys.Contains("acquireResult") || data["acquireResult"] == null ? null : data["acquireResult"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["action"] = Action,
                ["acquireRequest"] = AcquireRequest,
                ["statusCode"] = StatusCode,
                ["acquireResult"] = AcquireResult,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (AcquireRequest != null) {
                writer.WritePropertyName("acquireRequest");
                writer.Write(AcquireRequest.ToString());
            }
            if (StatusCode != null) {
                writer.WritePropertyName("statusCode");
                writer.Write((StatusCode.ToString().Contains(".") ? (int)double.Parse(StatusCode.ToString()) : int.Parse(StatusCode.ToString())));
            }
            if (AcquireResult != null) {
                writer.WritePropertyName("acquireResult");
                writer.Write(AcquireResult.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AcquireActionResult;
            var diff = 0;
            if (Action == null && Action == other.Action)
            {
                // null and null
            }
            else
            {
                diff += Action.CompareTo(other.Action);
            }
            if (AcquireRequest == null && AcquireRequest == other.AcquireRequest)
            {
                // null and null
            }
            else
            {
                diff += AcquireRequest.CompareTo(other.AcquireRequest);
            }
            if (StatusCode == null && StatusCode == other.StatusCode)
            {
                // null and null
            }
            else
            {
                diff += (int)(StatusCode - other.StatusCode);
            }
            if (AcquireResult == null && AcquireResult == other.AcquireResult)
            {
                // null and null
            }
            else
            {
                diff += AcquireResult.CompareTo(other.AcquireResult);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Action.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionResult", "account.acquireActionResult.action.error.tooLong"),
                    });
                }
            }
            {
                if (AcquireRequest.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionResult", "account.acquireActionResult.acquireRequest.error.tooLong"),
                    });
                }
            }
            {
                if (StatusCode < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionResult", "account.acquireActionResult.statusCode.error.invalid"),
                    });
                }
                if (StatusCode > 999) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionResult", "account.acquireActionResult.statusCode.error.invalid"),
                    });
                }
            }
            {
                if (AcquireResult.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("acquireActionResult", "account.acquireActionResult.acquireResult.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new AcquireActionResult {
                Action = Action,
                AcquireRequest = AcquireRequest,
                StatusCode = StatusCode,
                AcquireResult = AcquireResult,
            };
        }
    }
}