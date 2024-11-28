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
	public class VoteMultipleRequest : Gs2Request<VoteMultipleRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public Gs2.Gs2SeasonRating.Model.SignedBallot[] SignedBallots { set; get; } = null!;
         public Gs2.Gs2SeasonRating.Model.GameResult[] GameResults { set; get; } = null!;
         public string KeyId { set; get; } = null!;
        public VoteMultipleRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VoteMultipleRequest WithSignedBallots(Gs2.Gs2SeasonRating.Model.SignedBallot[] signedBallots) {
            this.SignedBallots = signedBallots;
            return this;
        }
        public VoteMultipleRequest WithGameResults(Gs2.Gs2SeasonRating.Model.GameResult[] gameResults) {
            this.GameResults = gameResults;
            return this;
        }
        public VoteMultipleRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VoteMultipleRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VoteMultipleRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSignedBallots(!data.Keys.Contains("signedBallots") || data["signedBallots"] == null || !data["signedBallots"].IsArray ? null : data["signedBallots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2SeasonRating.Model.SignedBallot.FromJson(v);
                }).ToArray())
                .WithGameResults(!data.Keys.Contains("gameResults") || data["gameResults"] == null || !data["gameResults"].IsArray ? null : data["gameResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2SeasonRating.Model.GameResult.FromJson(v);
                }).ToArray())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData signedBallotsJsonData = null;
            if (SignedBallots != null && SignedBallots.Length > 0)
            {
                signedBallotsJsonData = new JsonData();
                foreach (var signedBallot in SignedBallots)
                {
                    signedBallotsJsonData.Add(signedBallot.ToJson());
                }
            }
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
                ["signedBallots"] = signedBallotsJsonData,
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
            if (SignedBallots != null) {
                writer.WritePropertyName("signedBallots");
                writer.WriteArrayStart();
                foreach (var signedBallot in SignedBallots)
                {
                    if (signedBallot != null) {
                        signedBallot.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            key += SignedBallots + ":";
            key += GameResults + ":";
            key += KeyId + ":";
            return key;
        }
    }
}