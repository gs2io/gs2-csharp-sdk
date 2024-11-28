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
using Gs2.Gs2News.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2News.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class WantGrantByUserIdResult : IResult
	{
        public Gs2.Gs2News.Model.SetCookieRequestEntry[] Items { set; get; } = null!;
        public string BrowserUrl { set; get; } = null!;
        public string ZipUrl { set; get; } = null!;

        public WantGrantByUserIdResult WithItems(Gs2.Gs2News.Model.SetCookieRequestEntry[] items) {
            this.Items = items;
            return this;
        }

        public WantGrantByUserIdResult WithBrowserUrl(string browserUrl) {
            this.BrowserUrl = browserUrl;
            return this;
        }

        public WantGrantByUserIdResult WithZipUrl(string zipUrl) {
            this.ZipUrl = zipUrl;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WantGrantByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WantGrantByUserIdResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null || !data["items"].IsArray ? null : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2News.Model.SetCookieRequestEntry.FromJson(v);
                }).ToArray())
                .WithBrowserUrl(!data.Keys.Contains("browserUrl") || data["browserUrl"] == null ? null : data["browserUrl"].ToString())
                .WithZipUrl(!data.Keys.Contains("zipUrl") || data["zipUrl"] == null ? null : data["zipUrl"].ToString());
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
                ["browserUrl"] = BrowserUrl,
                ["zipUrl"] = ZipUrl,
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
            if (BrowserUrl != null) {
                writer.WritePropertyName("browserUrl");
                writer.Write(BrowserUrl.ToString());
            }
            if (ZipUrl != null) {
                writer.WritePropertyName("zipUrl");
                writer.Write(ZipUrl.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}