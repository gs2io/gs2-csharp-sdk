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

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System.Linq;
using Gs2.Core.Domain;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Freeze.Model.Cache
{
    public static class Gs2Freeze
    {
        /// <summary>
        /// Gs2Distributor:DescribeUserData（ユーザーの全データの一括取得）の 1 エントリを、kind に対応するモデルのキャッシュへ入れる。
        /// 戻り値は親キー（null は知らない kind ＝ SDK が古い / 対応表に無い）。呼び手は全ページを読み終えてから
        /// SetListCached(cache, timeOffset, kind, parentKey) を呼ぶ。対応表は sdk-gen の type/user_data_cache.py（kind → モデル）。
        /// </summary>
        public static string PutUserData(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            string kind,
            string payload
        ) {
            switch (kind) {
            }
            return null;
        }

        /// <summary>
        /// 一括取得で入れた kind の親キーに「リストが揃った印」を立てる（Describe のイテレータがサーバーへ出なくなる）。
        /// </summary>
        public static bool SetListCached(
            CacheDatabase cache,
            int? timeOffset,
            string kind,
            string parentKey
        ) {
            switch (kind) {
            }
            return false;
        }

        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "describeStages":
                    Result.DescribeStagesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeStagesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStage":
                    Result.GetStageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStageRequest.FromJson(requestPayload)
                    );
                    break;
                case "promoteStage":
                    Result.PromoteStageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PromoteStageRequest.FromJson(requestPayload)
                    );
                    break;
                case "rollbackStage":
                    Result.RollbackStageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RollbackStageRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeOutputs":
                    Result.DescribeOutputsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeOutputsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getOutput":
                    Result.GetOutputResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetOutputRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}