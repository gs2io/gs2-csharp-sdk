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
using Gs2.Gs2SeasonRating.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SeasonRating.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class VoteRequest : Gs2Request<VoteRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string BallotBody { set; get; } = null!;
         public string BallotSignature { set; get; } = null!;
         public Gs2.Gs2SeasonRating.Model.GameResult[] GameResults { set; get; } = null!;
         public string KeyId { set; get; } = null!;
        public VoteRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VoteRequest WithBallotBody(string ballotBody) {
            this.BallotBody = ballotBody;
            return this;
        }
        public VoteRequest WithBallotSignature(string ballotSignature) {
            this.BallotSignature = ballotSignature;
            return this;
        }
        public VoteRequest WithGameResults(Gs2.Gs2SeasonRating.Model.GameResult[] gameResults) {
            this.GameResults = gameResults;
            return this;
        }
        public VoteRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VoteRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VoteRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithBallotBody(!data.Keys.Contains("ballotBody") || data["ballotBody"] == null ? null : data["ballotBody"].ToString())
                .WithBallotSignature(!data.Keys.Contains("ballotSignature") || data["ballotSignature"] == null ? null : data["ballotSignature"].ToString())
                .WithGameResults(!data.Keys.Contains("gameResults") || data["gameResults"] == null || !data["gameResults"].IsArray ? null : data["gameResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2SeasonRating.Model.GameResult.FromJson(v);
                }).ToArray())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData gameResultsJsonData = null;
            if (GameResults != null && GameResults.Length > 0)
            {
                gameResultsJsonData = new JsonData();
                foreach (var gameResult in GameResults)
                {
                    gameResultsJsonData.Add(gameResult.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["ballotBody"] = BallotBody,
                ["ballotSignature"] = BallotSignature,
                ["gameResults"] = gameResultsJsonData,
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
            if (BallotBody != null) {
                writer.WritePropertyName("ballotBody");
                writer.Write(BallotBody.ToString());
            }
            if (BallotSignature != null) {
                writer.WritePropertyName("ballotSignature");
                writer.Write(BallotSignature.ToString());
            }
            if (GameResults != null) {
                writer.WritePropertyName("gameResults");
                writer.WriteArrayStart();
                foreach (var gameResult in GameResults)
                {
                    if (gameResult != null) {
                        gameResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += BallotBody + ":";
            key += BallotSignature + ":";
            key += GameResults + ":";
            key += KeyId + ":";
            return key;
        }
    }
}