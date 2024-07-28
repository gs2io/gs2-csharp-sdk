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
	public class DistributeRequest : Gs2Request<DistributeRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string DistributorName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public Gs2.Gs2Distributor.Model.DistributeResource DistributeResource { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public DistributeRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DistributeRequest WithDistributorName(string distributorName) {
            this.DistributorName = distributorName;
            return this;
        }
        public DistributeRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DistributeRequest WithDistributeResource(Gs2.Gs2Distributor.Model.DistributeResource distributeResource) {
            this.DistributeResource = distributeResource;
            return this;
        }
        public DistributeRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public DistributeRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DistributeRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DistributeRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDistributorName(!data.Keys.Contains("distributorName") || data["distributorName"] == null ? null : data["distributorName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDistributeResource(!data.Keys.Contains("distributeResource") || data["distributeResource"] == null ? null : Gs2.Gs2Distributor.Model.DistributeResource.FromJson(data["distributeResource"]))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["distributorName"] = DistributorName,
                ["userId"] = UserId,
                ["distributeResource"] = DistributeResource?.ToJson(),
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
            if (DistributorName != null) {
                writer.WritePropertyName("distributorName");
                writer.Write(DistributorName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (DistributeResource != null) {
                DistributeResource.WriteJson(writer);
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
            key += DistributorName + ":";
            key += UserId + ":";
            key += DistributeResource + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}