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
	public class PredictionByUserIdRequest : Gs2Request<PredictionByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string LotteryName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public long? RandomSeed { set; get; } = null!;
         public int? Count { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public PredictionByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PredictionByUserIdRequest WithLotteryName(string lotteryName) {
            this.LotteryName = lotteryName;
            return this;
        }
        public PredictionByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public PredictionByUserIdRequest WithRandomSeed(long? randomSeed) {
            this.RandomSeed = randomSeed;
            return this;
        }
        public PredictionByUserIdRequest WithCount(int? count) {
            this.Count = count;
            return this;
        }
        public PredictionByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public PredictionByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PredictionByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PredictionByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithLotteryName(!data.Keys.Contains("lotteryName") || data["lotteryName"] == null ? null : data["lotteryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRandomSeed(!data.Keys.Contains("randomSeed") || data["randomSeed"] == null ? null : (long?)(data["randomSeed"].ToString().Contains(".") ? (long)double.Parse(data["randomSeed"].ToString()) : long.Parse(data["randomSeed"].ToString())))
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["lotteryName"] = LotteryName,
                ["userId"] = UserId,
                ["randomSeed"] = RandomSeed,
                ["count"] = Count,
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
            if (LotteryName != null) {
                writer.WritePropertyName("lotteryName");
                writer.Write(LotteryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RandomSeed != null) {
                writer.WritePropertyName("randomSeed");
                writer.Write((RandomSeed.ToString().Contains(".") ? (long)double.Parse(RandomSeed.ToString()) : long.Parse(RandomSeed.ToString())));
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
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
            key += LotteryName + ":";
            key += UserId + ":";
            key += RandomSeed + ":";
            key += Count + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}