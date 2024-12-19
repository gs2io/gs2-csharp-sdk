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

namespace Gs2.Gs2Experience.Model.Cache
{
    public static class Gs2Experience
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
                case "describeExperienceModelMasters":
                    Result.DescribeExperienceModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeExperienceModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createExperienceModelMaster":
                    Result.CreateExperienceModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateExperienceModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getExperienceModelMaster":
                    Result.GetExperienceModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetExperienceModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateExperienceModelMaster":
                    Result.UpdateExperienceModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateExperienceModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteExperienceModelMaster":
                    Result.DeleteExperienceModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteExperienceModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeExperienceModels":
                    Result.DescribeExperienceModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeExperienceModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getExperienceModel":
                    Result.GetExperienceModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetExperienceModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeThresholdMasters":
                    Result.DescribeThresholdMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeThresholdMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createThresholdMaster":
                    Result.CreateThresholdMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateThresholdMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getThresholdMaster":
                    Result.GetThresholdMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetThresholdMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateThresholdMaster":
                    Result.UpdateThresholdMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateThresholdMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteThresholdMaster":
                    Result.DeleteThresholdMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteThresholdMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentExperienceMaster":
                    Result.GetCurrentExperienceMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentExperienceMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentExperienceMaster":
                    Result.UpdateCurrentExperienceMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentExperienceMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentExperienceMasterFromGitHub":
                    Result.UpdateCurrentExperienceMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentExperienceMasterFromGitHubRequest.FromJson(requestPayload)
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
                case "getStatusWithSignature":
                    Result.GetStatusWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStatusWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStatusWithSignatureByUserId":
                    Result.GetStatusWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStatusWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addExperienceByUserId":
                    Result.AddExperienceByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddExperienceByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "subExperience":
                    Result.SubExperienceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubExperienceRequest.FromJson(requestPayload)
                    );
                    break;
                case "subExperienceByUserId":
                    Result.SubExperienceByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubExperienceByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setExperienceByUserId":
                    Result.SetExperienceByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetExperienceByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addRankCapByUserId":
                    Result.AddRankCapByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddRankCapByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "subRankCap":
                    Result.SubRankCapResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubRankCapRequest.FromJson(requestPayload)
                    );
                    break;
                case "subRankCapByUserId":
                    Result.SubRankCapByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubRankCapByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setRankCapByUserId":
                    Result.SetRankCapByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetRankCapByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStatusByUserId":
                    Result.DeleteStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyRank":
                    Result.VerifyRankResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyRankRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyRankByUserId":
                    Result.VerifyRankByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyRankByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyRankCap":
                    Result.VerifyRankCapResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyRankCapRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyRankCapByUserId":
                    Result.VerifyRankCapByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyRankCapByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "multiplyAcquireActionsByUserId":
                    Result.MultiplyAcquireActionsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.MultiplyAcquireActionsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}