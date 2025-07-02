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

namespace Gs2.Gs2Money2.Model.Cache
{
    public static class Gs2Money2
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
                case "describeWallets":
                    Result.DescribeWalletsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeWalletsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeWalletsByUserId":
                    Result.DescribeWalletsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeWalletsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getWallet":
                    Result.GetWalletResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetWalletRequest.FromJson(requestPayload)
                    );
                    break;
                case "getWalletByUserId":
                    Result.GetWalletByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetWalletByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "depositByUserId":
                    Result.DepositByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DepositByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdraw":
                    Result.WithdrawResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WithdrawRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdrawByUserId":
                    Result.WithdrawByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WithdrawByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEventsByUserId":
                    Result.DescribeEventsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeEventsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEventByTransactionId":
                    Result.GetEventByTransactionIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetEventByTransactionIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyReceipt":
                    Result.VerifyReceiptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyReceiptRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyReceiptByUserId":
                    Result.VerifyReceiptByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyReceiptByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscriptionStatuses":
                    Result.DescribeSubscriptionStatusesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSubscriptionStatusesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscriptionStatusesByUserId":
                    Result.DescribeSubscriptionStatusesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSubscriptionStatusesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscriptionStatus":
                    Result.GetSubscriptionStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSubscriptionStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscriptionStatusByUserId":
                    Result.GetSubscriptionStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSubscriptionStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "allocateSubscriptionStatus":
                    Result.AllocateSubscriptionStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AllocateSubscriptionStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "allocateSubscriptionStatusByUserId":
                    Result.AllocateSubscriptionStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AllocateSubscriptionStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "takeoverSubscriptionStatus":
                    Result.TakeoverSubscriptionStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.TakeoverSubscriptionStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "takeoverSubscriptionStatusByUserId":
                    Result.TakeoverSubscriptionStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.TakeoverSubscriptionStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRefundHistoriesByUserId":
                    Result.DescribeRefundHistoriesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRefundHistoriesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRefundHistoriesByDate":
                    Result.DescribeRefundHistoriesByDateResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRefundHistoriesByDateRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRefundHistory":
                    Result.GetRefundHistoryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRefundHistoryRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreContentModels":
                    Result.DescribeStoreContentModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeStoreContentModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreContentModel":
                    Result.GetStoreContentModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStoreContentModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreContentModelMasters":
                    Result.DescribeStoreContentModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeStoreContentModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createStoreContentModelMaster":
                    Result.CreateStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreContentModelMaster":
                    Result.GetStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStoreContentModelMaster":
                    Result.UpdateStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStoreContentModelMaster":
                    Result.DeleteStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreSubscriptionContentModels":
                    Result.DescribeStoreSubscriptionContentModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeStoreSubscriptionContentModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreSubscriptionContentModel":
                    Result.GetStoreSubscriptionContentModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStoreSubscriptionContentModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreSubscriptionContentModelMasters":
                    Result.DescribeStoreSubscriptionContentModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeStoreSubscriptionContentModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createStoreSubscriptionContentModelMaster":
                    Result.CreateStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreSubscriptionContentModelMaster":
                    Result.GetStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStoreSubscriptionContentModelMaster":
                    Result.UpdateStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStoreSubscriptionContentModelMaster":
                    Result.DeleteStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
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
                case "getCurrentModelMaster":
                    Result.GetCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentModelMaster":
                    Result.PreUpdateCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMaster":
                    Result.UpdateCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMasterFromGitHub":
                    Result.UpdateCurrentModelMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentModelMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDailyTransactionHistoriesByCurrency":
                    Result.DescribeDailyTransactionHistoriesByCurrencyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDailyTransactionHistoriesByCurrencyRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDailyTransactionHistories":
                    Result.DescribeDailyTransactionHistoriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDailyTransactionHistoriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDailyTransactionHistory":
                    Result.GetDailyTransactionHistoryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDailyTransactionHistoryRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeUnusedBalances":
                    Result.DescribeUnusedBalancesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeUnusedBalancesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getUnusedBalance":
                    Result.GetUnusedBalanceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetUnusedBalanceRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}