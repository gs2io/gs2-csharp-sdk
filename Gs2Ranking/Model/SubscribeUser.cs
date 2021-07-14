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

namespace Gs2.Gs2Ranking.Model
{

	[Preserve]
	public class SubscribeUser : IComparable
	{
        public string CategoryName { set; get; }
        public string UserId { set; get; }
        public string TargetUserId { set; get; }

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

    	[Preserve]
        public static SubscribeUser FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscribeUser()
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetUserId(!data.Keys.Contains("targetUserId") || data["targetUserId"] == null ? null : data["targetUserId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["targetUserId"] = TargetUserId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
    }
}