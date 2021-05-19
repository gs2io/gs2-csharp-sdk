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
using Gs2.Gs2Money.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Money.Request
{
	[Preserve]
	[System.Serializable]
	public class WithdrawByStampTaskRequest : Gs2Request<WithdrawByStampTaskRequest>
	{

        /** スタンプタスク */
		[UnityEngine.SerializeField]
        public string stampTask;

        /**
         * スタンプタスクを設定
         *
         * @param stampTask スタンプタスク
         * @return this
         */
        public WithdrawByStampTaskRequest WithStampTask(string stampTask) {
            this.stampTask = stampTask;
            return this;
        }


        /** スタンプタスクの署名検証に使用する 暗号鍵 のGRN */
		[UnityEngine.SerializeField]
        public string keyId;

        /**
         * スタンプタスクの署名検証に使用する 暗号鍵 のGRNを設定
         *
         * @param keyId スタンプタスクの署名検証に使用する 暗号鍵 のGRN
         * @return this
         */
        public WithdrawByStampTaskRequest WithKeyId(string keyId) {
            this.keyId = keyId;
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
        public WithdrawByStampTaskRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static WithdrawByStampTaskRequest FromDict(JsonData data)
        {
            return new WithdrawByStampTaskRequest {
                stampTask = data.Keys.Contains("stampTask") && data["stampTask"] != null ? data["stampTask"].ToString(): null,
                keyId = data.Keys.Contains("keyId") && data["keyId"] != null ? data["keyId"].ToString(): null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["stampTask"] = stampTask;
            data["keyId"] = keyId;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}