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

namespace Gs2.Gs2Exchange.Model.Cache
{
    public static class Gs2Exchange
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
                case "describeIncrementalRateModels":
                    Result.DescribeIncrementalRateModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeIncrementalRateModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIncrementalRateModel":
                    Result.GetIncrementalRateModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetIncrementalRateModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIncrementalRateModelMasters":
                    Result.DescribeIncrementalRateModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeIncrementalRateModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createIncrementalRateModelMaster":
                    Result.CreateIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIncrementalRateModelMaster":
                    Result.GetIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateIncrementalRateModelMaster":
                    Result.UpdateIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteIncrementalRateModelMaster":
                    Result.DeleteIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exchange":
                    Result.ExchangeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ExchangeRequest.FromJson(requestPayload)
                    );
                    break;
                case "exchangeByUserId":
                    Result.ExchangeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ExchangeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "incrementalExchange":
                    Result.IncrementalExchangeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.IncrementalExchangeRequest.FromJson(requestPayload)
                    );
                    break;
                case "incrementalExchangeByUserId":
                    Result.IncrementalExchangeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.IncrementalExchangeByUserIdRequest.FromJson(requestPayload)
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
                case "createAwaitByUserId":
                    Result.CreateAwaitByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateAwaitByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAwaits":
                    Result.DescribeAwaitsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeAwaitsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAwaitsByUserId":
                    Result.DescribeAwaitsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeAwaitsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAwait":
                    Result.GetAwaitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetAwaitRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAwaitByUserId":
                    Result.GetAwaitByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetAwaitByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquire":
                    Result.AcquireResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireByUserId":
                    Result.AcquireByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireForceByUserId":
                    Result.AcquireForceByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireForceByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "skipByUserId":
                    Result.SkipByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SkipByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAwait":
                    Result.DeleteAwaitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteAwaitRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAwaitByUserId":
                    Result.DeleteAwaitByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteAwaitByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}