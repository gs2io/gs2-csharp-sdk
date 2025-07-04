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

namespace Gs2.Gs2Enhance.Model.Cache
{
    public static class Gs2Enhance
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
                case "describeRateModels":
                    Result.DescribeRateModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRateModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRateModel":
                    Result.GetRateModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRateModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRateModelMasters":
                    Result.DescribeRateModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRateModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRateModelMaster":
                    Result.CreateRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRateModelMaster":
                    Result.GetRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRateModelMaster":
                    Result.UpdateRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRateModelMaster":
                    Result.DeleteRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeUnleashRateModels":
                    Result.DescribeUnleashRateModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeUnleashRateModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getUnleashRateModel":
                    Result.GetUnleashRateModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetUnleashRateModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeUnleashRateModelMasters":
                    Result.DescribeUnleashRateModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeUnleashRateModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createUnleashRateModelMaster":
                    Result.CreateUnleashRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateUnleashRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getUnleashRateModelMaster":
                    Result.GetUnleashRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetUnleashRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateUnleashRateModelMaster":
                    Result.UpdateUnleashRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateUnleashRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteUnleashRateModelMaster":
                    Result.DeleteUnleashRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteUnleashRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "directEnhance":
                    Result.DirectEnhanceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DirectEnhanceRequest.FromJson(requestPayload)
                    );
                    break;
                case "directEnhanceByUserId":
                    Result.DirectEnhanceByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DirectEnhanceByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unleash":
                    Result.UnleashResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UnleashRequest.FromJson(requestPayload)
                    );
                    break;
                case "unleashByUserId":
                    Result.UnleashByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UnleashByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createProgressByUserId":
                    Result.CreateProgressByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateProgressByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProgress":
                    Result.GetProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProgressByUserId":
                    Result.GetProgressByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetProgressByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "start":
                    Result.StartResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.StartRequest.FromJson(requestPayload)
                    );
                    break;
                case "startByUserId":
                    Result.StartByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.StartByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "end":
                    Result.EndResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EndRequest.FromJson(requestPayload)
                    );
                    break;
                case "endByUserId":
                    Result.EndByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EndByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteProgress":
                    Result.DeleteProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteProgressByUserId":
                    Result.DeleteProgressByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteProgressByUserIdRequest.FromJson(requestPayload)
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
                case "getCurrentRateMaster":
                    Result.GetCurrentRateMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentRateMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentRateMaster":
                    Result.PreUpdateCurrentRateMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentRateMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRateMaster":
                    Result.UpdateCurrentRateMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentRateMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRateMasterFromGitHub":
                    Result.UpdateCurrentRateMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentRateMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}