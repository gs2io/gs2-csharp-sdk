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

namespace Gs2.Gs2Grade.Model.Cache
{
    public static class Gs2Grade
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
                case "getServiceVersion":
                    Result.GetServiceVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetServiceVersionRequest.FromJson(requestPayload)
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
                case "describeGradeModelMasters":
                    Result.DescribeGradeModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGradeModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGradeModelMaster":
                    Result.CreateGradeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGradeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGradeModelMaster":
                    Result.GetGradeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGradeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGradeModelMaster":
                    Result.UpdateGradeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGradeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGradeModelMaster":
                    Result.DeleteGradeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGradeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGradeModels":
                    Result.DescribeGradeModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGradeModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGradeModel":
                    Result.GetGradeModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGradeModelRequest.FromJson(requestPayload)
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
                case "addGradeByUserId":
                    Result.AddGradeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddGradeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "subGrade":
                    Result.SubGradeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubGradeRequest.FromJson(requestPayload)
                    );
                    break;
                case "subGradeByUserId":
                    Result.SubGradeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubGradeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setGradeByUserId":
                    Result.SetGradeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetGradeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "applyRankCap":
                    Result.ApplyRankCapResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ApplyRankCapRequest.FromJson(requestPayload)
                    );
                    break;
                case "applyRankCapByUserId":
                    Result.ApplyRankCapByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ApplyRankCapByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStatusByUserId":
                    Result.DeleteStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyGrade":
                    Result.VerifyGradeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyGradeRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyGradeByUserId":
                    Result.VerifyGradeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyGradeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyGradeUpMaterial":
                    Result.VerifyGradeUpMaterialResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyGradeUpMaterialRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyGradeUpMaterialByUserId":
                    Result.VerifyGradeUpMaterialByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyGradeUpMaterialByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "multiplyAcquireActionsByUserId":
                    Result.MultiplyAcquireActionsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.MultiplyAcquireActionsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentGradeMaster":
                    Result.GetCurrentGradeMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentGradeMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentGradeMaster":
                    Result.PreUpdateCurrentGradeMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PreUpdateCurrentGradeMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentGradeMaster":
                    Result.UpdateCurrentGradeMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentGradeMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentGradeMasterFromGitHub":
                    Result.UpdateCurrentGradeMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentGradeMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}