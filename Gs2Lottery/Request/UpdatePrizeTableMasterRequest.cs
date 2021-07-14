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
using UnityEngine.Scripting;

namespace Gs2.Gs2Lottery.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdatePrizeTableMasterRequest : Gs2Request<UpdatePrizeTableMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string PrizeTableName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Lottery.Model.Prize[] Prizes { set; get; }

        public UpdatePrizeTableMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdatePrizeTableMasterRequest WithPrizeTableName(string prizeTableName) {
            this.PrizeTableName = prizeTableName;
            return this;
        }

        public UpdatePrizeTableMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdatePrizeTableMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public UpdatePrizeTableMasterRequest WithPrizes(Gs2.Gs2Lottery.Model.Prize[] prizes) {
            this.Prizes = prizes;
            return this;
        }

    	[Preserve]
        public static UpdatePrizeTableMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdatePrizeTableMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithPrizeTableName(!data.Keys.Contains("prizeTableName") || data["prizeTableName"] == null ? null : data["prizeTableName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPrizes(!data.Keys.Contains("prizes") || data["prizes"] == null ? new Gs2.Gs2Lottery.Model.Prize[]{} : data["prizes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Lottery.Model.Prize.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["prizeTableName"] = PrizeTableName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["prizes"] = new JsonData(Prizes == null ? new JsonData[]{} :
                        Prizes.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (PrizeTableName != null) {
                writer.WritePropertyName("prizeTableName");
                writer.Write(PrizeTableName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            writer.WriteArrayStart();
            foreach (var prize in Prizes)
            {
                if (prize != null) {
                    prize.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}