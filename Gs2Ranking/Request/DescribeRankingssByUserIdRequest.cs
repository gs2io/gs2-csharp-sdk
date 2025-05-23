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
	public class DescribeRankingssByUserIdRequest : Gs2Request<DescribeRankingssByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string CategoryName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string AdditionalScopeName { set; get; } = null!;
         public long? StartIndex { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public DescribeRankingssByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DescribeRankingssByUserIdRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public DescribeRankingssByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DescribeRankingssByUserIdRequest WithAdditionalScopeName(string additionalScopeName) {
            this.AdditionalScopeName = additionalScopeName;
            return this;
        }
        public DescribeRankingssByUserIdRequest WithStartIndex(long? startIndex) {
            this.StartIndex = startIndex;
            return this;
        }
        public DescribeRankingssByUserIdRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public DescribeRankingssByUserIdRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }
        public DescribeRankingssByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeRankingssByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeRankingssByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAdditionalScopeName(!data.Keys.Contains("additionalScopeName") || data["additionalScopeName"] == null ? null : data["additionalScopeName"].ToString())
                .WithStartIndex(!data.Keys.Contains("startIndex") || data["startIndex"] == null ? null : (long?)(data["startIndex"].ToString().Contains(".") ? (long)double.Parse(data["startIndex"].ToString()) : long.Parse(data["startIndex"].ToString())))
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["additionalScopeName"] = AdditionalScopeName,
                ["startIndex"] = StartIndex,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
                ["timeOffsetToken"] = TimeOffsetToken,
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
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CategoryName + ":";
            key += UserId + ":";
            key += AdditionalScopeName + ":";
            key += StartIndex + ":";
            key += PageToken + ":";
            key += Limit + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}