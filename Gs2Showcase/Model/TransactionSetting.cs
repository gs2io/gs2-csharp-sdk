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

namespace Gs2.Gs2Showcase.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class TransactionSetting : IComparable
	{
        public bool? EnableAutoRun { set; get; } = null!;
        public bool? EnableAtomicCommit { set; get; } = null!;
        public bool? TransactionUseDistributor { set; get; } = null!;
        public bool? AcquireActionUseJobQueue { set; get; } = null!;
        public string DistributorNamespaceId { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public string KeyId { set; get; } = null!;
        public string QueueNamespaceId { set; get; } = null!;
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
        public TransactionSetting WithAcquireActionUseJobQueue(bool? acquireActionUseJobQueue) {
            this.AcquireActionUseJobQueue = acquireActionUseJobQueue;
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
                .WithAcquireActionUseJobQueue(!data.Keys.Contains("acquireActionUseJobQueue") || data["acquireActionUseJobQueue"] == null ? null : (bool?)bool.Parse(data["acquireActionUseJobQueue"].ToString()))
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
                ["acquireActionUseJobQueue"] = AcquireActionUseJobQueue,
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
            if (AcquireActionUseJobQueue != null) {
                writer.WritePropertyName("acquireActionUseJobQueue");
                writer.Write(bool.Parse(AcquireActionUseJobQueue.ToString()));
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
            var diff = 0;
            if (EnableAutoRun == null && EnableAutoRun == other.EnableAutoRun)
            {
                // null and null
            }
            else
            {
                diff += EnableAutoRun == other.EnableAutoRun ? 0 : 1;
            }
            if (EnableAtomicCommit == null && EnableAtomicCommit == other.EnableAtomicCommit)
            {
                // null and null
            }
            else
            {
                diff += EnableAtomicCommit == other.EnableAtomicCommit ? 0 : 1;
            }
            if (TransactionUseDistributor == null && TransactionUseDistributor == other.TransactionUseDistributor)
            {
                // null and null
            }
            else
            {
                diff += TransactionUseDistributor == other.TransactionUseDistributor ? 0 : 1;
            }
            if (AcquireActionUseJobQueue == null && AcquireActionUseJobQueue == other.AcquireActionUseJobQueue)
            {
                // null and null
            }
            else
            {
                diff += AcquireActionUseJobQueue == other.AcquireActionUseJobQueue ? 0 : 1;
            }
            if (DistributorNamespaceId == null && DistributorNamespaceId == other.DistributorNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += DistributorNamespaceId.CompareTo(other.DistributorNamespaceId);
            }
            if (KeyId == null && KeyId == other.KeyId)
            {
                // null and null
            }
            else
            {
                diff += KeyId.CompareTo(other.KeyId);
            }
            if (QueueNamespaceId == null && QueueNamespaceId == other.QueueNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += QueueNamespaceId.CompareTo(other.QueueNamespaceId);
            }
            return diff;
        }

        public void Validate() {
            {
            }
            if (EnableAutoRun == true) {
            }
            if (EnableAtomicCommit == true) {
            }
            if (EnableAtomicCommit == true) {
            }
            {
                if (DistributorNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionSetting", "showcase.transactionSetting.distributorNamespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (QueueNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionSetting", "showcase.transactionSetting.queueNamespaceId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new TransactionSetting {
                EnableAutoRun = EnableAutoRun,
                EnableAtomicCommit = EnableAtomicCommit,
                TransactionUseDistributor = TransactionUseDistributor,
                AcquireActionUseJobQueue = AcquireActionUseJobQueue,
                DistributorNamespaceId = DistributorNamespaceId,
                KeyId = KeyId,
                QueueNamespaceId = QueueNamespaceId,
            };
        }
    }
}