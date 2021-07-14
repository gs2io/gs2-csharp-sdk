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
using UnityEngine.Scripting;

namespace Gs2.Gs2Exchange.Request
{
	[Preserve]
	[System.Serializable]
	public class GetAwaitRequest : Gs2Request<GetAwaitRequest>
	{
        public string NamespaceName { set; get; }
        public string AccessToken { set; get; }
        public string RateName { set; get; }
        public string AwaitName { set; get; }

        public GetAwaitRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public GetAwaitRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public GetAwaitRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }

        public GetAwaitRequest WithAwaitName(string awaitName) {
            this.AwaitName = awaitName;
            return this;
        }

    	[Preserve]
        public static GetAwaitRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetAwaitRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithAwaitName(!data.Keys.Contains("awaitName") || data["awaitName"] == null ? null : data["awaitName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["rateName"] = RateName,
                ["awaitName"] = AwaitName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (AwaitName != null) {
                writer.WritePropertyName("awaitName");
                writer.Write(AwaitName.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}