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
	public class SendInGameLogRequest : Gs2Request<SendInGameLogRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public Gs2.Gs2Log.Model.InGameLogTag[] Tags { set; get; } = null!;
         public string Payload { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SendInGameLogRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SendInGameLogRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public SendInGameLogRequest WithTags(Gs2.Gs2Log.Model.InGameLogTag[] tags) {
            this.Tags = tags;
            return this;
        }
        public SendInGameLogRequest WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }

        public SendInGameLogRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SendInGameLogRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SendInGameLogRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTags(!data.Keys.Contains("tags") || data["tags"] == null || !data["tags"].IsArray ? null : data["tags"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.InGameLogTag.FromJson(v);
                }).ToArray())
                .WithPayload(!data.Keys.Contains("payload") || data["payload"] == null ? null : data["payload"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData tagsJsonData = null;
            if (Tags != null && Tags.Length > 0)
            {
                tagsJsonData = new JsonData();
                foreach (var tag in Tags)
                {
                    tagsJsonData.Add(tag.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["tags"] = tagsJsonData,
                ["payload"] = Payload,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (Tags != null) {
                writer.WritePropertyName("tags");
                writer.WriteArrayStart();
                foreach (var tag in Tags)
                {
                    if (tag != null) {
                        tag.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Payload != null) {
                writer.WritePropertyName("payload");
                writer.Write(Payload.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += Tags + ":";
            key += Payload + ":";
            return key;
        }
    }
}