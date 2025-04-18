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
	public class DebugInvokeRequest : Gs2Request<DebugInvokeRequest>
	{
         public string Script { set; get; } = null!;
         public string Args { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public Gs2.Gs2Script.Model.RandomStatus RandomStatus { set; get; } = null!;
         public bool? DisableStringNumberToNumber { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public DebugInvokeRequest WithScript(string script) {
            this.Script = script;
            return this;
        }
        public DebugInvokeRequest WithArgs(string args) {
            this.Args = args;
            return this;
        }
        public DebugInvokeRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DebugInvokeRequest WithRandomStatus(Gs2.Gs2Script.Model.RandomStatus randomStatus) {
            this.RandomStatus = randomStatus;
            return this;
        }
        public DebugInvokeRequest WithDisableStringNumberToNumber(bool? disableStringNumberToNumber) {
            this.DisableStringNumberToNumber = disableStringNumberToNumber;
            return this;
        }
        public DebugInvokeRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public DebugInvokeRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DebugInvokeRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DebugInvokeRequest()
                .WithScript(!data.Keys.Contains("script") || data["script"] == null ? null : data["script"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRandomStatus(!data.Keys.Contains("randomStatus") || data["randomStatus"] == null ? null : Gs2.Gs2Script.Model.RandomStatus.FromJson(data["randomStatus"]))
                .WithDisableStringNumberToNumber(!data.Keys.Contains("disableStringNumberToNumber") || data["disableStringNumberToNumber"] == null ? null : (bool?)bool.Parse(data["disableStringNumberToNumber"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["script"] = Script,
                ["args"] = Args,
                ["userId"] = UserId,
                ["randomStatus"] = RandomStatus?.ToJson(),
                ["disableStringNumberToNumber"] = DisableStringNumberToNumber,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Script != null) {
                writer.WritePropertyName("script");
                writer.Write(Script.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RandomStatus != null) {
                RandomStatus.WriteJson(writer);
            }
            if (DisableStringNumberToNumber != null) {
                writer.WritePropertyName("disableStringNumberToNumber");
                writer.Write(bool.Parse(DisableStringNumberToNumber.ToString()));
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Script + ":";
            key += Args + ":";
            key += UserId + ":";
            key += RandomStatus + ":";
            key += DisableStringNumberToNumber + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}