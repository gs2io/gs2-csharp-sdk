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
using Gs2.Gs2Friend.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Friend.Request
{
	[Preserve]
	[System.Serializable]
	public class DescribeFriendsByUserIdRequest : Gs2Request<DescribeFriendsByUserIdRequest>
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
        public DescribeFriendsByUserIdRequest WithNamespaceName(string namespaceName) {
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
        public DescribeFriendsByUserIdRequest WithUserId(string userId) {
            this.userId = userId;
            return this;
        }


        /** プロフィールも一緒に取得するか */
		[UnityEngine.SerializeField]
        public bool? withProfile;

        /**
         * プロフィールも一緒に取得するかを設定
         *
         * @param withProfile プロフィールも一緒に取得するか
         * @return this
         */
        public DescribeFriendsByUserIdRequest WithWithProfile(bool? withProfile) {
            this.withProfile = withProfile;
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
        public DescribeFriendsByUserIdRequest WithPageToken(string pageToken) {
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
        public DescribeFriendsByUserIdRequest WithLimit(long? limit) {
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
        public DescribeFriendsByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


    	[Preserve]
        public static DescribeFriendsByUserIdRequest FromDict(JsonData data)
        {
            return new DescribeFriendsByUserIdRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                userId = data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString(): null,
                withProfile = data.Keys.Contains("withProfile") && data["withProfile"] != null ? (bool?)bool.Parse(data["withProfile"].ToString()) : null,
                pageToken = data.Keys.Contains("pageToken") && data["pageToken"] != null ? data["pageToken"].ToString(): null,
                limit = data.Keys.Contains("limit") && data["limit"] != null ? (long?)long.Parse(data["limit"].ToString()) : null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["userId"] = userId;
            data["withProfile"] = withProfile;
            data["pageToken"] = pageToken;
            data["limit"] = limit;
            data["duplicationAvoider"] = duplicationAvoider;
            return data;
        }
	}
}