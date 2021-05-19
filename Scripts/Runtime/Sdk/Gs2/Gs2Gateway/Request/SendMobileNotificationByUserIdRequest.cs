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
using Gs2.Gs2Gateway.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Gateway.Request
{
	[Preserve]
	[System.Serializable]
	public class SendMobileNotificationByUserIdRequest : Gs2Request<SendMobileNotificationByUserIdRequest>
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
        public SendMobileNotificationByUserIdRequest WithNamespaceName(string namespaceName) {
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
        public SendMobileNotificationByUserIdRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** タイトル */
		[UnityEngine.SerializeField]
        public string subject;

        /**
         * タイトルを設定
         *
         * @param subject タイトル
         * @return this
         */
        public SendMobileNotificationByUserIdRequest WithSubject(string subject) {
            this.subject = subject;
            return this;
        }


        /** ペイロード */
		[UnityEngine.SerializeField]
        public string payload;

        /**
         * ペイロードを設定
         *
         * @param payload ペイロード
         * @return this
         */
        public SendMobileNotificationByUserIdRequest WithPayload(string payload) {
            this.payload = payload;
            return this;
        }


        /** 再生する音声ファイル名 */
		[UnityEngine.SerializeField]
        public string sound;

        /**
         * 再生する音声ファイル名を設定
         *
         * @param sound 再生する音声ファイル名
         * @return this
         */
        public SendMobileNotificationByUserIdRequest WithSound(string sound) {
            this.sound = sound;
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
        public SendMobileNotificationByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static SendMobileNotificationByUserIdRequest FromDict(JsonData data)
        {
            return new SendMobileNotificationByUserIdRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                subject = data.Keys.Contains("subject") && data["subject"] != null ? data["subject"].ToString(): null,
                payload = data.Keys.Contains("payload") && data["payload"] != null ? data["payload"].ToString(): null,
                sound = data.Keys.Contains("sound") && data["sound"] != null ? data["sound"].ToString(): null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["userId"] = userId;
            data["subject"] = subject;
            data["payload"] = payload;
            data["sound"] = sound;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}