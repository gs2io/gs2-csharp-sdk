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

namespace Gs2.Core.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class TransactionResult : IComparable
	{
        public string TransactionId { set; get; } = null!;
        public Gs2.Core.Model.VerifyActionResult[] VerifyResults { set; get; } = null!;
        public Gs2.Core.Model.ConsumeActionResult[] ConsumeResults { set; get; } = null!;
        public Gs2.Core.Model.AcquireActionResult[] AcquireResults { set; get; } = null!;
        public TransactionResult WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public TransactionResult WithVerifyResults(Gs2.Core.Model.VerifyActionResult[] verifyResults) {
            this.VerifyResults = verifyResults;
            return this;
        }
        public TransactionResult WithConsumeResults(Gs2.Core.Model.ConsumeActionResult[] consumeResults) {
            this.ConsumeResults = consumeResults;
            return this;
        }
        public TransactionResult WithAcquireResults(Gs2.Core.Model.AcquireActionResult[] acquireResults) {
            this.AcquireResults = acquireResults;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TransactionResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TransactionResult()
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithVerifyResults(!data.Keys.Contains("verifyResults") || data["verifyResults"] == null || !data["verifyResults"].IsArray ? new Gs2.Core.Model.VerifyActionResult[]{} : data["verifyResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyActionResult.FromJson(v);
                }).ToArray())
                .WithConsumeResults(!data.Keys.Contains("consumeResults") || data["consumeResults"] == null || !data["consumeResults"].IsArray ? new Gs2.Core.Model.ConsumeActionResult[]{} : data["consumeResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeActionResult.FromJson(v);
                }).ToArray())
                .WithAcquireResults(!data.Keys.Contains("acquireResults") || data["acquireResults"] == null || !data["acquireResults"].IsArray ? new Gs2.Core.Model.AcquireActionResult[]{} : data["acquireResults"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireActionResult.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData verifyResultsJsonData = null;
            if (VerifyResults != null && VerifyResults.Length > 0)
            {
                verifyResultsJsonData = new JsonData();
                foreach (var verifyResult in VerifyResults)
                {
                    verifyResultsJsonData.Add(verifyResult.ToJson());
                }
            }
            JsonData consumeResultsJsonData = null;
            if (ConsumeResults != null && ConsumeResults.Length > 0)
            {
                consumeResultsJsonData = new JsonData();
                foreach (var consumeResult in ConsumeResults)
                {
                    consumeResultsJsonData.Add(consumeResult.ToJson());
                }
            }
            JsonData acquireResultsJsonData = null;
            if (AcquireResults != null && AcquireResults.Length > 0)
            {
                acquireResultsJsonData = new JsonData();
                foreach (var acquireResult in AcquireResults)
                {
                    acquireResultsJsonData.Add(acquireResult.ToJson());
                }
            }
            return new JsonData {
                ["transactionId"] = TransactionId,
                ["verifyResults"] = verifyResultsJsonData,
                ["consumeResults"] = consumeResultsJsonData,
                ["acquireResults"] = acquireResultsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (VerifyResults != null) {
                writer.WritePropertyName("verifyResults");
                writer.WriteArrayStart();
                foreach (var verifyResult in VerifyResults)
                {
                    if (verifyResult != null) {
                        verifyResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ConsumeResults != null) {
                writer.WritePropertyName("consumeResults");
                writer.WriteArrayStart();
                foreach (var consumeResult in ConsumeResults)
                {
                    if (consumeResult != null) {
                        consumeResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireResults != null) {
                writer.WritePropertyName("acquireResults");
                writer.WriteArrayStart();
                foreach (var acquireResult in AcquireResults)
                {
                    if (acquireResult != null) {
                        acquireResult.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TransactionResult;
            var diff = 0;
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (VerifyResults == null && VerifyResults == other.VerifyResults)
            {
                // null and null
            }
            else
            {
                diff += VerifyResults.Length - other.VerifyResults.Length;
                for (var i = 0; i < VerifyResults.Length; i++)
                {
                    diff += VerifyResults[i].CompareTo(other.VerifyResults[i]);
                }
            }
            if (ConsumeResults == null && ConsumeResults == other.ConsumeResults)
            {
                // null and null
            }
            else
            {
                diff += ConsumeResults.Length - other.ConsumeResults.Length;
                for (var i = 0; i < ConsumeResults.Length; i++)
                {
                    diff += ConsumeResults[i].CompareTo(other.ConsumeResults[i]);
                }
            }
            if (AcquireResults == null && AcquireResults == other.AcquireResults)
            {
                // null and null
            }
            else
            {
                diff += AcquireResults.Length - other.AcquireResults.Length;
                for (var i = 0; i < AcquireResults.Length; i++)
                {
                    diff += AcquireResults[i].CompareTo(other.AcquireResults[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (TransactionId.Length < 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "account.transactionResult.transactionId.error.tooShort"),
                    });
                }
                if (TransactionId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "account.transactionResult.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyResults.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "account.transactionResult.verifyResults.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeResults.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "account.transactionResult.consumeResults.error.tooMany"),
                    });
                }
            }
            {
                if (AcquireResults.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionResult", "account.transactionResult.acquireResults.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new TransactionResult {
                TransactionId = TransactionId,
                VerifyResults = VerifyResults.Clone() as Gs2.Core.Model.VerifyActionResult[],
                ConsumeResults = ConsumeResults.Clone() as Gs2.Core.Model.ConsumeActionResult[],
                AcquireResults = AcquireResults.Clone() as Gs2.Core.Model.AcquireActionResult[],
            };
        }
    }
}