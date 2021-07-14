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
using Gs2.Gs2Key.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Key.Request
{
	[Preserve]
	[System.Serializable]
	public class DeleteKeyRequest : Gs2Request<DeleteKeyRequest>
	{
        public string NamespaceName { set; get; }
        public string KeyName { set; get; }

        public DeleteKeyRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DeleteKeyRequest WithKeyName(string keyName) {
            this.KeyName = keyName;
            return this;
        }

    	[Preserve]
        public static DeleteKeyRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteKeyRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithKeyName(!data.Keys.Contains("keyName") || data["keyName"] == null ? null : data["keyName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["keyName"] = KeyName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (KeyName != null) {
                writer.WritePropertyName("keyName");
                writer.Write(KeyName.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}