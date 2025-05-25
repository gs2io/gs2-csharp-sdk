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

namespace Gs2.Gs2News.Model.Cache
{
    public static class Gs2News
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
                case "describeProgresses":
                    Result.DescribeProgressesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeProgressesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProgress":
                    Result.GetProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeOutputs":
                    Result.DescribeOutputsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeOutputsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getOutput":
                    Result.GetOutputResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetOutputRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareUpdateCurrentNewsMaster":
                    Result.PrepareUpdateCurrentNewsMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PrepareUpdateCurrentNewsMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentNewsMaster":
                    Result.UpdateCurrentNewsMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentNewsMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareUpdateCurrentNewsMasterFromGitHub":
                    Result.PrepareUpdateCurrentNewsMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PrepareUpdateCurrentNewsMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeNews":
                    Result.DescribeNewsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeNewsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeNewsByUserId":
                    Result.DescribeNewsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeNewsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "wantGrant":
                    Result.WantGrantResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WantGrantRequest.FromJson(requestPayload)
                    );
                    break;
                case "wantGrantByUserId":
                    Result.WantGrantByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WantGrantByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}