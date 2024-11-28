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
	public class Vote : IComparable
	{
        public string VoteId { set; get; } = null!;
        public string SeasonName { set; get; } = null!;
        public string SessionName { set; get; } = null!;
        public Gs2.Gs2SeasonRating.Model.WrittenBallot[] WrittenBallots { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Vote WithVoteId(string voteId) {
            this.VoteId = voteId;
            return this;
        }
        public Vote WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public Vote WithSessionName(string sessionName) {
            this.SessionName = sessionName;
            return this;
        }
        public Vote WithWrittenBallots(Gs2.Gs2SeasonRating.Model.WrittenBallot[] writtenBallots) {
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
        public Vote WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):seasonRating:(?<namespaceName>.+):vote:(?<seasonName>.+):(?<sessionName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):seasonRating:(?<namespaceName>.+):vote:(?<seasonName>.+):(?<sessionName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):seasonRating:(?<namespaceName>.+):vote:(?<seasonName>.+):(?<sessionName>.+)",
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

        private static System.Text.RegularExpressions.Regex _seasonNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):seasonRating:(?<namespaceName>.+):vote:(?<seasonName>.+):(?<sessionName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSeasonNameFromGrn(
            string grn
        )
        {
            var match = _seasonNameRegex.Match(grn);
            if (!match.Success || !match.Groups["seasonName"].Success)
            {
                return null;
            }
            return match.Groups["seasonName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _sessionNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):seasonRating:(?<namespaceName>.+):vote:(?<seasonName>.+):(?<sessionName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSessionNameFromGrn(
            string grn
        )
        {
            var match = _sessionNameRegex.Match(grn);
            if (!match.Success || !match.Groups["sessionName"].Success)
            {
                return null;
            }
            return match.Groups["sessionName"].Value;
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
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSessionName(!data.Keys.Contains("sessionName") || data["sessionName"] == null ? null : data["sessionName"].ToString())
                .WithWrittenBallots(!data.Keys.Contains("writtenBallots") || data["writtenBallots"] == null || !data["writtenBallots"].IsArray ? null : data["writtenBallots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2SeasonRating.Model.WrittenBallot.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["seasonName"] = SeasonName,
                ["sessionName"] = SessionName,
                ["writtenBallots"] = writtenBallotsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (VoteId != null) {
                writer.WritePropertyName("voteId");
                writer.Write(VoteId.ToString());
            }
            if (SeasonName != null) {
                writer.WritePropertyName("seasonName");
                writer.Write(SeasonName.ToString());
            }
            if (SessionName != null) {
                writer.WritePropertyName("sessionName");
                writer.Write(SessionName.ToString());
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
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (SeasonName == null && SeasonName == other.SeasonName)
            {
                // null and null
            }
            else
            {
                diff += SeasonName.CompareTo(other.SeasonName);
            }
            if (SessionName == null && SessionName == other.SessionName)
            {
                // null and null
            }
            else
            {
                diff += SessionName.CompareTo(other.SessionName);
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (VoteId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.voteId.error.tooLong"),
                    });
                }
            }
            {
                if (SeasonName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.seasonName.error.tooLong"),
                    });
                }
            }
            {
                if (SessionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.sessionName.error.tooLong"),
                    });
                }
            }
            {
                if (WrittenBallots.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.writtenBallots.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("vote", "seasonRating.vote.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Vote {
                VoteId = VoteId,
                SeasonName = SeasonName,
                SessionName = SessionName,
                WrittenBallots = WrittenBallots.Clone() as Gs2.Gs2SeasonRating.Model.WrittenBallot[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}