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
	public class UpdateStaminaByUserIdRequest : Gs2Request<UpdateStaminaByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string StaminaName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? Value { set; get; } = null!;
         public int? MaxValue { set; get; } = null!;
         public int? RecoverIntervalMinutes { set; get; } = null!;
         public int? RecoverValue { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public UpdateStaminaByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateStaminaByUserIdRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }
        public UpdateStaminaByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public UpdateStaminaByUserIdRequest WithValue(int? value) {
            this.Value = value;
            return this;
        }
        public UpdateStaminaByUserIdRequest WithMaxValue(int? maxValue) {
            this.MaxValue = maxValue;
            return this;
        }
        public UpdateStaminaByUserIdRequest WithRecoverIntervalMinutes(int? recoverIntervalMinutes) {
            this.RecoverIntervalMinutes = recoverIntervalMinutes;
            return this;
        }
        public UpdateStaminaByUserIdRequest WithRecoverValue(int? recoverValue) {
            this.RecoverValue = recoverValue;
            return this;
        }
        public UpdateStaminaByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public UpdateStaminaByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateStaminaByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateStaminaByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (int?)(data["value"].ToString().Contains(".") ? (int)double.Parse(data["value"].ToString()) : int.Parse(data["value"].ToString())))
                .WithMaxValue(!data.Keys.Contains("maxValue") || data["maxValue"] == null ? null : (int?)(data["maxValue"].ToString().Contains(".") ? (int)double.Parse(data["maxValue"].ToString()) : int.Parse(data["maxValue"].ToString())))
                .WithRecoverIntervalMinutes(!data.Keys.Contains("recoverIntervalMinutes") || data["recoverIntervalMinutes"] == null ? null : (int?)(data["recoverIntervalMinutes"].ToString().Contains(".") ? (int)double.Parse(data["recoverIntervalMinutes"].ToString()) : int.Parse(data["recoverIntervalMinutes"].ToString())))
                .WithRecoverValue(!data.Keys.Contains("recoverValue") || data["recoverValue"] == null ? null : (int?)(data["recoverValue"].ToString().Contains(".") ? (int)double.Parse(data["recoverValue"].ToString()) : int.Parse(data["recoverValue"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["staminaName"] = StaminaName,
                ["userId"] = UserId,
                ["value"] = Value,
                ["maxValue"] = MaxValue,
                ["recoverIntervalMinutes"] = RecoverIntervalMinutes,
                ["recoverValue"] = RecoverValue,
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
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (int)double.Parse(Value.ToString()) : int.Parse(Value.ToString())));
            }
            if (MaxValue != null) {
                writer.WritePropertyName("maxValue");
                writer.Write((MaxValue.ToString().Contains(".") ? (int)double.Parse(MaxValue.ToString()) : int.Parse(MaxValue.ToString())));
            }
            if (RecoverIntervalMinutes != null) {
                writer.WritePropertyName("recoverIntervalMinutes");
                writer.Write((RecoverIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(RecoverIntervalMinutes.ToString()) : int.Parse(RecoverIntervalMinutes.ToString())));
            }
            if (RecoverValue != null) {
                writer.WritePropertyName("recoverValue");
                writer.Write((RecoverValue.ToString().Contains(".") ? (int)double.Parse(RecoverValue.ToString()) : int.Parse(RecoverValue.ToString())));
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
            key += Value + ":";
            key += MaxValue + ":";
            key += RecoverIntervalMinutes + ":";
            key += RecoverValue + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}