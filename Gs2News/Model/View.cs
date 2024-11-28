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

namespace Gs2.Gs2News.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class View : IComparable
	{
        public Gs2.Gs2News.Model.Content[] Contents { set; get; } = null!;
        public Gs2.Gs2News.Model.Content[] RemoveContents { set; get; } = null!;
        public View WithContents(Gs2.Gs2News.Model.Content[] contents) {
            this.Contents = contents;
            return this;
        }
        public View WithRemoveContents(Gs2.Gs2News.Model.Content[] removeContents) {
            this.RemoveContents = removeContents;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static View FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new View()
                .WithContents(!data.Keys.Contains("contents") || data["contents"] == null || !data["contents"].IsArray ? null : data["contents"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2News.Model.Content.FromJson(v);
                }).ToArray())
                .WithRemoveContents(!data.Keys.Contains("removeContents") || data["removeContents"] == null || !data["removeContents"].IsArray ? null : data["removeContents"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2News.Model.Content.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
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
            JsonData removeContentsJsonData = null;
            if (RemoveContents != null && RemoveContents.Length > 0)
            {
                removeContentsJsonData = new JsonData();
                foreach (var removeContent in RemoveContents)
                {
                    removeContentsJsonData.Add(removeContent.ToJson());
                }
            }
            return new JsonData {
                ["contents"] = contentsJsonData,
                ["removeContents"] = removeContentsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            if (RemoveContents != null) {
                writer.WritePropertyName("removeContents");
                writer.WriteArrayStart();
                foreach (var removeContent in RemoveContents)
                {
                    if (removeContent != null) {
                        removeContent.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as View;
            var diff = 0;
            if (Contents == null && Contents == other.Contents)
            {
                // null and null
            }
            else
            {
                diff += Contents.Length - other.Contents.Length;
                for (var i = 0; i < Contents.Length; i++)
                {
                    diff += Contents[i].CompareTo(other.Contents[i]);
                }
            }
            if (RemoveContents == null && RemoveContents == other.RemoveContents)
            {
                // null and null
            }
            else
            {
                diff += RemoveContents.Length - other.RemoveContents.Length;
                for (var i = 0; i < RemoveContents.Length; i++)
                {
                    diff += RemoveContents[i].CompareTo(other.RemoveContents[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (Contents.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("view", "news.view.contents.error.tooMany"),
                    });
                }
            }
            {
                if (RemoveContents.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("view", "news.view.removeContents.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new View {
                Contents = Contents.Clone() as Gs2.Gs2News.Model.Content[],
                RemoveContents = RemoveContents.Clone() as Gs2.Gs2News.Model.Content[],
            };
        }
    }
}