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
	public class Vote : IComparable
	{
        public string VoteId { set; get; } = null!;
        public string RatingName { set; get; } = null!;
        public string GatheringName { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.WrittenBallot[] WrittenBallots { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public Vote WithVoteId(string voteId) {
            this.VoteId = voteId;
            return this;
        }
        public Vote WithRatingName(string ratingName) {
            this.RatingName = ratingName;
            return this;
        }
        public Vote WithGatheringName(string gatheringName) {
            this.GatheringName = gatheringName;
            return this;
        }
        public Vote WithWrittenBallots(Gs2.Gs2Matchmaking.Model.WrittenBallot[] writtenBallots) {
            this.WrittenBallots = writtenBallots;
            return this;
        }
        public Vote WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Vote WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):vote:(?<ratingName>.+):(?<gatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):vote:(?<ratingName>.+):(?<gatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):vote:(?<ratingName>.+):(?<gatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ratingNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):vote:(?<ratingName>.+):(?<gatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRatingNameFromGrn(
            string grn
        )
        {
            var match = _ratingNameRegex.Match(grn);
            if (!match.Success || !match.Groups["ratingName"].Success)
            {
                return null;
            }
            return match.Groups["ratingName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _gatheringNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):vote:(?<ratingName>.+):(?<gatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGatheringNameFromGrn(
            string grn
        )
        {
            var match = _gatheringNameRegex.Match(grn);
            if (!match.Success || !match.Groups["gatheringName"].Success)
            {
                return null;
            }
            return match.Groups["gatheringName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Vote FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Vote()
                .WithVoteId(!data.Keys.Contains("voteId") || data["voteId"] == null ? null : data["voteId"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithGatheringName(!data.Keys.Contains("gatheringName") || data["gatheringName"] == null ? null : data["gatheringName"].ToString())
                .WithWrittenBallots(!data.Keys.Contains("writtenBallots") || data["writtenBallots"] == null || !data["writtenBallots"].IsArray ? null : data["writtenBallots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.WrittenBallot.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData writtenBallotsJsonData = null;
            if (WrittenBallots != null && WrittenBallots.Length > 0)
            {
                writtenBallotsJsonData = new JsonData();
                foreach (var writtenBallot in WrittenBallots)
                {
                    writtenBallotsJsonData.Add(writtenBallot.ToJson());
                }
            }
            return new JsonData {
                ["voteId"] = VoteId,
                ["ratingName"] = RatingName,
                ["gatheringName"] = GatheringName,
                ["writtenBallots"] = writtenBallotsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (VoteId != null) {
                writer.WritePropertyName("voteId");
                writer.Write(VoteId.ToString());
            }
            if (RatingName != null) {
                writer.WritePropertyName("ratingName");
                writer.Write(RatingName.ToString());
            }
            if (GatheringName != null) {
                writer.WritePropertyName("gatheringName");
                writer.Write(GatheringName.ToString());
            }
            if (WrittenBallots != null) {
                writer.WritePropertyName("writtenBallots");
                writer.WriteArrayStart();
                foreach (var writtenBallot in WrittenBallots)
                {
                    if (writtenBallot != null) {
                        writtenBallot.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Vote;
            var diff = 0;
            if (VoteId == null && VoteId == other.VoteId)
            {
                // null and null
            }
            else
            {
                diff += VoteId.CompareTo(other.VoteId);
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
            if (WrittenBallots == null && WrittenBallots == other.WrittenBallots)
            {
                // null and null
            }
            else
            {
                diff += WrittenBallots.Length - other.WrittenBallots.Length;
                for (var i = 0; i < WrittenBallots.Length; i++)
                {
                    diff += WrittenBallots[i].CompareTo(other.WrittenBallots[i]);
                }
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (VoteId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.voteId.error.tooLong"),
                    });
                }
            }
            {
                if (RatingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.ratingName.error.tooLong"),
                    });
                }
            }
            {
                if (GatheringName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.gatheringName.error.tooLong"),
                    });
                }
            }
            {
                if (WrittenBallots.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.writtenBallots.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "matchmaking.vote.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Vote {
                VoteId = VoteId,
                RatingName = RatingName,
                GatheringName = GatheringName,
                WrittenBallots = WrittenBallots.Clone() as Gs2.Gs2Matchmaking.Model.WrittenBallot[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}