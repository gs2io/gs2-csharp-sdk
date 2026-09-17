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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class News : IComparable
	{
        public string Section { set; get; }
        public string Content { set; get; }
        public string Title { set; get; }
        public string ScheduleEventId { set; get; }
        public long? Timestamp { set; get; }
        public string FrontMatter { set; get; }
        public News WithSection(string section) {
            this.Section = section;
            return this;
        }
        public News WithContent(string content) {
            this.Content = content;
            return this;
        }
        public News WithTitle(string title) {
            this.Title = title;
            return this;
        }
        public News WithScheduleEventId(string scheduleEventId) {
            this.ScheduleEventId = scheduleEventId;
            return this;
        }
        public News WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }
        public News WithFrontMatter(string frontMatter) {
            this.FrontMatter = frontMatter;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static News FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new News()
                .WithSection(!data.Keys.Contains("section") || data["section"] == null ? null : data["section"].ToString())
                .WithContent(!data.Keys.Contains("content") || data["content"] == null ? null : data["content"].ToString())
                .WithTitle(!data.Keys.Contains("title") || data["title"] == null ? null : data["title"].ToString())
                .WithScheduleEventId(!data.Keys.Contains("scheduleEventId") || data["scheduleEventId"] == null ? null : data["scheduleEventId"].ToString())
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["timestamp"].ToString()))
                .WithFrontMatter(!data.Keys.Contains("frontMatter") || data["frontMatter"] == null ? null : data["frontMatter"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["section"] = Section,
                ["content"] = Content,
                ["title"] = Title,
                ["scheduleEventId"] = ScheduleEventId,
                ["timestamp"] = Timestamp,
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
            if (Content != null) {
                writer.WritePropertyName("content");
                writer.Write(Content.ToString());
            }
            if (Title != null) {
                writer.WritePropertyName("title");
                writer.Write(Title.ToString());
            }
            if (ScheduleEventId != null) {
                writer.WritePropertyName("scheduleEventId");
                writer.Write(ScheduleEventId.ToString());
            }
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            if (FrontMatter != null) {
                writer.WritePropertyName("frontMatter");
                writer.Write(FrontMatter.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as News;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type News.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Section, other.Section);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Content, other.Content);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Title, other.Title);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ScheduleEventId, other.ScheduleEventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Timestamp, other.Timestamp);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(FrontMatter, other.FrontMatter);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Section.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("news", "news.news.section.error.tooLong"),
                    });
                }
            }
            {
                if (Content.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("news", "news.news.content.error.tooLong"),
                    });
                }
            }
            {
                if (Title.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("news", "news.news.title.error.tooLong"),
                    });
                }
            }
            {
                if (ScheduleEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("news", "news.news.scheduleEventId.error.tooLong"),
                    });
                }
            }
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("news", "news.news.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("news", "news.news.timestamp.error.invalid"),
                    });
                }
            }
            {
                if (FrontMatter.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("news", "news.news.frontMatter.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new News {
                Section = Section,
                Content = Content,
                Title = Title,
                ScheduleEventId = ScheduleEventId,
                Timestamp = Timestamp,
                FrontMatter = FrontMatter,
            };
        }
    }
}