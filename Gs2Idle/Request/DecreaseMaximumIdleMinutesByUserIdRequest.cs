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
using Gs2.Gs2Idle.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Idle.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DecreaseMaximumIdleMinutesByUserIdRequest : Gs2Request<DecreaseMaximumIdleMinutesByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string CategoryName { set; get; } = null!;
         public int? DecreaseMinutes { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public DecreaseMaximumIdleMinutesByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DecreaseMaximumIdleMinutesByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DecreaseMaximumIdleMinutesByUserIdRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }
        public DecreaseMaximumIdleMinutesByUserIdRequest WithDecreaseMinutes(int? decreaseMinutes) {
            this.DecreaseMinutes = decreaseMinutes;
            return this;
        }
        public DecreaseMaximumIdleMinutesByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public DecreaseMaximumIdleMinutesByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DecreaseMaximumIdleMinutesByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DecreaseMaximumIdleMinutesByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithDecreaseMinutes(!data.Keys.Contains("decreaseMinutes") || data["decreaseMinutes"] == null ? null : (int?)(data["decreaseMinutes"].ToString().Contains(".") ? (int)double.Parse(data["decreaseMinutes"].ToString()) : int.Parse(data["decreaseMinutes"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["categoryName"] = CategoryName,
                ["decreaseMinutes"] = DecreaseMinutes,
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
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (DecreaseMinutes != null) {
                writer.WritePropertyName("decreaseMinutes");
                writer.Write((DecreaseMinutes.ToString().Contains(".") ? (int)double.Parse(DecreaseMinutes.ToString()) : int.Parse(DecreaseMinutes.ToString())));
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
            key += CategoryName + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}