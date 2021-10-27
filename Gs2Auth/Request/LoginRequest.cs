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
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Auth.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class LoginRequest : Gs2Request<LoginRequest>
	{
        public string UserId { set; get; }
        public int? TimeOffset { set; get; }

        public LoginRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public LoginRequest WithTimeOffset(int? timeOffset) {
            this.TimeOffset = timeOffset;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LoginRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LoginRequest()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTimeOffset(!data.Keys.Contains("timeOffset") || data["timeOffset"] == null ? null : (int?)int.Parse(data["timeOffset"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["timeOffset"] = TimeOffset,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TimeOffset != null) {
                writer.WritePropertyName("timeOffset");
                writer.Write(int.Parse(TimeOffset.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}