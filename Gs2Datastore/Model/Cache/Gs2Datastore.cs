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

namespace Gs2.Gs2Datastore.Model.Cache
{
    public static class Gs2Datastore
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
                case "dataObject":
                    return Gs2.Gs2Datastore.Model.DataObject.FromJson(JsonMapper.ToObject(payload))
                        ?.PutUserData(cache, namespaceName, userId, timeOffset);
                case "dataObjectHistory":
                    return Gs2.Gs2Datastore.Model.DataObjectHistory.FromJson(JsonMapper.ToObject(payload))
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
                case "dataObject":
                    cache.SetListCached<Gs2.Gs2Datastore.Model.DataObject>(parentKey);
                    return true;
                case "dataObjectHistory":
                    cache.SetListCached<Gs2.Gs2Datastore.Model.DataObjectHistory>(parentKey);
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
                case "describeDataObjects":
                    Result.DescribeDataObjectsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDataObjectsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDataObjectsByUserId":
                    Result.DescribeDataObjectsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDataObjectsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareUpload":
                    Result.PrepareUploadResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareUploadRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareUploadByUserId":
                    Result.PrepareUploadByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareUploadByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateDataObject":
                    Result.UpdateDataObjectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateDataObjectRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateDataObjectByUserId":
                    Result.UpdateDataObjectByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateDataObjectByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareReUpload":
                    Result.PrepareReUploadResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareReUploadRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareReUploadByUserId":
                    Result.PrepareReUploadByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareReUploadByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "doneUpload":
                    Result.DoneUploadResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoneUploadRequest.FromJson(requestPayload)
                    );
                    break;
                case "doneUploadByUserId":
                    Result.DoneUploadByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoneUploadByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteDataObject":
                    Result.DeleteDataObjectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteDataObjectRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteDataObjectByUserId":
                    Result.DeleteDataObjectByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteDataObjectByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownload":
                    Result.PrepareDownloadResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownloadByUserId":
                    Result.PrepareDownloadByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownloadByGeneration":
                    Result.PrepareDownloadByGenerationResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadByGenerationRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownloadByGenerationAndUserId":
                    Result.PrepareDownloadByGenerationAndUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadByGenerationAndUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownloadOwnData":
                    Result.PrepareDownloadOwnDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadOwnDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownloadByUserIdAndDataObjectName":
                    Result.PrepareDownloadByUserIdAndDataObjectNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadByUserIdAndDataObjectNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownloadOwnDataByGeneration":
                    Result.PrepareDownloadOwnDataByGenerationResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadOwnDataByGenerationRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareDownloadByUserIdAndDataObjectNameAndGeneration":
                    Result.PrepareDownloadByUserIdAndDataObjectNameAndGenerationResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest.FromJson(requestPayload)
                    );
                    break;
                case "restoreDataObject":
                    Result.RestoreDataObjectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RestoreDataObjectRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDataObjectHistories":
                    Result.DescribeDataObjectHistoriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDataObjectHistoriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDataObjectHistoriesByUserId":
                    Result.DescribeDataObjectHistoriesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDataObjectHistoriesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDataObjectHistory":
                    Result.GetDataObjectHistoryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDataObjectHistoryRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDataObjectHistoryByUserId":
                    Result.GetDataObjectHistoryByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDataObjectHistoryByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}