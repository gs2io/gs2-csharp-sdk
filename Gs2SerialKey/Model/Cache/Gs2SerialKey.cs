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

namespace Gs2.Gs2SerialKey.Model.Cache
{
    public static class Gs2SerialKey
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
                case "describeIssueJobs":
                    Result.DescribeIssueJobsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeIssueJobsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIssueJob":
                    Result.GetIssueJobResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetIssueJobRequest.FromJson(requestPayload)
                    );
                    break;
                case "issue":
                    Result.IssueResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IssueRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSerialKeys":
                    Result.DescribeSerialKeysResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSerialKeysRequest.FromJson(requestPayload)
                    );
                    break;
                case "downloadSerialCodes":
                    Result.DownloadSerialCodesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DownloadSerialCodesRequest.FromJson(requestPayload)
                    );
                    break;
                case "issueOnce":
                    Result.IssueOnceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IssueOnceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSerialKey":
                    Result.GetSerialKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSerialKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCode":
                    Result.VerifyCodeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCodeRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCodeByUserId":
                    Result.VerifyCodeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCodeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "use":
                    Result.UseResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UseRequest.FromJson(requestPayload)
                    );
                    break;
                case "useByUserId":
                    Result.UseByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UseByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "revertUseByUserId":
                    Result.RevertUseByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RevertUseByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCampaignModels":
                    Result.DescribeCampaignModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCampaignModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCampaignModel":
                    Result.GetCampaignModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCampaignModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCampaignModelMasters":
                    Result.DescribeCampaignModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCampaignModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createCampaignModelMaster":
                    Result.CreateCampaignModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateCampaignModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCampaignModelMaster":
                    Result.GetCampaignModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCampaignModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCampaignModelMaster":
                    Result.UpdateCampaignModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCampaignModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCampaignModelMaster":
                    Result.DeleteCampaignModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteCampaignModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentCampaignMaster":
                    Result.GetCurrentCampaignMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentCampaignMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentCampaignMaster":
                    Result.UpdateCurrentCampaignMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentCampaignMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentCampaignMasterFromGitHub":
                    Result.UpdateCurrentCampaignMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentCampaignMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}