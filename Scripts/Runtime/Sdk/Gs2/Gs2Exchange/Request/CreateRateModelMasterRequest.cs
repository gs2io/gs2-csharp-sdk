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
using Gs2.Gs2Exchange.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Exchange.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateRateModelMasterRequest : Gs2Request<CreateRateModelMasterRequest>
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
        public CreateRateModelMasterRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** 交換レート名 */
		[UnityEngine.SerializeField]
        public string name;

        /**
         * 交換レート名を設定
         *
         * @param name 交換レート名
         * @return this
         */
        public CreateRateModelMasterRequest WithName(string name) {
            this.name = name;
            return this;
        }


        /** 交換レートマスターの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * 交換レートマスターの説明を設定
         *
         * @param description 交換レートマスターの説明
         * @return this
         */
        public CreateRateModelMasterRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


        /** 交換レートのメタデータ */
		[UnityEngine.SerializeField]
        public string metadata;

        /**
         * 交換レートのメタデータを設定
         *
         * @param metadata 交換レートのメタデータ
         * @return this
         */
        public CreateRateModelMasterRequest WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }


        /** 交換の種類 */
		[UnityEngine.SerializeField]
        public string timingType;

        /**
         * 交換の種類を設定
         *
         * @param timingType 交換の種類
         * @return this
         */
        public CreateRateModelMasterRequest WithTimingType(string timingType) {
            this.timingType = timingType;
            return this;
        }


        /** 交換実行から実際に報酬を受け取れるようになるまでの待ち時間（分） */
		[UnityEngine.SerializeField]
        public int? lockTime;

        /**
         * 交換実行から実際に報酬を受け取れるようになるまでの待ち時間（分）を設定
         *
         * @param lockTime 交換実行から実際に報酬を受け取れるようになるまでの待ち時間（分）
         * @return this
         */
        public CreateRateModelMasterRequest WithLockTime(int? lockTime) {
            this.lockTime = lockTime;
            return this;
        }


        /** スキップをすることができるか */
		[UnityEngine.SerializeField]
        public bool? enableSkip;

        /**
         * スキップをすることができるかを設定
         *
         * @param enableSkip スキップをすることができるか
         * @return this
         */
        public CreateRateModelMasterRequest WithEnableSkip(bool? enableSkip) {
            this.enableSkip = enableSkip;
            return this;
        }


        /** 時短消費アクションリスト */
		[UnityEngine.SerializeField]
        public List<ConsumeAction> skipConsumeActions;

        /**
         * 時短消費アクションリストを設定
         *
         * @param skipConsumeActions 時短消費アクションリスト
         * @return this
         */
        public CreateRateModelMasterRequest WithSkipConsumeActions(List<ConsumeAction> skipConsumeActions) {
            this.skipConsumeActions = skipConsumeActions;
            return this;
        }


        /** 入手アクションリスト */
		[UnityEngine.SerializeField]
        public List<AcquireAction> acquireActions;

        /**
         * 入手アクションリストを設定
         *
         * @param acquireActions 入手アクションリスト
         * @return this
         */
        public CreateRateModelMasterRequest WithAcquireActions(List<AcquireAction> acquireActions) {
            this.acquireActions = acquireActions;
            return this;
        }


        /** 消費アクションリスト */
		[UnityEngine.SerializeField]
        public List<ConsumeAction> consumeActions;

        /**
         * 消費アクションリストを設定
         *
         * @param consumeActions 消費アクションリスト
         * @return this
         */
        public CreateRateModelMasterRequest WithConsumeActions(List<ConsumeAction> consumeActions) {
            this.consumeActions = consumeActions;
            return this;
        }


    	[Preserve]
        public static CreateRateModelMasterRequest FromDict(JsonData data)
        {
            return new CreateRateModelMasterRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                metadata = data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString(): null,
                timingType = data.Keys.Contains("timingType") && data["timingType"] != null ? data["timingType"].ToString(): null,
                lockTime = data.Keys.Contains("lockTime") && data["lockTime"] != null ? (int?)int.Parse(data["lockTime"].ToString()) : null,
                enableSkip = data.Keys.Contains("enableSkip") && data["enableSkip"] != null ? (bool?)bool.Parse(data["enableSkip"].ToString()) : null,
                skipConsumeActions = data.Keys.Contains("skipConsumeActions") && data["skipConsumeActions"] != null ? data["skipConsumeActions"].Cast<JsonData>().Select(value =>
                    {
                        return ConsumeAction.FromDict(value);
                    }
                ).ToList() : null,
                acquireActions = data.Keys.Contains("acquireActions") && data["acquireActions"] != null ? data["acquireActions"].Cast<JsonData>().Select(value =>
                    {
                        return AcquireAction.FromDict(value);
                    }
                ).ToList() : null,
                consumeActions = data.Keys.Contains("consumeActions") && data["consumeActions"] != null ? data["consumeActions"].Cast<JsonData>().Select(value =>
                    {
                        return ConsumeAction.FromDict(value);
                    }
                ).ToList() : null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["name"] = name;
            data["description"] = description;
            data["metadata"] = metadata;
            data["timingType"] = timingType;
            data["lockTime"] = lockTime;
            data["enableSkip"] = enableSkip;
            data["skipConsumeActions"] = new JsonData(skipConsumeActions.Select(item => item.ToDict()));
            data["acquireActions"] = new JsonData(acquireActions.Select(item => item.ToDict()));
            data["consumeActions"] = new JsonData(consumeActions.Select(item => item.ToDict()));
            return data;
        }
	}
}