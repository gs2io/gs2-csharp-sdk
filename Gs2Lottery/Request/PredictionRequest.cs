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
using Gs2.Gs2Lottery.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Lottery.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PredictionRequest : Gs2Request<PredictionRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string LotteryName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public long? RandomSeed { set; get; } = null!;
         public int? Count { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public PredictionRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PredictionRequest WithLotteryName(string lotteryName) {
            this.LotteryName = lotteryName;
            return this;
        }
        public PredictionRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public PredictionRequest WithRandomSeed(long? randomSeed) {
            this.RandomSeed = randomSeed;
            return this;
        }
        public PredictionRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }

        public PredictionRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PredictionRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PredictionRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithLotteryName(!data.Keys.Contains("lotteryName") || data["lotteryName"] == null ? null : data["lotteryName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithRandomSeed(!data.Keys.Contains("randomSeed") || data["randomSeed"] == null ? null : (long?)(data["randomSeed"].ToString().Contains(".") ? (long)double.Parse(data["randomSeed"].ToString()) : long.Parse(data["randomSeed"].ToString())))
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["lotteryName"] = LotteryName,
                ["accessToken"] = AccessToken,
                ["randomSeed"] = RandomSeed,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (LotteryName != null) {
                writer.WritePropertyName("lotteryName");
                writer.Write(LotteryName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (RandomSeed != null) {
                writer.WritePropertyName("randomSeed");
                writer.Write((RandomSeed.ToString().Contains(".") ? (long)double.Parse(RandomSeed.ToString()) : long.Parse(RandomSeed.ToString())));
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += LotteryName + ":";
            key += AccessToken + ":";
            key += RandomSeed + ":";
            key += Count + ":";
            return key;
        }
    }
}