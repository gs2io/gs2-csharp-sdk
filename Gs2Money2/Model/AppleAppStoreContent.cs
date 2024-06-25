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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AppleAppStoreContent : IComparable
	{
        public string ProductId { set; get; } = null!;
        public AppleAppStoreContent WithProductId(string productId) {
            this.ProductId = productId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AppleAppStoreContent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AppleAppStoreContent()
                .WithProductId(!data.Keys.Contains("productId") || data["productId"] == null ? null : data["productId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["productId"] = ProductId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ProductId != null) {
                writer.WritePropertyName("productId");
                writer.Write(ProductId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AppleAppStoreContent;
            var diff = 0;
            if (ProductId == null && ProductId == other.ProductId)
            {
                // null and null
            }
            else
            {
                diff += ProductId.CompareTo(other.ProductId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ProductId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appleAppStoreContent", "money2.appleAppStoreContent.productId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new AppleAppStoreContent {
                ProductId = ProductId,
            };
        }
    }
}