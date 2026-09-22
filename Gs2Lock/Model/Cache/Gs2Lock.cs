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

namespace Gs2.Gs2Lock.Model.Cache
{
    public static class Gs2Lock
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
                case "mutex":
                    return Gs2.Gs2Lock.Model.Mutex.FromJson(JsonMapper.ToObject(payload))
                        ?.PutUserData(cache, namespaceName, userId, timeOffset);
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
                case "mutex":
                    cache.SetListCached<Gs2.Gs2Lock.Model.Mutex>(parentKey);
                    return true;
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
                case "describeNamespaces":
                    Result.DescribeNamespacesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeNamespacesRequest.FromJson(requestPayload)
                    );
                    break;
                case "createNamespace":
                    Result.CreateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespaceStatus":
                    Result.GetNamespaceStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetNamespaceStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespace":
                    Result.GetNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNamespace":
                    Result.UpdateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteNamespace":
                    Result.DeleteNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteNamespaceRequest.FromJson(requestPayload)
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
                case "lock":
                    Result.LockResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.LockRequest.FromJson(requestPayload)
                    );
                    break;
                case "lockByUserId":
                    Result.LockByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.LockByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unlock":
                    Result.UnlockResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UnlockRequest.FromJson(requestPayload)
                    );
                    break;
                case "unlockByUserId":
                    Result.UnlockByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UnlockByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMutex":
                    Result.GetMutexResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetMutexRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMutexByUserId":
                    Result.GetMutexByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetMutexByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMutexByUserId":
                    Result.DeleteMutexByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteMutexByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}