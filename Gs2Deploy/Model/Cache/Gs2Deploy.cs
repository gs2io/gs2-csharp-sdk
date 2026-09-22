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

namespace Gs2.Gs2Deploy.Model.Cache
{
    public static class Gs2Deploy
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
                case "describeStacks":
                    Result.DescribeStacksResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeStacksRequest.FromJson(requestPayload)
                    );
                    break;
                case "preCreateStack":
                    Result.PreCreateStackResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreCreateStackRequest.FromJson(requestPayload)
                    );
                    break;
                case "createStack":
                    Result.CreateStackResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateStackRequest.FromJson(requestPayload)
                    );
                    break;
                case "createStackFromGitHub":
                    Result.CreateStackFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateStackFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "preValidate":
                    Result.PreValidateResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreValidateRequest.FromJson(requestPayload)
                    );
                    break;
                case "validate":
                    Result.ValidateResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ValidateRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStackStatus":
                    Result.GetStackStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStackStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStack":
                    Result.GetStackResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStackRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateStack":
                    Result.PreUpdateStackResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateStackRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStack":
                    Result.UpdateStackResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateStackRequest.FromJson(requestPayload)
                    );
                    break;
                case "preChangeSet":
                    Result.PreChangeSetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreChangeSetRequest.FromJson(requestPayload)
                    );
                    break;
                case "changeSet":
                    Result.ChangeSetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ChangeSetRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStackFromGitHub":
                    Result.UpdateStackFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateStackFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStack":
                    Result.DeleteStackResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteStackRequest.FromJson(requestPayload)
                    );
                    break;
                case "forceDeleteStack":
                    Result.ForceDeleteStackResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ForceDeleteStackRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStackResources":
                    Result.DeleteStackResourcesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteStackResourcesRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStackEntity":
                    Result.DeleteStackEntityResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteStackEntityRequest.FromJson(requestPayload)
                    );
                    break;
                case "getServiceVersion":
                    Result.GetServiceVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetServiceVersionRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeResources":
                    Result.DescribeResourcesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeResourcesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getResource":
                    Result.GetResourceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetResourceRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEvents":
                    Result.DescribeEventsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeEventsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEvent":
                    Result.GetEventResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetEventRequest.FromJson(requestPayload)
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