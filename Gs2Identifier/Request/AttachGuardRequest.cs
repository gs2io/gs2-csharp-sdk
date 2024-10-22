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
using Gs2.Gs2Identifier.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Identifier.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AttachGuardRequest : Gs2Request<AttachGuardRequest>
	{
         public string UserName { set; get; } = null!;
         public string ClientId { set; get; } = null!;
         public string GuardNamespaceId { set; get; } = null!;
        public AttachGuardRequest WithUserName(string userName) {
            this.UserName = userName;
            return this;
        }
        public AttachGuardRequest WithClientId(string clientId) {
            this.ClientId = clientId;
            return this;
        }
        public AttachGuardRequest WithGuardNamespaceId(string guardNamespaceId) {
            this.GuardNamespaceId = guardNamespaceId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AttachGuardRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AttachGuardRequest()
                .WithUserName(!data.Keys.Contains("userName") || data["userName"] == null ? null : data["userName"].ToString())
                .WithClientId(!data.Keys.Contains("clientId") || data["clientId"] == null ? null : data["clientId"].ToString())
                .WithGuardNamespaceId(!data.Keys.Contains("guardNamespaceId") || data["guardNamespaceId"] == null ? null : data["guardNamespaceId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["userName"] = UserName,
                ["clientId"] = ClientId,
                ["guardNamespaceId"] = GuardNamespaceId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserName != null) {
                writer.WritePropertyName("userName");
                writer.Write(UserName.ToString());
            }
            if (ClientId != null) {
                writer.WritePropertyName("clientId");
                writer.Write(ClientId.ToString());
            }
            if (GuardNamespaceId != null) {
                writer.WritePropertyName("guardNamespaceId");
                writer.Write(GuardNamespaceId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += UserName + ":";
            key += ClientId + ":";
            key += GuardNamespaceId + ":";
            return key;
        }
    }
}