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

namespace Gs2.Gs2MegaField.Model.Cache
{
    public static class Gs2MegaField
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
                case "describeAreaModels":
                    Result.DescribeAreaModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAreaModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAreaModel":
                    Result.GetAreaModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAreaModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAreaModelMasters":
                    Result.DescribeAreaModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAreaModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createAreaModelMaster":
                    Result.CreateAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAreaModelMaster":
                    Result.GetAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateAreaModelMaster":
                    Result.UpdateAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAreaModelMaster":
                    Result.DeleteAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLayerModels":
                    Result.DescribeLayerModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLayerModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLayerModel":
                    Result.GetLayerModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLayerModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLayerModelMasters":
                    Result.DescribeLayerModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLayerModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createLayerModelMaster":
                    Result.CreateLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateLayerModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLayerModelMaster":
                    Result.GetLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLayerModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateLayerModelMaster":
                    Result.UpdateLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateLayerModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteLayerModelMaster":
                    Result.DeleteLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteLayerModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentFieldMaster":
                    Result.GetCurrentFieldMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentFieldMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentFieldMaster":
                    Result.PreUpdateCurrentFieldMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PreUpdateCurrentFieldMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFieldMaster":
                    Result.UpdateCurrentFieldMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentFieldMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFieldMasterFromGitHub":
                    Result.UpdateCurrentFieldMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentFieldMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "putPosition":
                    Result.PutPositionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutPositionRequest.FromJson(requestPayload)
                    );
                    break;
                case "putPositionByUserId":
                    Result.PutPositionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutPositionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "fetchPosition":
                    Result.FetchPositionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.FetchPositionRequest.FromJson(requestPayload)
                    );
                    break;
                case "fetchPositionFromSystem":
                    Result.FetchPositionFromSystemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.FetchPositionFromSystemRequest.FromJson(requestPayload)
                    );
                    break;
                case "nearUserIds":
                    Result.NearUserIdsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.NearUserIdsRequest.FromJson(requestPayload)
                    );
                    break;
                case "nearUserIdsFromSystem":
                    Result.NearUserIdsFromSystemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.NearUserIdsFromSystemRequest.FromJson(requestPayload)
                    );
                    break;
                case "action":
                    Result.ActionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ActionRequest.FromJson(requestPayload)
                    );
                    break;
                case "actionByUserId":
                    Result.ActionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ActionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}