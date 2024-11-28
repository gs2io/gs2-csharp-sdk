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
	public class CreatePrizeTableMasterRequest : Gs2Request<CreatePrizeTableMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public Gs2.Gs2Lottery.Model.Prize[] Prizes { set; get; } = null!;
        public CreatePrizeTableMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreatePrizeTableMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreatePrizeTableMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreatePrizeTableMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreatePrizeTableMasterRequest WithPrizes(Gs2.Gs2Lottery.Model.Prize[] prizes) {
            this.Prizes = prizes;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreatePrizeTableMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreatePrizeTableMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPrizes(!data.Keys.Contains("prizes") || data["prizes"] == null || !data["prizes"].IsArray ? null : data["prizes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Lottery.Model.Prize.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData prizesJsonData = null;
            if (Prizes != null && Prizes.Length > 0)
            {
                prizesJsonData = new JsonData();
                foreach (var prize in Prizes)
                {
                    prizesJsonData.Add(prize.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["prizes"] = prizesJsonData,
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
            if (Prizes != null) {
                writer.WritePropertyName("prizes");
                writer.WriteArrayStart();
                foreach (var prize in Prizes)
                {
                    if (prize != null) {
                        prize.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Prizes + ":";
            return key;
        }
    }
}