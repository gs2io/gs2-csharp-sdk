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
	public class Content : IComparable
	{
        public string Section { set; get; } = null!;
        public string Value { set; get; } = null!;
        public string FrontMatter { set; get; } = null!;
        public Content WithSection(string section) {
            this.Section = section;
            return this;
        }
        public Content WithValue(string value) {
            this.Value = value;
            return this;
        }
        public Content WithFrontMatter(string frontMatter) {
            this.FrontMatter = frontMatter;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Content FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Content()
                .WithSection(!data.Keys.Contains("section") || data["section"] == null ? null : data["section"].ToString())
                .WithValue(!data.Keys.Contains("content") || data["content"] == null ? null : data["content"].ToString())
                .WithFrontMatter(!data.Keys.Contains("frontMatter") || data["frontMatter"] == null ? null : data["frontMatter"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["section"] = Section,
                ["content"] = Value,
                ["frontMatter"] = FrontMatter,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Section != null) {
                writer.WritePropertyName("section");
                writer.Write(Section.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(Value.ToString());
            }
            if (FrontMatter != null) {
                writer.WritePropertyName("frontMatter");
                writer.Write(FrontMatter.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Content;
            var diff = 0;
            if (Section == null && Section == other.Section)
            {
                // null and null
            }
            else
            {
                diff += Section.CompareTo(other.Section);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += Value.CompareTo(other.Value);
            }
            if (FrontMatter == null && FrontMatter == other.FrontMatter)
            {
                // null and null
            }
            else
            {
                diff += FrontMatter.CompareTo(other.FrontMatter);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Section.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("content", "news.content.section.error.tooLong"),
                    });
                }
            }
            {
                if (Value.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("content", "news.content.content.error.tooLong"),
                    });
                }
            }
            {
                if (FrontMatter.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("content", "news.content.frontMatter.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new Content {
                Section = Section,
                Value = Value,
                FrontMatter = FrontMatter,
            };
        }
    }
}