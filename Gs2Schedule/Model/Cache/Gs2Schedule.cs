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

namespace Gs2.Gs2Schedule.Model.Cache
{
    public static class Gs2Schedule
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
                case "describeEventMasters":
                    Result.DescribeEventMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEventMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createEventMaster":
                    Result.CreateEventMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateEventMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEventMaster":
                    Result.GetEventMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEventMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateEventMaster":
                    Result.UpdateEventMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateEventMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteEventMaster":
                    Result.DeleteEventMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteEventMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTriggers":
                    Result.DescribeTriggersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeTriggersRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTriggersByUserId":
                    Result.DescribeTriggersByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeTriggersByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTrigger":
                    Result.GetTriggerResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetTriggerRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTriggerByUserId":
                    Result.GetTriggerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetTriggerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "triggerByUserId":
                    Result.TriggerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.TriggerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTrigger":
                    Result.DeleteTriggerResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteTriggerRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTriggerByUserId":
                    Result.DeleteTriggerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteTriggerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyTrigger":
                    Result.VerifyTriggerResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyTriggerRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyTriggerByUserId":
                    Result.VerifyTriggerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyTriggerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEvents":
                    Result.DescribeEventsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEventsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEventsByUserId":
                    Result.DescribeEventsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEventsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRawEvents":
                    Result.DescribeRawEventsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRawEventsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEvent":
                    Result.GetEventResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEventRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEventByUserId":
                    Result.GetEventByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEventByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRawEvent":
                    Result.GetRawEventResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRawEventRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyEvent":
                    Result.VerifyEventResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyEventRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyEventByUserId":
                    Result.VerifyEventByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyEventByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentEventMaster":
                    Result.GetCurrentEventMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentEventMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentEventMaster":
                    Result.UpdateCurrentEventMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentEventMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentEventMasterFromGitHub":
                    Result.UpdateCurrentEventMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentEventMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}