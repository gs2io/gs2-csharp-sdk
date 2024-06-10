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
	public class RaiseMaxValueByUserIdRequest : Gs2Request<RaiseMaxValueByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string StaminaName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? RaiseValue { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public RaiseMaxValueByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RaiseMaxValueByUserIdRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }
        public RaiseMaxValueByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public RaiseMaxValueByUserIdRequest WithRaiseValue(int? raiseValue) {
            this.RaiseValue = raiseValue;
            return this;
        }
        public RaiseMaxValueByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public RaiseMaxValueByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RaiseMaxValueByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RaiseMaxValueByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRaiseValue(!data.Keys.Contains("raiseValue") || data["raiseValue"] == null ? null : (int?)(data["raiseValue"].ToString().Contains(".") ? (int)double.Parse(data["raiseValue"].ToString()) : int.Parse(data["raiseValue"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["staminaName"] = StaminaName,
                ["userId"] = UserId,
                ["raiseValue"] = RaiseValue,
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
            if (StaminaName != null) {
                writer.WritePropertyName("staminaName");
                writer.Write(StaminaName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RaiseValue != null) {
                writer.WritePropertyName("raiseValue");
                writer.Write((RaiseValue.ToString().Contains(".") ? (int)double.Parse(RaiseValue.ToString()) : int.Parse(RaiseValue.ToString())));
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
            key += StaminaName + ":";
            key += UserId + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}