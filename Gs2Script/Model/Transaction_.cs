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

namespace Gs2.Gs2Script.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Transaction_ : IComparable
	{
        public string TransactionId { set; get; }
        public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public Transaction_ WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public Transaction_ WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
            return this;
        }
        public Transaction_ WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }
        public Transaction_ WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Transaction_ FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Transaction_()
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithVerifyActions(!data.Keys.Contains("verifyActions") || data["verifyActions"] == null || !data["verifyActions"].IsArray ? null : data["verifyActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? null : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData verifyActionsJsonData = null;
            if (VerifyActions != null && VerifyActions.Length > 0)
            {
                verifyActionsJsonData = new JsonData();
                foreach (var verifyAction in VerifyActions)
                {
                    verifyActionsJsonData.Add(verifyAction.ToJson());
                }
            }
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null && ConsumeActions.Length > 0)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["transactionId"] = TransactionId,
                ["verifyActions"] = verifyActionsJsonData,
                ["consumeActions"] = consumeActionsJsonData,
                ["acquireActions"] = acquireActionsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (VerifyActions != null) {
                writer.WritePropertyName("verifyActions");
                writer.WriteArrayStart();
                foreach (var verifyAction in VerifyActions)
                {
                    if (verifyAction != null) {
                        verifyAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ConsumeActions != null) {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach (var consumeAction in ConsumeActions)
                {
                    if (consumeAction != null) {
                        consumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Transaction_;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Transaction_.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(TransactionId, other.TransactionId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(VerifyActions, other.VerifyActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(ConsumeActions, other.ConsumeActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(AcquireActions, other.AcquireActions);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (TransactionId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transaction", "script.transaction.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transaction", "script.transaction.verifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transaction", "script.transaction.consumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transaction", "script.transaction.acquireActions.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new Transaction_ {
                TransactionId = TransactionId,
                VerifyActions = VerifyActions?.Clone() as Gs2.Core.Model.VerifyAction[],
                ConsumeActions = ConsumeActions?.Clone() as Gs2.Core.Model.ConsumeAction[],
                AcquireActions = AcquireActions?.Clone() as Gs2.Core.Model.AcquireAction[],
            };
        }
    }
}