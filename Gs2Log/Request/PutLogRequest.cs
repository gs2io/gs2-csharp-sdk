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
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PutLogRequest : Gs2Request<PutLogRequest>
	{
         public string LoggingNamespaceId { set; get; }
         public string LogCategory { set; get; }
         public string Payload { set; get; }
        public PutLogRequest WithLoggingNamespaceId(string loggingNamespaceId) {
            this.LoggingNamespaceId = loggingNamespaceId;
            return this;
        }
        public PutLogRequest WithLogCategory(string logCategory) {
            this.LogCategory = logCategory;
            return this;
        }
        public PutLogRequest WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PutLogRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PutLogRequest()
                .WithLoggingNamespaceId(!data.Keys.Contains("loggingNamespaceId") || data["loggingNamespaceId"] == null ? null : data["loggingNamespaceId"].ToString())
                .WithLogCategory(!data.Keys.Contains("logCategory") || data["logCategory"] == null ? null : data["logCategory"].ToString())
                .WithPayload(!data.Keys.Contains("payload") || data["payload"] == null ? null : data["payload"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["loggingNamespaceId"] = LoggingNamespaceId,
                ["logCategory"] = LogCategory,
                ["payload"] = Payload,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (LoggingNamespaceId != null) {
                writer.WritePropertyName("loggingNamespaceId");
                writer.Write(LoggingNamespaceId.ToString());
            }
            if (LogCategory != null) {
                writer.WritePropertyName("logCategory");
                writer.Write(LogCategory.ToString());
            }
            if (Payload != null) {
                writer.WritePropertyName("payload");
                writer.Write(Payload.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += LoggingNamespaceId + ":";
            key += LogCategory + ":";
            key += Payload + ":";
            return key;
        }
    }
}