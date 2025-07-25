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

namespace Gs2.Gs2Limit.Model.Cache
{
    public static class Gs2Limit
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
                case "dumpUserDataByUserId":
                    Result.DumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkDumpUserDataByUserId":
                    Result.CheckDumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckDumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "cleanUserDataByUserId":
                    Result.CleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkCleanUserDataByUserId":
                    Result.CheckCleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckCleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareImportUserDataByUserId":
                    Result.PrepareImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "importUserDataByUserId":
                    Result.ImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkImportUserDataByUserId":
                    Result.CheckImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCounters":
                    Result.DescribeCountersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeCountersRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCountersByUserId":
                    Result.DescribeCountersByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeCountersByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCounter":
                    Result.GetCounterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCounterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCounterByUserId":
                    Result.GetCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "countUp":
                    Result.CountUpResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CountUpRequest.FromJson(requestPayload)
                    );
                    break;
                case "countUpByUserId":
                    Result.CountUpByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CountUpByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "countDownByUserId":
                    Result.CountDownByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CountDownByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCounterByUserId":
                    Result.DeleteCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCounter":
                    Result.VerifyCounterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyCounterRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCounterByUserId":
                    Result.VerifyCounterByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyCounterByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLimitModelMasters":
                    Result.DescribeLimitModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeLimitModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createLimitModelMaster":
                    Result.CreateLimitModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateLimitModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLimitModelMaster":
                    Result.GetLimitModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLimitModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateLimitModelMaster":
                    Result.UpdateLimitModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateLimitModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteLimitModelMaster":
                    Result.DeleteLimitModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteLimitModelMasterRequest.FromJson(requestPayload)
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
                case "getCurrentLimitMaster":
                    Result.GetCurrentLimitMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentLimitMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentLimitMaster":
                    Result.PreUpdateCurrentLimitMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentLimitMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentLimitMaster":
                    Result.UpdateCurrentLimitMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentLimitMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentLimitMasterFromGitHub":
                    Result.UpdateCurrentLimitMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentLimitMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLimitModels":
                    Result.DescribeLimitModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeLimitModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLimitModel":
                    Result.GetLimitModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLimitModelRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}