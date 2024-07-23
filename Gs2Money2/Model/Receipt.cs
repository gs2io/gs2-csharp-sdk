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
	public class Receipt : IComparable
	{
        public string Store { set; get; } = null!;
        public string TransactionID { set; get; } = null!;
        public string Payload { set; get; } = null!;
        public Receipt WithStore(string store) {
            this.Store = store;
            return this;
        }
        public Receipt WithTransactionID(string transactionID) {
            this.TransactionID = transactionID;
            return this;
        }
        public Receipt WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Receipt FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Receipt()
                .WithStore(!data.Keys.Contains("Store") || data["Store"] == null ? null : data["Store"].ToString())
                .WithTransactionID(!data.Keys.Contains("TransactionID") || data["TransactionID"] == null ? null : data["TransactionID"].ToString())
                .WithPayload(!data.Keys.Contains("Payload") || data["Payload"] == null ? null : data["Payload"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["Store"] = Store,
                ["TransactionID"] = TransactionID,
                ["Payload"] = Payload,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Store != null) {
                writer.WritePropertyName("Store");
                writer.Write(Store.ToString());
            }
            if (TransactionID != null) {
                writer.WritePropertyName("TransactionID");
                writer.Write(TransactionID.ToString());
            }
            if (Payload != null) {
                writer.WritePropertyName("Payload");
                writer.Write(Payload.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Receipt;
            var diff = 0;
            if (Store == null && Store == other.Store)
            {
                // null and null
            }
            else
            {
                diff += Store.CompareTo(other.Store);
            }
            if (TransactionID == null && TransactionID == other.TransactionID)
            {
                // null and null
            }
            else
            {
                diff += TransactionID.CompareTo(other.TransactionID);
            }
            if (Payload == null && Payload == other.Payload)
            {
                // null and null
            }
            else
            {
                diff += Payload.CompareTo(other.Payload);
            }
            return diff;
        }

        public void Validate() {
            {
                switch (Store) {
                    case "AppleAppStore":
                    case "GooglePlay":
                    case "fake":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("receipt", "money2.receipt.store.error.invalid"),
                        });
                }
            }
            {
                if (TransactionID.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money2.receipt.transactionID.error.tooLong"),
                    });
                }
            }
            {
                if (Payload.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receipt", "money2.receipt.payload.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new Receipt {
                Store = Store,
                TransactionID = TransactionID,
                Payload = Payload,
            };
        }
    }
}