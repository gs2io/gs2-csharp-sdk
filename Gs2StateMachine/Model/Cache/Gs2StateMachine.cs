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

namespace Gs2.Gs2StateMachine.Model.Cache
{
    public static class Gs2StateMachine
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
                case "describeStateMachineMasters":
                    Result.DescribeStateMachineMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStateMachineMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStateMachineMaster":
                    Result.UpdateStateMachineMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateStateMachineMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStateMachineMaster":
                    Result.GetStateMachineMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStateMachineMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStateMachineMaster":
                    Result.DeleteStateMachineMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStateMachineMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStatuses":
                    Result.DescribeStatusesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStatusesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStatusesByUserId":
                    Result.DescribeStatusesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStatusesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStatus":
                    Result.GetStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStatusByUserId":
                    Result.GetStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "startStateMachineByUserId":
                    Result.StartStateMachineByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.StartStateMachineByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "emit":
                    Result.EmitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.EmitRequest.FromJson(requestPayload)
                    );
                    break;
                case "emitByUserId":
                    Result.EmitByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.EmitByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "report":
                    Result.ReportResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReportRequest.FromJson(requestPayload)
                    );
                    break;
                case "reportByUserId":
                    Result.ReportByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReportByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStatusByUserId":
                    Result.DeleteStatusByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStatusByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exitStateMachine":
                    Result.ExitStateMachineResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExitStateMachineRequest.FromJson(requestPayload)
                    );
                    break;
                case "exitStateMachineByUserId":
                    Result.ExitStateMachineByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExitStateMachineByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}