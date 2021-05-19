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
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Log.Request
{
	[Preserve]
	[System.Serializable]
	public class CountAccessLogRequest : Gs2Request<CountAccessLogRequest>
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
        public CountAccessLogRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** マイクロサービスの種類を集計軸に使用するか */
		[UnityEngine.SerializeField]
        public bool? service;

        /**
         * マイクロサービスの種類を集計軸に使用するかを設定
         *
         * @param service マイクロサービスの種類を集計軸に使用するか
         * @return this
         */
        public CountAccessLogRequest WithService(bool? service) {
            this.service = service;
            return this;
        }


        /** マイクロサービスのメソッドを集計軸に使用するか */
		[UnityEngine.SerializeField]
        public bool? method;

        /**
         * マイクロサービスのメソッドを集計軸に使用するかを設定
         *
         * @param method マイクロサービスのメソッドを集計軸に使用するか
         * @return this
         */
        public CountAccessLogRequest WithMethod(bool? method) {
            this.method = method;
            return this;
        }


        /** ユーザIDを集計軸に使用するか */
		[UnityEngine.SerializeField]
        public bool? userId;

        /**
         * ユーザIDを集計軸に使用するかを設定
         *
         * @param userId ユーザIDを集計軸に使用するか
         * @return this
         */
        public CountAccessLogRequest WithUserId(bool? userId) {
            this.userId = userId;
            return this;
        }


        /** データの取得を開始する位置を指定するトークン */
		[UnityEngine.SerializeField]
        public string pageToken;

        /**
         * データの取得を開始する位置を指定するトークンを設定
         *
         * @param pageToken データの取得を開始する位置を指定するトークン
         * @return this
         */
        public CountAccessLogRequest WithPageToken(string pageToken) {
            this.pageToken = pageToken;
            return this;
        }


        /** データの取得件数 */
		[UnityEngine.SerializeField]
        public long? limit;

        /**
         * データの取得件数を設定
         *
         * @param limit データの取得件数
         * @return this
         */
        public CountAccessLogRequest WithLimit(long? limit) {
            this.limit = limit;
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
        public CountAccessLogRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static CountAccessLogRequest FromDict(JsonData data)
        {
            return new CountAccessLogRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                service = data.Keys.Contains("service") && data["service"] != null ? (bool?)bool.Parse(data["service"].ToString()) : null,
                method = data.Keys.Contains("method") && data["method"] != null ? (bool?)bool.Parse(data["method"].ToString()) : null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? (bool?)bool.Parse(data["userId"].ToString()) : null,
                pageToken = data.Keys.Contains("pageToken") && data["pageToken"] != null ? data["pageToken"].ToString(): null,
                limit = data.Keys.Contains("limit") && data["limit"] != null ? (long?)long.Parse(data["limit"].ToString()) : null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["service"] = service;
            data["method"] = method;
            data["userId"] = userId;
            data["pageToken"] = pageToken;
            data["limit"] = limit;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}