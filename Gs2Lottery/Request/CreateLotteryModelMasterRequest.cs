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
using Gs2.Gs2Lottery.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Lottery.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateLotteryModelMasterRequest : Gs2Request<CreateLotteryModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string Mode { set; get; } = null!;
         public string Method { set; get; } = null!;
         public string PrizeTableName { set; get; } = null!;
         public string ChoicePrizeTableScriptId { set; get; } = null!;
        public CreateLotteryModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateLotteryModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateLotteryModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateLotteryModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateLotteryModelMasterRequest WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public CreateLotteryModelMasterRequest WithMethod(string method) {
            this.Method = method;
            return this;
        }
        public CreateLotteryModelMasterRequest WithPrizeTableName(string prizeTableName) {
            this.PrizeTableName = prizeTableName;
            return this;
        }
        public CreateLotteryModelMasterRequest WithChoicePrizeTableScriptId(string choicePrizeTableScriptId) {
            this.ChoicePrizeTableScriptId = choicePrizeTableScriptId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateLotteryModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateLotteryModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithPrizeTableName(!data.Keys.Contains("prizeTableName") || data["prizeTableName"] == null ? null : data["prizeTableName"].ToString())
                .WithChoicePrizeTableScriptId(!data.Keys.Contains("choicePrizeTableScriptId") || data["choicePrizeTableScriptId"] == null ? null : data["choicePrizeTableScriptId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["mode"] = Mode,
                ["method"] = Method,
                ["prizeTableName"] = PrizeTableName,
                ["choicePrizeTableScriptId"] = ChoicePrizeTableScriptId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
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
            if (Mode != null) {
                writer.WritePropertyName("mode");
                writer.Write(Mode.ToString());
            }
            if (Method != null) {
                writer.WritePropertyName("method");
                writer.Write(Method.ToString());
            }
            if (PrizeTableName != null) {
                writer.WritePropertyName("prizeTableName");
                writer.Write(PrizeTableName.ToString());
            }
            if (ChoicePrizeTableScriptId != null) {
                writer.WritePropertyName("choicePrizeTableScriptId");
                writer.Write(ChoicePrizeTableScriptId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Mode + ":";
            key += Method + ":";
            key += PrizeTableName + ":";
            key += ChoicePrizeTableScriptId + ":";
            return key;
        }
    }
}