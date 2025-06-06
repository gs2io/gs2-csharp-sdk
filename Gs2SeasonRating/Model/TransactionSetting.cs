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

namespace Gs2.Gs2SeasonRating.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class TransactionSetting : IComparable
	{
        public bool? EnableAtomicCommit { set; get; }
        public bool? TransactionUseDistributor { set; get; }
        public bool? AcquireActionUseJobQueue { set; get; }
        public string DistributorNamespaceId { set; get; }
        public string QueueNamespaceId { set; get; }
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
                .WithEnableAtomicCommit(!data.Keys.Contains("enableAtomicCommit") || data["enableAtomicCommit"] == null ? null : (bool?)bool.Parse(data["enableAtomicCommit"].ToString()))
                .WithTransactionUseDistributor(!data.Keys.Contains("transactionUseDistributor") || data["transactionUseDistributor"] == null ? null : (bool?)bool.Parse(data["transactionUseDistributor"].ToString()))
                .WithAcquireActionUseJobQueue(!data.Keys.Contains("acquireActionUseJobQueue") || data["acquireActionUseJobQueue"] == null ? null : (bool?)bool.Parse(data["acquireActionUseJobQueue"].ToString()))
                .WithDistributorNamespaceId(!data.Keys.Contains("distributorNamespaceId") || data["distributorNamespaceId"] == null ? null : data["distributorNamespaceId"].ToString())
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
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
            if (EnableAtomicCommit == true) {
            }
            if (EnableAtomicCommit == true) {
            }
            {
                if (DistributorNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionSetting", "seasonRating.transactionSetting.distributorNamespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (QueueNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionSetting", "seasonRating.transactionSetting.queueNamespaceId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new TransactionSetting {
                EnableAtomicCommit = EnableAtomicCommit,
                TransactionUseDistributor = TransactionUseDistributor,
                AcquireActionUseJobQueue = AcquireActionUseJobQueue,
                DistributorNamespaceId = DistributorNamespaceId,
                QueueNamespaceId = QueueNamespaceId,
            };
        }
    }
}