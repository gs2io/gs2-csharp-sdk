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
	public class MarkReceivedByUserIdRequest : Gs2Request<MarkReceivedByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string BonusModelName { set; get; }
        public string UserId { set; get; }
        public int? StepNumber { set; get; }
        public string DuplicationAvoider { set; get; }

        public MarkReceivedByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public MarkReceivedByUserIdRequest WithBonusModelName(string bonusModelName) {
            this.BonusModelName = bonusModelName;
            return this;
        }

        public MarkReceivedByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public MarkReceivedByUserIdRequest WithStepNumber(int? stepNumber) {
            this.StepNumber = stepNumber;
            return this;
        }

        public MarkReceivedByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MarkReceivedByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MarkReceivedByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithBonusModelName(!data.Keys.Contains("bonusModelName") || data["bonusModelName"] == null ? null : data["bonusModelName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithStepNumber(!data.Keys.Contains("stepNumber") || data["stepNumber"] == null ? null : (int?)(data["stepNumber"].ToString().Contains(".") ? (int)double.Parse(data["stepNumber"].ToString()) : int.Parse(data["stepNumber"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["bonusModelName"] = BonusModelName,
                ["userId"] = UserId,
                ["stepNumber"] = StepNumber,
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += BonusModelName + ":";
            key += UserId + ":";
            key += StepNumber + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new MarkReceivedByUserIdRequest {
                NamespaceName = NamespaceName,
                BonusModelName = BonusModelName,
                UserId = UserId,
                StepNumber = StepNumber,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (MarkReceivedByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values MarkReceivedByUserIdRequest::namespaceName");
            }
            if (BonusModelName != y.BonusModelName) {
                throw new ArithmeticException("mismatch parameter values MarkReceivedByUserIdRequest::bonusModelName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values MarkReceivedByUserIdRequest::userId");
            }
            if (StepNumber != y.StepNumber) {
                throw new ArithmeticException("mismatch parameter values MarkReceivedByUserIdRequest::stepNumber");
            }
            return new MarkReceivedByUserIdRequest {
                NamespaceName = NamespaceName,
                BonusModelName = BonusModelName,
                UserId = UserId,
                StepNumber = StepNumber,
            };
        }
    }
}