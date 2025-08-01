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

namespace Gs2.Gs2Chat.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class CategoryModel : IComparable
	{
        public string CategoryModelId { set; get; }
        public int? Category { set; get; }
        public string RejectAccessTokenPost { set; get; }
        public CategoryModel WithCategoryModelId(string categoryModelId) {
            this.CategoryModelId = categoryModelId;
            return this;
        }
        public CategoryModel WithCategory(int? category) {
            this.Category = category;
            return this;
        }
        public CategoryModel WithRejectAccessTokenPost(string rejectAccessTokenPost) {
            this.RejectAccessTokenPost = rejectAccessTokenPost;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):model:(?<category>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):model:(?<category>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):model:(?<category>.+)",
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

        private static System.Text.RegularExpressions.Regex _categoryRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):model:(?<category>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCategoryFromGrn(
            string grn
        )
        {
            var match = _categoryRegex.Match(grn);
            if (!match.Success || !match.Groups["category"].Success)
            {
                return null;
            }
            return match.Groups["category"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CategoryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CategoryModel()
                .WithCategoryModelId(!data.Keys.Contains("categoryModelId") || data["categoryModelId"] == null ? null : data["categoryModelId"].ToString())
                .WithCategory(!data.Keys.Contains("category") || data["category"] == null ? null : (int?)(data["category"].ToString().Contains(".") ? (int)double.Parse(data["category"].ToString()) : int.Parse(data["category"].ToString())))
                .WithRejectAccessTokenPost(!data.Keys.Contains("rejectAccessTokenPost") || data["rejectAccessTokenPost"] == null ? null : data["rejectAccessTokenPost"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["categoryModelId"] = CategoryModelId,
                ["category"] = Category,
                ["rejectAccessTokenPost"] = RejectAccessTokenPost,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CategoryModelId != null) {
                writer.WritePropertyName("categoryModelId");
                writer.Write(CategoryModelId.ToString());
            }
            if (Category != null) {
                writer.WritePropertyName("category");
                writer.Write((Category.ToString().Contains(".") ? (int)double.Parse(Category.ToString()) : int.Parse(Category.ToString())));
            }
            if (RejectAccessTokenPost != null) {
                writer.WritePropertyName("rejectAccessTokenPost");
                writer.Write(RejectAccessTokenPost.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CategoryModel;
            var diff = 0;
            if (CategoryModelId == null && CategoryModelId == other.CategoryModelId)
            {
                // null and null
            }
            else
            {
                diff += CategoryModelId.CompareTo(other.CategoryModelId);
            }
            if (Category == null && Category == other.Category)
            {
                // null and null
            }
            else
            {
                diff += (int)(Category - other.Category);
            }
            if (RejectAccessTokenPost == null && RejectAccessTokenPost == other.RejectAccessTokenPost)
            {
                // null and null
            }
            else
            {
                diff += RejectAccessTokenPost.CompareTo(other.RejectAccessTokenPost);
            }
            return diff;
        }

        public void Validate() {
            {
                if (CategoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "chat.categoryModel.categoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Category < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "chat.categoryModel.category.error.invalid"),
                    });
                }
                if (Category > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("categoryModel", "chat.categoryModel.category.error.invalid"),
                    });
                }
            }
            {
                switch (RejectAccessTokenPost) {
                    case "Enabled":
                    case "Disabled":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("categoryModel", "chat.categoryModel.rejectAccessTokenPost.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new CategoryModel {
                CategoryModelId = CategoryModelId,
                Category = Category,
                RejectAccessTokenPost = RejectAccessTokenPost,
            };
        }
    }
}