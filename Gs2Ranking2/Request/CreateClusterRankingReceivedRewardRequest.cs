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
using Gs2.Gs2Ranking2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateClusterRankingReceivedRewardRequest : Gs2Request<CreateClusterRankingReceivedRewardRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RankingName { set; get; } = null!;
         public string ClusterName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public long? Season { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public CreateClusterRankingReceivedRewardRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateClusterRankingReceivedRewardRequest WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public CreateClusterRankingReceivedRewardRequest WithClusterName(string clusterName) {
            this.ClusterName = clusterName;
            return this;
        }
        public CreateClusterRankingReceivedRewardRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public CreateClusterRankingReceivedRewardRequest WithSeason(long? season) {
            this.Season = season;
            return this;
        }

        public CreateClusterRankingReceivedRewardRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateClusterRankingReceivedRewardRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateClusterRankingReceivedRewardRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithClusterName(!data.Keys.Contains("clusterName") || data["clusterName"] == null ? null : data["clusterName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["rankingName"] = RankingName,
                ["clusterName"] = ClusterName,
                ["accessToken"] = AccessToken,
                ["season"] = Season,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RankingName != null) {
                writer.WritePropertyName("rankingName");
                writer.Write(RankingName.ToString());
            }
            if (ClusterName != null) {
                writer.WritePropertyName("clusterName");
                writer.Write(ClusterName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (Season != null) {
                writer.WritePropertyName("season");
                writer.Write((Season.ToString().Contains(".") ? (long)double.Parse(Season.ToString()) : long.Parse(Season.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RankingName + ":";
            key += ClusterName + ":";
            key += AccessToken + ":";
            key += Season + ":";
            return key;
        }
    }
}