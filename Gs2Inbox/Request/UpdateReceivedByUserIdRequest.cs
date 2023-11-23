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
using Gs2.Gs2Inbox.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateReceivedByUserIdRequest : Gs2Request<UpdateReceivedByUserIdRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public string[] ReceivedGlobalMessageNames { set; get; }
        public string DuplicationAvoider { set; get; }
        public UpdateReceivedByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateReceivedByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public UpdateReceivedByUserIdRequest WithReceivedGlobalMessageNames(string[] receivedGlobalMessageNames) {
            this.ReceivedGlobalMessageNames = receivedGlobalMessageNames;
            return this;
        }

        public UpdateReceivedByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateReceivedByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateReceivedByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithReceivedGlobalMessageNames(!data.Keys.Contains("receivedGlobalMessageNames") || data["receivedGlobalMessageNames"] == null || !data["receivedGlobalMessageNames"].IsArray ? new string[]{} : data["receivedGlobalMessageNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData receivedGlobalMessageNamesJsonData = null;
            if (ReceivedGlobalMessageNames != null && ReceivedGlobalMessageNames.Length > 0)
            {
                receivedGlobalMessageNamesJsonData = new JsonData();
                foreach (var receivedGlobalMessageName in ReceivedGlobalMessageNames)
                {
                    receivedGlobalMessageNamesJsonData.Add(receivedGlobalMessageName);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["receivedGlobalMessageNames"] = receivedGlobalMessageNamesJsonData,
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
            if (ReceivedGlobalMessageNames != null) {
                writer.WritePropertyName("receivedGlobalMessageNames");
                writer.WriteArrayStart();
                foreach (var receivedGlobalMessageName in ReceivedGlobalMessageNames)
                {
                    writer.Write(receivedGlobalMessageName.ToString());
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += ReceivedGlobalMessageNames + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateReceivedByUserIdRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateReceivedByUserIdRequest)x;
            return this;
        }
    }
}