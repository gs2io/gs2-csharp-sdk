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
using Gs2.Gs2Deploy.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Deploy.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetEventRequest : Gs2Request<GetEventRequest>
	{
         public string StackName { set; get; } = null!;
         public string EventName { set; get; } = null!;
        public GetEventRequest WithStackName(string stackName) {
            this.StackName = stackName;
            return this;
        }
        public GetEventRequest WithEventName(string eventName) {
            this.EventName = eventName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetEventRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetEventRequest()
                .WithStackName(!data.Keys.Contains("stackName") || data["stackName"] == null ? null : data["stackName"].ToString())
                .WithEventName(!data.Keys.Contains("eventName") || data["eventName"] == null ? null : data["eventName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["stackName"] = StackName,
                ["eventName"] = EventName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StackName != null) {
                writer.WritePropertyName("stackName");
                writer.Write(StackName.ToString());
            }
            if (EventName != null) {
                writer.WritePropertyName("eventName");
                writer.Write(EventName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += StackName + ":";
            key += EventName + ":";
            return key;
        }
    }
}