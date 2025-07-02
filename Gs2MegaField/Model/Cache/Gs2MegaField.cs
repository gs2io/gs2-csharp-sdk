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
                case "describeAreaModels":
                    Result.DescribeAreaModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeAreaModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAreaModel":
                    Result.GetAreaModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetAreaModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAreaModelMasters":
                    Result.DescribeAreaModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeAreaModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createAreaModelMaster":
                    Result.CreateAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAreaModelMaster":
                    Result.GetAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateAreaModelMaster":
                    Result.UpdateAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAreaModelMaster":
                    Result.DeleteAreaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteAreaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLayerModels":
                    Result.DescribeLayerModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeLayerModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLayerModel":
                    Result.GetLayerModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLayerModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLayerModelMasters":
                    Result.DescribeLayerModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeLayerModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createLayerModelMaster":
                    Result.CreateLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateLayerModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLayerModelMaster":
                    Result.GetLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLayerModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateLayerModelMaster":
                    Result.UpdateLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateLayerModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteLayerModelMaster":
                    Result.DeleteLayerModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteLayerModelMasterRequest.FromJson(requestPayload)
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
                case "getCurrentFieldMaster":
                    Result.GetCurrentFieldMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentFieldMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentFieldMaster":
                    Result.PreUpdateCurrentFieldMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentFieldMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFieldMaster":
                    Result.UpdateCurrentFieldMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentFieldMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFieldMasterFromGitHub":
                    Result.UpdateCurrentFieldMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentFieldMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "putPosition":
                    Result.PutPositionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PutPositionRequest.FromJson(requestPayload)
                    );
                    break;
                case "putPositionByUserId":
                    Result.PutPositionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PutPositionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "fetchPosition":
                    Result.FetchPositionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FetchPositionRequest.FromJson(requestPayload)
                    );
                    break;
                case "fetchPositionFromSystem":
                    Result.FetchPositionFromSystemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FetchPositionFromSystemRequest.FromJson(requestPayload)
                    );
                    break;
                case "nearUserIds":
                    Result.NearUserIdsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.NearUserIdsRequest.FromJson(requestPayload)
                    );
                    break;
                case "nearUserIdsFromSystem":
                    Result.NearUserIdsFromSystemResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.NearUserIdsFromSystemRequest.FromJson(requestPayload)
                    );
                    break;
                case "action":
                    Result.ActionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ActionRequest.FromJson(requestPayload)
                    );
                    break;
                case "actionByUserId":
                    Result.ActionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ActionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}