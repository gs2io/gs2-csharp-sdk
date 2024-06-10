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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SubscribeUser : IComparable
	{
        public string SubscribeUserId { set; get; } = null!;
        public string CategoryName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string TargetUserId { set; get; } = null!;
        public SubscribeUser WithSubscribeUserId(string subscribeUserId) {
            this.SubscribeUserId = subscribeUserId;
            return this;
        }
        public SubscribeUser WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public SubscribeUser WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SubscribeUser WithTargetUserId(string targetUserId) {
            this.TargetUserId = targetUserId;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+):(?<targetUserId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+):(?<targetUserId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+):(?<targetUserId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+):(?<targetUserId>.+)",
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

        private static System.Text.RegularExpressions.Regex _categoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+):(?<targetUserId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCategoryNameFromGrn(
            string grn
        )
        {
            var match = _categoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["categoryName"].Success)
            {
                return null;
            }
            return match.Groups["categoryName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _targetUserIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):subscribe:category:(?<categoryName>.+):(?<targetUserId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTargetUserIdFromGrn(
            string grn
        )
        {
            var match = _targetUserIdRegex.Match(grn);
            if (!match.Success || !match.Groups["targetUserId"].Success)
            {
                return null;
            }
            return match.Groups["targetUserId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SubscribeUser FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscribeUser()
                .WithSubscribeUserId(!data.Keys.Contains("subscribeUserId") || data["subscribeUserId"] == null ? null : data["subscribeUserId"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetUserId(!data.Keys.Contains("targetUserId") || data["targetUserId"] == null ? null : data["targetUserId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscribeUserId"] = SubscribeUserId,
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["targetUserId"] = TargetUserId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SubscribeUserId != null) {
                writer.WritePropertyName("subscribeUserId");
                writer.Write(SubscribeUserId.ToString());
            }
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TargetUserId != null) {
                writer.WritePropertyName("targetUserId");
                writer.Write(TargetUserId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SubscribeUser;
            var diff = 0;
            if (SubscribeUserId == null && SubscribeUserId == other.SubscribeUserId)
            {
                // null and null
            }
            else
            {
                diff += SubscribeUserId.CompareTo(other.SubscribeUserId);
            }
            if (CategoryName == null && CategoryName == other.CategoryName)
            {
                // null and null
            }
            else
            {
                diff += CategoryName.CompareTo(other.CategoryName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TargetUserId == null && TargetUserId == other.TargetUserId)
            {
                // null and null
            }
            else
            {
                diff += TargetUserId.CompareTo(other.TargetUserId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (SubscribeUserId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeUser", "ranking.subscribeUser.subscribeUserId.error.tooLong"),
                    });
                }
            }
            {
                if (CategoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeUser", "ranking.subscribeUser.categoryName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeUser", "ranking.subscribeUser.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TargetUserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeUser", "ranking.subscribeUser.targetUserId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SubscribeUser {
                SubscribeUserId = SubscribeUserId,
                CategoryName = CategoryName,
                UserId = UserId,
                TargetUserId = TargetUserId,
            };
        }
    }
}