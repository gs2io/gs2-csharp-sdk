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

namespace Gs2.Gs2Dictionary.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class TransactionSettingV2 : IComparable
	{
        public string DistributorNamespaceId { set; get; }
        public bool? EnableParallelExecution { set; get; }
        public TransactionSettingV2 WithDistributorNamespaceId(string distributorNamespaceId) {
            this.DistributorNamespaceId = distributorNamespaceId;
            return this;
        }
        public TransactionSettingV2 WithEnableParallelExecution(bool? enableParallelExecution) {
            this.EnableParallelExecution = enableParallelExecution;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TransactionSettingV2 FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TransactionSettingV2()
                .WithDistributorNamespaceId(!data.Keys.Contains("distributorNamespaceId") || data["distributorNamespaceId"] == null ? null : data["distributorNamespaceId"].ToString())
                .WithEnableParallelExecution(!data.Keys.Contains("enableParallelExecution") || data["enableParallelExecution"] == null ? null : (bool?)bool.Parse(data["enableParallelExecution"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["distributorNamespaceId"] = DistributorNamespaceId,
                ["enableParallelExecution"] = EnableParallelExecution,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DistributorNamespaceId != null) {
                writer.WritePropertyName("distributorNamespaceId");
                writer.Write(DistributorNamespaceId.ToString());
            }
            if (EnableParallelExecution != null) {
                writer.WritePropertyName("enableParallelExecution");
                writer.Write(bool.Parse(EnableParallelExecution.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TransactionSettingV2;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type TransactionSettingV2.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(DistributorNamespaceId, other.DistributorNamespaceId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableParallelExecution, other.EnableParallelExecution);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (DistributorNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("transactionSettingV2", "dictionary.transactionSettingV2.distributorNamespaceId.error.tooLong"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new TransactionSettingV2 {
                DistributorNamespaceId = DistributorNamespaceId,
                EnableParallelExecution = EnableParallelExecution,
            };
        }
    }
}