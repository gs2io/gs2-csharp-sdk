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
	public class EndRequest : Gs2Request<EndRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public Gs2.Gs2Quest.Model.Reward[] Rewards { set; get; } = null!;
         public bool? IsComplete { set; get; } = null!;
         public Gs2.Gs2Quest.Model.Config[] Config { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public EndRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public EndRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public EndRequest WithRewards(Gs2.Gs2Quest.Model.Reward[] rewards) {
            this.Rewards = rewards;
            return this;
        }
        public EndRequest WithIsComplete(bool? isComplete) {
            this.IsComplete = isComplete;
            return this;
        }
        public EndRequest WithConfig(Gs2.Gs2Quest.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public EndRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static EndRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new EndRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithRewards(!data.Keys.Contains("rewards") || data["rewards"] == null || !data["rewards"].IsArray ? null : data["rewards"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Reward.FromJson(v);
                }).ToArray())
                .WithIsComplete(!data.Keys.Contains("isComplete") || data["isComplete"] == null ? null : (bool?)bool.Parse(data["isComplete"].ToString()))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? null : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.Config.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData rewardsJsonData = null;
            if (Rewards != null && Rewards.Length > 0)
            {
                rewardsJsonData = new JsonData();
                foreach (var reward in Rewards)
                {
                    rewardsJsonData.Add(reward.ToJson());
                }
            }
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
                ["accessToken"] = AccessToken,
                ["rewards"] = rewardsJsonData,
                ["isComplete"] = IsComplete,
                ["config"] = configJsonData,
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
            if (Rewards != null) {
                writer.WritePropertyName("rewards");
                writer.WriteArrayStart();
                foreach (var reward in Rewards)
                {
                    if (reward != null) {
                        reward.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (IsComplete != null) {
                writer.WritePropertyName("isComplete");
                writer.Write(bool.Parse(IsComplete.ToString()));
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += Rewards + ":";
            key += IsComplete + ":";
            key += Config + ":";
            return key;
        }
    }
}