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


        /** マイクロサービスの種類 */
		[UnityEngine.SerializeField]
        public string service;

        /**
         * マイクロサービスの種類を設定
         *
         * @param service マイクロサービスの種類
         * @return this
         */
        public CountAccessLogRequest WithService(string service) {
            this.service = service;
            return this;
        }


        /** マイクロサービスのメソッド */
		[UnityEngine.SerializeField]
        public string method;

        /**
         * マイクロサービスのメソッドを設定
         *
         * @param method マイクロサービスのメソッド
         * @return this
         */
        public CountAccessLogRequest WithMethod(string method) {
            this.method = method;
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
        public CountAccessLogRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** 検索範囲開始日時 */
		[UnityEngine.SerializeField]
        public long? begin;

        /**
         * 検索範囲開始日時を設定
         *
         * @param begin 検索範囲開始日時
         * @return this
         */
        public CountAccessLogRequest WithBegin(long? begin) {
            this.begin = begin;
            return this;
        }


        /** 検索範囲終了日時 */
		[UnityEngine.SerializeField]
        public long? end;

        /**
         * 検索範囲終了日時を設定
         *
         * @param end 検索範囲終了日時
         * @return this
         */
        public CountAccessLogRequest WithEnd(long? end) {
            this.end = end;
            return this;
        }


        /** 7日より長い期間のログを検索対象とするか */
		[UnityEngine.SerializeField]
        public bool? longTerm;

        /**
         * 7日より長い期間のログを検索対象とするかを設定
         *
         * @param longTerm 7日より長い期間のログを検索対象とするか
         * @return this
         */
        public CountAccessLogRequest WithLongTerm(bool? longTerm) {
            this.longTerm = longTerm;
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
                service = data.Keys.Contains("service") && data["service"] != null ? data["service"].ToString(): null,
                method = data.Keys.Contains("method") && data["method"] != null ? data["method"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                begin = data.Keys.Contains("begin") && data["begin"] != null ? (long?)long.Parse(data["begin"].ToString()) : null,
                end = data.Keys.Contains("end") && data["end"] != null ? (long?)long.Parse(data["end"].ToString()) : null,
                longTerm = data.Keys.Contains("longTerm") && data["longTerm"] != null ? (bool?)bool.Parse(data["longTerm"].ToString()) : null,
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
            data["begin"] = begin;
            data["end"] = end;
            data["longTerm"] = longTerm;
            data["pageToken"] = pageToken;
            data["limit"] = limit;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}