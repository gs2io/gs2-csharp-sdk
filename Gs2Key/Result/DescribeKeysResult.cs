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
using Gs2.Gs2Key.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Key.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DescribeKeysResult : IResult
	{
        public Gs2.Gs2Key.Model.Key[] Items { set; get; } = null!;
        public string NextPageToken { set; get; } = null!;

        public DescribeKeysResult WithItems(Gs2.Gs2Key.Model.Key[] items) {
            this.Items = items;
            return this;
        }

        public DescribeKeysResult WithNextPageToken(string nextPageToken) {
            this.NextPageToken = nextPageToken;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeKeysResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeKeysResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null || !data["items"].IsArray ? new Gs2.Gs2Key.Model.Key[]{} : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Key.Model.Key.FromJson(v);
                }).ToArray())
                .WithNextPageToken(!data.Keys.Contains("nextPageToken") || data["nextPageToken"] == null ? null : data["nextPageToken"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData itemsJsonData = null;
            if (Items != null && Items.Length > 0)
            {
                itemsJsonData = new JsonData();
                foreach (var item in Items)
                {
                    itemsJsonData.Add(item.ToJson());
                }
            }
            return new JsonData {
                ["items"] = itemsJsonData,
                ["nextPageToken"] = NextPageToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            if (NextPageToken != null) {
                writer.WritePropertyName("nextPageToken");
                writer.Write(NextPageToken.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}