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
using Gs2.Gs2Ranking2.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking2.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AddSubscribeRequest : Gs2Request<AddSubscribeRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RankingName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string TargetUserId { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public AddSubscribeRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public AddSubscribeRequest WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public AddSubscribeRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public AddSubscribeRequest WithTargetUserId(string targetUserId) {
            this.TargetUserId = targetUserId;
            return this;
        }

        public AddSubscribeRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AddSubscribeRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AddSubscribeRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTargetUserId(!data.Keys.Contains("targetUserId") || data["targetUserId"] == null ? null : data["targetUserId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["rankingName"] = RankingName,
                ["accessToken"] = AccessToken,
                ["targetUserId"] = TargetUserId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RankingName != null) {
                writer.WritePropertyName("rankingName");
                writer.Write(RankingName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (TargetUserId != null) {
                writer.WritePropertyName("targetUserId");
                writer.Write(TargetUserId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RankingName + ":";
            key += AccessToken + ":";
            key += TargetUserId + ":";
            return key;
        }
    }
}