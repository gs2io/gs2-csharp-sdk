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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SeasonRating.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class WrittenBallot : IComparable
	{
        public Gs2.Gs2SeasonRating.Model.Ballot Ballot { set; get; } = null!;
        public Gs2.Gs2SeasonRating.Model.GameResult[] GameResults { set; get; } = null!;
        public WrittenBallot WithBallot(Gs2.Gs2SeasonRating.Model.Ballot ballot) {
            this.Ballot = ballot;
            return this;
        }
        public WrittenBallot WithGameResults(Gs2.Gs2SeasonRating.Model.GameResult[] gameResults) {
            this.GameResults = gameResults;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WrittenBallot FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WrittenBallot()
                .WithBallot(!data.Keys.Contains("ballot") || data["ballot"] == null ? null : Gs2.Gs2SeasonRating.Model.Ballot.FromJson(data["ballot"]))
                .WithGameResults(!data.Keys.Contains("gameResults") || data["gameResults"] == null || !data["gameResults"].IsArray ? null : data["gameResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2SeasonRating.Model.GameResult.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
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
                ["ballot"] = Ballot?.ToJson(),
                ["gameResults"] = gameResultsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Ballot != null) {
                writer.WritePropertyName("ballot");
                Ballot.WriteJson(writer);
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

        public int CompareTo(object obj)
        {
            var other = obj as WrittenBallot;
            var diff = 0;
            if (Ballot == null && Ballot == other.Ballot)
            {
                // null and null
            }
            else
            {
                diff += Ballot.CompareTo(other.Ballot);
            }
            if (GameResults == null && GameResults == other.GameResults)
            {
                // null and null
            }
            else
            {
                diff += GameResults.Length - other.GameResults.Length;
                for (var i = 0; i < GameResults.Length; i++)
                {
                    diff += GameResults[i].CompareTo(other.GameResults[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
            }
            {
                if (GameResults.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("writtenBallot", "seasonRating.writtenBallot.gameResults.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new WrittenBallot {
                Ballot = Ballot.Clone() as Gs2.Gs2SeasonRating.Model.Ballot,
                GameResults = GameResults.Clone() as Gs2.Gs2SeasonRating.Model.GameResult[],
            };
        }
    }
}