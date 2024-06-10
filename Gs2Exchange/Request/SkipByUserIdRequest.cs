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
using Gs2.Gs2Exchange.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Exchange.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SkipByUserIdRequest : Gs2Request<SkipByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string AwaitName { set; get; } = null!;
         public string SkipType { set; get; } = null!;
         public int? Minutes { set; get; } = null!;
         public float? Rate { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SkipByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SkipByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SkipByUserIdRequest WithAwaitName(string awaitName) {
            this.AwaitName = awaitName;
            return this;
        }
        public SkipByUserIdRequest WithSkipType(string skipType) {
            this.SkipType = skipType;
            return this;
        }
        public SkipByUserIdRequest WithMinutes(int? minutes) {
            this.Minutes = minutes;
            return this;
        }
        public SkipByUserIdRequest WithRate(float? rate) {
            this.Rate = rate;
            return this;
        }
        public SkipByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public SkipByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SkipByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SkipByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAwaitName(!data.Keys.Contains("awaitName") || data["awaitName"] == null ? null : data["awaitName"].ToString())
                .WithSkipType(!data.Keys.Contains("skipType") || data["skipType"] == null ? null : data["skipType"].ToString())
                .WithMinutes(!data.Keys.Contains("minutes") || data["minutes"] == null ? null : (int?)(data["minutes"].ToString().Contains(".") ? (int)double.Parse(data["minutes"].ToString()) : int.Parse(data["minutes"].ToString())))
                .WithRate(!data.Keys.Contains("rate") || data["rate"] == null ? null : (float?)float.Parse(data["rate"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["awaitName"] = AwaitName,
                ["skipType"] = SkipType,
                ["minutes"] = Minutes,
                ["rate"] = Rate,
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
            if (AwaitName != null) {
                writer.WritePropertyName("awaitName");
                writer.Write(AwaitName.ToString());
            }
            if (SkipType != null) {
                writer.WritePropertyName("skipType");
                writer.Write(SkipType.ToString());
            }
            if (Minutes != null) {
                writer.WritePropertyName("minutes");
                writer.Write((Minutes.ToString().Contains(".") ? (int)double.Parse(Minutes.ToString()) : int.Parse(Minutes.ToString())));
            }
            if (Rate != null) {
                writer.WritePropertyName("rate");
                writer.Write(float.Parse(Rate.ToString()));
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
            key += AwaitName + ":";
            key += SkipType + ":";
            key += Rate + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}