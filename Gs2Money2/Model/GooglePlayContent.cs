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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class GooglePlayContent : IComparable
	{
        public string ProductId { set; get; }
        public GooglePlayContent WithProductId(string productId) {
            this.ProductId = productId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GooglePlayContent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GooglePlayContent()
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
            var other = obj as GooglePlayContent;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type GooglePlayContent.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(ProductId, other.ProductId);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (ProductId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("googlePlayContent", "money2.googlePlayContent.productId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GooglePlayContent {
                ProductId = ProductId,
            };
        }
    }
}