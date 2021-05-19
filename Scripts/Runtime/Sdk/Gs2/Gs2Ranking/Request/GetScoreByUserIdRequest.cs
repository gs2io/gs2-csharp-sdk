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
using Gs2.Gs2Ranking.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Ranking.Request
{
	[Preserve]
	[System.Serializable]
	public class GetScoreByUserIdRequest : Gs2Request<GetScoreByUserIdRequest>
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
        public GetScoreByUserIdRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** カテゴリ名 */
		[UnityEngine.SerializeField]
        public string categoryName;

        /**
         * カテゴリ名を設定
         *
         * @param categoryName カテゴリ名
         * @return this
         */
        public GetScoreByUserIdRequest WithCategoryName(string categoryName) {
            this.categoryName = categoryName;
            return this;
        }


        /** ユーザID */
		[UnityEngine.SerializeField]
        public string userId;

        /**
         * ユーザIDを設定
         *
         * @param userId ユーザID
         * @return this
         */
        public GetScoreByUserIdRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** スコアを獲得したユーザID */
		[UnityEngine.SerializeField]
        public string scorerUserId;

        /**
         * スコアを獲得したユーザIDを設定
         *
         * @param scorerUserId スコアを獲得したユーザID
         * @return this
         */
        public GetScoreByUserIdRequest WithScorerUserId(string scorerUserId) {
            this.scorerUserId = scorerUserId;
            return this;
        }


        /** スコアのユニークID */
		[UnityEngine.SerializeField]
        public string uniqueId;

        /**
         * スコアのユニークIDを設定
         *
         * @param uniqueId スコアのユニークID
         * @return this
         */
        public GetScoreByUserIdRequest WithUniqueId(string uniqueId) {
            this.uniqueId = uniqueId;
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
        public GetScoreByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static GetScoreByUserIdRequest FromDict(JsonData data)
        {
            return new GetScoreByUserIdRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                categoryName = data.Keys.Contains("categoryName") && data["categoryName"] != null ? data["categoryName"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                scorerUserId = data.Keys.Contains("scorerUserId") && data["scorerUserId"] != null ? data["scorerUserId"].ToString(): null,
                uniqueId = data.Keys.Contains("uniqueId") && data["uniqueId"] != null ? data["uniqueId"].ToString(): null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["categoryName"] = categoryName;
            data["userId"] = userId;
            data["scorerUserId"] = scorerUserId;
            data["uniqueId"] = uniqueId;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}