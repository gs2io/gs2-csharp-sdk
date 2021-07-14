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
using UnityEngine.Scripting;

namespace Gs2.Gs2Matchmaking.Request
{
	[Preserve]
	[System.Serializable]
	public class PutResultRequest : Gs2Request<PutResultRequest>
	{
        public string NamespaceName { set; get; }
        public string RatingName { set; get; }
        public Gs2.Gs2Matchmaking.Model.GameResult[] GameResults { set; get; }

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

    	[Preserve]
        public static PutResultRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PutResultRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithGameResults(!data.Keys.Contains("gameResults") || data["gameResults"] == null ? new Gs2.Gs2Matchmaking.Model.GameResult[]{} : data["gameResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.GameResult.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["ratingName"] = RatingName,
                ["gameResults"] = new JsonData(GameResults == null ? new JsonData[]{} :
                        GameResults.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
            writer.WriteArrayStart();
            foreach (var gameResult in GameResults)
            {
                if (gameResult != null) {
                    gameResult.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}