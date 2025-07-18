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
using Gs2.Gs2Schedule.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Schedule.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ExtendTriggerByUserIdRequest : Gs2Request<ExtendTriggerByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string TriggerName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? ExtendSeconds { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public ExtendTriggerByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ExtendTriggerByUserIdRequest WithTriggerName(string triggerName) {
            this.TriggerName = triggerName;
            return this;
        }
        public ExtendTriggerByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ExtendTriggerByUserIdRequest WithExtendSeconds(int? extendSeconds) {
            this.ExtendSeconds = extendSeconds;
            return this;
        }
        public ExtendTriggerByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public ExtendTriggerByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ExtendTriggerByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ExtendTriggerByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithTriggerName(!data.Keys.Contains("triggerName") || data["triggerName"] == null ? null : data["triggerName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithExtendSeconds(!data.Keys.Contains("extendSeconds") || data["extendSeconds"] == null ? null : (int?)(data["extendSeconds"].ToString().Contains(".") ? (int)double.Parse(data["extendSeconds"].ToString()) : int.Parse(data["extendSeconds"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["triggerName"] = TriggerName,
                ["userId"] = UserId,
                ["extendSeconds"] = ExtendSeconds,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (TriggerName != null) {
                writer.WritePropertyName("triggerName");
                writer.Write(TriggerName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ExtendSeconds != null) {
                writer.WritePropertyName("extendSeconds");
                writer.Write((ExtendSeconds.ToString().Contains(".") ? (int)double.Parse(ExtendSeconds.ToString()) : int.Parse(ExtendSeconds.ToString())));
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += TriggerName + ":";
            key += UserId + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}