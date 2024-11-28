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
using Gs2.Gs2Quest.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Quest.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateQuestModelMasterRequest : Gs2Request<UpdateQuestModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string QuestGroupName { set; get; } = null!;
         public string QuestName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public Gs2.Gs2Quest.Model.Contents[] Contents { set; get; } = null!;
         public string ChallengePeriodEventId { set; get; } = null!;
         public Gs2.Core.Model.AcquireAction[] FirstCompleteAcquireActions { set; get; } = null!;
         public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; } = null!;
         public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; } = null!;
         public Gs2.Core.Model.AcquireAction[] FailedAcquireActions { set; get; } = null!;
         public string[] PremiseQuestNames { set; get; } = null!;
        public UpdateQuestModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateQuestModelMasterRequest WithQuestGroupName(string questGroupName) {
            this.QuestGroupName = questGroupName;
            return this;
        }
        public UpdateQuestModelMasterRequest WithQuestName(string questName) {
            this.QuestName = questName;
            return this;
        }
        public UpdateQuestModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateQuestModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateQuestModelMasterRequest WithContents(Gs2.Gs2Quest.Model.Contents[] contents) {
            this.Contents = contents;
            return this;
        }
        public UpdateQuestModelMasterRequest WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }
        public UpdateQuestModelMasterRequest WithFirstCompleteAcquireActions(Gs2.Core.Model.AcquireAction[] firstCompleteAcquireActions) {
            this.FirstCompleteAcquireActions = firstCompleteAcquireActions;
            return this;
        }
        public UpdateQuestModelMasterRequest WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
            return this;
        }
        public UpdateQuestModelMasterRequest WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }
        public UpdateQuestModelMasterRequest WithFailedAcquireActions(Gs2.Core.Model.AcquireAction[] failedAcquireActions) {
            this.FailedAcquireActions = failedAcquireActions;
            return this;
        }
        public UpdateQuestModelMasterRequest WithPremiseQuestNames(string[] premiseQuestNames) {
            this.PremiseQuestNames = premiseQuestNames;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateQuestModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateQuestModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithQuestGroupName(!data.Keys.Contains("questGroupName") || data["questGroupName"] == null ? null : data["questGroupName"].ToString())
                .WithQuestName(!data.Keys.Contains("questName") || data["questName"] == null ? null : data["questName"].ToString())
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
                }).ToArray());
        }

        public override JsonData ToJson()
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
                ["namespaceName"] = NamespaceName,
                ["questGroupName"] = QuestGroupName,
                ["questName"] = QuestName,
                ["description"] = Description,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (QuestGroupName != null) {
                writer.WritePropertyName("questGroupName");
                writer.Write(QuestGroupName.ToString());
            }
            if (QuestName != null) {
                writer.WritePropertyName("questName");
                writer.Write(QuestName.ToString());
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
                    writer.Write(premiseQuestName.ToString());
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += QuestGroupName + ":";
            key += QuestName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Contents + ":";
            key += ChallengePeriodEventId + ":";
            key += FirstCompleteAcquireActions + ":";
            key += VerifyActions + ":";
            key += ConsumeActions + ":";
            key += FailedAcquireActions + ":";
            key += PremiseQuestNames + ":";
            return key;
        }
    }
}