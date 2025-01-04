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

namespace Gs2.Gs2Inbox.Model.Cache
{
    public static class Gs2Inbox
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
                case "sendMessageByUserId":
                    Result.SendMessageByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SendMessageByUserIdRequest.FromJson(requestPayload)
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
                case "receiveGlobalMessage":
                    Result.ReceiveGlobalMessageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveGlobalMessageRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveGlobalMessageByUserId":
                    Result.ReceiveGlobalMessageByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveGlobalMessageByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "openMessage":
                    Result.OpenMessageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.OpenMessageRequest.FromJson(requestPayload)
                    );
                    break;
                case "openMessageByUserId":
                    Result.OpenMessageByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.OpenMessageByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "readMessage":
                    Result.ReadMessageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReadMessageRequest.FromJson(requestPayload)
                    );
                    break;
                case "readMessageByUserId":
                    Result.ReadMessageByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReadMessageByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMessage":
                    Result.DeleteMessageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMessageRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMessageByUserId":
                    Result.DeleteMessageByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMessageByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentMessageMaster":
                    Result.GetCurrentMessageMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentMessageMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentMessageMaster":
                    Result.UpdateCurrentMessageMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentMessageMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentMessageMasterFromGitHub":
                    Result.UpdateCurrentMessageMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentMessageMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalMessageMasters":
                    Result.DescribeGlobalMessageMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalMessageMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGlobalMessageMaster":
                    Result.CreateGlobalMessageMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGlobalMessageMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalMessageMaster":
                    Result.GetGlobalMessageMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalMessageMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGlobalMessageMaster":
                    Result.UpdateGlobalMessageMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGlobalMessageMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGlobalMessageMaster":
                    Result.DeleteGlobalMessageMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGlobalMessageMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalMessages":
                    Result.DescribeGlobalMessagesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalMessagesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalMessage":
                    Result.GetGlobalMessageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalMessageRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceivedByUserId":
                    Result.GetReceivedByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetReceivedByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateReceivedByUserId":
                    Result.UpdateReceivedByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateReceivedByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteReceivedByUserId":
                    Result.DeleteReceivedByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteReceivedByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}