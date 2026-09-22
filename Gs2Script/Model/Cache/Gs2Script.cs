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

namespace Gs2.Gs2Script.Model.Cache
{
    public static class Gs2Script
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
                case "describeScripts":
                    Result.DescribeScriptsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeScriptsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createScript":
                    Result.CreateScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "createScriptFromGitHub":
                    Result.CreateScriptFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateScriptFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "getScript":
                    Result.GetScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateScript":
                    Result.UpdateScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateScriptFromGitHub":
                    Result.UpdateScriptFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateScriptFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteScript":
                    Result.DeleteScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "invokeScript":
                    Result.InvokeScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.InvokeScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "debugInvoke":
                    Result.DebugInvokeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DebugInvokeRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}