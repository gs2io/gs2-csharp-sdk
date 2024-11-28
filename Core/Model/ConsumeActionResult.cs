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
	public class ConsumeActionResult : IComparable
	{
        public string Action { set; get; } = null!;
        public string ConsumeRequest { set; get; } = null!;
        public int? StatusCode { set; get; } = null!;
        public string ConsumeResult { set; get; } = null!;
        public ConsumeActionResult WithAction(string action) {
            this.Action = action;
            return this;
        }
        public ConsumeActionResult WithConsumeRequest(string consumeRequest) {
            this.ConsumeRequest = consumeRequest;
            return this;
        }
        public ConsumeActionResult WithStatusCode(int? statusCode) {
            this.StatusCode = statusCode;
            return this;
        }
        public ConsumeActionResult WithConsumeResult(string consumeResult) {
            this.ConsumeResult = consumeResult;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ConsumeActionResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ConsumeActionResult()
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithConsumeRequest(!data.Keys.Contains("consumeRequest") || data["consumeRequest"] == null ? null : data["consumeRequest"].ToString())
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)(data["statusCode"].ToString().Contains(".") ? (int)double.Parse(data["statusCode"].ToString()) : int.Parse(data["statusCode"].ToString())))
                .WithConsumeResult(!data.Keys.Contains("consumeResult") || data["consumeResult"] == null ? null : data["consumeResult"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["action"] = Action,
                ["consumeRequest"] = ConsumeRequest,
                ["statusCode"] = StatusCode,
                ["consumeResult"] = ConsumeResult,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (ConsumeRequest != null) {
                writer.WritePropertyName("consumeRequest");
                writer.Write(ConsumeRequest.ToString());
            }
            if (StatusCode != null) {
                writer.WritePropertyName("statusCode");
                writer.Write((StatusCode.ToString().Contains(".") ? (int)double.Parse(StatusCode.ToString()) : int.Parse(StatusCode.ToString())));
            }
            if (ConsumeResult != null) {
                writer.WritePropertyName("consumeResult");
                writer.Write(ConsumeResult.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ConsumeActionResult;
            var diff = 0;
            if (Action == null && Action == other.Action)
            {
                // null and null
            }
            else
            {
                diff += Action.CompareTo(other.Action);
            }
            if (ConsumeRequest == null && ConsumeRequest == other.ConsumeRequest)
            {
                // null and null
            }
            else
            {
                diff += ConsumeRequest.CompareTo(other.ConsumeRequest);
            }
            if (StatusCode == null && StatusCode == other.StatusCode)
            {
                // null and null
            }
            else
            {
                diff += (int)(StatusCode - other.StatusCode);
            }
            if (ConsumeResult == null && ConsumeResult == other.ConsumeResult)
            {
                // null and null
            }
            else
            {
                diff += ConsumeResult.CompareTo(other.ConsumeResult);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Action.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("consumeActionResult", "account.consumeActionResult.action.error.tooLong"),
                    });
                }
            }
            {
                if (ConsumeRequest.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("consumeActionResult", "account.consumeActionResult.consumeRequest.error.tooLong"),
                    });
                }
            }
            {
                if (StatusCode < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("consumeActionResult", "account.consumeActionResult.statusCode.error.invalid"),
                    });
                }
                if (StatusCode > 999) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("consumeActionResult", "account.consumeActionResult.statusCode.error.invalid"),
                    });
                }
            }
            {
                if (ConsumeResult.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("consumeActionResult", "account.consumeActionResult.consumeResult.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ConsumeActionResult {
                Action = Action,
                ConsumeRequest = ConsumeRequest,
                StatusCode = StatusCode,
                ConsumeResult = ConsumeResult,
            };
        }
    }
}