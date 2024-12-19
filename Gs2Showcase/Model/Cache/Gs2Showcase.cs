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

namespace Gs2.Gs2Showcase.Model.Cache
{
    public static class Gs2Showcase
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
                case "describeSalesItemMasters":
                    Result.DescribeSalesItemMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSalesItemMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSalesItemMaster":
                    Result.CreateSalesItemMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateSalesItemMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSalesItemMaster":
                    Result.GetSalesItemMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSalesItemMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSalesItemMaster":
                    Result.UpdateSalesItemMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateSalesItemMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSalesItemMaster":
                    Result.DeleteSalesItemMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSalesItemMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSalesItemGroupMasters":
                    Result.DescribeSalesItemGroupMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSalesItemGroupMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSalesItemGroupMaster":
                    Result.CreateSalesItemGroupMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateSalesItemGroupMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSalesItemGroupMaster":
                    Result.GetSalesItemGroupMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSalesItemGroupMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSalesItemGroupMaster":
                    Result.UpdateSalesItemGroupMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateSalesItemGroupMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSalesItemGroupMaster":
                    Result.DeleteSalesItemGroupMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSalesItemGroupMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeShowcaseMasters":
                    Result.DescribeShowcaseMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeShowcaseMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createShowcaseMaster":
                    Result.CreateShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getShowcaseMaster":
                    Result.GetShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateShowcaseMaster":
                    Result.UpdateShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteShowcaseMaster":
                    Result.DeleteShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentShowcaseMaster":
                    Result.GetCurrentShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentShowcaseMaster":
                    Result.UpdateCurrentShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentShowcaseMasterFromGitHub":
                    Result.UpdateCurrentShowcaseMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentShowcaseMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeShowcases":
                    Result.DescribeShowcasesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeShowcasesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeShowcasesByUserId":
                    Result.DescribeShowcasesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeShowcasesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getShowcase":
                    Result.GetShowcaseResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetShowcaseRequest.FromJson(requestPayload)
                    );
                    break;
                case "getShowcaseByUserId":
                    Result.GetShowcaseByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetShowcaseByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "buy":
                    Result.BuyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.BuyRequest.FromJson(requestPayload)
                    );
                    break;
                case "buyByUserId":
                    Result.BuyByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.BuyByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRandomShowcaseMasters":
                    Result.DescribeRandomShowcaseMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRandomShowcaseMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRandomShowcaseMaster":
                    Result.CreateRandomShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRandomShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRandomShowcaseMaster":
                    Result.GetRandomShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRandomShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRandomShowcaseMaster":
                    Result.UpdateRandomShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRandomShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRandomShowcaseMaster":
                    Result.DeleteRandomShowcaseMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRandomShowcaseMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "incrementPurchaseCount":
                    Result.IncrementPurchaseCountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IncrementPurchaseCountRequest.FromJson(requestPayload)
                    );
                    break;
                case "incrementPurchaseCountByUserId":
                    Result.IncrementPurchaseCountByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IncrementPurchaseCountByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "decrementPurchaseCountByUserId":
                    Result.DecrementPurchaseCountByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecrementPurchaseCountByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "forceReDrawByUserId":
                    Result.ForceReDrawByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ForceReDrawByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRandomDisplayItems":
                    Result.DescribeRandomDisplayItemsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRandomDisplayItemsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRandomDisplayItemsByUserId":
                    Result.DescribeRandomDisplayItemsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRandomDisplayItemsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRandomDisplayItem":
                    Result.GetRandomDisplayItemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRandomDisplayItemRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRandomDisplayItemByUserId":
                    Result.GetRandomDisplayItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRandomDisplayItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "randomShowcaseBuy":
                    Result.RandomShowcaseBuyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RandomShowcaseBuyRequest.FromJson(requestPayload)
                    );
                    break;
                case "randomShowcaseBuyByUserId":
                    Result.RandomShowcaseBuyByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RandomShowcaseBuyByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}