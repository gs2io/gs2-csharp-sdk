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
	public class VerifyActionResult : IComparable
	{
        public string Action { set; get; } = null!;
        public string VerifyRequest { set; get; } = null!;
        public int? StatusCode { set; get; } = null!;
        public string VerifyResult { set; get; } = null!;
        public VerifyActionResult WithAction(string action) {
            this.Action = action;
            return this;
        }
        public VerifyActionResult WithVerifyRequest(string verifyRequest) {
            this.VerifyRequest = verifyRequest;
            return this;
        }
        public VerifyActionResult WithStatusCode(int? statusCode) {
            this.StatusCode = statusCode;
            return this;
        }
        public VerifyActionResult WithVerifyResult(string verifyResult) {
            this.VerifyResult = verifyResult;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyActionResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyActionResult()
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithVerifyRequest(!data.Keys.Contains("verifyRequest") || data["verifyRequest"] == null ? null : data["verifyRequest"].ToString())
                .WithStatusCode(!data.Keys.Contains("statusCode") || data["statusCode"] == null ? null : (int?)(data["statusCode"].ToString().Contains(".") ? (int)double.Parse(data["statusCode"].ToString()) : int.Parse(data["statusCode"].ToString())))
                .WithVerifyResult(!data.Keys.Contains("verifyResult") || data["verifyResult"] == null ? null : data["verifyResult"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["action"] = Action,
                ["verifyRequest"] = VerifyRequest,
                ["statusCode"] = StatusCode,
                ["verifyResult"] = VerifyResult,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (VerifyRequest != null) {
                writer.WritePropertyName("verifyRequest");
                writer.Write(VerifyRequest.ToString());
            }
            if (StatusCode != null) {
                writer.WritePropertyName("statusCode");
                writer.Write((StatusCode.ToString().Contains(".") ? (int)double.Parse(StatusCode.ToString()) : int.Parse(StatusCode.ToString())));
            }
            if (VerifyResult != null) {
                writer.WritePropertyName("verifyResult");
                writer.Write(VerifyResult.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as VerifyActionResult;
            var diff = 0;
            if (Action == null && Action == other.Action)
            {
                // null and null
            }
            else
            {
                diff += Action.CompareTo(other.Action);
            }
            if (VerifyRequest == null && VerifyRequest == other.VerifyRequest)
            {
                // null and null
            }
            else
            {
                diff += VerifyRequest.CompareTo(other.VerifyRequest);
            }
            if (StatusCode == null && StatusCode == other.StatusCode)
            {
                // null and null
            }
            else
            {
                diff += (int)(StatusCode - other.StatusCode);
            }
            if (VerifyResult == null && VerifyResult == other.VerifyResult)
            {
                // null and null
            }
            else
            {
                diff += VerifyResult.CompareTo(other.VerifyResult);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Action.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("verifyActionResult", "account.verifyActionResult.action.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyRequest.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("verifyActionResult", "account.verifyActionResult.verifyRequest.error.tooLong"),
                    });
                }
            }
            {
                if (StatusCode < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("verifyActionResult", "account.verifyActionResult.statusCode.error.invalid"),
                    });
                }
                if (StatusCode > 999) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("verifyActionResult", "account.verifyActionResult.statusCode.error.invalid"),
                    });
                }
            }
            {
                if (VerifyResult.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("verifyActionResult", "account.verifyActionResult.verifyResult.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new VerifyActionResult {
                Action = Action,
                VerifyRequest = VerifyRequest,
                StatusCode = StatusCode,
                VerifyResult = VerifyResult,
            };
        }
    }
}