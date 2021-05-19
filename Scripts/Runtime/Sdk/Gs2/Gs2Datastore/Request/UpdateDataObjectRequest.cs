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
using UnityEngine.Scripting;

namespace Gs2.Gs2Datastore.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateDataObjectRequest : Gs2Request<UpdateDataObjectRequest>
	{

        /** ネームスペース名 */
		[UnityEngine.SerializeField]
        public string namespaceName;

        /**
         * ネームスペース名を設定
         *
         * @param namespaceName ネームスペース名
         * @return this
         */
        public UpdateDataObjectRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** データの名前 */
		[UnityEngine.SerializeField]
        public string dataObjectName;

        /**
         * データの名前を設定
         *
         * @param dataObjectName データの名前
         * @return this
         */
        public UpdateDataObjectRequest WithDataObjectName(string dataObjectName) {
            this.dataObjectName = dataObjectName;
            return this;
        }


        /** ファイルのアクセス権 */
		[UnityEngine.SerializeField]
        public string scope;

        /**
         * ファイルのアクセス権を設定
         *
         * @param scope ファイルのアクセス権
         * @return this
         */
        public UpdateDataObjectRequest WithScope(string scope) {
            this.scope = scope;
            return this;
        }


        /** 公開するユーザIDリスト */
		[UnityEngine.SerializeField]
        public List<string> allowUserIds;

        /**
         * 公開するユーザIDリストを設定
         *
         * @param allowUserIds 公開するユーザIDリスト
         * @return this
         */
        public UpdateDataObjectRequest WithAllowUserIds(List<string> allowUserIds) {
            this.allowUserIds = allowUserIds;
            return this;
        }


        /** 重複実行回避機能に使用するID */
		[UnityEngine.SerializeField]
        public string duplicationAvoider;

        /**
         * 重複実行回避機能に使用するIDを設定
         *
         * @param duplicationAvoider 重複実行回避機能に使用するID
         * @return this
         */
        public UpdateDataObjectRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


        /** アクセストークン */
        public string accessToken { set; get; }

        /**
         * アクセストークンを設定
         *
         * @param accessToken アクセストークン
         * @return this
         */
        public UpdateDataObjectRequest WithAccessToken(string accessToken) {
            this.accessToken = accessToken;
            return this;
        }

    	[Preserve]
        public static UpdateDataObjectRequest FromDict(JsonData data)
        {
            return new UpdateDataObjectRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                dataObjectName = data.Keys.Contains("dataObjectName") && data["dataObjectName"] != null ? data["dataObjectName"].ToString(): null,
                scope = data.Keys.Contains("scope") && data["scope"] != null ? data["scope"].ToString(): null,
                allowUserIds = data.Keys.Contains("allowUserIds") && data["allowUserIds"] != null ? data["allowUserIds"].Cast<JsonData>().Select(value =>
                    {
                        return value.ToString();
                    }
                ).ToList() : null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["dataObjectName"] = dataObjectName;
            data["scope"] = scope;
            data["allowUserIds"] = new JsonData(allowUserIds);
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}