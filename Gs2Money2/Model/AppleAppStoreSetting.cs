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
	public partial class AppleAppStoreSetting : IComparable
	{
        public string BundleId { set; get; }
        public string SharedSecretKey { set; get; }
        public string IssuerId { set; get; }
        public string KeyId { set; get; }
        public string PrivateKeyPem { set; get; }
        public AppleAppStoreSetting WithBundleId(string bundleId) {
            this.BundleId = bundleId;
            return this;
        }
        public AppleAppStoreSetting WithSharedSecretKey(string sharedSecretKey) {
            this.SharedSecretKey = sharedSecretKey;
            return this;
        }
        public AppleAppStoreSetting WithIssuerId(string issuerId) {
            this.IssuerId = issuerId;
            return this;
        }
        public AppleAppStoreSetting WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }
        public AppleAppStoreSetting WithPrivateKeyPem(string privateKeyPem) {
            this.PrivateKeyPem = privateKeyPem;
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
                .WithBundleId(!data.Keys.Contains("bundleId") || data["bundleId"] == null ? null : data["bundleId"].ToString())
                .WithSharedSecretKey(!data.Keys.Contains("sharedSecretKey") || data["sharedSecretKey"] == null ? null : data["sharedSecretKey"].ToString())
                .WithIssuerId(!data.Keys.Contains("issuerId") || data["issuerId"] == null ? null : data["issuerId"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString())
                .WithPrivateKeyPem(!data.Keys.Contains("privateKeyPem") || data["privateKeyPem"] == null ? null : data["privateKeyPem"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["bundleId"] = BundleId,
                ["sharedSecretKey"] = SharedSecretKey,
                ["issuerId"] = IssuerId,
                ["keyId"] = KeyId,
                ["privateKeyPem"] = PrivateKeyPem,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BundleId != null) {
                writer.WritePropertyName("bundleId");
                writer.Write(BundleId.ToString());
            }
            if (SharedSecretKey != null) {
                writer.WritePropertyName("sharedSecretKey");
                writer.Write(SharedSecretKey.ToString());
            }
            if (IssuerId != null) {
                writer.WritePropertyName("issuerId");
                writer.Write(IssuerId.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            if (PrivateKeyPem != null) {
                writer.WritePropertyName("privateKeyPem");
                writer.Write(PrivateKeyPem.ToString());
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
            if (SharedSecretKey == null && SharedSecretKey == other.SharedSecretKey)
            {
                // null and null
            }
            else
            {
                diff += SharedSecretKey.CompareTo(other.SharedSecretKey);
            }
            if (IssuerId == null && IssuerId == other.IssuerId)
            {
                // null and null
            }
            else
            {
                diff += IssuerId.CompareTo(other.IssuerId);
            }
            if (KeyId == null && KeyId == other.KeyId)
            {
                // null and null
            }
            else
            {
                diff += KeyId.CompareTo(other.KeyId);
            }
            if (PrivateKeyPem == null && PrivateKeyPem == other.PrivateKeyPem)
            {
                // null and null
            }
            else
            {
                diff += PrivateKeyPem.CompareTo(other.PrivateKeyPem);
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
            {
                if (SharedSecretKey.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appleAppStoreSetting", "money2.appleAppStoreSetting.sharedSecretKey.error.tooLong"),
                    });
                }
            }
            {
                if (IssuerId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appleAppStoreSetting", "money2.appleAppStoreSetting.issuerId.error.tooLong"),
                    });
                }
            }
            {
                if (KeyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appleAppStoreSetting", "money2.appleAppStoreSetting.keyId.error.tooLong"),
                    });
                }
            }
            {
                if (PrivateKeyPem.Length > 10240) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appleAppStoreSetting", "money2.appleAppStoreSetting.privateKeyPem.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new AppleAppStoreSetting {
                BundleId = BundleId,
                SharedSecretKey = SharedSecretKey,
                IssuerId = IssuerId,
                KeyId = KeyId,
                PrivateKeyPem = PrivateKeyPem,
            };
        }
    }
}