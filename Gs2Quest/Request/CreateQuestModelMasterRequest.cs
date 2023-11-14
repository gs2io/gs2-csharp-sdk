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
	public class CreateQuestModelMasterRequest : Gs2Request<CreateQuestModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string QuestGroupName { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Quest.Model.Contents[] Contents { set; get; }
        public string ChallengePeriodEventId { set; get; }
        public Gs2.Core.Model.AcquireAction[] FirstCompleteAcquireActions { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] FailedAcquireActions { set; get; }
        public string[] PremiseQuestNames { set; get; }

        public CreateQuestModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public CreateQuestModelMasterRequest WithQuestGroupName(string questGroupName) {
            this.QuestGroupName = questGroupName;
            return this;
        }

        public CreateQuestModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public CreateQuestModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public CreateQuestModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public CreateQuestModelMasterRequest WithContents(Gs2.Gs2Quest.Model.Contents[] contents) {
            this.Contents = contents;
            return this;
        }

        public CreateQuestModelMasterRequest WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }

        public CreateQuestModelMasterRequest WithFirstCompleteAcquireActions(Gs2.Core.Model.AcquireAction[] firstCompleteAcquireActions) {
            this.FirstCompleteAcquireActions = firstCompleteAcquireActions;
            return this;
        }

        public CreateQuestModelMasterRequest WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }

        public CreateQuestModelMasterRequest WithFailedAcquireActions(Gs2.Core.Model.AcquireAction[] failedAcquireActions) {
            this.FailedAcquireActions = failedAcquireActions;
            return this;
        }

        public CreateQuestModelMasterRequest WithPremiseQuestNames(string[] premiseQuestNames) {
            this.PremiseQuestNames = premiseQuestNames;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateQuestModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateQuestModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithQuestGroupName(!data.Keys.Contains("questGroupName") || data["questGroupName"] == null ? null : data["questGroupName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithContents(!data.Keys.Contains("contents") || data["contents"] == null || !data["contents"].IsArray ? new Gs2.Gs2Quest.Model.Contents[]{} : data["contents"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Contents.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithFirstCompleteAcquireActions(!data.Keys.Contains("firstCompleteAcquireActions") || data["firstCompleteAcquireActions"] == null || !data["firstCompleteAcquireActions"].IsArray ? new Gs2.Core.Model.AcquireAction[]{} : data["firstCompleteAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? new Gs2.Core.Model.ConsumeAction[]{} : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithFailedAcquireActions(!data.Keys.Contains("failedAcquireActions") || data["failedAcquireActions"] == null || !data["failedAcquireActions"].IsArray ? new Gs2.Core.Model.AcquireAction[]{} : data["failedAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithPremiseQuestNames(!data.Keys.Contains("premiseQuestNames") || data["premiseQuestNames"] == null || !data["premiseQuestNames"].IsArray ? new string[]{} : data["premiseQuestNames"].Cast<JsonData>().Select(v => {
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
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["contents"] = contentsJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["firstCompleteAcquireActions"] = firstCompleteAcquireActionsJsonData,
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
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Contents + ":";
            key += ChallengePeriodEventId + ":";
            key += FirstCompleteAcquireActions + ":";
            key += ConsumeActions + ":";
            key += FailedAcquireActions + ":";
            key += PremiseQuestNames + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply CreateQuestModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CreateQuestModelMasterRequest)x;
            return this;
        }
    }
}