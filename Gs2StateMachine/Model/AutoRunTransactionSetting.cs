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

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AutoRunTransactionSetting : IComparable
	{
        public string DistributorNamespaceId { set; get; }
        public string QueueNamespaceId { set; get; }
        public AutoRunTransactionSetting WithDistributorNamespaceId(string distributorNamespaceId) {
            this.DistributorNamespaceId = distributorNamespaceId;
            return this;
        }
        public AutoRunTransactionSetting WithQueueNamespaceId(string queueNamespaceId) {
            this.QueueNamespaceId = queueNamespaceId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AutoRunTransactionSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AutoRunTransactionSetting()
                .WithDistributorNamespaceId(!data.Keys.Contains("distributorNamespaceId") || data["distributorNamespaceId"] == null ? null : data["distributorNamespaceId"].ToString())
                .WithQueueNamespaceId(!data.Keys.Contains("queueNamespaceId") || data["queueNamespaceId"] == null ? null : data["queueNamespaceId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["distributorNamespaceId"] = DistributorNamespaceId,
                ["queueNamespaceId"] = QueueNamespaceId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            var other = obj as AutoRunTransactionSetting;
            var diff = 0;
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
    }
}