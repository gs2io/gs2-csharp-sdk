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
using Gs2.Gs2Ranking.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Ranking.Result
{
	[Preserve]
	[System.Serializable]
	public class DescribeSubscribesByCategoryNameAndUserIdResult : IResult
	{
        public Gs2.Gs2Ranking.Model.SubscribeUser[] Items { set; get; }

        public DescribeSubscribesByCategoryNameAndUserIdResult WithItems(Gs2.Gs2Ranking.Model.SubscribeUser[] items) {
            this.Items = items;
            return this;
        }

    	[Preserve]
        public static DescribeSubscribesByCategoryNameAndUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeSubscribesByCategoryNameAndUserIdResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null ? new Gs2.Gs2Ranking.Model.SubscribeUser[]{} : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Ranking.Model.SubscribeUser.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
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
            writer.WriteArrayStart();
            foreach (var item in Items)
            {
                if (item != null) {
                    item.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}