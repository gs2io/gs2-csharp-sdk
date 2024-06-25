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
	public class AppleAppStoreSetting : IComparable
	{
        public string BundleId { set; get; } = null!;
        public AppleAppStoreSetting WithBundleId(string bundleId) {
            this.BundleId = bundleId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AppleAppStoreSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AppleAppStoreSetting()
                .WithBundleId(!data.Keys.Contains("bundleId") || data["bundleId"] == null ? null : data["bundleId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["bundleId"] = BundleId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BundleId != null) {
                writer.WritePropertyName("bundleId");
                writer.Write(BundleId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AppleAppStoreSetting;
            var diff = 0;
            if (BundleId == null && BundleId == other.BundleId)
            {
                // null and null
            }
            else
            {
                diff += BundleId.CompareTo(other.BundleId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (BundleId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appleAppStoreSetting", "money2.appleAppStoreSetting.bundleId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new AppleAppStoreSetting {
                BundleId = BundleId,
            };
        }
    }
}