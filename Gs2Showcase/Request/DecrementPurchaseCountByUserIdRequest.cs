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
using Gs2.Gs2Showcase.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Showcase.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DecrementPurchaseCountByUserIdRequest : Gs2Request<DecrementPurchaseCountByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string ShowcaseName { set; get; }
        public string DisplayItemName { set; get; }
        public string UserId { set; get; }
        public int? Count { set; get; }
        public string DuplicationAvoider { set; get; }

        public DecrementPurchaseCountByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DecrementPurchaseCountByUserIdRequest WithShowcaseName(string showcaseName) {
            this.ShowcaseName = showcaseName;
            return this;
        }

        public DecrementPurchaseCountByUserIdRequest WithDisplayItemName(string displayItemName) {
            this.DisplayItemName = displayItemName;
            return this;
        }

        public DecrementPurchaseCountByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public DecrementPurchaseCountByUserIdRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public DecrementPurchaseCountByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DecrementPurchaseCountByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DecrementPurchaseCountByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithShowcaseName(!data.Keys.Contains("showcaseName") || data["showcaseName"] == null ? null : data["showcaseName"].ToString())
                .WithDisplayItemName(!data.Keys.Contains("displayItemName") || data["displayItemName"] == null ? null : data["displayItemName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["showcaseName"] = ShowcaseName,
                ["displayItemName"] = DisplayItemName,
                ["userId"] = UserId,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (ShowcaseName != null) {
                writer.WritePropertyName("showcaseName");
                writer.Write(ShowcaseName.ToString());
            }
            if (DisplayItemName != null) {
                writer.WritePropertyName("displayItemName");
                writer.Write(DisplayItemName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ShowcaseName + ":";
            key += DisplayItemName + ":";
            key += UserId + ":";
            key += Count + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new DecrementPurchaseCountByUserIdRequest {
                NamespaceName = NamespaceName,
                ShowcaseName = ShowcaseName,
                DisplayItemName = DisplayItemName,
                UserId = UserId,
                Count = Count,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (DecrementPurchaseCountByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values DecrementPurchaseCountByUserIdRequest::namespaceName");
            }
            if (ShowcaseName != y.ShowcaseName) {
                throw new ArithmeticException("mismatch parameter values DecrementPurchaseCountByUserIdRequest::showcaseName");
            }
            if (DisplayItemName != y.DisplayItemName) {
                throw new ArithmeticException("mismatch parameter values DecrementPurchaseCountByUserIdRequest::displayItemName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values DecrementPurchaseCountByUserIdRequest::userId");
            }
            if (Count != y.Count) {
                throw new ArithmeticException("mismatch parameter values DecrementPurchaseCountByUserIdRequest::count");
            }
            return new DecrementPurchaseCountByUserIdRequest {
                NamespaceName = NamespaceName,
                ShowcaseName = ShowcaseName,
                DisplayItemName = DisplayItemName,
                UserId = UserId,
                Count = Count,
            };
        }
    }
}