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
	public class ScriptTransactionResult : IComparable
	{
        public string ScriptId { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public Gs2.Core.Model.TransactionResult TransactionResult { set; get; } = null!;
        public ScriptTransactionResult WithScriptId(string scriptId) {
            this.ScriptId = scriptId;
            return this;
        }
        public ScriptTransactionResult WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public ScriptTransactionResult WithTransactionResult(Gs2.Core.Model.TransactionResult transactionResult) {
            this.TransactionResult = transactionResult;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ScriptTransactionResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ScriptTransactionResult()
                .WithScriptId(!data.Keys.Contains("scriptId") || data["scriptId"] == null ? null : data["scriptId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithTransactionResult(!data.Keys.Contains("transactionResult") || data["transactionResult"] == null ? null : TransactionResult.FromJson(data["transactionResult"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["scriptId"] = ScriptId,
                ["transactionId"] = TransactionId,
                ["transactionResult"] = TransactionResult.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ScriptId != null) {
                writer.WritePropertyName("scriptId");
                writer.Write(ScriptId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (TransactionResult != null) {
                writer.WritePropertyName("transactionResult");
                TransactionResult.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ScriptTransactionResult;
            var diff = 0;
            if (ScriptId == null && ScriptId == other.ScriptId)
            {
                // null and null
            }
            else
            {
                diff += ScriptId.CompareTo(other.ScriptId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            return diff;
        }

        public void Validate() {
        }

        public object Clone() {
            return new ScriptTransactionResult {
                ScriptId = ScriptId,
                TransactionId = TransactionId,
                TransactionResult = TransactionResult?.Clone() as Gs2.Core.Model.TransactionResult,
            };
        }
    }
}