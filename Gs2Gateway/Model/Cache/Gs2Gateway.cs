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

namespace Gs2.Gs2Gateway.Model.Cache
{
    public static class Gs2Gateway
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
                case "describeWebSocketSessions":
                    Result.DescribeWebSocketSessionsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeWebSocketSessionsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeWebSocketSessionsByUserId":
                    Result.DescribeWebSocketSessionsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeWebSocketSessionsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setUserId":
                    Result.SetUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setUserIdByUserId":
                    Result.SetUserIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetUserIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendNotification":
                    Result.SendNotificationResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SendNotificationRequest.FromJson(requestPayload)
                    );
                    break;
                case "disconnectByUserId":
                    Result.DisconnectByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DisconnectByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "disconnectAll":
                    Result.DisconnectAllResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DisconnectAllRequest.FromJson(requestPayload)
                    );
                    break;
                case "setFirebaseToken":
                    Result.SetFirebaseTokenResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetFirebaseTokenRequest.FromJson(requestPayload)
                    );
                    break;
                case "setFirebaseTokenByUserId":
                    Result.SetFirebaseTokenByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetFirebaseTokenByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFirebaseToken":
                    Result.GetFirebaseTokenResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFirebaseTokenRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFirebaseTokenByUserId":
                    Result.GetFirebaseTokenByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFirebaseTokenByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFirebaseToken":
                    Result.DeleteFirebaseTokenResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteFirebaseTokenRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFirebaseTokenByUserId":
                    Result.DeleteFirebaseTokenByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteFirebaseTokenByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendMobileNotificationByUserId":
                    Result.SendMobileNotificationByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SendMobileNotificationByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}