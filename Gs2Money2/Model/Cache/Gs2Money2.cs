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
                case "describeWallets":
                    Result.DescribeWalletsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeWalletsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeWalletsByUserId":
                    Result.DescribeWalletsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeWalletsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getWallet":
                    Result.GetWalletResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetWalletRequest.FromJson(requestPayload)
                    );
                    break;
                case "getWalletByUserId":
                    Result.GetWalletByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetWalletByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "depositByUserId":
                    Result.DepositByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DepositByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdraw":
                    Result.WithdrawResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WithdrawRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdrawByUserId":
                    Result.WithdrawByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WithdrawByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEventsByUserId":
                    Result.DescribeEventsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEventsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEventByTransactionId":
                    Result.GetEventByTransactionIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEventByTransactionIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyReceipt":
                    Result.VerifyReceiptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyReceiptRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyReceiptByUserId":
                    Result.VerifyReceiptByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyReceiptByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscriptionStatuses":
                    Result.DescribeSubscriptionStatusesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscriptionStatusesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscriptionStatusesByUserId":
                    Result.DescribeSubscriptionStatusesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscriptionStatusesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscriptionStatus":
                    Result.GetSubscriptionStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscriptionStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscriptionStatusByUserId":
                    Result.GetSubscriptionStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscriptionStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreContentModels":
                    Result.DescribeStoreContentModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStoreContentModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreContentModel":
                    Result.GetStoreContentModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStoreContentModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreContentModelMasters":
                    Result.DescribeStoreContentModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStoreContentModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createStoreContentModelMaster":
                    Result.CreateStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreContentModelMaster":
                    Result.GetStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStoreContentModelMaster":
                    Result.UpdateStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStoreContentModelMaster":
                    Result.DeleteStoreContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStoreContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreSubscriptionContentModels":
                    Result.DescribeStoreSubscriptionContentModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStoreSubscriptionContentModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreSubscriptionContentModel":
                    Result.GetStoreSubscriptionContentModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStoreSubscriptionContentModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStoreSubscriptionContentModelMasters":
                    Result.DescribeStoreSubscriptionContentModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStoreSubscriptionContentModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createStoreSubscriptionContentModelMaster":
                    Result.CreateStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStoreSubscriptionContentModelMaster":
                    Result.GetStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStoreSubscriptionContentModelMaster":
                    Result.UpdateStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStoreSubscriptionContentModelMaster":
                    Result.DeleteStoreSubscriptionContentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStoreSubscriptionContentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentModelMaster":
                    Result.GetCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMaster":
                    Result.UpdateCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMasterFromGitHub":
                    Result.UpdateCurrentModelMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentModelMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDailyTransactionHistoriesByCurrency":
                    Result.DescribeDailyTransactionHistoriesByCurrencyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeDailyTransactionHistoriesByCurrencyRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDailyTransactionHistories":
                    Result.DescribeDailyTransactionHistoriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeDailyTransactionHistoriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDailyTransactionHistory":
                    Result.GetDailyTransactionHistoryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetDailyTransactionHistoryRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeUnusedBalances":
                    Result.DescribeUnusedBalancesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeUnusedBalancesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getUnusedBalance":
                    Result.GetUnusedBalanceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetUnusedBalanceRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}