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
	public class PrepareUploadByUserIdRequest : Gs2Request<PrepareUploadByUserIdRequest>
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
        public PrepareUploadByUserIdRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** ユーザーID */
		[UnityEngine.SerializeField]
        public string userId;

        /**
         * ユーザーIDを設定
         *
         * @param userId ユーザーID
         * @return this
         */
        public PrepareUploadByUserIdRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** データの名前 */
		[UnityEngine.SerializeField]
        public string name;

        /**
         * データの名前を設定
         *
         * @param name データの名前
         * @return this
         */
        public PrepareUploadByUserIdRequest WithName(string name) {
            this.name = name;
            return this;
        }


        /** アップロードするデータの MIME-Type */
		[UnityEngine.SerializeField]
        public string contentType;

        /**
         * アップロードするデータの MIME-Typeを設定
         *
         * @param contentType アップロードするデータの MIME-Type
         * @return this
         */
        public PrepareUploadByUserIdRequest WithContentType(string contentType) {
            this.contentType = contentType;
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
        public PrepareUploadByUserIdRequest WithScope(string scope) {
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
        public PrepareUploadByUserIdRequest WithAllowUserIds(List<string> allowUserIds) {
            this.allowUserIds = allowUserIds;
            return this;
        }


        /** 既にデータが存在する場合にエラーとするか、データを更新するか */
		[UnityEngine.SerializeField]
        public bool? updateIfExists;

        /**
         * 既にデータが存在する場合にエラーとするか、データを更新するかを設定
         *
         * @param updateIfExists 既にデータが存在する場合にエラーとするか、データを更新するか
         * @return this
         */
        public PrepareUploadByUserIdRequest WithUpdateIfExists(bool? updateIfExists) {
            this.updateIfExists = updateIfExists;
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
        public PrepareUploadByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static PrepareUploadByUserIdRequest FromDict(JsonData data)
        {
            return new PrepareUploadByUserIdRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                contentType = data.Keys.Contains("contentType") && data["contentType"] != null ? data["contentType"].ToString(): null,
                scope = data.Keys.Contains("scope") && data["scope"] != null ? data["scope"].ToString(): null,
                allowUserIds = data.Keys.Contains("allowUserIds") && data["allowUserIds"] != null ? data["allowUserIds"].Cast<JsonData>().Select(value =>
                    {
                        return value.ToString();
                    }
                ).ToList() : null,
                updateIfExists = data.Keys.Contains("updateIfExists") && data["updateIfExists"] != null ? (bool?)bool.Parse(data["updateIfExists"].ToString()) : null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["userId"] = userId;
            data["name"] = name;
            data["contentType"] = contentType;
            data["scope"] = scope;
            data["allowUserIds"] = new JsonData(allowUserIds);
            data["updateIfExists"] = updateIfExists;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}