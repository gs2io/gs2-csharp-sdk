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
using Gs2.Gs2LoginReward.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2LoginReward.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class MissedReceiveByUserIdRequest : Gs2Request<MissedReceiveByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string BonusModelName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? StepNumber { set; get; } = null!;
         public Gs2.Gs2LoginReward.Model.Config[] Config { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public MissedReceiveByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public MissedReceiveByUserIdRequest WithBonusModelName(string bonusModelName) {
            this.BonusModelName = bonusModelName;
            return this;
        }
        public MissedReceiveByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public MissedReceiveByUserIdRequest WithStepNumber(int? stepNumber) {
            this.StepNumber = stepNumber;
            return this;
        }
        public MissedReceiveByUserIdRequest WithConfig(Gs2.Gs2LoginReward.Model.Config[] config) {
            this.Config = config;
            return this;
        }
        public MissedReceiveByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public MissedReceiveByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MissedReceiveByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MissedReceiveByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithBonusModelName(!data.Keys.Contains("bonusModelName") || data["bonusModelName"] == null ? null : data["bonusModelName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithStepNumber(!data.Keys.Contains("stepNumber") || data["stepNumber"] == null ? null : (int?)(data["stepNumber"].ToString().Contains(".") ? (int)double.Parse(data["stepNumber"].ToString()) : int.Parse(data["stepNumber"].ToString())))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? null : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2LoginReward.Model.Config.FromJson(v);
                }).ToArray())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData configJsonData = null;
            if (Config != null && Config.Length > 0)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["bonusModelName"] = BonusModelName,
                ["userId"] = UserId,
                ["stepNumber"] = StepNumber,
                ["config"] = configJsonData,
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
            if (BonusModelName != null) {
                writer.WritePropertyName("bonusModelName");
                writer.Write(BonusModelName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (StepNumber != null) {
                writer.WritePropertyName("stepNumber");
                writer.Write((StepNumber.ToString().Contains(".") ? (int)double.Parse(StepNumber.ToString()) : int.Parse(StepNumber.ToString())));
            }
            if (Config != null) {
                writer.WritePropertyName("config");
                writer.WriteArrayStart();
                foreach (var confi in Config)
                {
                    if (confi != null) {
                        confi.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            key += BonusModelName + ":";
            key += UserId + ":";
            key += StepNumber + ":";
            key += Config + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}