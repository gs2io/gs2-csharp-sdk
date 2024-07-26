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
using Gs2.Gs2Distributor.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Distributor.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class RunVerifyTaskRequest : Gs2Request<RunVerifyTaskRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string VerifyTask { set; get; } = null!;
         public string KeyId { set; get; } = null!;
        public RunVerifyTaskRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RunVerifyTaskRequest WithVerifyTask(string verifyTask) {
            this.VerifyTask = verifyTask;
            return this;
        }
        public RunVerifyTaskRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RunVerifyTaskRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunVerifyTaskRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithVerifyTask(!data.Keys.Contains("verifyTask") || data["verifyTask"] == null ? null : data["verifyTask"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["verifyTask"] = VerifyTask,
                ["keyId"] = KeyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (VerifyTask != null) {
                writer.WritePropertyName("verifyTask");
                writer.Write(VerifyTask.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += VerifyTask + ":";
            key += KeyId + ":";
            return key;
        }
    }
}