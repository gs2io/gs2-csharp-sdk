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

namespace Gs2.Gs2Chat.Model.Cache
{
    public static class Gs2Chat
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
                case "describeRooms":
                    Result.DescribeRoomsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRoomsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRoom":
                    Result.CreateRoomResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRoomRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRoomFromBackend":
                    Result.CreateRoomFromBackendResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRoomFromBackendRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRoom":
                    Result.GetRoomResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRoomRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRoom":
                    Result.UpdateRoomResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRoomRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRoomFromBackend":
                    Result.UpdateRoomFromBackendResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRoomFromBackendRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRoom":
                    Result.DeleteRoomResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRoomRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRoomFromBackend":
                    Result.DeleteRoomFromBackendResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRoomFromBackendRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMessages":
                    Result.DescribeMessagesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMessagesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMessagesByUserId":
                    Result.DescribeMessagesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMessagesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLatestMessages":
                    Result.DescribeLatestMessagesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLatestMessagesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLatestMessagesByUserId":
                    Result.DescribeLatestMessagesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLatestMessagesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "post":
                    Result.PostResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PostRequest.FromJson(requestPayload)
                    );
                    break;
                case "postByUserId":
                    Result.PostByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PostByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMessage":
                    Result.GetMessageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMessageRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMessageByUserId":
                    Result.GetMessageByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMessageByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMessage":
                    Result.DeleteMessageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMessageRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribes":
                    Result.DescribeSubscribesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribesByUserId":
                    Result.DescribeSubscribesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribesByRoomName":
                    Result.DescribeSubscribesByRoomNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribesByRoomNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "subscribe":
                    Result.SubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "subscribeByUserId":
                    Result.SubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribe":
                    Result.GetSubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeByUserId":
                    Result.GetSubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNotificationType":
                    Result.UpdateNotificationTypeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateNotificationTypeRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNotificationTypeByUserId":
                    Result.UpdateNotificationTypeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateNotificationTypeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unsubscribe":
                    Result.UnsubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnsubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "unsubscribeByUserId":
                    Result.UnsubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnsubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}