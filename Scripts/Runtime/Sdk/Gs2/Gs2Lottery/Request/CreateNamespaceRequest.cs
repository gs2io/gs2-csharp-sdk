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
using Gs2.Gs2Lottery.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Lottery.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{

        /** ネームスペース名 */
		[UnityEngine.SerializeField]
        public string name;

        /**
         * ネームスペース名を設定
         *
         * @param name ネームスペース名
         * @return this
         */
        public CreateNamespaceRequest WithName(string name) {
            this.name = name;
            return this;
        }


        /** ネームスペースの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * ネームスペースの説明を設定
         *
         * @param description ネームスペースの説明
         * @return this
         */
        public CreateNamespaceRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


        /** 景品付与処理をジョブとして追加するキューのネームスペース のGRN */
		[UnityEngine.SerializeField]
        public string queueNamespaceId;

        /**
         * 景品付与処理をジョブとして追加するキューのネームスペース のGRNを設定
         *
         * @param queueNamespaceId 景品付与処理をジョブとして追加するキューのネームスペース のGRN
         * @return this
         */
        public CreateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.queueNamespaceId = queueNamespaceId;
            return this;
        }


        /** 景品付与処理のスタンプシートで使用する暗号鍵GRN */
		[UnityEngine.SerializeField]
        public string keyId;

        /**
         * 景品付与処理のスタンプシートで使用する暗号鍵GRNを設定
         *
         * @param keyId 景品付与処理のスタンプシートで使用する暗号鍵GRN
         * @return this
         */
        public CreateNamespaceRequest WithKeyId(string keyId) {
            this.keyId = keyId;
            return this;
        }


        /** 抽選処理時 に実行されるスクリプト のGRN */
		[UnityEngine.SerializeField]
        public string lotteryTriggerScriptId;

        /**
         * 抽選処理時 に実行されるスクリプト のGRNを設定
         *
         * @param lotteryTriggerScriptId 抽選処理時 に実行されるスクリプト のGRN
         * @return this
         */
        public CreateNamespaceRequest WithLotteryTriggerScriptId(string lotteryTriggerScriptId) {
            this.lotteryTriggerScriptId = lotteryTriggerScriptId;
            return this;
        }


        /** 排出テーブル選択時 に実行されるスクリプト のGRN */
		[UnityEngine.SerializeField]
        public string choicePrizeTableScriptId;

        /**
         * 排出テーブル選択時 に実行されるスクリプト のGRNを設定
         *
         * @param choicePrizeTableScriptId 排出テーブル選択時 に実行されるスクリプト のGRN
         * @return this
         */
        public CreateNamespaceRequest WithChoicePrizeTableScriptId(string choicePrizeTableScriptId) {
            this.choicePrizeTableScriptId = choicePrizeTableScriptId;
            return this;
        }


        /** ログの出力設定 */
		[UnityEngine.SerializeField]
        public global::Gs2.Gs2Lottery.Model.LogSetting logSetting;

        /**
         * ログの出力設定を設定
         *
         * @param logSetting ログの出力設定
         * @return this
         */
        public CreateNamespaceRequest WithLogSetting(global::Gs2.Gs2Lottery.Model.LogSetting logSetting) {
            this.logSetting = logSetting;
            return this;
        }


    	[Preserve]
        public static CreateNamespaceRequest FromDict(JsonData data)
        {
            return new CreateNamespaceRequest {
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                queueNamespaceId = data.Keys.Contains("queueNamespaceId") && data["queueNamespaceId"] != null ? data["queueNamespaceId"].ToString(): null,
                keyId = data.Keys.Contains("keyId") && data["keyId"] != null ? data["keyId"].ToString(): null,
                lotteryTriggerScriptId = data.Keys.Contains("lotteryTriggerScriptId") && data["lotteryTriggerScriptId"] != null ? data["lotteryTriggerScriptId"].ToString(): null,
                choicePrizeTableScriptId = data.Keys.Contains("choicePrizeTableScriptId") && data["choicePrizeTableScriptId"] != null ? data["choicePrizeTableScriptId"].ToString(): null,
                logSetting = data.Keys.Contains("logSetting") && data["logSetting"] != null ? global::Gs2.Gs2Lottery.Model.LogSetting.FromDict(data["logSetting"]) : null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["name"] = name;
            data["description"] = description;
            data["queueNamespaceId"] = queueNamespaceId;
            data["keyId"] = keyId;
            data["lotteryTriggerScriptId"] = lotteryTriggerScriptId;
            data["choicePrizeTableScriptId"] = choicePrizeTableScriptId;
            data["logSetting"] = logSetting.ToDict();
            return data;
        }
	}
}