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
using Gs2.Gs2Stamina.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Stamina.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DecreaseMaxValueByStampTaskRequest : Gs2Request<DecreaseMaxValueByStampTaskRequest>
	{
         public string StampTask { set; get; } = null!;
         public string KeyId { set; get; } = null!;
        public DecreaseMaxValueByStampTaskRequest WithStampTask(string stampTask) {
            this.StampTask = stampTask;
            return this;
        }
        public DecreaseMaxValueByStampTaskRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DecreaseMaxValueByStampTaskRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DecreaseMaxValueByStampTaskRequest()
                .WithStampTask(!data.Keys.Contains("stampTask") || data["stampTask"] == null ? null : data["stampTask"].ToString())
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["stampTask"] = StampTask,
                ["keyId"] = KeyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StampTask != null) {
                writer.WritePropertyName("stampTask");
                writer.Write(StampTask.ToString());
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += StampTask + ":";
            key += KeyId + ":";
            return key;
        }
    }
}