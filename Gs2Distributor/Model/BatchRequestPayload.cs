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
	public partial class BatchRequestPayload : IComparable
	{
        public string RequestId { set; get; }
        public string Service { set; get; }
        public string MethodName { set; get; }
        public string Parameter { set; get; }
        public BatchRequestPayload WithRequestId(string requestId) {
            this.RequestId = requestId;
            return this;
        }
        public BatchRequestPayload WithService(string service) {
            this.Service = service;
            return this;
        }
        public BatchRequestPayload WithMethodName(string methodName) {
            this.MethodName = methodName;
            return this;
        }
        public BatchRequestPayload WithParameter(string parameter) {
            this.Parameter = parameter;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BatchRequestPayload FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BatchRequestPayload()
                .WithRequestId(!data.Keys.Contains("requestId") || data["requestId"] == null ? null : data["requestId"].ToString())
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithMethodName(!data.Keys.Contains("methodName") || data["methodName"] == null ? null : data["methodName"].ToString())
                .WithParameter(!data.Keys.Contains("parameter") || data["parameter"] == null ? null : data["parameter"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["requestId"] = RequestId,
                ["service"] = Service,
                ["methodName"] = MethodName,
                ["parameter"] = Parameter,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RequestId != null) {
                writer.WritePropertyName("requestId");
                writer.Write(RequestId.ToString());
            }
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            if (MethodName != null) {
                writer.WritePropertyName("methodName");
                writer.Write(MethodName.ToString());
            }
            if (Parameter != null) {
                writer.WritePropertyName("parameter");
                writer.Write(Parameter.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BatchRequestPayload;
            var diff = 0;
            if (RequestId == null && RequestId == other.RequestId)
            {
                // null and null
            }
            else
            {
                diff += RequestId.CompareTo(other.RequestId);
            }
            if (Service == null && Service == other.Service)
            {
                // null and null
            }
            else
            {
                diff += Service.CompareTo(other.Service);
            }
            if (MethodName == null && MethodName == other.MethodName)
            {
                // null and null
            }
            else
            {
                diff += MethodName.CompareTo(other.MethodName);
            }
            if (Parameter == null && Parameter == other.Parameter)
            {
                // null and null
            }
            else
            {
                diff += Parameter.CompareTo(other.Parameter);
            }
            return diff;
        }

        public void Validate() {
            {
                if (RequestId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("batchRequestPayload", "distributor.batchRequestPayload.requestId.error.tooLong"),
                    });
                }
            }
            {
                switch (Service) {
                    case "account":
                    case "adReward":
                    case "auth":
                    case "buff":
                    case "chat":
                    case "datastore":
                    case "deploy":
                    case "dictionary":
                    case "distributor":
                    case "enchant":
                    case "enhance":
                    case "exchange":
                    case "experience":
                    case "formation":
                    case "friend":
                    case "gateway":
                    case "grade":
                    case "guard":
                    case "guild":
                    case "identifier":
                    case "idle":
                    case "inbox":
                    case "inventory":
                    case "jobQueue":
                    case "key":
                    case "limit":
                    case "lock":
                    case "log":
                    case "loginReward":
                    case "lottery":
                    case "matchmaking":
                    case "megaField":
                    case "mission":
                    case "money":
                    case "money2":
                    case "news":
                    case "quest":
                    case "ranking":
                    case "ranking2":
                    case "realtime":
                    case "schedule":
                    case "script":
                    case "seasonRating":
                    case "serialKey":
                    case "showcase":
                    case "skillTree":
                    case "stamina":
                    case "stateMachine":
                    case "version":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("batchRequestPayload", "distributor.batchRequestPayload.service.error.invalid"),
                        });
                }
            }
            {
                if (MethodName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("batchRequestPayload", "distributor.batchRequestPayload.methodName.error.tooLong"),
                    });
                }
            }
            {
                if (Parameter.Length > 10240) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("batchRequestPayload", "distributor.batchRequestPayload.parameter.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new BatchRequestPayload {
                RequestId = RequestId,
                Service = Service,
                MethodName = MethodName,
                Parameter = Parameter,
            };
        }
    }
}