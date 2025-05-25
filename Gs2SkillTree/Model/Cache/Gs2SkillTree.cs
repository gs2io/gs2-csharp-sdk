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

namespace Gs2.Gs2SkillTree.Model.Cache
{
    public static class Gs2SkillTree
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
                case "describeNodeModels":
                    Result.DescribeNodeModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeNodeModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNodeModel":
                    Result.GetNodeModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetNodeModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeNodeModelMasters":
                    Result.DescribeNodeModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeNodeModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createNodeModelMaster":
                    Result.CreateNodeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateNodeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNodeModelMaster":
                    Result.GetNodeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetNodeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNodeModelMaster":
                    Result.UpdateNodeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateNodeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteNodeModelMaster":
                    Result.DeleteNodeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteNodeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "markReleaseByUserId":
                    Result.MarkReleaseByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.MarkReleaseByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "release":
                    Result.ReleaseResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReleaseRequest.FromJson(requestPayload)
                    );
                    break;
                case "releaseByUserId":
                    Result.ReleaseByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReleaseByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "markRestrain":
                    Result.MarkRestrainResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.MarkRestrainRequest.FromJson(requestPayload)
                    );
                    break;
                case "markRestrainByUserId":
                    Result.MarkRestrainByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.MarkRestrainByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "restrain":
                    Result.RestrainResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RestrainRequest.FromJson(requestPayload)
                    );
                    break;
                case "restrainByUserId":
                    Result.RestrainByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RestrainByUserIdRequest.FromJson(requestPayload)
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
                case "reset":
                    Result.ResetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetByUserId":
                    Result.ResetByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentTreeMaster":
                    Result.GetCurrentTreeMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentTreeMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentTreeMaster":
                    Result.PreUpdateCurrentTreeMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PreUpdateCurrentTreeMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentTreeMaster":
                    Result.UpdateCurrentTreeMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentTreeMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentTreeMasterFromGitHub":
                    Result.UpdateCurrentTreeMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentTreeMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}