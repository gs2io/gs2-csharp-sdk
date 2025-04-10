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

#pragma warning disable CS0618 // Obsolete with a message

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
	public class CommitVoteRequest : Gs2Request<CommitVoteRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RatingName { set; get; } = null!;
         public string GatheringName { set; get; } = null!;
        public CommitVoteRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CommitVoteRequest WithRatingName(string ratingName) {
            this.RatingName = ratingName;
            return this;
        }
        public CommitVoteRequest WithGatheringName(string gatheringName) {
            this.GatheringName = gatheringName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CommitVoteRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CommitVoteRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithGatheringName(!data.Keys.Contains("gatheringName") || data["gatheringName"] == null ? null : data["gatheringName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["ratingName"] = RatingName,
                ["gatheringName"] = GatheringName,
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RatingName + ":";
            key += GatheringName + ":";
            return key;
        }
    }
}