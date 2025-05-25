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

namespace Gs2.Gs2Version.Model.Cache
{
    public static class Gs2Version
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
                case "describeVersionModelMasters":
                    Result.DescribeVersionModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeVersionModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createVersionModelMaster":
                    Result.CreateVersionModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateVersionModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getVersionModelMaster":
                    Result.GetVersionModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetVersionModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateVersionModelMaster":
                    Result.UpdateVersionModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateVersionModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteVersionModelMaster":
                    Result.DeleteVersionModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteVersionModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeVersionModels":
                    Result.DescribeVersionModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeVersionModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getVersionModel":
                    Result.GetVersionModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetVersionModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAcceptVersions":
                    Result.DescribeAcceptVersionsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAcceptVersionsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAcceptVersionsByUserId":
                    Result.DescribeAcceptVersionsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAcceptVersionsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "accept":
                    Result.AcceptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcceptRequest.FromJson(requestPayload)
                    );
                    break;
                case "acceptByUserId":
                    Result.AcceptByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcceptByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "reject":
                    Result.RejectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RejectRequest.FromJson(requestPayload)
                    );
                    break;
                case "rejectByUserId":
                    Result.RejectByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RejectByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAcceptVersion":
                    Result.GetAcceptVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAcceptVersionRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAcceptVersionByUserId":
                    Result.GetAcceptVersionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAcceptVersionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAcceptVersion":
                    Result.DeleteAcceptVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteAcceptVersionRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAcceptVersionByUserId":
                    Result.DeleteAcceptVersionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteAcceptVersionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkVersion":
                    Result.CheckVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckVersionRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkVersionByUserId":
                    Result.CheckVersionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckVersionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "calculateSignature":
                    Result.CalculateSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CalculateSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentVersionMaster":
                    Result.GetCurrentVersionMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentVersionMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentVersionMaster":
                    Result.PreUpdateCurrentVersionMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PreUpdateCurrentVersionMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentVersionMaster":
                    Result.UpdateCurrentVersionMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentVersionMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentVersionMasterFromGitHub":
                    Result.UpdateCurrentVersionMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentVersionMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}