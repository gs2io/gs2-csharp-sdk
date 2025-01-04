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

namespace Gs2.Gs2Mission.Model.Cache
{
    public static class Gs2Mission
    {
        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "describeCompletes":
                    Result.DescribeCompletesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCompletesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCompletesByUserId":
                    Result.DescribeCompletesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCompletesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "complete":
                    Result.CompleteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CompleteRequest.FromJson(requestPayload)
                    );
                    break;
                case "completeByUserId":
                    Result.CompleteByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CompleteByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchComplete":
                    Result.BatchCompleteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.BatchCompleteRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchCompleteByUserId":
                    Result.BatchCompleteByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.BatchCompleteByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveByUserId":
                    Result.ReceiveByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchReceiveByUserId":
                    Result.BatchReceiveByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.BatchReceiveByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "revertReceiveByUserId":
                    Result.RevertReceiveByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RevertReceiveByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getComplete":
                    Result.GetCompleteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCompleteRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCompleteByUserId":
                    Result.GetCompleteByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCompleteByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCompleteByUserId":
                    Result.DeleteCompleteByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteCompleteByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyComplete":
                    Result.VerifyCompleteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCompleteRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCompleteByUserId":
                    Result.VerifyCompleteByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCompleteByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCounterModelMasters":
                    Result.DescribeCounterModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCounterModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createCounterModelMaster":
                    Result.CreateCounterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateCounterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCounterModelMaster":
                    Result.GetCounterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCounterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCounterModelMaster":
                    Result.UpdateCounterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCounterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCounterModelMaster":
                    Result.DeleteCounterModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteCounterModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMissionGroupModelMasters":
                    Result.DescribeMissionGroupModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMissionGroupModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createMissionGroupModelMaster":
                    Result.CreateMissionGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateMissionGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMissionGroupModelMaster":
                    Result.GetMissionGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMissionGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMissionGroupModelMaster":
                    Result.UpdateMissionGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateMissionGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMissionGroupModelMaster":
                    Result.DeleteMissionGroupModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMissionGroupModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
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
                case "describeCounters":
                    Result.DescribeCountersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCountersRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCountersByUserId":
                    Result.DescribeCountersByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCountersByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "increaseCounterByUserId":
                    Result.IncreaseCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IncreaseCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setCounterByUserId":
                    Result.SetCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseCounter":
                    Result.DecreaseCounterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseCounterRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseCounterByUserId":
                    Result.DecreaseCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCounter":
                    Result.GetCounterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCounterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCounterByUserId":
                    Result.GetCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCounterValue":
                    Result.VerifyCounterValueResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCounterValueRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCounterValueByUserId":
                    Result.VerifyCounterValueByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCounterValueByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCounterByUserId":
                    Result.DeleteCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentMissionMaster":
                    Result.GetCurrentMissionMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentMissionMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentMissionMaster":
                    Result.UpdateCurrentMissionMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentMissionMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentMissionMasterFromGitHub":
                    Result.UpdateCurrentMissionMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentMissionMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCounterModels":
                    Result.DescribeCounterModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCounterModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCounterModel":
                    Result.GetCounterModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCounterModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMissionGroupModels":
                    Result.DescribeMissionGroupModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMissionGroupModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMissionGroupModel":
                    Result.GetMissionGroupModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMissionGroupModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMissionTaskModels":
                    Result.DescribeMissionTaskModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMissionTaskModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMissionTaskModel":
                    Result.GetMissionTaskModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMissionTaskModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMissionTaskModelMasters":
                    Result.DescribeMissionTaskModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMissionTaskModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createMissionTaskModelMaster":
                    Result.CreateMissionTaskModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateMissionTaskModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMissionTaskModelMaster":
                    Result.GetMissionTaskModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMissionTaskModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMissionTaskModelMaster":
                    Result.UpdateMissionTaskModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateMissionTaskModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMissionTaskModelMaster":
                    Result.DeleteMissionTaskModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMissionTaskModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}