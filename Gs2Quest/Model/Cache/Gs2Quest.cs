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

using Gs2.Core.Domain;

namespace Gs2.Gs2Quest.Model.Cache
{
    public static class Gs2Quest
    {
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
                case "describeQuestGroupModelMasters":
                    Result.DescribeQuestGroupModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeQuestGroupModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createQuestGroupModelMaster":
                    Result.CreateQuestGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateQuestGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getQuestGroupModelMaster":
                    Result.GetQuestGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetQuestGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateQuestGroupModelMaster":
                    Result.UpdateQuestGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateQuestGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteQuestGroupModelMaster":
                    Result.DeleteQuestGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteQuestGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeQuestModelMasters":
                    Result.DescribeQuestModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeQuestModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createQuestModelMaster":
                    Result.CreateQuestModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateQuestModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getQuestModelMaster":
                    Result.GetQuestModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetQuestModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateQuestModelMaster":
                    Result.UpdateQuestModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateQuestModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteQuestModelMaster":
                    Result.DeleteQuestModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteQuestModelMasterRequest.FromJson(requestPayload)
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
                case "getCurrentQuestMaster":
                    Result.GetCurrentQuestMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentQuestMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentQuestMaster":
                    Result.PreUpdateCurrentQuestMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentQuestMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentQuestMaster":
                    Result.UpdateCurrentQuestMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentQuestMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentQuestMasterFromGitHub":
                    Result.UpdateCurrentQuestMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentQuestMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeProgressesByUserId":
                    Result.DescribeProgressesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeProgressesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createProgressByUserId":
                    Result.CreateProgressByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateProgressByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProgress":
                    Result.GetProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProgressByUserId":
                    Result.GetProgressByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetProgressByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "start":
                    Result.StartResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.StartRequest.FromJson(requestPayload)
                    );
                    break;
                case "startByUserId":
                    Result.StartByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.StartByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "end":
                    Result.EndResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EndRequest.FromJson(requestPayload)
                    );
                    break;
                case "endByUserId":
                    Result.EndByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EndByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteProgress":
                    Result.DeleteProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteProgressByUserId":
                    Result.DeleteProgressByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteProgressByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCompletedQuestLists":
                    Result.DescribeCompletedQuestListsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeCompletedQuestListsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCompletedQuestListsByUserId":
                    Result.DescribeCompletedQuestListsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeCompletedQuestListsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCompletedQuestList":
                    Result.GetCompletedQuestListResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCompletedQuestListRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCompletedQuestListByUserId":
                    Result.GetCompletedQuestListByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCompletedQuestListByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCompletedQuestListByUserId":
                    Result.DeleteCompletedQuestListByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteCompletedQuestListByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeQuestGroupModels":
                    Result.DescribeQuestGroupModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeQuestGroupModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getQuestGroupModel":
                    Result.GetQuestGroupModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetQuestGroupModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeQuestModels":
                    Result.DescribeQuestModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeQuestModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getQuestModel":
                    Result.GetQuestModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetQuestModelRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}