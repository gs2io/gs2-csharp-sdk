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
	public class GetRankingRequest : Gs2Request<GetRankingRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string CategoryName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string ScorerUserId { set; get; } = null!;
         public string UniqueId { set; get; } = null!;
         public string AdditionalScopeName { set; get; } = null!;
        public GetRankingRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetRankingRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public GetRankingRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public GetRankingRequest WithScorerUserId(string scorerUserId) {
            this.ScorerUserId = scorerUserId;
            return this;
        }
        public GetRankingRequest WithUniqueId(string uniqueId) {
            this.UniqueId = uniqueId;
            return this;
        }
        public GetRankingRequest WithAdditionalScopeName(string additionalScopeName) {
            this.AdditionalScopeName = additionalScopeName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetRankingRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetRankingRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithScorerUserId(!data.Keys.Contains("scorerUserId") || data["scorerUserId"] == null ? null : data["scorerUserId"].ToString())
                .WithUniqueId(!data.Keys.Contains("uniqueId") || data["uniqueId"] == null ? null : data["uniqueId"].ToString())
                .WithAdditionalScopeName(!data.Keys.Contains("additionalScopeName") || data["additionalScopeName"] == null ? null : data["additionalScopeName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["categoryName"] = CategoryName,
                ["accessToken"] = AccessToken,
                ["scorerUserId"] = ScorerUserId,
                ["uniqueId"] = UniqueId,
                ["additionalScopeName"] = AdditionalScopeName,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (ScorerUserId != null) {
                writer.WritePropertyName("scorerUserId");
                writer.Write(ScorerUserId.ToString());
            }
            if (UniqueId != null) {
                writer.WritePropertyName("uniqueId");
                writer.Write(UniqueId.ToString());
            }
            if (AdditionalScopeName != null) {
                writer.WritePropertyName("additionalScopeName");
                writer.Write(AdditionalScopeName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CategoryName + ":";
            key += AccessToken + ":";
            key += ScorerUserId + ":";
            key += UniqueId + ":";
            key += AdditionalScopeName + ":";
            return key;
        }
    }
}