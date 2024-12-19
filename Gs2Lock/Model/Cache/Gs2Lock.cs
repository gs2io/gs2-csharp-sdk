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

namespace Gs2.Gs2Lock.Model.Cache
{
    public static class Gs2Lock
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
                case "lock":
                    Result.LockResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.LockRequest.FromJson(requestPayload)
                    );
                    break;
                case "lockByUserId":
                    Result.LockByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.LockByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unlock":
                    Result.UnlockResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnlockRequest.FromJson(requestPayload)
                    );
                    break;
                case "unlockByUserId":
                    Result.UnlockByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnlockByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMutex":
                    Result.GetMutexResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMutexRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMutexByUserId":
                    Result.GetMutexByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMutexByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMutexByUserId":
                    Result.DeleteMutexByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMutexByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}