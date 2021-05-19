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
using Gs2.Gs2Enhance.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Enhance.Request
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


        /** DirectEnhance を利用できるようにするか */
		[UnityEngine.SerializeField]
        public bool? enableDirectEnhance;

        /**
         * DirectEnhance を利用できるようにするかを設定
         *
         * @param enableDirectEnhance DirectEnhance を利用できるようにするか
         * @return this
         */
        public CreateNamespaceRequest WithEnableDirectEnhance(bool? enableDirectEnhance) {
            this.enableDirectEnhance = enableDirectEnhance;
            return this;
        }


        /** 交換処理をジョブとして追加するキューのネームスペース のGRN */
		[UnityEngine.SerializeField]
        public string queueNamespaceId;

        /**
         * 交換処理をジョブとして追加するキューのネームスペース のGRNを設定
         *
         * @param queueNamespaceId 交換処理をジョブとして追加するキューのネームスペース のGRN
         * @return this
         */
        public CreateNamespaceRequest WithQueueNamespaceId(string queueNamespaceId) {
            this.queueNamespaceId = queueNamespaceId;
            return this;
        }


        /** 交換処理のスタンプシートで使用する暗号鍵GRN */
		[UnityEngine.SerializeField]
        public string keyId;

        /**
         * 交換処理のスタンプシートで使用する暗号鍵GRNを設定
         *
         * @param keyId 交換処理のスタンプシートで使用する暗号鍵GRN
         * @return this
         */
        public CreateNamespaceRequest WithKeyId(string keyId) {
            this.keyId = keyId;
            return this;
        }


        /** ログの出力設定 */
		[UnityEngine.SerializeField]
        public global::Gs2.Gs2Enhance.Model.LogSetting logSetting;

        /**
         * ログの出力設定を設定
         *
         * @param logSetting ログの出力設定
         * @return this
         */
        public CreateNamespaceRequest WithLogSetting(global::Gs2.Gs2Enhance.Model.LogSetting logSetting) {
            this.logSetting = logSetting;
            return this;
        }


    	[Preserve]
        public static CreateNamespaceRequest FromDict(JsonData data)
        {
            return new CreateNamespaceRequest {
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                enableDirectEnhance = data.Keys.Contains("enableDirectEnhance") && data["enableDirectEnhance"] != null ? (bool?)bool.Parse(data["enableDirectEnhance"].ToString()) : null,
                queueNamespaceId = data.Keys.Contains("queueNamespaceId") && data["queueNamespaceId"] != null ? data["queueNamespaceId"].ToString(): null,
                keyId = data.Keys.Contains("keyId") && data["keyId"] != null ? data["keyId"].ToString(): null,
                logSetting = data.Keys.Contains("logSetting") && data["logSetting"] != null ? global::Gs2.Gs2Enhance.Model.LogSetting.FromDict(data["logSetting"]) : null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["name"] = name;
            data["description"] = description;
            data["enableDirectEnhance"] = enableDirectEnhance;
            data["queueNamespaceId"] = queueNamespaceId;
            data["keyId"] = keyId;
            data["logSetting"] = logSetting.ToDict();
            return data;
        }
	}
}