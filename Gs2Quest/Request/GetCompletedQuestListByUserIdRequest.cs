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
	public class GetCompletedQuestListByUserIdRequest : Gs2Request<GetCompletedQuestListByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string QuestGroupName { set; get; }
        public string UserId { set; get; }

        public GetCompletedQuestListByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public GetCompletedQuestListByUserIdRequest WithQuestGroupName(string questGroupName) {
            this.QuestGroupName = questGroupName;
            return this;
        }

        public GetCompletedQuestListByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetCompletedQuestListByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetCompletedQuestListByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithQuestGroupName(!data.Keys.Contains("questGroupName") || data["questGroupName"] == null ? null : data["questGroupName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["questGroupName"] = QuestGroupName,
                ["userId"] = UserId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (QuestGroupName != null) {
                writer.WritePropertyName("questGroupName");
                writer.Write(QuestGroupName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}