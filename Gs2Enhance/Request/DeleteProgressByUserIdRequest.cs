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
using Gs2.Gs2Enhance.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Enhance.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DeleteProgressByUserIdRequest : Gs2Request<DeleteProgressByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string RateName { set; get; }
        public string ProgressName { set; get; }
        public string DuplicationAvoider { set; get; }
        public DeleteProgressByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DeleteProgressByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DeleteProgressByUserIdRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public DeleteProgressByUserIdRequest WithProgressName(string progressName) {
            this.ProgressName = progressName;
            return this;
        }

        public DeleteProgressByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DeleteProgressByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteProgressByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithProgressName(!data.Keys.Contains("progressName") || data["progressName"] == null ? null : data["progressName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["rateName"] = RateName,
                ["progressName"] = ProgressName,
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
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (ProgressName != null) {
                writer.WritePropertyName("progressName");
                writer.Write(ProgressName.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}