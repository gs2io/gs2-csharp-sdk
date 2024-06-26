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
using Gs2.Gs2Matchmaking.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Matchmaking.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetJoinedSeasonGatheringRequest : Gs2Request<GetJoinedSeasonGatheringRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string SeasonName { set; get; } = null!;
         public long? Season { set; get; } = null!;
        public GetJoinedSeasonGatheringRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetJoinedSeasonGatheringRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public GetJoinedSeasonGatheringRequest WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public GetJoinedSeasonGatheringRequest WithSeason(long? season) {
            this.Season = season;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetJoinedSeasonGatheringRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetJoinedSeasonGatheringRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["seasonName"] = SeasonName,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (SeasonName != null) {
                writer.WritePropertyName("seasonName");
                writer.Write(SeasonName.ToString());
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
            key += AccessToken + ":";
            key += SeasonName + ":";
            key += Season + ":";
            return key;
        }
    }
}