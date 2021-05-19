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
using UnityEngine.Scripting;

namespace Gs2.Gs2Identifier.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateUserRequest : Gs2Request<UpdateUserRequest>
	{

        /** ユーザー名 */
		[UnityEngine.SerializeField]
        public string userName;

        /**
         * ユーザー名を設定
         *
         * @param userName ユーザー名
         * @return this
         */
        public UpdateUserRequest WithUserName(string userName) {
            this.userName = userName;
            return this;
        }


        /** ユーザの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * ユーザの説明を設定
         *
         * @param description ユーザの説明
         * @return this
         */
        public UpdateUserRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


    	[Preserve]
        public static UpdateUserRequest FromDict(JsonData data)
        {
            return new UpdateUserRequest {
                userName = data.Keys.Contains("userName") && data["userName"] != null ? data["userName"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["userName"] = userName;
            data["description"] = description;
            return data;
        }
	}
}