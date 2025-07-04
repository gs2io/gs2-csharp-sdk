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

namespace Gs2.Gs2LoginReward.Model.Cache
{
    public static class Gs2LoginReward
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
                case "describeBonusModelMasters":
                    Result.DescribeBonusModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBonusModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createBonusModelMaster":
                    Result.CreateBonusModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateBonusModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBonusModelMaster":
                    Result.GetBonusModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBonusModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBonusModelMaster":
                    Result.UpdateBonusModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateBonusModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBonusModelMaster":
                    Result.DeleteBonusModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteBonusModelMasterRequest.FromJson(requestPayload)
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
                case "getCurrentBonusMaster":
                    Result.GetCurrentBonusMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentBonusMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentBonusMaster":
                    Result.PreUpdateCurrentBonusMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentBonusMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentBonusMaster":
                    Result.UpdateCurrentBonusMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentBonusMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentBonusMasterFromGitHub":
                    Result.UpdateCurrentBonusMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentBonusMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBonusModels":
                    Result.DescribeBonusModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBonusModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBonusModel":
                    Result.GetBonusModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBonusModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "receive":
                    Result.ReceiveResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ReceiveRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveByUserId":
                    Result.ReceiveByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ReceiveByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "missedReceive":
                    Result.MissedReceiveResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.MissedReceiveRequest.FromJson(requestPayload)
                    );
                    break;
                case "missedReceiveByUserId":
                    Result.MissedReceiveByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.MissedReceiveByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceiveStatuses":
                    Result.DescribeReceiveStatusesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeReceiveStatusesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceiveStatusesByUserId":
                    Result.DescribeReceiveStatusesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeReceiveStatusesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveStatus":
                    Result.GetReceiveStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetReceiveStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveStatusByUserId":
                    Result.GetReceiveStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetReceiveStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteReceiveStatusByUserId":
                    Result.DeleteReceiveStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteReceiveStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "markReceived":
                    Result.MarkReceivedResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.MarkReceivedRequest.FromJson(requestPayload)
                    );
                    break;
                case "markReceivedByUserId":
                    Result.MarkReceivedByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.MarkReceivedByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unmarkReceivedByUserId":
                    Result.UnmarkReceivedByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UnmarkReceivedByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}