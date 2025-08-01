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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Chat.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Chat.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateCategoryModelMasterRequest : Gs2Request<UpdateCategoryModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public int? Category { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string RejectAccessTokenPost { set; get; } = null!;
        public UpdateCategoryModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateCategoryModelMasterRequest WithCategory(int? category) {
            this.Category = category;
            return this;
        }
        public UpdateCategoryModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateCategoryModelMasterRequest WithRejectAccessTokenPost(string rejectAccessTokenPost) {
            this.RejectAccessTokenPost = rejectAccessTokenPost;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateCategoryModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateCategoryModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCategory(!data.Keys.Contains("category") || data["category"] == null ? null : (int?)(data["category"].ToString().Contains(".") ? (int)double.Parse(data["category"].ToString()) : int.Parse(data["category"].ToString())))
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithRejectAccessTokenPost(!data.Keys.Contains("rejectAccessTokenPost") || data["rejectAccessTokenPost"] == null ? null : data["rejectAccessTokenPost"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["category"] = Category,
                ["description"] = Description,
                ["rejectAccessTokenPost"] = RejectAccessTokenPost,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Category != null) {
                writer.WritePropertyName("category");
                writer.Write((Category.ToString().Contains(".") ? (int)double.Parse(Category.ToString()) : int.Parse(Category.ToString())));
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (RejectAccessTokenPost != null) {
                writer.WritePropertyName("rejectAccessTokenPost");
                writer.Write(RejectAccessTokenPost.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Category + ":";
            key += Description + ":";
            key += RejectAccessTokenPost + ":";
            return key;
        }
    }
}