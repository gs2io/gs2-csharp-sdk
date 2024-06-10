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
	public class DistributeResource : IComparable
	{
        public string Action { set; get; } = null!;
        public string Request { set; get; } = null!;
        public DistributeResource WithAction(string action) {
            this.Action = action;
            return this;
        }
        public DistributeResource WithRequest(string request) {
            this.Request = request;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DistributeResource FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DistributeResource()
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithRequest(!data.Keys.Contains("request") || data["request"] == null ? null : data["request"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["action"] = Action,
                ["request"] = Request,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (Request != null) {
                writer.WritePropertyName("request");
                writer.Write(Request.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DistributeResource;
            var diff = 0;
            if (Action == null && Action == other.Action)
            {
                // null and null
            }
            else
            {
                diff += Action.CompareTo(other.Action);
            }
            if (Request == null && Request == other.Request)
            {
                // null and null
            }
            else
            {
                diff += Request.CompareTo(other.Request);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Action.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("distributeResource", "distributor.distributeResource.action.error.tooLong"),
                    });
                }
            }
            {
                if (Request.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("distributeResource", "distributor.distributeResource.request.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new DistributeResource {
                Action = Action,
                Request = Request,
            };
        }
    }
}