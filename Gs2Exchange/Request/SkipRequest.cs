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
	public class SkipRequest : Gs2Request<SkipRequest>
	{
        public string NamespaceName { set; get; }
        public string AccessToken { set; get; }
        public string RateName { set; get; }
        public string AwaitName { set; get; }
        public Gs2.Gs2Exchange.Model.Config[] Config { set; get; }
        public SkipRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SkipRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public SkipRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public SkipRequest WithAwaitName(string awaitName) {
            this.AwaitName = awaitName;
            return this;
        }
        public SkipRequest WithConfig(Gs2.Gs2Exchange.Model.Config[] config) {
            this.Config = config;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SkipRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SkipRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithAwaitName(!data.Keys.Contains("awaitName") || data["awaitName"] == null ? null : data["awaitName"].ToString())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2Exchange.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Exchange.Model.Config.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["rateName"] = RateName,
                ["awaitName"] = AwaitName,
                ["config"] = new JsonData(Config == null ? new JsonData[]{} :
                        Config.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
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
            writer.WriteArrayStart();
            foreach (var confi in Config)
            {
                if (confi != null) {
                    confi.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}