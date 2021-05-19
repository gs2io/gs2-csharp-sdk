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
using Gs2.Gs2Script.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Script.Request
{
	[Preserve]
	[System.Serializable]
	public class InvokeRequest : Gs2Request<InvokeRequest>
	{

        /** スクリプト */
		[UnityEngine.SerializeField]
        public string scriptId;

        /**
         * スクリプトを設定
         *
         * @param scriptId スクリプト
         * @return this
         */
        public InvokeRequest WithScriptId(string scriptId) {
            this.scriptId = scriptId;
            return this;
        }


        /** オーナーID */
		[UnityEngine.SerializeField]
        public string ownerId;

        /**
         * オーナーIDを設定
         *
         * @param ownerId オーナーID
         * @return this
         */
        public InvokeRequest WithOwnerId(string ownerId) {
            this.ownerId = ownerId;
            return this;
        }


        /** None */
		[UnityEngine.SerializeField]
        public string args;

        /**
         * Noneを設定
         *
         * @param args None
         * @return this
         */
        public InvokeRequest WithArgs(string args) {
            this.args = args;
            return this;
        }


    	[Preserve]
        public static InvokeRequest FromDict(JsonData data)
        {
            return new InvokeRequest {
                scriptId = data.Keys.Contains("scriptId") && data["scriptId"] != null ? data["scriptId"].ToString(): null,
                ownerId = data.Keys.Contains("ownerId") && data["ownerId"] != null ? data["ownerId"].ToString(): null,
                args = data.Keys.Contains("args") && data["args"] != null ? data["args"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["scriptId"] = scriptId;
            data["ownerId"] = ownerId;
            data["args"] = args;
            return data;
        }
	}
}