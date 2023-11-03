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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Inventory.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inventory.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AcquireSimpleItemsByUserIdRequest : Gs2Request<AcquireSimpleItemsByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string InventoryName { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2Inventory.Model.AcquireCount[] AcquireCounts { set; get; }
        public string DuplicationAvoider { set; get; }

        public AcquireSimpleItemsByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public AcquireSimpleItemsByUserIdRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }

        public AcquireSimpleItemsByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public AcquireSimpleItemsByUserIdRequest WithAcquireCounts(Gs2.Gs2Inventory.Model.AcquireCount[] acquireCounts) {
            this.AcquireCounts = acquireCounts;
            return this;
        }

        public AcquireSimpleItemsByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireSimpleItemsByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireSimpleItemsByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAcquireCounts(!data.Keys.Contains("acquireCounts") || data["acquireCounts"] == null ? new Gs2.Gs2Inventory.Model.AcquireCount[]{} : data["acquireCounts"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inventory.Model.AcquireCount.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData acquireCountsJsonData = null;
            if (AcquireCounts != null)
            {
                acquireCountsJsonData = new JsonData();
                foreach (var acquireCount in AcquireCounts)
                {
                    acquireCountsJsonData.Add(acquireCount.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["acquireCounts"] = acquireCountsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var acquireCount in AcquireCounts)
            {
                if (acquireCount != null) {
                    acquireCount.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += InventoryName + ":";
            key += UserId + ":";
            key += AcquireCounts + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new AcquireSimpleItemsByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                AcquireCounts = AcquireCounts,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (AcquireSimpleItemsByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values AcquireSimpleItemsByUserIdRequest::namespaceName");
            }
            if (InventoryName != y.InventoryName) {
                throw new ArithmeticException("mismatch parameter values AcquireSimpleItemsByUserIdRequest::inventoryName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values AcquireSimpleItemsByUserIdRequest::userId");
            }
            if (AcquireCounts != y.AcquireCounts) {
                throw new ArithmeticException("mismatch parameter values AcquireSimpleItemsByUserIdRequest::acquireCounts");
            }
            return new AcquireSimpleItemsByUserIdRequest {
                NamespaceName = NamespaceName,
                InventoryName = InventoryName,
                UserId = UserId,
                AcquireCounts = AcquireCounts,
            };
        }
    }
}