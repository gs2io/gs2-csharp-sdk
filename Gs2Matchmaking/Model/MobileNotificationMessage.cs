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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class MobileNotificationMessage : IComparable
	{
        public string Locale { set; get; }
        public string Title { set; get; }
        public string Message { set; get; }
        public MobileNotificationMessage WithLocale(string locale) {
            this.Locale = locale;
            return this;
        }
        public MobileNotificationMessage WithTitle(string title) {
            this.Title = title;
            return this;
        }
        public MobileNotificationMessage WithMessage(string message) {
            this.Message = message;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MobileNotificationMessage FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new MobileNotificationMessage()
                .WithLocale(!data.Keys.Contains("locale") || data["locale"] == null ? null : data["locale"].ToString())
                .WithTitle(!data.Keys.Contains("title") || data["title"] == null ? null : data["title"].ToString())
                .WithMessage(!data.Keys.Contains("message") || data["message"] == null ? null : data["message"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["locale"] = Locale,
                ["title"] = Title,
                ["message"] = Message,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Locale != null) {
                writer.WritePropertyName("locale");
                writer.Write(Locale.ToString());
            }
            if (Title != null) {
                writer.WritePropertyName("title");
                writer.Write(Title.ToString());
            }
            if (Message != null) {
                writer.WritePropertyName("message");
                writer.Write(Message.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as MobileNotificationMessage;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type MobileNotificationMessage.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Locale, other.Locale);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Title, other.Title);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Message, other.Message);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Locale.Length > 32) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("mobileNotificationMessage", "matchmaking.mobileNotificationMessage.locale.error.tooLong"),
                    });
                }
            }
            {
                if (Title.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("mobileNotificationMessage", "matchmaking.mobileNotificationMessage.title.error.tooLong"),
                    });
                }
            }
            {
                if (Message.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("mobileNotificationMessage", "matchmaking.mobileNotificationMessage.message.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new MobileNotificationMessage {
                Locale = Locale,
                Title = Title,
                Message = Message,
            };
        }
    }
}