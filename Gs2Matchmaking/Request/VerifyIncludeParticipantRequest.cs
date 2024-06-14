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
	public class VerifyIncludeParticipantRequest : Gs2Request<VerifyIncludeParticipantRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string SeasonName { set; get; } = null!;
         public long? Season { set; get; } = null!;
         public long? Tier { set; get; } = null!;
         public string SeasonGatheringName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyIncludeParticipantRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyIncludeParticipantRequest WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public VerifyIncludeParticipantRequest WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public VerifyIncludeParticipantRequest WithTier(long? tier) {
            this.Tier = tier;
            return this;
        }
        public VerifyIncludeParticipantRequest WithSeasonGatheringName(string seasonGatheringName) {
            this.SeasonGatheringName = seasonGatheringName;
            return this;
        }
        public VerifyIncludeParticipantRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyIncludeParticipantRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }

        public VerifyIncludeParticipantRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyIncludeParticipantRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyIncludeParticipantRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())))
                .WithTier(!data.Keys.Contains("tier") || data["tier"] == null ? null : (long?)(data["tier"].ToString().Contains(".") ? (long)double.Parse(data["tier"].ToString()) : long.Parse(data["tier"].ToString())))
                .WithSeasonGatheringName(!data.Keys.Contains("seasonGatheringName") || data["seasonGatheringName"] == null ? null : data["seasonGatheringName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["seasonName"] = SeasonName,
                ["season"] = Season,
                ["tier"] = Tier,
                ["seasonGatheringName"] = SeasonGatheringName,
                ["accessToken"] = AccessToken,
                ["verifyType"] = VerifyType,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (SeasonName != null) {
                writer.WritePropertyName("seasonName");
                writer.Write(SeasonName.ToString());
            }
            if (Season != null) {
                writer.WritePropertyName("season");
                writer.Write((Season.ToString().Contains(".") ? (long)double.Parse(Season.ToString()) : long.Parse(Season.ToString())));
            }
            if (Tier != null) {
                writer.WritePropertyName("tier");
                writer.Write((Tier.ToString().Contains(".") ? (long)double.Parse(Tier.ToString()) : long.Parse(Tier.ToString())));
            }
            if (SeasonGatheringName != null) {
                writer.WritePropertyName("seasonGatheringName");
                writer.Write(SeasonGatheringName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += SeasonName + ":";
            key += Season + ":";
            key += Tier + ":";
            key += SeasonGatheringName + ":";
            key += AccessToken + ":";
            key += VerifyType + ":";
            return key;
        }
    }
}