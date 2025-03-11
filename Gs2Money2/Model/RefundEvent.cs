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
	public partial class RefundEvent : IComparable
	{
        public string ContentName { set; get; }
        public string Platform { set; get; }
        public Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent AppleAppStoreRefundEvent { set; get; }
        public Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent GooglePlayRefundEvent { set; get; }
        public RefundEvent WithContentName(string contentName) {
            this.ContentName = contentName;
            return this;
        }
        public RefundEvent WithPlatform(string platform) {
            this.Platform = platform;
            return this;
        }
        public RefundEvent WithAppleAppStoreRefundEvent(Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent appleAppStoreRefundEvent) {
            this.AppleAppStoreRefundEvent = appleAppStoreRefundEvent;
            return this;
        }
        public RefundEvent WithGooglePlayRefundEvent(Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent googlePlayRefundEvent) {
            this.GooglePlayRefundEvent = googlePlayRefundEvent;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RefundEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RefundEvent()
                .WithContentName(!data.Keys.Contains("contentName") || data["contentName"] == null ? null : data["contentName"].ToString())
                .WithPlatform(!data.Keys.Contains("platform") || data["platform"] == null ? null : data["platform"].ToString())
                .WithAppleAppStoreRefundEvent(!data.Keys.Contains("appleAppStoreRefundEvent") || data["appleAppStoreRefundEvent"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent.FromJson(data["appleAppStoreRefundEvent"]))
                .WithGooglePlayRefundEvent(!data.Keys.Contains("googlePlayRefundEvent") || data["googlePlayRefundEvent"] == null ? null : Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent.FromJson(data["googlePlayRefundEvent"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["contentName"] = ContentName,
                ["platform"] = Platform,
                ["appleAppStoreRefundEvent"] = AppleAppStoreRefundEvent?.ToJson(),
                ["googlePlayRefundEvent"] = GooglePlayRefundEvent?.ToJson(),
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
            if (AppleAppStoreRefundEvent != null) {
                writer.WritePropertyName("appleAppStoreRefundEvent");
                AppleAppStoreRefundEvent.WriteJson(writer);
            }
            if (GooglePlayRefundEvent != null) {
                writer.WritePropertyName("googlePlayRefundEvent");
                GooglePlayRefundEvent.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RefundEvent;
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
            if (AppleAppStoreRefundEvent == null && AppleAppStoreRefundEvent == other.AppleAppStoreRefundEvent)
            {
                // null and null
            }
            else
            {
                diff += AppleAppStoreRefundEvent.CompareTo(other.AppleAppStoreRefundEvent);
            }
            if (GooglePlayRefundEvent == null && GooglePlayRefundEvent == other.GooglePlayRefundEvent)
            {
                // null and null
            }
            else
            {
                diff += GooglePlayRefundEvent.CompareTo(other.GooglePlayRefundEvent);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ContentName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("refundEvent", "money2.refundEvent.contentName.error.tooLong"),
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
                            new RequestError("refundEvent", "money2.refundEvent.platform.error.invalid"),
                        });
                }
            }
            {
            }
            {
            }
        }

        public object Clone() {
            return new RefundEvent {
                ContentName = ContentName,
                Platform = Platform,
                AppleAppStoreRefundEvent = AppleAppStoreRefundEvent?.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreVerifyReceiptEvent,
                GooglePlayRefundEvent = GooglePlayRefundEvent?.Clone() as Gs2.Gs2Money2.Model.GooglePlayVerifyReceiptEvent,
            };
        }
    }
}