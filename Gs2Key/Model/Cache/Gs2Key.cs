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

namespace Gs2.Gs2Key.Model.Cache
{
    public static class Gs2Key
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
                case "describeKeys":
                    Result.DescribeKeysResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeKeysRequest.FromJson(requestPayload)
                    );
                    break;
                case "createKey":
                    Result.CreateKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateKey":
                    Result.UpdateKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "getKey":
                    Result.GetKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteKey":
                    Result.DeleteKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "encrypt":
                    Result.EncryptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EncryptRequest.FromJson(requestPayload)
                    );
                    break;
                case "decrypt":
                    Result.DecryptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DecryptRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGitHubApiKeys":
                    Result.DescribeGitHubApiKeysResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeGitHubApiKeysRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGitHubApiKey":
                    Result.CreateGitHubApiKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateGitHubApiKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGitHubApiKey":
                    Result.UpdateGitHubApiKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateGitHubApiKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGitHubApiKey":
                    Result.GetGitHubApiKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetGitHubApiKeyRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGitHubApiKey":
                    Result.DeleteGitHubApiKeyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteGitHubApiKeyRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}