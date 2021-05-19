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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Formation.Request
{
	[Preserve]
	[System.Serializable]
	public class AcquireActionsToFormPropertiesRequest : Gs2Request<AcquireActionsToFormPropertiesRequest>
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
        public AcquireActionsToFormPropertiesRequest WithNamespaceName(string namespaceName) {
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
        public AcquireActionsToFormPropertiesRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** フォームの保存領域の名前 */
		[UnityEngine.SerializeField]
        public string moldName;

        /**
         * フォームの保存領域の名前を設定
         *
         * @param moldName フォームの保存領域の名前
         * @return this
         */
        public AcquireActionsToFormPropertiesRequest WithMoldName(string moldName) {
            this.moldName = moldName;
            return this;
        }


        /** 保存領域のインデックス */
		[UnityEngine.SerializeField]
        public int? index;

        /**
         * 保存領域のインデックスを設定
         *
         * @param index 保存領域のインデックス
         * @return this
         */
        public AcquireActionsToFormPropertiesRequest WithIndex(int? index) {
            this.index = index;
            return this;
        }


        /** フォームのプロパティに適用する入手アクション */
		[UnityEngine.SerializeField]
        public global::Gs2.Gs2Formation.Model.AcquireAction acquireAction;

        /**
         * フォームのプロパティに適用する入手アクションを設定
         *
         * @param acquireAction フォームのプロパティに適用する入手アクション
         * @return this
         */
        public AcquireActionsToFormPropertiesRequest WithAcquireAction(global::Gs2.Gs2Formation.Model.AcquireAction acquireAction) {
            this.acquireAction = acquireAction;
            return this;
        }


        /** 入手処理を登録する GS2-JobQueue のネームスペース のGRN */
		[UnityEngine.SerializeField]
        public string queueNamespaceId;

        /**
         * 入手処理を登録する GS2-JobQueue のネームスペース のGRNを設定
         *
         * @param queueNamespaceId 入手処理を登録する GS2-JobQueue のネームスペース のGRN
         * @return this
         */
        public AcquireActionsToFormPropertiesRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.queueNamespaceId = queueNamespaceId;
            return this;
        }


        /** スタンプシートの発行に使用する GS2-Key の暗号鍵 のGRN */
		[UnityEngine.SerializeField]
        public string keyId;

        /**
         * スタンプシートの発行に使用する GS2-Key の暗号鍵 のGRNを設定
         *
         * @param keyId スタンプシートの発行に使用する GS2-Key の暗号鍵 のGRN
         * @return this
         */
        public AcquireActionsToFormPropertiesRequest WithKeyId(string keyId) {
            this.keyId = keyId;
            return this;
        }


        /** 入手アクションに適用するコンフィグ */
		[UnityEngine.SerializeField]
        public List<AcquireActionConfig> config;

        /**
         * 入手アクションに適用するコンフィグを設定
         *
         * @param config 入手アクションに適用するコンフィグ
         * @return this
         */
        public AcquireActionsToFormPropertiesRequest WithConfig(List<AcquireActionConfig> config) {
            this.config = config;
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
        public AcquireActionsToFormPropertiesRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static AcquireActionsToFormPropertiesRequest FromDict(JsonData data)
        {
            return new AcquireActionsToFormPropertiesRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                moldName = data.Keys.Contains("moldName") && data["moldName"] != null ? data["moldName"].ToString(): null,
                index = data.Keys.Contains("index") && data["index"] != null ? (int?)int.Parse(data["index"].ToString()) : null,
                acquireAction = data.Keys.Contains("acquireAction") && data["acquireAction"] != null ? global::Gs2.Gs2Formation.Model.AcquireAction.FromDict(data["acquireAction"]) : null,
                queueNamespaceId = data.Keys.Contains("queueNamespaceId") && data["queueNamespaceId"] != null ? data["queueNamespaceId"].ToString(): null,
                keyId = data.Keys.Contains("keyId") && data["keyId"] != null ? data["keyId"].ToString(): null,
                config = data.Keys.Contains("config") && data["config"] != null ? data["config"].Cast<JsonData>().Select(value =>
                    {
                        return AcquireActionConfig.FromDict(value);
                    }
                ).ToList() : null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["userId"] = userId;
            data["moldName"] = moldName;
            data["index"] = index;
            data["acquireAction"] = acquireAction.ToDict();
            data["queueNamespaceId"] = queueNamespaceId;
            data["keyId"] = keyId;
            data["config"] = new JsonData(config.Select(item => item.ToDict()));
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}