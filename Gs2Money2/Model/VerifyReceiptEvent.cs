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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class VerifyReceiptEvent : IComparable
	{
        public string ContentName { set; get; }
        public string Platform { set; get; }
        public Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent AppleAppStoreVerifyReceiptEvent { set; get; }
        public Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent GooglePlayVerifyReceiptEvent { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type VerifyReceiptEvent.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(ContentName, other.ContentName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Platform, other.Platform);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AppleAppStoreVerifyReceiptEvent, other.AppleAppStoreVerifyReceiptEvent);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(GooglePlayVerifyReceiptEvent, other.GooglePlayVerifyReceiptEvent);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
                AppleAppStoreVerifyReceiptEvent = AppleAppStoreVerifyReceiptEvent?.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent,
                GooglePlayVerifyReceiptEvent = GooglePlayVerifyReceiptEvent?.Clone() as Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent,
            };
        }
    }
}