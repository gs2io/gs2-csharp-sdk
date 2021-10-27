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

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BoxItems : IComparable
	{
        public string BoxId { set; get; }
        public string PrizeTableName { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2Lottery.Model.BoxItem[] Items { set; get; }

        public BoxItems WithBoxId(string boxId) {
            this.BoxId = boxId;
            return this;
        }

        public BoxItems WithPrizeTableName(string prizeTableName) {
            this.PrizeTableName = prizeTableName;
            return this;
        }

        public BoxItems WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public BoxItems WithItems(Gs2.Gs2Lottery.Model.BoxItem[] items) {
            this.Items = items;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BoxItems FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BoxItems()
                .WithBoxId(!data.Keys.Contains("boxId") || data["boxId"] == null ? null : data["boxId"].ToString())
                .WithPrizeTableName(!data.Keys.Contains("prizeTableName") || data["prizeTableName"] == null ? null : data["prizeTableName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItems(!data.Keys.Contains("items") || data["items"] == null ? new Gs2.Gs2Lottery.Model.BoxItem[]{} : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Lottery.Model.BoxItem.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["boxId"] = BoxId,
                ["prizeTableName"] = PrizeTableName,
                ["userId"] = UserId,
                ["items"] = new JsonData(Items == null ? new JsonData[]{} :
                        Items.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BoxId != null) {
                writer.WritePropertyName("boxId");
                writer.Write(BoxId.ToString());
            }
            if (PrizeTableName != null) {
                writer.WritePropertyName("prizeTableName");
                writer.Write(PrizeTableName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Items != null) {
                writer.WritePropertyName("items");
                writer.WriteArrayStart();
                foreach (var item in Items)
                {
                    if (item != null) {
                        item.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BoxItems;
            var diff = 0;
            if (BoxId == null && BoxId == other.BoxId)
            {
                // null and null
            }
            else
            {
                diff += BoxId.CompareTo(other.BoxId);
            }
            if (PrizeTableName == null && PrizeTableName == other.PrizeTableName)
            {
                // null and null
            }
            else
            {
                diff += PrizeTableName.CompareTo(other.PrizeTableName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Items == null && Items == other.Items)
            {
                // null and null
            }
            else
            {
                diff += Items.Length - other.Items.Length;
                for (var i = 0; i < Items.Length; i++)
                {
                    diff += Items[i].CompareTo(other.Items[i]);
                }
            }
            return diff;
        }
    }
}