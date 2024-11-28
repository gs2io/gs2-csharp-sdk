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
	public class PutResultRequest : Gs2Request<PutResultRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RatingName { set; get; } = null!;
         public Gs2.Gs2Matchmaking.Model.GameResult[] GameResults { set; get; } = null!;
        public PutResultRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PutResultRequest WithRatingName(string ratingName) {
            this.RatingName = ratingName;
            return this;
        }
        public PutResultRequest WithGameResults(Gs2.Gs2Matchmaking.Model.GameResult[] gameResults) {
            this.GameResults = gameResults;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PutResultRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PutResultRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithGameResults(!data.Keys.Contains("gameResults") || data["gameResults"] == null || !data["gameResults"].IsArray ? null : data["gameResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.GameResult.FromJson(v);
                }).ToArray());
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
                ["ratingName"] = RatingName,
                ["gameResults"] = gameResultsJsonData,
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RatingName + ":";
            key += GameResults + ":";
            return key;
        }
    }
}