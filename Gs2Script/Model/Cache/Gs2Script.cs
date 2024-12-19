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

namespace Gs2.Gs2Script.Model.Cache
{
    public static class Gs2Script
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
                case "describeScripts":
                    Result.DescribeScriptsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeScriptsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createScript":
                    Result.CreateScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "createScriptFromGitHub":
                    Result.CreateScriptFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateScriptFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "getScript":
                    Result.GetScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateScript":
                    Result.UpdateScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateScriptFromGitHub":
                    Result.UpdateScriptFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateScriptFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteScript":
                    Result.DeleteScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "invokeScript":
                    Result.InvokeScriptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.InvokeScriptRequest.FromJson(requestPayload)
                    );
                    break;
                case "debugInvoke":
                    Result.DebugInvokeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DebugInvokeRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}