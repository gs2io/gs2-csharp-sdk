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
	public class GetBallotByUserIdRequest : Gs2Request<GetBallotByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string RatingName { set; get; }
        public string GatheringName { set; get; }
        public string UserId { set; get; }
        public int? NumberOfPlayer { set; get; }
        public string KeyId { set; get; }

        public GetBallotByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public GetBallotByUserIdRequest WithRatingName(string ratingName) {
            this.RatingName = ratingName;
            return this;
        }

        public GetBallotByUserIdRequest WithGatheringName(string gatheringName) {
            this.GatheringName = gatheringName;
            return this;
        }

        public GetBallotByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public GetBallotByUserIdRequest WithNumberOfPlayer(int? numberOfPlayer) {
            this.NumberOfPlayer = numberOfPlayer;
            return this;
        }

        public GetBallotByUserIdRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetBallotByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetBallotByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithGatheringName(!data.Keys.Contains("gatheringName") || data["gatheringName"] == null ? null : data["gatheringName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithNumberOfPlayer(!data.Keys.Contains("numberOfPlayer") || data["numberOfPlayer"] == null ? null : (int?)int.Parse(data["numberOfPlayer"].ToString()))
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["ratingName"] = RatingName,
                ["gatheringName"] = GatheringName,
                ["userId"] = UserId,
                ["numberOfPlayer"] = NumberOfPlayer,
                ["keyId"] = KeyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RatingName != null) {
                writer.WritePropertyName("ratingName");
                writer.Write(RatingName.ToString());
            }
            if (GatheringName != null) {
                writer.WritePropertyName("gatheringName");
                writer.Write(GatheringName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (NumberOfPlayer != null) {
                writer.WritePropertyName("numberOfPlayer");
                writer.Write(int.Parse(NumberOfPlayer.ToString()));
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}