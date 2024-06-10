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
using Gs2.Gs2Realtime.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Realtime.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class NowResult : IResult
	{
        public long? Timestamp { set; get; } = null!;

        public NowResult WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static NowResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new NowResult()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : (long?)(data["timestamp"].ToString().Contains(".") ? (long)double.Parse(data["timestamp"].ToString()) : long.Parse(data["timestamp"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["timestamp"] = Timestamp,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            writer.WriteObjectEnd();
        }
    }
}