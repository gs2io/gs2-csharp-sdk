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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class VerifyReceiptEvent : IComparable
	{
        public string ContentName { set; get; } = null!;
        public string Platform { set; get; } = null!;
        public Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent AppleAppStoreVerifyReceiptEvent { set; get; } = null!;
        public Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent GooglePlayVerifyReceiptEvent { set; get; } = null!;
        public VerifyReceiptEvent WithContentName(string contentName) {
            this.ContentName = contentName;
            return this;
        }
        public VerifyReceiptEvent WithPlatform(string platform) {
            this.Platform = platform;
            return this;
        }
        public VerifyReceiptEvent WithAppleAppStoreVerifyReceiptEvent(Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent appleAppStoreVerifyReceiptEvent) {
            this.AppleAppStoreVerifyReceiptEvent = appleAppStoreVerifyReceiptEvent;
            return this;
        }
        public VerifyReceiptEvent WithGooglePlayVerifyReceiptEvent(Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent googlePlayVerifyReceiptEvent) {
            this.GooglePlayVerifyReceiptEvent = googlePlayVerifyReceiptEvent;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyReceiptEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyReceiptEvent()
                .WithContentName(!data.Keys.Contains("contentName") || data["contentName"] == null ? null : data["contentName"].ToString())
                .WithPlatform(!data.Keys.Contains("platform") || data["platform"] == null ? null : data["platform"].ToString())
                .WithAppleAppStoreVerifyReceiptEvent(!data.Keys.Contains("appleAppStoreVerifyReceiptEvent") || data["appleAppStoreVerifyReceiptEvent"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent.FromJson(data["appleAppStoreVerifyReceiptEvent"]))
                .WithGooglePlayVerifyReceiptEvent(!data.Keys.Contains("googlePlayVerifyReceiptEvent") || data["googlePlayVerifyReceiptEvent"] == null ? null : Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent.FromJson(data["googlePlayVerifyReceiptEvent"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["contentName"] = ContentName,
                ["platform"] = Platform,
                ["appleAppStoreVerifyReceiptEvent"] = AppleAppStoreVerifyReceiptEvent?.ToJson(),
                ["googlePlayVerifyReceiptEvent"] = GooglePlayVerifyReceiptEvent?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ContentName != null) {
                writer.WritePropertyName("contentName");
                writer.Write(ContentName.ToString());
            }
            if (Platform != null) {
                writer.WritePropertyName("platform");
                writer.Write(Platform.ToString());
            }
            if (AppleAppStoreVerifyReceiptEvent != null) {
                writer.WritePropertyName("appleAppStoreVerifyReceiptEvent");
                AppleAppStoreVerifyReceiptEvent.WriteJson(writer);
            }
            if (GooglePlayVerifyReceiptEvent != null) {
                writer.WritePropertyName("googlePlayVerifyReceiptEvent");
                GooglePlayVerifyReceiptEvent.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as VerifyReceiptEvent;
            var diff = 0;
            if (ContentName == null && ContentName == other.ContentName)
            {
                // null and null
            }
            else
            {
                diff += ContentName.CompareTo(other.ContentName);
            }
            if (Platform == null && Platform == other.Platform)
            {
                // null and null
            }
            else
            {
                diff += Platform.CompareTo(other.Platform);
            }
            if (AppleAppStoreVerifyReceiptEvent == null && AppleAppStoreVerifyReceiptEvent == other.AppleAppStoreVerifyReceiptEvent)
            {
                // null and null
            }
            else
            {
                diff += AppleAppStoreVerifyReceiptEvent.CompareTo(other.AppleAppStoreVerifyReceiptEvent);
            }
            if (GooglePlayVerifyReceiptEvent == null && GooglePlayVerifyReceiptEvent == other.GooglePlayVerifyReceiptEvent)
            {
                // null and null
            }
            else
            {
                diff += GooglePlayVerifyReceiptEvent.CompareTo(other.GooglePlayVerifyReceiptEvent);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ContentName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("verifyReceiptEvent", "money2.verifyReceiptEvent.contentName.error.tooLong"),
                    });
                }
            }
            {
                switch (Platform) {
                    case "AppleAppStore":
                    case "GooglePlay":
                    case "fake":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("verifyReceiptEvent", "money2.verifyReceiptEvent.platform.error.invalid"),
                        });
                }
            }
            {
            }
            {
            }
        }

        public object Clone() {
            return new VerifyReceiptEvent {
                ContentName = ContentName,
                Platform = Platform,
                AppleAppStoreVerifyReceiptEvent = AppleAppStoreVerifyReceiptEvent.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent,
                GooglePlayVerifyReceiptEvent = GooglePlayVerifyReceiptEvent.Clone() as Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent,
            };
        }
    }
}