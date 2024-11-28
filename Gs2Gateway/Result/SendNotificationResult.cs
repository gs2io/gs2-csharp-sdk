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
using Gs2.Gs2Gateway.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Gateway.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SendNotificationResult : IResult
	{
        public string Protocol { set; get; } = null!;
        public string[] SendConnectionIds { set; get; } = null!;

        public SendNotificationResult WithProtocol(string protocol) {
            this.Protocol = protocol;
            return this;
        }

        public SendNotificationResult WithSendConnectionIds(string[] sendConnectionIds) {
            this.SendConnectionIds = sendConnectionIds;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SendNotificationResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SendNotificationResult()
                .WithProtocol(!data.Keys.Contains("protocol") || data["protocol"] == null ? null : data["protocol"].ToString())
                .WithSendConnectionIds(!data.Keys.Contains("sendConnectionIds") || data["sendConnectionIds"] == null || !data["sendConnectionIds"].IsArray ? null : data["sendConnectionIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData sendConnectionIdsJsonData = null;
            if (SendConnectionIds != null && SendConnectionIds.Length > 0)
            {
                sendConnectionIdsJsonData = new JsonData();
                foreach (var sendConnectionId in SendConnectionIds)
                {
                    sendConnectionIdsJsonData.Add(sendConnectionId);
                }
            }
            return new JsonData {
                ["protocol"] = Protocol,
                ["sendConnectionIds"] = sendConnectionIdsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Protocol != null) {
                writer.WritePropertyName("protocol");
                writer.Write(Protocol.ToString());
            }
            if (SendConnectionIds != null) {
                writer.WritePropertyName("sendConnectionIds");
                writer.WriteArrayStart();
                foreach (var sendConnectionId in SendConnectionIds)
                {
                    if (sendConnectionId != null) {
                        writer.Write(sendConnectionId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }
    }
}