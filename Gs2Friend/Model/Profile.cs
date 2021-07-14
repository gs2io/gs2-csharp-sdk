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
using UnityEngine.Scripting;

namespace Gs2.Gs2Friend.Model
{

	[Preserve]
	public class Profile : IComparable
	{
        public string ProfileId { set; get; }
        public string UserId { set; get; }
        public string PublicProfile { set; get; }
        public string FollowerProfile { set; get; }
        public string FriendProfile { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Profile WithProfileId(string profileId) {
            this.ProfileId = profileId;
            return this;
        }

        public Profile WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Profile WithPublicProfile(string publicProfile) {
            this.PublicProfile = publicProfile;
            return this;
        }

        public Profile WithFollowerProfile(string followerProfile) {
            this.FollowerProfile = followerProfile;
            return this;
        }

        public Profile WithFriendProfile(string friendProfile) {
            this.FriendProfile = friendProfile;
            return this;
        }

        public Profile WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Profile WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static Profile FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Profile()
                .WithProfileId(!data.Keys.Contains("profileId") || data["profileId"] == null ? null : data["profileId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPublicProfile(!data.Keys.Contains("publicProfile") || data["publicProfile"] == null ? null : data["publicProfile"].ToString())
                .WithFollowerProfile(!data.Keys.Contains("followerProfile") || data["followerProfile"] == null ? null : data["followerProfile"].ToString())
                .WithFriendProfile(!data.Keys.Contains("friendProfile") || data["friendProfile"] == null ? null : data["friendProfile"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["profileId"] = ProfileId,
                ["userId"] = UserId,
                ["publicProfile"] = PublicProfile,
                ["followerProfile"] = FollowerProfile,
                ["friendProfile"] = FriendProfile,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ProfileId != null) {
                writer.WritePropertyName("profileId");
                writer.Write(ProfileId.ToString());
            }
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
            if (FriendProfile != null) {
                writer.WritePropertyName("friendProfile");
                writer.Write(FriendProfile.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Profile;
            var diff = 0;
            if (ProfileId == null && ProfileId == other.ProfileId)
            {
                // null and null
            }
            else
            {
                diff += ProfileId.CompareTo(other.ProfileId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (PublicProfile == null && PublicProfile == other.PublicProfile)
            {
                // null and null
            }
            else
            {
                diff += PublicProfile.CompareTo(other.PublicProfile);
            }
            if (FollowerProfile == null && FollowerProfile == other.FollowerProfile)
            {
                // null and null
            }
            else
            {
                diff += FollowerProfile.CompareTo(other.FollowerProfile);
            }
            if (FriendProfile == null && FriendProfile == other.FriendProfile)
            {
                // null and null
            }
            else
            {
                diff += FriendProfile.CompareTo(other.FriendProfile);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}