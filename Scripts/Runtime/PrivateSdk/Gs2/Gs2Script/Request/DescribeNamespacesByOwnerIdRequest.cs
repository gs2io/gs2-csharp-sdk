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
using Gs2.Gs2Script.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Script.Request
{
	[Preserve]
	[System.Serializable]
	public class DescribeNamespacesByOwnerIdRequest : Gs2Request<DescribeNamespacesByOwnerIdRequest>
	{

        /** オーナーID */
		[UnityEngine.SerializeField]
        public string ownerId;

        /**
         * オーナーIDを設定
         *
         * @param ownerId オーナーID
         * @return this
         */
        public DescribeNamespacesByOwnerIdRequest WithOwnerId(string ownerId) {
            this.ownerId = ownerId;
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
        public DescribeNamespacesByOwnerIdRequest WithPageToken(string pageToken) {
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
        public DescribeNamespacesByOwnerIdRequest WithLimit(long? limit) {
            this.limit = limit;
            return this;
        }


    	[Preserve]
        public static DescribeNamespacesByOwnerIdRequest FromDict(JsonData data)
        {
            return new DescribeNamespacesByOwnerIdRequest {
                ownerId = data.Keys.Contains("ownerId") && data["ownerId"] != null ? data["ownerId"].ToString(): null,
                pageToken = data.Keys.Contains("pageToken") && data["pageToken"] != null ? data["pageToken"].ToString(): null,
                limit = data.Keys.Contains("limit") && data["limit"] != null ? (long?)long.Parse(data["limit"].ToString()) : null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["ownerId"] = ownerId;
            data["pageToken"] = pageToken;
            data["limit"] = limit;
            return data;
        }
	}
}