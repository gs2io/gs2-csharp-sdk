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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Schedule.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Schedule.Request
{
	[Preserve]
	[System.Serializable]
	public class GetEventRequest : Gs2Request<GetEventRequest>
	{
        public string NamespaceName { set; get; }
        public string EventName { set; get; }
        public string AccessToken { set; get; }

        public GetEventRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public GetEventRequest WithEventName(string eventName) {
            this.EventName = eventName;
            return this;
        }

        public GetEventRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

    	[Preserve]
        public static GetEventRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetEventRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithEventName(!data.Keys.Contains("eventName") || data["eventName"] == null ? null : data["eventName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["eventName"] = EventName,
                ["accessToken"] = AccessToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (EventName != null) {
                writer.WritePropertyName("eventName");
                writer.Write(EventName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}