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
using Gs2.Gs2Deploy.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Deploy.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateStackRequest : Gs2Request<CreateStackRequest>
	{

        /** スタック名 */
		[UnityEngine.SerializeField]
        public string name;

        /**
         * スタック名を設定
         *
         * @param name スタック名
         * @return this
         */
        public CreateStackRequest WithName(string name) {
            this.name = name;
            return this;
        }


        /** スタックの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * スタックの説明を設定
         *
         * @param description スタックの説明
         * @return this
         */
        public CreateStackRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


        /** テンプレートデータ */
		[UnityEngine.SerializeField]
        public string template;

        /**
         * テンプレートデータを設定
         *
         * @param template テンプレートデータ
         * @return this
         */
        public CreateStackRequest WithTemplate(string template) {
            this.template = template;
            return this;
        }


    	[Preserve]
        public static CreateStackRequest FromDict(JsonData data)
        {
            return new CreateStackRequest {
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                template = data.Keys.Contains("template") && data["template"] != null ? data["template"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["name"] = name;
            data["description"] = description;
            data["template"] = template;
            return data;
        }
	}
}