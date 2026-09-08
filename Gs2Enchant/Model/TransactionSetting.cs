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

namespace Gs2.Gs2Enchant.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class TransactionSetting : IComparable
	{
        public bool? EnableAutoRun { set; get; }
        public bool? EnableAtomicCommit { set; get; }
        public bool? TransactionUseDistributor { set; get; }
        public bool? CommitScriptResultInUseDistributor { set; get; }
        public bool? AcquireActionUseJobQueue { set; get; }
        public bool? EnableSequentialExecution { set; get; }
        public string DistributorNamespaceId { set; get; }
        [Obsolete("This method is deprecated")]
        public string KeyId { set; get; }
        public string QueueNamespaceId { set; get; }
        public TransactionSetting WithEnableAutoRun(bool? enableAutoRun) {
            this.EnableAutoRun = enableAutoRun;
            return this;
        }
        public TransactionSetting WithEnableAtomicCommit(bool? enableAtomicCommit) {
            this.EnableAtomicCommit = enableAtomicCommit;
            return this;
        }
        public TransactionSetting WithTransactionUseDistributor(bool? transactionUseDistributor) {
            this.TransactionUseDistributor = transactionUseDistributor;
            return this;
        }
        public TransactionSetting WithCommitScriptResultInUseDistributor(bool? commitScriptResultInUseDistributor) {
            this.CommitScriptResultInUseDistributor = commitScriptResultInUseDistributor;
            return this;
        }
        public TransactionSetting WithAcquireActionUseJobQueue(bool? acquireActionUseJobQueue) {
            this.AcquireActionUseJobQueue = acquireActionUseJobQueue;
            return this;
        }
        public TransactionSetting WithEnableSequentialExecution(bool? enableSequentialExecution) {
            this.EnableSequentialExecution = enableSequentialExecution;
            return this;
        }
        public TransactionSetting WithDistributorNamespaceId(string distributorNamespaceId) {
            this.DistributorNamespaceId = distributorNamespaceId;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public TransactionSetting WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }
        public TransactionSetting WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TransactionSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TransactionSetting()
                .WithEnableAutoRun(!data.Keys.Contains("enableAutoRun") || data["enableAutoRun"] == null ? null : (bool?)bool.Parse(data["enableAutoRun"].ToString()))
                .WithEnableAtomicCommit(!data.Keys.Contains("enableAtomicCommit") || data["enableAtomicCommit"] == null ? null : (bool?)bool.Parse(data["enableAtomicCommit"].ToString()))
                .WithTransactionUseDistributor(!data.Keys.Contains("transactionUseDistributor") || data["transactionUseDistributor"] == null ? null : (bool?)bool.Parse(data["transactionUseDistributor"].ToString()))
                .WithCommitScriptResultInUseDistributor(!data.Keys.Contains("commitScriptResultInUseDistributor") || data["commitScriptResultInUseDistributor"] == null ? null : (bool?)bool.Parse(data["commitScriptResultInUseDistributor"].ToString()))
                .WithAcquireActionUseJobQueue(!data.Keys.Contains("acquireActionUseJobQueue") || data["acquireActionUseJobQueue"] == null ? null : (bool?)bool.Parse(data["acquireActionUseJobQueue"].ToString()))
                .WithEnableSequentialExecution(!data.Keys.Contains("enableSequentialExecution") || data["enableSequentialExecution"] == null ? null : (bool?)bool.Parse(data["enableSequentialExecution"].ToString()))
                .WithDistributorNamespaceId(!data.Keys.Contains("distributorNamespaceId") || data["distributorNamespaceId"] == null ? null : data["distributorNamespaceId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["enableAutoRun"] = EnableAutoRun,
                ["enableAtomicCommit"] = EnableAtomicCommit,
                ["transactionUseDistributor"] = TransactionUseDistributor,
                ["commitScriptResultInUseDistributor"] = CommitScriptResultInUseDistributor,
                ["acquireActionUseJobQueue"] = AcquireActionUseJobQueue,
                ["enableSequentialExecution"] = EnableSequentialExecution,
                ["distributorNamespaceId"] = DistributorNamespaceId,
                ["queueNamespaceId"] = QueueNamespaceId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (EnableAutoRun != null) {
                writer.WritePropertyName("enableAutoRun");
                writer.Write(bool.Parse(EnableAutoRun.ToString()));
            }
            if (EnableAtomicCommit != null) {
                writer.WritePropertyName("enableAtomicCommit");
                writer.Write(bool.Parse(EnableAtomicCommit.ToString()));
            }
            if (TransactionUseDistributor != null) {
                writer.WritePropertyName("transactionUseDistributor");
                writer.Write(bool.Parse(TransactionUseDistributor.ToString()));
            }
            if (CommitScriptResultInUseDistributor != null) {
                writer.WritePropertyName("commitScriptResultInUseDistributor");
                writer.Write(bool.Parse(CommitScriptResultInUseDistributor.ToString()));
            }
            if (AcquireActionUseJobQueue != null) {
                writer.WritePropertyName("acquireActionUseJobQueue");
                writer.Write(bool.Parse(AcquireActionUseJobQueue.ToString()));
            }
            if (EnableSequentialExecution != null) {
                writer.WritePropertyName("enableSequentialExecution");
                writer.Write(bool.Parse(EnableSequentialExecution.ToString()));
            }
            if (DistributorNamespaceId != null) {
                writer.WritePropertyName("distributorNamespaceId");
                writer.Write(DistributorNamespaceId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (QueueNamespaceId != null) {
                writer.WritePropertyName("queueNamespaceId");
                writer.Write(QueueNamespaceId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TransactionSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type TransactionSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(EnableAutoRun, other.EnableAutoRun);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableAtomicCommit, other.EnableAtomicCommit);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TransactionUseDistributor, other.TransactionUseDistributor);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CommitScriptResultInUseDistributor, other.CommitScriptResultInUseDistributor);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AcquireActionUseJobQueue, other.AcquireActionUseJobQueue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableSequentialExecution, other.EnableSequentialExecution);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(DistributorNamespaceId, other.DistributorNamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(KeyId, other.KeyId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(QueueNamespaceId, other.QueueNamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
            }
            if (EnableAutoRun == true) {
            }
            if (EnableAtomicCommit == true) {
            }
            if (TransactionUseDistributor == true) {
            }
            if (EnableAtomicCommit == true) {
            }
            if (EnableAtomicCommit == true) {
            }
            {
                if (DistributorNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionSetting", "enchant.transactionSetting.distributorNamespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (QueueNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionSetting", "enchant.transactionSetting.queueNamespaceId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new TransactionSetting {
                EnableAutoRun = EnableAutoRun,
                EnableAtomicCommit = EnableAtomicCommit,
                TransactionUseDistributor = TransactionUseDistributor,
                CommitScriptResultInUseDistributor = CommitScriptResultInUseDistributor,
                AcquireActionUseJobQueue = AcquireActionUseJobQueue,
                EnableSequentialExecution = EnableSequentialExecution,
                DistributorNamespaceId = DistributorNamespaceId,
                KeyId = KeyId,
                QueueNamespaceId = QueueNamespaceId,
            };
        }
    }
}