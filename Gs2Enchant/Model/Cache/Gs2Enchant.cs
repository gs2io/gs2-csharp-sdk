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

namespace Gs2.Gs2Enchant.Model.Cache
{
    public static class Gs2Enchant
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
                case "describeBalanceParameterModels":
                    Result.DescribeBalanceParameterModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBalanceParameterModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBalanceParameterModel":
                    Result.GetBalanceParameterModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBalanceParameterModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBalanceParameterModelMasters":
                    Result.DescribeBalanceParameterModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBalanceParameterModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createBalanceParameterModelMaster":
                    Result.CreateBalanceParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateBalanceParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBalanceParameterModelMaster":
                    Result.GetBalanceParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBalanceParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBalanceParameterModelMaster":
                    Result.UpdateBalanceParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateBalanceParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBalanceParameterModelMaster":
                    Result.DeleteBalanceParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteBalanceParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRarityParameterModels":
                    Result.DescribeRarityParameterModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRarityParameterModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRarityParameterModel":
                    Result.GetRarityParameterModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRarityParameterModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRarityParameterModelMasters":
                    Result.DescribeRarityParameterModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRarityParameterModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRarityParameterModelMaster":
                    Result.CreateRarityParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRarityParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRarityParameterModelMaster":
                    Result.GetRarityParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRarityParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRarityParameterModelMaster":
                    Result.UpdateRarityParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRarityParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRarityParameterModelMaster":
                    Result.DeleteRarityParameterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRarityParameterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentParameterMaster":
                    Result.GetCurrentParameterMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentParameterMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentParameterMaster":
                    Result.UpdateCurrentParameterMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentParameterMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentParameterMasterFromGitHub":
                    Result.UpdateCurrentParameterMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentParameterMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBalanceParameterStatuses":
                    Result.DescribeBalanceParameterStatusesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBalanceParameterStatusesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBalanceParameterStatusesByUserId":
                    Result.DescribeBalanceParameterStatusesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBalanceParameterStatusesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBalanceParameterStatus":
                    Result.GetBalanceParameterStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBalanceParameterStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBalanceParameterStatusByUserId":
                    Result.GetBalanceParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBalanceParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBalanceParameterStatusByUserId":
                    Result.DeleteBalanceParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteBalanceParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "reDrawBalanceParameterStatusByUserId":
                    Result.ReDrawBalanceParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReDrawBalanceParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setBalanceParameterStatusByUserId":
                    Result.SetBalanceParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetBalanceParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRarityParameterStatuses":
                    Result.DescribeRarityParameterStatusesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRarityParameterStatusesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRarityParameterStatusesByUserId":
                    Result.DescribeRarityParameterStatusesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRarityParameterStatusesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRarityParameterStatus":
                    Result.GetRarityParameterStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRarityParameterStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRarityParameterStatusByUserId":
                    Result.GetRarityParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRarityParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRarityParameterStatusByUserId":
                    Result.DeleteRarityParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRarityParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "reDrawRarityParameterStatusByUserId":
                    Result.ReDrawRarityParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReDrawRarityParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addRarityParameterStatusByUserId":
                    Result.AddRarityParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddRarityParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyRarityParameterStatus":
                    Result.VerifyRarityParameterStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyRarityParameterStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyRarityParameterStatusByUserId":
                    Result.VerifyRarityParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyRarityParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setRarityParameterStatusByUserId":
                    Result.SetRarityParameterStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetRarityParameterStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}