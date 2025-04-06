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
using Gs2.Gs2Script.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Script.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class InvokeScriptRequest : Gs2Request<InvokeScriptRequest>
	{
         public string ScriptId { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string Args { set; get; } = null!;
         public Gs2.Gs2Script.Model.RandomStatus RandomStatus { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public InvokeScriptRequest WithScriptId(string scriptId) {
            this.ScriptId = scriptId;
            return this;
        }
        public InvokeScriptRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public InvokeScriptRequest WithArgs(string args) {
            this.Args = args;
            return this;
        }
        public InvokeScriptRequest WithRandomStatus(Gs2.Gs2Script.Model.RandomStatus randomStatus) {
            this.RandomStatus = randomStatus;
            return this;
        }
        public InvokeScriptRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public InvokeScriptRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static InvokeScriptRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new InvokeScriptRequest()
                .WithScriptId(!data.Keys.Contains("scriptId") || data["scriptId"] == null ? null : data["scriptId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithRandomStatus(!data.Keys.Contains("randomStatus") || data["randomStatus"] == null ? null : Gs2.Gs2Script.Model.RandomStatus.FromJson(data["randomStatus"]))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["scriptId"] = ScriptId,
                ["userId"] = UserId,
                ["args"] = Args,
                ["randomStatus"] = RandomStatus?.ToJson(),
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ScriptId != null) {
                writer.WritePropertyName("scriptId");
                writer.Write(ScriptId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            if (RandomStatus != null) {
                RandomStatus.WriteJson(writer);
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += ScriptId + ":";
            key += UserId + ":";
            key += Args + ":";
            key += RandomStatus + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}