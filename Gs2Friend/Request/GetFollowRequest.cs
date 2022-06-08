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
using Gs2.Gs2Friend.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Friend.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetFollowRequest : Gs2Request<GetFollowRequest>
	{
        public string NamespaceName { set; get; }
        public string AccessToken { set; get; }
        public string TargetUserId { set; get; }
        public bool? WithProfile { set; get; }
        public GetFollowRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetFollowRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public GetFollowRequest WithTargetUserId(string targetUserId) {
            this.TargetUserId = targetUserId;
            return this;
        }
        public GetFollowRequest WithWithProfile(bool? withProfile) {
            this.WithProfile = withProfile;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetFollowRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetFollowRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTargetUserId(!data.Keys.Contains("targetUserId") || data["targetUserId"] == null ? null : data["targetUserId"].ToString())
                .WithWithProfile(!data.Keys.Contains("withProfile") || data["withProfile"] == null ? null : (bool?)bool.Parse(data["withProfile"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["targetUserId"] = TargetUserId,
                ["withProfile"] = WithProfile,
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
            if (TargetUserId != null) {
                writer.WritePropertyName("targetUserId");
                writer.Write(TargetUserId.ToString());
            }
            if (WithProfile != null) {
                writer.WritePropertyName("withProfile");
                writer.Write(bool.Parse(WithProfile.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}