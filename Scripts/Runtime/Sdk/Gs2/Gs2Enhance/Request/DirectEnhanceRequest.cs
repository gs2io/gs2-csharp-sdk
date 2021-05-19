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
	public class DirectEnhanceRequest : Gs2Request<DirectEnhanceRequest>
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
        public DirectEnhanceRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** 強化レート名 */
		[UnityEngine.SerializeField]
        public string rateName;

        /**
         * 強化レート名を設定
         *
         * @param rateName 強化レート名
         * @return this
         */
        public DirectEnhanceRequest WithRateName(string rateName) {
            this.rateName = rateName;
            return this;
        }


        /** 強化対象の GS2-Inventory アイテムセットGRN */
		[UnityEngine.SerializeField]
        public string targetItemSetId;

        /**
         * 強化対象の GS2-Inventory アイテムセットGRNを設定
         *
         * @param targetItemSetId 強化対象の GS2-Inventory アイテムセットGRN
         * @return this
         */
        public DirectEnhanceRequest WithTargetItemSetId(string targetItemSetId) {
            this.targetItemSetId = targetItemSetId;
            return this;
        }


        /** 強化素材リスト */
		[UnityEngine.SerializeField]
        public List<Material> materials;

        /**
         * 強化素材リストを設定
         *
         * @param materials 強化素材リスト
         * @return this
         */
        public DirectEnhanceRequest WithMaterials(List<Material> materials) {
            this.materials = materials;
            return this;
        }


        /** 設定値 */
		[UnityEngine.SerializeField]
        public List<Config> config;

        /**
         * 設定値を設定
         *
         * @param config 設定値
         * @return this
         */
        public DirectEnhanceRequest WithConfig(List<Config> config) {
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
        public DirectEnhanceRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


        /** アクセストークン */
        public string accessToken { set; get; }

        /**
         * アクセストークンを設定
         *
         * @param accessToken アクセストークン
         * @return this
         */
        public DirectEnhanceRequest WithAccessToken(string accessToken) {
            this.accessToken = accessToken;
            return this;
        }

    	[Preserve]
        public static DirectEnhanceRequest FromDict(JsonData data)
        {
            return new DirectEnhanceRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                rateName = data.Keys.Contains("rateName") && data["rateName"] != null ? data["rateName"].ToString(): null,
                targetItemSetId = data.Keys.Contains("targetItemSetId") && data["targetItemSetId"] != null ? data["targetItemSetId"].ToString(): null,
                materials = data.Keys.Contains("materials") && data["materials"] != null ? data["materials"].Cast<JsonData>().Select(value =>
                    {
                        return Material.FromDict(value);
                    }
                ).ToList() : null,
                config = data.Keys.Contains("config") && data["config"] != null ? data["config"].Cast<JsonData>().Select(value =>
                    {
                        return Config.FromDict(value);
                    }
                ).ToList() : null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["rateName"] = rateName;
            data["targetItemSetId"] = targetItemSetId;
            data["materials"] = new JsonData(materials.Select(item => item.ToDict()));
            data["config"] = new JsonData(config.Select(item => item.ToDict()));
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}