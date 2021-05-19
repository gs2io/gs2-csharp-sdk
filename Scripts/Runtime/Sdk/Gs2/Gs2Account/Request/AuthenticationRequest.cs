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
using Gs2.Gs2Account.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Account.Request
{
	[Preserve]
	[System.Serializable]
	public class AuthenticationRequest : Gs2Request<AuthenticationRequest>
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
        public AuthenticationRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** アカウントID */
		[UnityEngine.SerializeField]
        public string userId;

        /**
         * アカウントIDを設定
         *
         * @param userId アカウントID
         * @return this
         */
        public AuthenticationRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** 認証トークンの暗号化に使用する暗号鍵 のGRN */
		[UnityEngine.SerializeField]
        public string keyId;

        /**
         * 認証トークンの暗号化に使用する暗号鍵 のGRNを設定
         *
         * @param keyId 認証トークンの暗号化に使用する暗号鍵 のGRN
         * @return this
         */
        public AuthenticationRequest WithKeyId(string keyId) {
            this.keyId = keyId;
            return this;
        }


        /** パスワード */
		[UnityEngine.SerializeField]
        public string password;

        /**
         * パスワードを設定
         *
         * @param password パスワード
         * @return this
         */
        public AuthenticationRequest WithPassword(string password) {
            this.password = password;
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
        public AuthenticationRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static AuthenticationRequest FromDict(JsonData data)
        {
            return new AuthenticationRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                keyId = data.Keys.Contains("keyId") && data["keyId"] != null ? data["keyId"].ToString(): null,
                password = data.Keys.Contains("password") && data["password"] != null ? data["password"].ToString(): null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["userId"] = userId;
            data["keyId"] = keyId;
            data["password"] = password;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}