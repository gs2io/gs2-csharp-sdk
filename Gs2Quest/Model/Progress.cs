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

namespace Gs2.Gs2Quest.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Progress : IComparable
	{
        public string ProgressId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public string QuestModelId { set; get; } = null!;
        public long? RandomSeed { set; get; } = null!;
        public Gs2.Gs2Quest.Model.Reward[] Rewards { set; get; } = null!;
        public Gs2.Gs2Quest.Model.Reward[] FailedRewards { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Progress WithProgressId(string progressId) {
            this.ProgressId = progressId;
            return this;
        }
        public Progress WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Progress WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public Progress WithQuestModelId(string questModelId) {
            this.QuestModelId = questModelId;
            return this;
        }
        public Progress WithRandomSeed(long? randomSeed) {
            this.RandomSeed = randomSeed;
            return this;
        }
        public Progress WithRewards(Gs2.Gs2Quest.Model.Reward[] rewards) {
            this.Rewards = rewards;
            return this;
        }
        public Progress WithFailedRewards(Gs2.Gs2Quest.Model.Reward[] failedRewards) {
            this.FailedRewards = failedRewards;
            return this;
        }
        public Progress WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public Progress WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Progress WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Progress WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):progress",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):progress",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):progress",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):user:(?<userId>.+):progress",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Progress FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Progress()
                .WithProgressId(!data.Keys.Contains("progressId") || data["progressId"] == null ? null : data["progressId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithQuestModelId(!data.Keys.Contains("questModelId") || data["questModelId"] == null ? null : data["questModelId"].ToString())
                .WithRandomSeed(!data.Keys.Contains("randomSeed") || data["randomSeed"] == null ? null : (long?)(data["randomSeed"].ToString().Contains(".") ? (long)double.Parse(data["randomSeed"].ToString()) : long.Parse(data["randomSeed"].ToString())))
                .WithRewards(!data.Keys.Contains("rewards") || data["rewards"] == null || !data["rewards"].IsArray ? null : data["rewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Reward.FromJson(v);
                }).ToArray())
                .WithFailedRewards(!data.Keys.Contains("failedRewards") || data["failedRewards"] == null || !data["failedRewards"].IsArray ? null : data["failedRewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Reward.FromJson(v);
                }).ToArray())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData rewardsJsonData = null;
            if (Rewards != null && Rewards.Length > 0)
            {
                rewardsJsonData = new JsonData();
                foreach (var reward in Rewards)
                {
                    rewardsJsonData.Add(reward.ToJson());
                }
            }
            JsonData failedRewardsJsonData = null;
            if (FailedRewards != null && FailedRewards.Length > 0)
            {
                failedRewardsJsonData = new JsonData();
                foreach (var failedReward in FailedRewards)
                {
                    failedRewardsJsonData.Add(failedReward.ToJson());
                }
            }
            return new JsonData {
                ["progressId"] = ProgressId,
                ["userId"] = UserId,
                ["transactionId"] = TransactionId,
                ["questModelId"] = QuestModelId,
                ["randomSeed"] = RandomSeed,
                ["rewards"] = rewardsJsonData,
                ["failedRewards"] = failedRewardsJsonData,
                ["metadata"] = Metadata,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ProgressId != null) {
                writer.WritePropertyName("progressId");
                writer.Write(ProgressId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (QuestModelId != null) {
                writer.WritePropertyName("questModelId");
                writer.Write(QuestModelId.ToString());
            }
            if (RandomSeed != null) {
                writer.WritePropertyName("randomSeed");
                writer.Write((RandomSeed.ToString().Contains(".") ? (long)double.Parse(RandomSeed.ToString()) : long.Parse(RandomSeed.ToString())));
            }
            if (Rewards != null) {
                writer.WritePropertyName("rewards");
                writer.WriteArrayStart();
                foreach (var reward in Rewards)
                {
                    if (reward != null) {
                        reward.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (FailedRewards != null) {
                writer.WritePropertyName("failedRewards");
                writer.WriteArrayStart();
                foreach (var failedReward in FailedRewards)
                {
                    if (failedReward != null) {
                        failedReward.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
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
            var other = obj as Progress;
            var diff = 0;
            if (ProgressId == null && ProgressId == other.ProgressId)
            {
                // null and null
            }
            else
            {
                diff += ProgressId.CompareTo(other.ProgressId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (QuestModelId == null && QuestModelId == other.QuestModelId)
            {
                // null and null
            }
            else
            {
                diff += QuestModelId.CompareTo(other.QuestModelId);
            }
            if (RandomSeed == null && RandomSeed == other.RandomSeed)
            {
                // null and null
            }
            else
            {
                diff += (int)(RandomSeed - other.RandomSeed);
            }
            if (Rewards == null && Rewards == other.Rewards)
            {
                // null and null
            }
            else
            {
                diff += Rewards.Length - other.Rewards.Length;
                for (var i = 0; i < Rewards.Length; i++)
                {
                    diff += Rewards[i].CompareTo(other.Rewards[i]);
                }
            }
            if (FailedRewards == null && FailedRewards == other.FailedRewards)
            {
                // null and null
            }
            else
            {
                diff += FailedRewards.Length - other.FailedRewards.Length;
                for (var i = 0; i < FailedRewards.Length; i++)
                {
                    diff += FailedRewards[i].CompareTo(other.FailedRewards[i]);
                }
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
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
                if (ProgressId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.progressId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (QuestModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.questModelId.error.tooLong"),
                    });
                }
            }
            {
                if (RandomSeed < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.randomSeed.error.invalid"),
                    });
                }
                if (RandomSeed > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.randomSeed.error.invalid"),
                    });
                }
            }
            {
                if (Rewards.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.rewards.error.tooMany"),
                    });
                }
            }
            {
                if (FailedRewards.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.failedRewards.error.tooMany"),
                    });
                }
            }
            {
                if (Metadata.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("progress", "quest.progress.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Progress {
                ProgressId = ProgressId,
                UserId = UserId,
                TransactionId = TransactionId,
                QuestModelId = QuestModelId,
                RandomSeed = RandomSeed,
                Rewards = Rewards.Clone() as Gs2.Gs2Quest.Model.Reward[],
                FailedRewards = FailedRewards.Clone() as Gs2.Gs2Quest.Model.Reward[],
                Metadata = Metadata,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}