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
	public class RunVerifyTaskWithoutNamespaceRequest : Gs2Request<RunVerifyTaskWithoutNamespaceRequest>
	{
         public string VerifyTask { set; get; } = null!;
         public string KeyId { set; get; } = null!;
        public RunVerifyTaskWithoutNamespaceRequest WithVerifyTask(string verifyTask) {
            this.VerifyTask = verifyTask;
            return this;
        }
        public RunVerifyTaskWithoutNamespaceRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RunVerifyTaskWithoutNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunVerifyTaskWithoutNamespaceRequest()
                .WithVerifyTask(!data.Keys.Contains("verifyTask") || data["verifyTask"] == null ? null : data["verifyTask"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["verifyTask"] = VerifyTask,
                ["keyId"] = KeyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            key += VerifyTask + ":";
            key += KeyId + ":";
            return key;
        }
    }
}