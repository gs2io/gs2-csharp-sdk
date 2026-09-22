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

namespace Gs2.Gs2Buff.Model.Cache
{
    public static class Gs2Buff
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
                case "dumpUserDataByUserId":
                    Result.DumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkDumpUserDataByUserId":
                    Result.CheckDumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckDumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "cleanUserDataByUserId":
                    Result.CleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkCleanUserDataByUserId":
                    Result.CheckCleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckCleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareImportUserDataByUserId":
                    Result.PrepareImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "importUserDataByUserId":
                    Result.ImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkImportUserDataByUserId":
                    Result.CheckImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBuffEntryModels":
                    Result.DescribeBuffEntryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBuffEntryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBuffEntryModel":
                    Result.GetBuffEntryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBuffEntryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBuffEntryModelMasters":
                    Result.DescribeBuffEntryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBuffEntryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createBuffEntryModelMaster":
                    Result.CreateBuffEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateBuffEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBuffEntryModelMaster":
                    Result.GetBuffEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBuffEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBuffEntryModelMaster":
                    Result.UpdateBuffEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateBuffEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBuffEntryModelMaster":
                    Result.DeleteBuffEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteBuffEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "applyBuff":
                    Result.ApplyBuffResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ApplyBuffRequest.FromJson(requestPayload)
                    );
                    break;
                case "applyBuffByUserId":
                    Result.ApplyBuffByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ApplyBuffByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentBuffMaster":
                    Result.GetCurrentBuffMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentBuffMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentBuffMaster":
                    Result.PreUpdateCurrentBuffMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentBuffMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentBuffMaster":
                    Result.UpdateCurrentBuffMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentBuffMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentBuffMasterFromGitHub":
                    Result.UpdateCurrentBuffMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentBuffMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}