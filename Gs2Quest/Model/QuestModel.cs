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
	public class QuestModel : IComparable
	{
        public string QuestModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Quest.Model.Contents[] Contents { set; get; } = null!;
        public string ChallengePeriodEventId { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] FirstCompleteAcquireActions { set; get; } = null!;
        public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; } = null!;
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] FailedAcquireActions { set; get; } = null!;
        public string[] PremiseQuestNames { set; get; } = null!;
        public QuestModel WithQuestModelId(string questModelId) {
            this.QuestModelId = questModelId;
            return this;
        }
        public QuestModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public QuestModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public QuestModel WithContents(Gs2.Gs2Quest.Model.Contents[] contents) {
            this.Contents = contents;
            return this;
        }
        public QuestModel WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }
        public QuestModel WithFirstCompleteAcquireActions(Gs2.Core.Model.AcquireAction[] firstCompleteAcquireActions) {
            this.FirstCompleteAcquireActions = firstCompleteAcquireActions;
            return this;
        }
        public QuestModel WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
            return this;
        }
        public QuestModel WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }
        public QuestModel WithFailedAcquireActions(Gs2.Core.Model.AcquireAction[] failedAcquireActions) {
            this.FailedAcquireActions = failedAcquireActions;
            return this;
        }
        public QuestModel WithPremiseQuestNames(string[] premiseQuestNames) {
            this.PremiseQuestNames = premiseQuestNames;
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
        public static QuestModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QuestModel()
                .WithQuestModelId(!data.Keys.Contains("questModelId") || data["questModelId"] == null ? null : data["questModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
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
                }).ToArray());
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
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["contents"] = contentsJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["firstCompleteAcquireActions"] = firstCompleteAcquireActionsJsonData,
                ["verifyActions"] = verifyActionsJsonData,
                ["consumeActions"] = consumeActionsJsonData,
                ["failedAcquireActions"] = failedAcquireActionsJsonData,
                ["premiseQuestNames"] = premiseQuestNamesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (QuestModelId != null) {
                writer.WritePropertyName("questModelId");
                writer.Write(QuestModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as QuestModel;
            var diff = 0;
            if (QuestModelId == null && QuestModelId == other.QuestModelId)
            {
                // null and null
            }
            else
            {
                diff += QuestModelId.CompareTo(other.QuestModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Contents == null && Contents == other.Contents)
            {
                // null and null
            }
            else
            {
                diff += Contents.Length - other.Contents.Length;
                for (var i = 0; i < Contents.Length; i++)
                {
                    diff += Contents[i].CompareTo(other.Contents[i]);
                }
            }
            if (ChallengePeriodEventId == null && ChallengePeriodEventId == other.ChallengePeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += ChallengePeriodEventId.CompareTo(other.ChallengePeriodEventId);
            }
            if (FirstCompleteAcquireActions == null && FirstCompleteAcquireActions == other.FirstCompleteAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += FirstCompleteAcquireActions.Length - other.FirstCompleteAcquireActions.Length;
                for (var i = 0; i < FirstCompleteAcquireActions.Length; i++)
                {
                    diff += FirstCompleteAcquireActions[i].CompareTo(other.FirstCompleteAcquireActions[i]);
                }
            }
            if (VerifyActions == null && VerifyActions == other.VerifyActions)
            {
                // null and null
            }
            else
            {
                diff += VerifyActions.Length - other.VerifyActions.Length;
                for (var i = 0; i < VerifyActions.Length; i++)
                {
                    diff += VerifyActions[i].CompareTo(other.VerifyActions[i]);
                }
            }
            if (ConsumeActions == null && ConsumeActions == other.ConsumeActions)
            {
                // null and null
            }
            else
            {
                diff += ConsumeActions.Length - other.ConsumeActions.Length;
                for (var i = 0; i < ConsumeActions.Length; i++)
                {
                    diff += ConsumeActions[i].CompareTo(other.ConsumeActions[i]);
                }
            }
            if (FailedAcquireActions == null && FailedAcquireActions == other.FailedAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += FailedAcquireActions.Length - other.FailedAcquireActions.Length;
                for (var i = 0; i < FailedAcquireActions.Length; i++)
                {
                    diff += FailedAcquireActions[i].CompareTo(other.FailedAcquireActions[i]);
                }
            }
            if (PremiseQuestNames == null && PremiseQuestNames == other.PremiseQuestNames)
            {
                // null and null
            }
            else
            {
                diff += PremiseQuestNames.Length - other.PremiseQuestNames.Length;
                for (var i = 0; i < PremiseQuestNames.Length; i++)
                {
                    diff += PremiseQuestNames[i].CompareTo(other.PremiseQuestNames[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (QuestModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.questModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Contents.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.contents.error.tooFew"),
                    });
                }
                if (Contents.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.contents.error.tooMany"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (FirstCompleteAcquireActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.firstCompleteAcquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (VerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.verifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.consumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (FailedAcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.failedAcquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (PremiseQuestNames.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("questModel", "quest.questModel.premiseQuestNames.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new QuestModel {
                QuestModelId = QuestModelId,
                Name = Name,
                Metadata = Metadata,
                Contents = Contents.Clone() as Gs2.Gs2Quest.Model.Contents[],
                ChallengePeriodEventId = ChallengePeriodEventId,
                FirstCompleteAcquireActions = FirstCompleteAcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
                VerifyActions = VerifyActions.Clone() as Gs2.Core.Model.VerifyAction[],
                ConsumeActions = ConsumeActions.Clone() as Gs2.Core.Model.ConsumeAction[],
                FailedAcquireActions = FailedAcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
                PremiseQuestNames = PremiseQuestNames.Clone() as string[],
            };
        }
    }
}