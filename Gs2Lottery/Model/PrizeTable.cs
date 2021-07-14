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
using UnityEngine.Scripting;

namespace Gs2.Gs2Lottery.Model
{

	[Preserve]
	public class PrizeTable : IComparable
	{
        public string PrizeTableId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Lottery.Model.Prize[] Prizes { set; get; }

        public PrizeTable WithPrizeTableId(string prizeTableId) {
            this.PrizeTableId = prizeTableId;
            return this;
        }

        public PrizeTable WithName(string name) {
            this.Name = name;
            return this;
        }

        public PrizeTable WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public PrizeTable WithPrizes(Gs2.Gs2Lottery.Model.Prize[] prizes) {
            this.Prizes = prizes;
            return this;
        }

    	[Preserve]
        public static PrizeTable FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrizeTable()
                .WithPrizeTableId(!data.Keys.Contains("prizeTableId") || data["prizeTableId"] == null ? null : data["prizeTableId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPrizes(!data.Keys.Contains("prizes") || data["prizes"] == null ? new Gs2.Gs2Lottery.Model.Prize[]{} : data["prizes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Lottery.Model.Prize.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["prizeTableId"] = PrizeTableId,
                ["name"] = Name,
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
            if (PrizeTableId != null) {
                writer.WritePropertyName("prizeTableId");
                writer.Write(PrizeTableId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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

        public int CompareTo(object obj)
        {
            var other = obj as PrizeTable;
            var diff = 0;
            if (PrizeTableId == null && PrizeTableId == other.PrizeTableId)
            {
                // null and null
            }
            else
            {
                diff += PrizeTableId.CompareTo(other.PrizeTableId);
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
            if (Prizes == null && Prizes == other.Prizes)
            {
                // null and null
            }
            else
            {
                diff += Prizes.Length - other.Prizes.Length;
                for (var i = 0; i < Prizes.Length; i++)
                {
                    diff += Prizes[i].CompareTo(other.Prizes[i]);
                }
            }
            return diff;
        }
    }
}