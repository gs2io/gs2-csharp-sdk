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

namespace Gs2.Gs2Idle.Model.Cache
{
    public static class Gs2Idle
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
                        Request.DescribeNamespacesRequest.FromJson(requestPayload)
                    );
                    break;
                case "createNamespace":
                    Result.CreateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespaceStatus":
                    Result.GetNamespaceStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetNamespaceStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespace":
                    Result.GetNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNamespace":
                    Result.UpdateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteNamespace":
                    Result.DeleteNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "dumpUserDataByUserId":
                    Result.DumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkDumpUserDataByUserId":
                    Result.CheckDumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckDumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "cleanUserDataByUserId":
                    Result.CleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkCleanUserDataByUserId":
                    Result.CheckCleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckCleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareImportUserDataByUserId":
                    Result.PrepareImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PrepareImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "importUserDataByUserId":
                    Result.ImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkImportUserDataByUserId":
                    Result.CheckImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCategoryModelMasters":
                    Result.DescribeCategoryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCategoryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createCategoryModelMaster":
                    Result.CreateCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCategoryModelMaster":
                    Result.GetCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCategoryModelMaster":
                    Result.UpdateCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCategoryModelMaster":
                    Result.DeleteCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCategoryModels":
                    Result.DescribeCategoryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCategoryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCategoryModel":
                    Result.GetCategoryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCategoryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStatuses":
                    Result.DescribeStatusesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStatusesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStatusesByUserId":
                    Result.DescribeStatusesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStatusesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStatus":
                    Result.GetStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStatusByUserId":
                    Result.GetStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prediction":
                    Result.PredictionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PredictionRequest.FromJson(requestPayload)
                    );
                    break;
                case "predictionByUserId":
                    Result.PredictionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PredictionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "receive":
                    Result.ReceiveResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveByUserId":
                    Result.ReceiveByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "increaseMaximumIdleMinutesByUserId":
                    Result.IncreaseMaximumIdleMinutesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IncreaseMaximumIdleMinutesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaximumIdleMinutes":
                    Result.DecreaseMaximumIdleMinutesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseMaximumIdleMinutesRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaximumIdleMinutesByUserId":
                    Result.DecreaseMaximumIdleMinutesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseMaximumIdleMinutesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setMaximumIdleMinutesByUserId":
                    Result.SetMaximumIdleMinutesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetMaximumIdleMinutesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentCategoryMaster":
                    Result.GetCurrentCategoryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentCategoryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentCategoryMaster":
                    Result.UpdateCurrentCategoryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentCategoryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentCategoryMasterFromGitHub":
                    Result.UpdateCurrentCategoryMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentCategoryMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}