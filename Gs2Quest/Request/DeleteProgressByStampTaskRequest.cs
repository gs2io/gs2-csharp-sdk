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
using Gs2.Gs2Quest.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Quest.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DeleteProgressByStampTaskRequest : Gs2Request<DeleteProgressByStampTaskRequest>
	{
         public string StampTask { set; get; }
         public string KeyId { set; get; }
        public DeleteProgressByStampTaskRequest WithStampTask(string stampTask) {
            this.StampTask = stampTask;
            return this;
        }
        public DeleteProgressByStampTaskRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DeleteProgressByStampTaskRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteProgressByStampTaskRequest()
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

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply DeleteProgressByStampTaskRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (DeleteProgressByStampTaskRequest)x;
            return this;
        }
    }
}