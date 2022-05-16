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
using Gs2.Gs2Datastore.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Datastore.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateDataObjectByUserIdRequest : Gs2Request<UpdateDataObjectByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string DataObjectName { set; get; }
        public string UserId { set; get; }
        public string Scope { set; get; }
        public string[] AllowUserIds { set; get; }
        public string DuplicationAvoider { set; get; }

        public UpdateDataObjectByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateDataObjectByUserIdRequest WithDataObjectName(string dataObjectName) {
            this.DataObjectName = dataObjectName;
            return this;
        }

        public UpdateDataObjectByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public UpdateDataObjectByUserIdRequest WithScope(string scope) {
            this.Scope = scope;
            return this;
        }

        public UpdateDataObjectByUserIdRequest WithAllowUserIds(string[] allowUserIds) {
            this.AllowUserIds = allowUserIds;
            return this;
        }

        public UpdateDataObjectByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateDataObjectByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateDataObjectByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDataObjectName(!data.Keys.Contains("dataObjectName") || data["dataObjectName"] == null ? null : data["dataObjectName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithAllowUserIds(!data.Keys.Contains("allowUserIds") || data["allowUserIds"] == null ? new string[]{} : data["allowUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["dataObjectName"] = DataObjectName,
                ["userId"] = UserId,
                ["scope"] = Scope,
                ["allowUserIds"] = new JsonData(AllowUserIds == null ? new JsonData[]{} :
                        AllowUserIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (DataObjectName != null) {
                writer.WritePropertyName("dataObjectName");
                writer.Write(DataObjectName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            writer.WriteArrayStart();
            foreach (var allowUserId in AllowUserIds)
            {
                writer.Write(allowUserId.ToString());
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}