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
	public partial class QuestModelMaster : IComparable
	{
        public string QuestModelId { set; get; }
        public string QuestGroupName { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Quest.Model.Contents[] Contents { set; get; }
        public string ChallengePeriodEventId { set; get; }
        public Gs2.Core.Model.AcquireAction[] FirstCompleteAcquireActions { set; get; }
        public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] FailedAcquireActions { set; get; }
        public string[] PremiseQuestNames { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public QuestModelMaster WithQuestModelId(string questModelId) {
            this.QuestModelId = questModelId;
            return this;
        }
        public QuestModelMaster WithQuestGroupName(string questGroupName) {
            this.QuestGroupName = questGroupName;
            return this;
        }
        public QuestModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public QuestModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public QuestModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public QuestModelMaster WithContents(Gs2.Gs2Quest.Model.Contents[] contents) {
            this.Contents = contents;
            return this;
        }
        public QuestModelMaster WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }
        public QuestModelMaster WithFirstCompleteAcquireActions(Gs2.Core.Model.AcquireAction[] firstCompleteAcquireActions) {
            this.FirstCompleteAcquireActions = firstCompleteAcquireActions;
            return this;
        }
        public QuestModelMaster WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
            return this;
        }
        public QuestModelMaster WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }
        public QuestModelMaster WithFailedAcquireActions(Gs2.Core.Model.AcquireAction[] failedAcquireActions) {
            this.FailedAcquireActions = failedAcquireActions;
            return this;
        }
        public QuestModelMaster WithPremiseQuestNames(string[] premiseQuestNames) {
            this.PremiseQuestNames = premiseQuestNames;
            return this;
        }
        public QuestModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public QuestModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public QuestModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+):quest:(?<questName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+):quest:(?<questName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+):quest:(?<questName>.+)",
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

        private static System.Text.RegularExpressions.Regex _questGroupNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+):quest:(?<questName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetQuestGroupNameFromGrn(
            string grn
        )
        {
            var match = _questGroupNameRegex.Match(grn);
            if (!match.Success || !match.Groups["questGroupName"].Success)
            {
                return null;
            }
            return match.Groups["questGroupName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _questNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):quest:(?<namespaceName>.+):group:(?<questGroupName>.+):quest:(?<questName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetQuestNameFromGrn(
            string grn
        )
        {
            var match = _questNameRegex.Match(grn);
            if (!match.Success || !match.Groups["questName"].Success)
            {
                return null;
            }
            return match.Groups["questName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static QuestModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new QuestModelMaster()
                .WithQuestModelId(!data.Keys.Contains("questModelId") || data["questModelId"] == null ? null : data["questModelId"].ToString())
                .WithQuestGroupName(!data.Keys.Contains("questGroupName") || data["questGroupName"] == null ? null : data["questGroupName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithContents(!data.Keys.Contains("contents") || data["contents"] == null || !data["contents"].IsArray ? null : data["contents"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Contents.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithFirstCompleteAcquireActions(!data.Keys.Contains("firstCompleteAcquireActions") || data["firstCompleteAcquireActions"] == null || !data["firstCompleteAcquireActions"].IsArray ? null : data["firstCompleteAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithVerifyActions(!data.Keys.Contains("verifyActions") || data["verifyActions"] == null || !data["verifyActions"].IsArray ? null : data["verifyActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? null : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithFailedAcquireActions(!data.Keys.Contains("failedAcquireActions") || data["failedAcquireActions"] == null || !data["failedAcquireActions"].IsArray ? null : data["failedAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithPremiseQuestNames(!data.Keys.Contains("premiseQuestNames") || data["premiseQuestNames"] == null || !data["premiseQuestNames"].IsArray ? null : data["premiseQuestNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData contentsJsonData = null;
            if (Contents != null && Contents.Length > 0)
            {
                contentsJsonData = new JsonData();
                foreach (var content in Contents)
                {
                    contentsJsonData.Add(content.ToJson());
                }
            }
            JsonData firstCompleteAcquireActionsJsonData = null;
            if (FirstCompleteAcquireActions != null && FirstCompleteAcquireActions.Length > 0)
            {
                firstCompleteAcquireActionsJsonData = new JsonData();
                foreach (var firstCompleteAcquireAction in FirstCompleteAcquireActions)
                {
                    firstCompleteAcquireActionsJsonData.Add(firstCompleteAcquireAction.ToJson());
                }
            }
            JsonData verifyActionsJsonData = null;
            if (VerifyActions != null && VerifyActions.Length > 0)
            {
                verifyActionsJsonData = new JsonData();
                foreach (var verifyAction in VerifyActions)
                {
                    verifyActionsJsonData.Add(verifyAction.ToJson());
                }
            }
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null && ConsumeActions.Length > 0)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            JsonData failedAcquireActionsJsonData = null;
            if (FailedAcquireActions != null && FailedAcquireActions.Length > 0)
            {
                failedAcquireActionsJsonData = new JsonData();
                foreach (var failedAcquireAction in FailedAcquireActions)
                {
                    failedAcquireActionsJsonData.Add(failedAcquireAction.ToJson());
                }
            }
            JsonData premiseQuestNamesJsonData = null;
            if (PremiseQuestNames != null && PremiseQuestNames.Length > 0)
            {
                premiseQuestNamesJsonData = new JsonData();
                foreach (var premiseQuestName in PremiseQuestNames)
                {
                    premiseQuestNamesJsonData.Add(premiseQuestName);
                }
            }
            return new JsonData {
                ["questModelId"] = QuestModelId,
                ["questGroupName"] = QuestGroupName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["contents"] = contentsJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["firstCompleteAcquireActions"] = firstCompleteAcquireActionsJsonData,
                ["verifyActions"] = verifyActionsJsonData,
                ["consumeActions"] = consumeActionsJsonData,
                ["failedAcquireActions"] = failedAcquireActionsJsonData,
                ["premiseQuestNames"] = premiseQuestNamesJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (QuestModelId != null) {
                writer.WritePropertyName("questModelId");
                writer.Write(QuestModelId.ToString());
            }
            if (QuestGroupName != null) {
                writer.WritePropertyName("questGroupName");
                writer.Write(QuestGroupName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Contents != null) {
                writer.WritePropertyName("contents");
                writer.WriteArrayStart();
                foreach (var content in Contents)
                {
                    if (content != null) {
                        content.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            if (FirstCompleteAcquireActions != null) {
                writer.WritePropertyName("firstCompleteAcquireActions");
                writer.WriteArrayStart();
                foreach (var firstCompleteAcquireAction in FirstCompleteAcquireActions)
                {
                    if (firstCompleteAcquireAction != null) {
                        firstCompleteAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (VerifyActions != null) {
                writer.WritePropertyName("verifyActions");
                writer.WriteArrayStart();
                foreach (var verifyAction in VerifyActions)
                {
                    if (verifyAction != null) {
                        verifyAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ConsumeActions != null) {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach (var consumeAction in ConsumeActions)
                {
                    if (consumeAction != null) {
                        consumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (FailedAcquireActions != null) {
                writer.WritePropertyName("failedAcquireActions");
                writer.WriteArrayStart();
                foreach (var failedAcquireAction in FailedAcquireActions)
                {
                    if (failedAcquireAction != null) {
                        failedAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (PremiseQuestNames != null) {
                writer.WritePropertyName("premiseQuestNames");
                writer.WriteArrayStart();
                foreach (var premiseQuestName in PremiseQuestNames)
                {
                    if (premiseQuestName != null) {
                        writer.Write(premiseQuestName.ToString());
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
            var other = obj as QuestModelMaster;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type QuestModelMaster.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(QuestModelId, other.QuestModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(QuestGroupName, other.QuestGroupName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Contents, other.Contents);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ChallengePeriodEventId, other.ChallengePeriodEventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(FirstCompleteAcquireActions, other.FirstCompleteAcquireActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(VerifyActions, other.VerifyActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(ConsumeActions, other.ConsumeActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(FailedAcquireActions, other.FailedAcquireActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(PremiseQuestNames, other.PremiseQuestNames);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (QuestModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.questModelId.error.tooLong"),
                    });
                }
            }
            {
                if (QuestGroupName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.questGroupName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Contents.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.contents.error.tooFew"),
                    });
                }
                if (Contents.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.contents.error.tooMany"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (FirstCompleteAcquireActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.firstCompleteAcquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (VerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.verifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.consumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (FailedAcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.failedAcquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (PremiseQuestNames.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.premiseQuestNames.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModelMaster", "quest.questModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new QuestModelMaster {
                QuestModelId = QuestModelId,
                QuestGroupName = QuestGroupName,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                Contents = Contents?.Clone() as Gs2.Gs2Quest.Model.Contents[],
                ChallengePeriodEventId = ChallengePeriodEventId,
                FirstCompleteAcquireActions = FirstCompleteAcquireActions?.Clone() as Gs2.Core.Model.AcquireAction[],
                VerifyActions = VerifyActions?.Clone() as Gs2.Core.Model.VerifyAction[],
                ConsumeActions = ConsumeActions?.Clone() as Gs2.Core.Model.ConsumeAction[],
                FailedAcquireActions = FailedAcquireActions?.Clone() as Gs2.Core.Model.AcquireAction[],
                PremiseQuestNames = PremiseQuestNames?.Clone() as string[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}