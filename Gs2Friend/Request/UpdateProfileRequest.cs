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
	public class UpdateProfileRequest : Gs2Request<UpdateProfileRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string PublicProfile { set; get; } = null!;
         public string FollowerProfile { set; get; } = null!;
         public string FriendProfile { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public UpdateProfileRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateProfileRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public UpdateProfileRequest WithPublicProfile(string publicProfile) {
            this.PublicProfile = publicProfile;
            return this;
        }
        public UpdateProfileRequest WithFollowerProfile(string followerProfile) {
            this.FollowerProfile = followerProfile;
            return this;
        }
        public UpdateProfileRequest WithFriendProfile(string friendProfile) {
            this.FriendProfile = friendProfile;
            return this;
        }

        public UpdateProfileRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateProfileRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateProfileRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithPublicProfile(!data.Keys.Contains("publicProfile") || data["publicProfile"] == null ? null : data["publicProfile"].ToString())
                .WithFollowerProfile(!data.Keys.Contains("followerProfile") || data["followerProfile"] == null ? null : data["followerProfile"].ToString())
                .WithFriendProfile(!data.Keys.Contains("friendProfile") || data["friendProfile"] == null ? null : data["friendProfile"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["publicProfile"] = PublicProfile,
                ["followerProfile"] = FollowerProfile,
                ["friendProfile"] = FriendProfile,
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
            if (PublicProfile != null) {
                writer.WritePropertyName("publicProfile");
                writer.Write(PublicProfile.ToString());
            }
            if (FollowerProfile != null) {
                writer.WritePropertyName("followerProfile");
                writer.Write(FollowerProfile.ToString());
            }
            if (FriendProfile != null) {
                writer.WritePropertyName("friendProfile");
                writer.Write(FriendProfile.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += PublicProfile + ":";
            key += FollowerProfile + ":";
            key += FriendProfile + ":";
            return key;
        }
    }
}