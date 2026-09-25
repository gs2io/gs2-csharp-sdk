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
	public partial class Receipt : IComparable
	{
        public string Store { set; get; }
        public string TransactionID { set; get; }
        public string Payload { set; get; }
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
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Receipt.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Store, other.Store);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TransactionID, other.TransactionID);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Payload, other.Payload);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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