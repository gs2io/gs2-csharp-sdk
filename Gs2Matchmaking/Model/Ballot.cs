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
	public class Ballot : IComparable
	{
        public string UserId { set; get; }
        public string RatingName { set; get; }
        public string GatheringName { set; get; }
        public int? NumberOfPlayer { set; get; }

        public Ballot WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Ballot WithRatingName(string ratingName) {
            this.RatingName = ratingName;
            return this;
        }

        public Ballot WithGatheringName(string gatheringName) {
            this.GatheringName = gatheringName;
            return this;
        }

        public Ballot WithNumberOfPlayer(int? numberOfPlayer) {
            this.NumberOfPlayer = numberOfPlayer;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Ballot FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Ballot()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithGatheringName(!data.Keys.Contains("gatheringName") || data["gatheringName"] == null ? null : data["gatheringName"].ToString())
                .WithNumberOfPlayer(!data.Keys.Contains("numberOfPlayer") || data["numberOfPlayer"] == null ? null : (int?)(data["numberOfPlayer"].ToString().Contains(".") ? (int)double.Parse(data["numberOfPlayer"].ToString()) : int.Parse(data["numberOfPlayer"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["ratingName"] = RatingName,
                ["gatheringName"] = GatheringName,
                ["numberOfPlayer"] = NumberOfPlayer,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RatingName != null) {
                writer.WritePropertyName("ratingName");
                writer.Write(RatingName.ToString());
            }
            if (GatheringName != null) {
                writer.WritePropertyName("gatheringName");
                writer.Write(GatheringName.ToString());
            }
            if (NumberOfPlayer != null) {
                writer.WritePropertyName("numberOfPlayer");
                writer.Write((NumberOfPlayer.ToString().Contains(".") ? (int)double.Parse(NumberOfPlayer.ToString()) : int.Parse(NumberOfPlayer.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Ballot;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (RatingName == null && RatingName == other.RatingName)
            {
                // null and null
            }
            else
            {
                diff += RatingName.CompareTo(other.RatingName);
            }
            if (GatheringName == null && GatheringName == other.GatheringName)
            {
                // null and null
            }
            else
            {
                diff += GatheringName.CompareTo(other.GatheringName);
            }
            if (NumberOfPlayer == null && NumberOfPlayer == other.NumberOfPlayer)
            {
                // null and null
            }
            else
            {
                diff += (int)(NumberOfPlayer - other.NumberOfPlayer);
            }
            return diff;
        }
    }
}