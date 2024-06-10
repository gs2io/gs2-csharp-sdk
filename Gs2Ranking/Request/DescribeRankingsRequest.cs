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
	public class DescribeRankingsRequest : Gs2Request<DescribeRankingsRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string CategoryName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string AdditionalScopeName { set; get; } = null!;
         public long? StartIndex { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
        public DescribeRankingsRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DescribeRankingsRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public DescribeRankingsRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public DescribeRankingsRequest WithAdditionalScopeName(string additionalScopeName) {
            this.AdditionalScopeName = additionalScopeName;
            return this;
        }
        public DescribeRankingsRequest WithStartIndex(long? startIndex) {
            this.StartIndex = startIndex;
            return this;
        }
        public DescribeRankingsRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public DescribeRankingsRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeRankingsRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeRankingsRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithAdditionalScopeName(!data.Keys.Contains("additionalScopeName") || data["additionalScopeName"] == null ? null : data["additionalScopeName"].ToString())
                .WithStartIndex(!data.Keys.Contains("startIndex") || data["startIndex"] == null ? null : (long?)(data["startIndex"].ToString().Contains(".") ? (long)double.Parse(data["startIndex"].ToString()) : long.Parse(data["startIndex"].ToString())))
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["categoryName"] = CategoryName,
                ["accessToken"] = AccessToken,
                ["additionalScopeName"] = AdditionalScopeName,
                ["startIndex"] = StartIndex,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
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
            if (AdditionalScopeName != null) {
                writer.WritePropertyName("additionalScopeName");
                writer.Write(AdditionalScopeName.ToString());
            }
            if (StartIndex != null) {
                writer.WritePropertyName("startIndex");
                writer.Write((StartIndex.ToString().Contains(".") ? (long)double.Parse(StartIndex.ToString()) : long.Parse(StartIndex.ToString())));
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CategoryName + ":";
            key += AccessToken + ":";
            key += AdditionalScopeName + ":";
            key += StartIndex + ":";
            key += PageToken + ":";
            key += Limit + ":";
            return key;
        }
    }
}