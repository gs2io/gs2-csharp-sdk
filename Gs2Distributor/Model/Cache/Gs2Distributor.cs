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

namespace Gs2.Gs2Distributor.Model.Cache
{
    public static class Gs2Distributor
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
                case "describeDistributorModelMasters":
                    Result.DescribeDistributorModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDistributorModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createDistributorModelMaster":
                    Result.CreateDistributorModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateDistributorModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDistributorModelMaster":
                    Result.GetDistributorModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDistributorModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateDistributorModelMaster":
                    Result.UpdateDistributorModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateDistributorModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteDistributorModelMaster":
                    Result.DeleteDistributorModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteDistributorModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDistributorModels":
                    Result.DescribeDistributorModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDistributorModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDistributorModel":
                    Result.GetDistributorModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDistributorModelRequest.FromJson(requestPayload)
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
                case "getCurrentDistributorMaster":
                    Result.GetCurrentDistributorMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentDistributorMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentDistributorMaster":
                    Result.PreUpdateCurrentDistributorMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentDistributorMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentDistributorMaster":
                    Result.UpdateCurrentDistributorMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentDistributorMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentDistributorMasterFromGitHub":
                    Result.UpdateCurrentDistributorMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentDistributorMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "distribute":
                    Result.DistributeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DistributeRequest.FromJson(requestPayload)
                    );
                    break;
                case "distributeWithoutOverflowProcess":
                    Result.DistributeWithoutOverflowProcessResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DistributeWithoutOverflowProcessRequest.FromJson(requestPayload)
                    );
                    break;
                case "runVerifyTask":
                    Result.RunVerifyTaskResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunVerifyTaskRequest.FromJson(requestPayload)
                    );
                    break;
                case "runStampTask":
                    Result.RunStampTaskResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunStampTaskRequest.FromJson(requestPayload)
                    );
                    break;
                case "runStampSheet":
                    Result.RunStampSheetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunStampSheetRequest.FromJson(requestPayload)
                    );
                    break;
                case "runStampSheetExpress":
                    Result.RunStampSheetExpressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunStampSheetExpressRequest.FromJson(requestPayload)
                    );
                    break;
                case "runVerifyTaskWithoutNamespace":
                    Result.RunVerifyTaskWithoutNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunVerifyTaskWithoutNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "runStampTaskWithoutNamespace":
                    Result.RunStampTaskWithoutNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunStampTaskWithoutNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "runStampSheetWithoutNamespace":
                    Result.RunStampSheetWithoutNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunStampSheetWithoutNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "runStampSheetExpressWithoutNamespace":
                    Result.RunStampSheetExpressWithoutNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunStampSheetExpressWithoutNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "setTransactionDefaultConfig":
                    Result.SetTransactionDefaultConfigResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetTransactionDefaultConfigRequest.FromJson(requestPayload)
                    );
                    break;
                case "setTransactionDefaultConfigByUserId":
                    Result.SetTransactionDefaultConfigByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetTransactionDefaultConfigByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "freezeMasterData":
                    Result.FreezeMasterDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FreezeMasterDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "freezeMasterDataByUserId":
                    Result.FreezeMasterDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FreezeMasterDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "signFreezeMasterDataTimestamp":
                    Result.SignFreezeMasterDataTimestampResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SignFreezeMasterDataTimestampRequest.FromJson(requestPayload)
                    );
                    break;
                case "freezeMasterDataBySignedTimestamp":
                    Result.FreezeMasterDataBySignedTimestampResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FreezeMasterDataBySignedTimestampRequest.FromJson(requestPayload)
                    );
                    break;
                case "freezeMasterDataByTimestamp":
                    Result.FreezeMasterDataByTimestampResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FreezeMasterDataByTimestampRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchExecuteApi":
                    Result.BatchExecuteApiResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.BatchExecuteApiRequest.FromJson(requestPayload)
                    );
                    break;
                case "ifExpressionByUserId":
                    Result.IfExpressionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.IfExpressionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "andExpressionByUserId":
                    Result.AndExpressionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AndExpressionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "orExpressionByUserId":
                    Result.OrExpressionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.OrExpressionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStampSheetResult":
                    Result.GetStampSheetResultResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStampSheetResultRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStampSheetResultByUserId":
                    Result.GetStampSheetResultByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStampSheetResultByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "runTransaction":
                    Result.RunTransactionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RunTransactionRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTransactionResult":
                    Result.GetTransactionResultResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetTransactionResultRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTransactionResultByUserId":
                    Result.GetTransactionResultByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetTransactionResultByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}