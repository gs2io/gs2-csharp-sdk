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
using Gs2.Gs2Quest.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Quest.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class EndByUserIdRequest : Gs2Request<EndByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string TransactionId { set; get; }
        public Gs2.Gs2Quest.Model.Reward[] Rewards { set; get; }
        public bool? IsComplete { set; get; }
        public Gs2.Gs2Quest.Model.Config[] Config { set; get; }
        public string DuplicationAvoider { set; get; }

        public EndByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public EndByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public EndByUserIdRequest WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }

        public EndByUserIdRequest WithRewards(Gs2.Gs2Quest.Model.Reward[] rewards) {
            this.Rewards = rewards;
            return this;
        }

        public EndByUserIdRequest WithIsComplete(bool? isComplete) {
            this.IsComplete = isComplete;
            return this;
        }

        public EndByUserIdRequest WithConfig(Gs2.Gs2Quest.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public EndByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static EndByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new EndByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithRewards(!data.Keys.Contains("rewards") || data["rewards"] == null ? new Gs2.Gs2Quest.Model.Reward[]{} : data["rewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Reward.FromJson(v);
                }).ToArray())
                .WithIsComplete(!data.Keys.Contains("isComplete") || data["isComplete"] == null ? null : (bool?)bool.Parse(data["isComplete"].ToString()))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2Quest.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Config.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["transactionId"] = TransactionId,
                ["rewards"] = new JsonData(Rewards == null ? new JsonData[]{} :
                        Rewards.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["isComplete"] = IsComplete,
                ["config"] = new JsonData(Config == null ? new JsonData[]{} :
                        Config.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var reward in Rewards)
            {
                if (reward != null) {
                    reward.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (IsComplete != null) {
                writer.WritePropertyName("isComplete");
                writer.Write(bool.Parse(IsComplete.ToString()));
            }
            writer.WriteArrayStart();
            foreach (var confi in Config)
            {
                if (confi != null) {
                    confi.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}