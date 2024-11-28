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
using Gs2.Gs2Distributor.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Distributor.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class IfExpressionByUserIdRequest : Gs2Request<IfExpressionByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public Gs2.Core.Model.VerifyAction Condition { set; get; } = null!;
         public Gs2.Core.Model.ConsumeAction[] TrueActions { set; get; } = null!;
         public Gs2.Core.Model.ConsumeAction[] FalseActions { set; get; } = null!;
         public bool? MultiplyValueSpecifyingQuantity { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public IfExpressionByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public IfExpressionByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public IfExpressionByUserIdRequest WithCondition(Gs2.Core.Model.VerifyAction condition) {
            this.Condition = condition;
            return this;
        }
        public IfExpressionByUserIdRequest WithTrueActions(Gs2.Core.Model.ConsumeAction[] trueActions) {
            this.TrueActions = trueActions;
            return this;
        }
        public IfExpressionByUserIdRequest WithFalseActions(Gs2.Core.Model.ConsumeAction[] falseActions) {
            this.FalseActions = falseActions;
            return this;
        }
        public IfExpressionByUserIdRequest WithMultiplyValueSpecifyingQuantity(bool? multiplyValueSpecifyingQuantity) {
            this.MultiplyValueSpecifyingQuantity = multiplyValueSpecifyingQuantity;
            return this;
        }
        public IfExpressionByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public IfExpressionByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IfExpressionByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IfExpressionByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCondition(!data.Keys.Contains("condition") || data["condition"] == null ? null : Gs2.Core.Model.VerifyAction.FromJson(data["condition"]))
                .WithTrueActions(!data.Keys.Contains("trueActions") || data["trueActions"] == null || !data["trueActions"].IsArray ? null : data["trueActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithFalseActions(!data.Keys.Contains("falseActions") || data["falseActions"] == null || !data["falseActions"].IsArray ? null : data["falseActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithMultiplyValueSpecifyingQuantity(!data.Keys.Contains("multiplyValueSpecifyingQuantity") || data["multiplyValueSpecifyingQuantity"] == null ? null : (bool?)bool.Parse(data["multiplyValueSpecifyingQuantity"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData trueActionsJsonData = null;
            if (TrueActions != null && TrueActions.Length > 0)
            {
                trueActionsJsonData = new JsonData();
                foreach (var trueAction in TrueActions)
                {
                    trueActionsJsonData.Add(trueAction.ToJson());
                }
            }
            JsonData falseActionsJsonData = null;
            if (FalseActions != null && FalseActions.Length > 0)
            {
                falseActionsJsonData = new JsonData();
                foreach (var falseAction in FalseActions)
                {
                    falseActionsJsonData.Add(falseAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["condition"] = Condition?.ToJson(),
                ["trueActions"] = trueActionsJsonData,
                ["falseActions"] = falseActionsJsonData,
                ["multiplyValueSpecifyingQuantity"] = MultiplyValueSpecifyingQuantity,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Condition != null) {
                Condition.WriteJson(writer);
            }
            if (TrueActions != null) {
                writer.WritePropertyName("trueActions");
                writer.WriteArrayStart();
                foreach (var trueAction in TrueActions)
                {
                    if (trueAction != null) {
                        trueAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (FalseActions != null) {
                writer.WritePropertyName("falseActions");
                writer.WriteArrayStart();
                foreach (var falseAction in FalseActions)
                {
                    if (falseAction != null) {
                        falseAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (MultiplyValueSpecifyingQuantity != null) {
                writer.WritePropertyName("multiplyValueSpecifyingQuantity");
                writer.Write(bool.Parse(MultiplyValueSpecifyingQuantity.ToString()));
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
            key += UserId + ":";
            key += Condition + ":";
            key += TrueActions + ":";
            key += FalseActions + ":";
            key += MultiplyValueSpecifyingQuantity + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}