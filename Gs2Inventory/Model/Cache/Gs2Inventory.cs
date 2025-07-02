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

namespace Gs2.Gs2Inventory.Model.Cache
{
    public static class Gs2Inventory
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
                case "describeInventoryModelMasters":
                    Result.DescribeInventoryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeInventoryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createInventoryModelMaster":
                    Result.CreateInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getInventoryModelMaster":
                    Result.GetInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateInventoryModelMaster":
                    Result.UpdateInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteInventoryModelMaster":
                    Result.DeleteInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeInventoryModels":
                    Result.DescribeInventoryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeInventoryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getInventoryModel":
                    Result.GetInventoryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetInventoryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeItemModelMasters":
                    Result.DescribeItemModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeItemModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createItemModelMaster":
                    Result.CreateItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getItemModelMaster":
                    Result.GetItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateItemModelMaster":
                    Result.UpdateItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteItemModelMaster":
                    Result.DeleteItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeItemModels":
                    Result.DescribeItemModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeItemModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getItemModel":
                    Result.GetItemModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetItemModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSimpleInventoryModelMasters":
                    Result.DescribeSimpleInventoryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSimpleInventoryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSimpleInventoryModelMaster":
                    Result.CreateSimpleInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateSimpleInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleInventoryModelMaster":
                    Result.GetSimpleInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSimpleInventoryModelMaster":
                    Result.UpdateSimpleInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateSimpleInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSimpleInventoryModelMaster":
                    Result.DeleteSimpleInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteSimpleInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSimpleInventoryModels":
                    Result.DescribeSimpleInventoryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSimpleInventoryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleInventoryModel":
                    Result.GetSimpleInventoryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleInventoryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSimpleItemModelMasters":
                    Result.DescribeSimpleItemModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSimpleItemModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSimpleItemModelMaster":
                    Result.CreateSimpleItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateSimpleItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleItemModelMaster":
                    Result.GetSimpleItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSimpleItemModelMaster":
                    Result.UpdateSimpleItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateSimpleItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSimpleItemModelMaster":
                    Result.DeleteSimpleItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteSimpleItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSimpleItemModels":
                    Result.DescribeSimpleItemModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSimpleItemModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleItemModel":
                    Result.GetSimpleItemModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleItemModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBigInventoryModelMasters":
                    Result.DescribeBigInventoryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBigInventoryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createBigInventoryModelMaster":
                    Result.CreateBigInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateBigInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBigInventoryModelMaster":
                    Result.GetBigInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBigInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBigInventoryModelMaster":
                    Result.UpdateBigInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateBigInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBigInventoryModelMaster":
                    Result.DeleteBigInventoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteBigInventoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBigInventoryModels":
                    Result.DescribeBigInventoryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBigInventoryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBigInventoryModel":
                    Result.GetBigInventoryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBigInventoryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBigItemModelMasters":
                    Result.DescribeBigItemModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBigItemModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createBigItemModelMaster":
                    Result.CreateBigItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateBigItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBigItemModelMaster":
                    Result.GetBigItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBigItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBigItemModelMaster":
                    Result.UpdateBigItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateBigItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBigItemModelMaster":
                    Result.DeleteBigItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteBigItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBigItemModels":
                    Result.DescribeBigItemModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBigItemModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBigItemModel":
                    Result.GetBigItemModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBigItemModelRequest.FromJson(requestPayload)
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
                case "getCurrentItemModelMaster":
                    Result.GetCurrentItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentItemModelMaster":
                    Result.PreUpdateCurrentItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentItemModelMaster":
                    Result.UpdateCurrentItemModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentItemModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentItemModelMasterFromGitHub":
                    Result.UpdateCurrentItemModelMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentItemModelMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeInventories":
                    Result.DescribeInventoriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeInventoriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeInventoriesByUserId":
                    Result.DescribeInventoriesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeInventoriesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getInventory":
                    Result.GetInventoryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetInventoryRequest.FromJson(requestPayload)
                    );
                    break;
                case "getInventoryByUserId":
                    Result.GetInventoryByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetInventoryByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addCapacityByUserId":
                    Result.AddCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AddCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setCapacityByUserId":
                    Result.SetCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteInventoryByUserId":
                    Result.DeleteInventoryByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteInventoryByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyInventoryCurrentMaxCapacity":
                    Result.VerifyInventoryCurrentMaxCapacityResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyInventoryCurrentMaxCapacityRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyInventoryCurrentMaxCapacityByUserId":
                    Result.VerifyInventoryCurrentMaxCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyInventoryCurrentMaxCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeItemSets":
                    Result.DescribeItemSetsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeItemSetsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeItemSetsByUserId":
                    Result.DescribeItemSetsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeItemSetsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getItemSet":
                    Result.GetItemSetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetItemSetRequest.FromJson(requestPayload)
                    );
                    break;
                case "getItemSetByUserId":
                    Result.GetItemSetByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetItemSetByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getItemWithSignature":
                    Result.GetItemWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetItemWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getItemWithSignatureByUserId":
                    Result.GetItemWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetItemWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireItemSetByUserId":
                    Result.AcquireItemSetByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireItemSetByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireItemSetWithGradeByUserId":
                    Result.AcquireItemSetWithGradeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireItemSetWithGradeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeItemSet":
                    Result.ConsumeItemSetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ConsumeItemSetRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeItemSetByUserId":
                    Result.ConsumeItemSetByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ConsumeItemSetByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteItemSetByUserId":
                    Result.DeleteItemSetByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteItemSetByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyItemSet":
                    Result.VerifyItemSetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyItemSetRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyItemSetByUserId":
                    Result.VerifyItemSetByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyItemSetByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReferenceOf":
                    Result.DescribeReferenceOfResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeReferenceOfRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReferenceOfByUserId":
                    Result.DescribeReferenceOfByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeReferenceOfByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReferenceOf":
                    Result.GetReferenceOfResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetReferenceOfRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReferenceOfByUserId":
                    Result.GetReferenceOfByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetReferenceOfByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyReferenceOf":
                    Result.VerifyReferenceOfResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyReferenceOfRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyReferenceOfByUserId":
                    Result.VerifyReferenceOfByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyReferenceOfByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addReferenceOf":
                    Result.AddReferenceOfResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AddReferenceOfRequest.FromJson(requestPayload)
                    );
                    break;
                case "addReferenceOfByUserId":
                    Result.AddReferenceOfByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AddReferenceOfByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteReferenceOf":
                    Result.DeleteReferenceOfResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteReferenceOfRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteReferenceOfByUserId":
                    Result.DeleteReferenceOfByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteReferenceOfByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSimpleItems":
                    Result.DescribeSimpleItemsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSimpleItemsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSimpleItemsByUserId":
                    Result.DescribeSimpleItemsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSimpleItemsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleItem":
                    Result.GetSimpleItemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleItemRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleItemByUserId":
                    Result.GetSimpleItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleItemWithSignature":
                    Result.GetSimpleItemWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleItemWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSimpleItemWithSignatureByUserId":
                    Result.GetSimpleItemWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSimpleItemWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireSimpleItemsByUserId":
                    Result.AcquireSimpleItemsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireSimpleItemsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeSimpleItems":
                    Result.ConsumeSimpleItemsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ConsumeSimpleItemsRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeSimpleItemsByUserId":
                    Result.ConsumeSimpleItemsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ConsumeSimpleItemsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setSimpleItemsByUserId":
                    Result.SetSimpleItemsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetSimpleItemsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSimpleItemsByUserId":
                    Result.DeleteSimpleItemsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteSimpleItemsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifySimpleItem":
                    Result.VerifySimpleItemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifySimpleItemRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifySimpleItemByUserId":
                    Result.VerifySimpleItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifySimpleItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBigItems":
                    Result.DescribeBigItemsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBigItemsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBigItemsByUserId":
                    Result.DescribeBigItemsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBigItemsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBigItem":
                    Result.GetBigItemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBigItemRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBigItemByUserId":
                    Result.GetBigItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBigItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireBigItemByUserId":
                    Result.AcquireBigItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireBigItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeBigItem":
                    Result.ConsumeBigItemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ConsumeBigItemRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeBigItemByUserId":
                    Result.ConsumeBigItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ConsumeBigItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setBigItemByUserId":
                    Result.SetBigItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetBigItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBigItemByUserId":
                    Result.DeleteBigItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteBigItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyBigItem":
                    Result.VerifyBigItemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyBigItemRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyBigItemByUserId":
                    Result.VerifyBigItemByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyBigItemByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}