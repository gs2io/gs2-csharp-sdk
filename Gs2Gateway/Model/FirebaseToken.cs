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
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Gateway.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class FirebaseToken : IComparable
	{
        public string FirebaseTokenId { set; get; }
        public string UserId { set; get; }
        public string Token { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public FirebaseToken WithFirebaseTokenId(string firebaseTokenId) {
            this.FirebaseTokenId = firebaseTokenId;
            return this;
        }
        public FirebaseToken WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public FirebaseToken WithToken(string token) {
            this.Token = token;
            return this;
        }
        public FirebaseToken WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public FirebaseToken WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):firebase:token",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):firebase:token",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):firebase:token",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):firebase:token",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static FirebaseToken FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new FirebaseToken()
                .WithFirebaseTokenId(!data.Keys.Contains("firebaseTokenId") || data["firebaseTokenId"] == null ? null : data["firebaseTokenId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithToken(!data.Keys.Contains("token") || data["token"] == null ? null : data["token"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["firebaseTokenId"] = FirebaseTokenId,
                ["userId"] = UserId,
                ["token"] = Token,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (FirebaseTokenId != null) {
                writer.WritePropertyName("firebaseTokenId");
                writer.Write(FirebaseTokenId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Token != null) {
                writer.WritePropertyName("token");
                writer.Write(Token.ToString());
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
            var other = obj as FirebaseToken;
            var diff = 0;
            if (FirebaseTokenId == null && FirebaseTokenId == other.FirebaseTokenId)
            {
                // null and null
            }
            else
            {
                diff += FirebaseTokenId.CompareTo(other.FirebaseTokenId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Token == null && Token == other.Token)
            {
                // null and null
            }
            else
            {
                diff += Token.CompareTo(other.Token);
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