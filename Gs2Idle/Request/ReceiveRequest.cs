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
	public class ReceiveRequest : Gs2Request<ReceiveRequest>
	{
        public string NamespaceName { set; get; }
        public string AccessToken { set; get; }
        public string CategoryName { set; get; }
        public Gs2.Gs2Idle.Model.Config[] Config { set; get; }
        public string DuplicationAvoider { set; get; }

        public ReceiveRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public ReceiveRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public ReceiveRequest WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }

        public ReceiveRequest WithConfig(Gs2.Gs2Idle.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public ReceiveRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReceiveRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReceiveRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? new Gs2.Gs2Idle.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Idle.Model.Config.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData configJsonData = null;
            if (Config != null)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["categoryName"] = CategoryName,
                ["config"] = configJsonData,
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
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
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

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += CategoryName + ":";
            key += Config + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply ReceiveRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ReceiveRequest)x;
            return this;
        }
    }
}