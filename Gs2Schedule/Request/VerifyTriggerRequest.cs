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
	public class VerifyTriggerRequest : Gs2Request<VerifyTriggerRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string TriggerName { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public int? ElapsedMinutes { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyTriggerRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyTriggerRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public VerifyTriggerRequest WithTriggerName(string triggerName) {
            this.TriggerName = triggerName;
            return this;
        }
        public VerifyTriggerRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyTriggerRequest WithElapsedMinutes(int? elapsedMinutes) {
            this.ElapsedMinutes = elapsedMinutes;
            return this;
        }

        public VerifyTriggerRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyTriggerRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyTriggerRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTriggerName(!data.Keys.Contains("triggerName") || data["triggerName"] == null ? null : data["triggerName"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithElapsedMinutes(!data.Keys.Contains("elapsedMinutes") || data["elapsedMinutes"] == null ? null : (int?)(data["elapsedMinutes"].ToString().Contains(".") ? (int)double.Parse(data["elapsedMinutes"].ToString()) : int.Parse(data["elapsedMinutes"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["triggerName"] = TriggerName,
                ["verifyType"] = VerifyType,
                ["elapsedMinutes"] = ElapsedMinutes,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (TriggerName != null) {
                writer.WritePropertyName("triggerName");
                writer.Write(TriggerName.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (ElapsedMinutes != null) {
                writer.WritePropertyName("elapsedMinutes");
                writer.Write((ElapsedMinutes.ToString().Contains(".") ? (int)double.Parse(ElapsedMinutes.ToString()) : int.Parse(ElapsedMinutes.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += TriggerName + ":";
            key += VerifyType + ":";
            key += ElapsedMinutes + ":";
            return key;
        }
    }
}