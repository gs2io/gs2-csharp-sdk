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
	public class DescribeNearRankingsRequest : Gs2Request<DescribeNearRankingsRequest>
	{
        public string NamespaceName { set; get; }
        public string CategoryName { set; get; }
        public string AdditionalScopeName { set; get; }
        public long? Score { set; get; }

        public DescribeNearRankingsRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DescribeNearRankingsRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }

        public DescribeNearRankingsRequest WithAdditionalScopeName(string additionalScopeName) {
            this.AdditionalScopeName = additionalScopeName;
            return this;
        }

        public DescribeNearRankingsRequest WithScore(long? score) {
            this.Score = score;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeNearRankingsRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeNearRankingsRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithAdditionalScopeName(!data.Keys.Contains("additionalScopeName") || data["additionalScopeName"] == null ? null : data["additionalScopeName"].ToString())
                .WithScore(!data.Keys.Contains("score") || data["score"] == null ? null : (long?)long.Parse(data["score"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["categoryName"] = CategoryName,
                ["additionalScopeName"] = AdditionalScopeName,
                ["score"] = Score,
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
            if (AdditionalScopeName != null) {
                writer.WritePropertyName("additionalScopeName");
                writer.Write(AdditionalScopeName.ToString());
            }
            if (Score != null) {
                writer.WritePropertyName("score");
                writer.Write(long.Parse(Score.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CategoryName + ":";
            key += AdditionalScopeName + ":";
            key += Score + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply DescribeNearRankingsRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (DescribeNearRankingsRequest)x;
            return this;
        }
    }
}