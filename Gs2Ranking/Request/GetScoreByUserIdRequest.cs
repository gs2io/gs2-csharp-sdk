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
using Gs2.Gs2Ranking.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetScoreByUserIdRequest : Gs2Request<GetScoreByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string CategoryName { set; get; }
        public string UserId { set; get; }
        public string ScorerUserId { set; get; }
        public string UniqueId { set; get; }

        public GetScoreByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public GetScoreByUserIdRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }

        public GetScoreByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public GetScoreByUserIdRequest WithScorerUserId(string scorerUserId) {
            this.ScorerUserId = scorerUserId;
            return this;
        }

        public GetScoreByUserIdRequest WithUniqueId(string uniqueId) {
            this.UniqueId = uniqueId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetScoreByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetScoreByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithScorerUserId(!data.Keys.Contains("scorerUserId") || data["scorerUserId"] == null ? null : data["scorerUserId"].ToString())
                .WithUniqueId(!data.Keys.Contains("uniqueId") || data["uniqueId"] == null ? null : data["uniqueId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["scorerUserId"] = ScorerUserId,
                ["uniqueId"] = UniqueId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ScorerUserId != null) {
                writer.WritePropertyName("scorerUserId");
                writer.Write(ScorerUserId.ToString());
            }
            if (UniqueId != null) {
                writer.WritePropertyName("uniqueId");
                writer.Write(UniqueId.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}