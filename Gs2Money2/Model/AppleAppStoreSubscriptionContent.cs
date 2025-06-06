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
	public partial class AppleAppStoreSubscriptionContent : IComparable
	{
        public string SubscriptionGroupIdentifier { set; get; }
        public AppleAppStoreSubscriptionContent WithSubscriptionGroupIdentifier(string subscriptionGroupIdentifier) {
            this.SubscriptionGroupIdentifier = subscriptionGroupIdentifier;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AppleAppStoreSubscriptionContent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AppleAppStoreSubscriptionContent()
                .WithSubscriptionGroupIdentifier(!data.Keys.Contains("subscriptionGroupIdentifier") || data["subscriptionGroupIdentifier"] == null ? null : data["subscriptionGroupIdentifier"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscriptionGroupIdentifier"] = SubscriptionGroupIdentifier,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SubscriptionGroupIdentifier != null) {
                writer.WritePropertyName("subscriptionGroupIdentifier");
                writer.Write(SubscriptionGroupIdentifier.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AppleAppStoreSubscriptionContent;
            var diff = 0;
            if (SubscriptionGroupIdentifier == null && SubscriptionGroupIdentifier == other.SubscriptionGroupIdentifier)
            {
                // null and null
            }
            else
            {
                diff += SubscriptionGroupIdentifier.CompareTo(other.SubscriptionGroupIdentifier);
            }
            return diff;
        }

        public void Validate() {
            {
                if (SubscriptionGroupIdentifier.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appleAppStoreSubscriptionContent", "money2.appleAppStoreSubscriptionContent.subscriptionGroupIdentifier.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new AppleAppStoreSubscriptionContent {
                SubscriptionGroupIdentifier = SubscriptionGroupIdentifier,
            };
        }
    }
}