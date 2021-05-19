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
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Project.Request
{
	[Preserve]
	[System.Serializable]
	public class SignInRequest : Gs2Request<SignInRequest>
	{

        /** メールアドレス */
		[UnityEngine.SerializeField]
        public string email;

        /**
         * メールアドレスを設定
         *
         * @param email メールアドレス
         * @return this
         */
        public SignInRequest WithEmail(string email) {
            this.email = email;
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
        public SignInRequest WithPassword(string password) {
            this.password = password;
            return this;
        }


    	[Preserve]
        public static SignInRequest FromDict(JsonData data)
        {
            return new SignInRequest {
                email = data.Keys.Contains("email") && data["email"] != null ? data["email"].ToString(): null,
                password = data.Keys.Contains("password") && data["password"] != null ? data["password"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["email"] = email;
            data["password"] = password;
            return data;
        }
	}
}