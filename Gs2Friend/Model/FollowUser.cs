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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Friend.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class FollowUser : IComparable
	{
        public string UserId { set; get; }
        public string PublicProfile { set; get; }
        public string FollowerProfile { set; get; }
        public FollowUser WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public FollowUser WithPublicProfile(string publicProfile) {
            this.PublicProfile = publicProfile;
            return this;
        }
        public FollowUser WithFollowerProfile(string followerProfile) {
            this.FollowerProfile = followerProfile;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static FollowUser FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new FollowUser()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPublicProfile(!data.Keys.Contains("publicProfile") || data["publicProfile"] == null ? null : data["publicProfile"].ToString())
                .WithFollowerProfile(!data.Keys.Contains("followerProfile") || data["followerProfile"] == null ? null : data["followerProfile"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["publicProfile"] = PublicProfile,
                ["followerProfile"] = FollowerProfile,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (PublicProfile != null) {
                writer.WritePropertyName("publicProfile");
                writer.Write(PublicProfile.ToString());
            }
            if (FollowerProfile != null) {
                writer.WritePropertyName("followerProfile");
                writer.Write(FollowerProfile.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as FollowUser;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type FollowUser.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(PublicProfile, other.PublicProfile);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(FollowerProfile, other.FollowerProfile);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("followUser", "friend.followUser.userId.error.tooLong"),
                    });
                }
            }
            {
                if (PublicProfile.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("followUser", "friend.followUser.publicProfile.error.tooLong"),
                    });
                }
            }
            {
                if (FollowerProfile.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("followUser", "friend.followUser.followerProfile.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new FollowUser {
                UserId = UserId,
                PublicProfile = PublicProfile,
                FollowerProfile = FollowerProfile,
            };
        }
    }
}