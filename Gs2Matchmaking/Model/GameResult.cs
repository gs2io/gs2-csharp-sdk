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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class GameResult : IComparable
	{
        public int? Rank { set; get; }
        public string UserId { set; get; }
        public GameResult WithRank(int? rank) {
            this.Rank = rank;
            return this;
        }
        public GameResult WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GameResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GameResult()
                .WithRank(!data.Keys.Contains("rank") || data["rank"] == null ? null : (int?)int.Parse(data["rank"].ToString()))
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["rank"] = Rank,
                ["userId"] = UserId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Rank != null) {
                writer.WritePropertyName("rank");
                writer.Write(int.Parse(Rank.ToString()));
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GameResult;
            var diff = 0;
            if (Rank == null && Rank == other.Rank)
            {
                // null and null
            }
            else
            {
                diff += (int)(Rank - other.Rank);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            return diff;
        }
    }
}