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
	public class GooglePlayVerifyReceiptEvent : IComparable
	{
        public string PurchaseToken { set; get; } = null!;
        public GooglePlayVerifyReceiptEvent WithPurchaseToken(string purchaseToken) {
            this.PurchaseToken = purchaseToken;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GooglePlayVerifyReceiptEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GooglePlayVerifyReceiptEvent()
                .WithPurchaseToken(!data.Keys.Contains("purchaseToken") || data["purchaseToken"] == null ? null : data["purchaseToken"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["purchaseToken"] = PurchaseToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PurchaseToken != null) {
                writer.WritePropertyName("purchaseToken");
                writer.Write(PurchaseToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GooglePlayVerifyReceiptEvent;
            var diff = 0;
            if (PurchaseToken == null && PurchaseToken == other.PurchaseToken)
            {
                // null and null
            }
            else
            {
                diff += PurchaseToken.CompareTo(other.PurchaseToken);
            }
            return diff;
        }

        public void Validate() {
            {
                if (PurchaseToken.Length > 4096) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("googlePlayVerifyReceiptEvent", "money2.googlePlayVerifyReceiptEvent.purchaseToken.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GooglePlayVerifyReceiptEvent {
                PurchaseToken = PurchaseToken,
            };
        }
    }
}