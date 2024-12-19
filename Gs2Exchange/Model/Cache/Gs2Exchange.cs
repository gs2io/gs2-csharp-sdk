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
                case "describeRateModels":
                    Result.DescribeRateModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRateModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRateModel":
                    Result.GetRateModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRateModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRateModelMasters":
                    Result.DescribeRateModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRateModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRateModelMaster":
                    Result.CreateRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRateModelMaster":
                    Result.GetRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRateModelMaster":
                    Result.UpdateRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRateModelMaster":
                    Result.DeleteRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIncrementalRateModels":
                    Result.DescribeIncrementalRateModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeIncrementalRateModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIncrementalRateModel":
                    Result.GetIncrementalRateModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetIncrementalRateModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIncrementalRateModelMasters":
                    Result.DescribeIncrementalRateModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeIncrementalRateModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createIncrementalRateModelMaster":
                    Result.CreateIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIncrementalRateModelMaster":
                    Result.GetIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateIncrementalRateModelMaster":
                    Result.UpdateIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteIncrementalRateModelMaster":
                    Result.DeleteIncrementalRateModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteIncrementalRateModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exchange":
                    Result.ExchangeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExchangeRequest.FromJson(requestPayload)
                    );
                    break;
                case "exchangeByUserId":
                    Result.ExchangeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExchangeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "incrementalExchange":
                    Result.IncrementalExchangeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IncrementalExchangeRequest.FromJson(requestPayload)
                    );
                    break;
                case "incrementalExchangeByUserId":
                    Result.IncrementalExchangeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IncrementalExchangeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentRateMaster":
                    Result.GetCurrentRateMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentRateMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRateMaster":
                    Result.UpdateCurrentRateMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentRateMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRateMasterFromGitHub":
                    Result.UpdateCurrentRateMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentRateMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "createAwaitByUserId":
                    Result.CreateAwaitByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateAwaitByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAwaits":
                    Result.DescribeAwaitsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAwaitsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAwaitsByUserId":
                    Result.DescribeAwaitsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAwaitsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAwait":
                    Result.GetAwaitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAwaitRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAwaitByUserId":
                    Result.GetAwaitByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAwaitByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquire":
                    Result.AcquireResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcquireRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireByUserId":
                    Result.AcquireByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcquireByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireForceByUserId":
                    Result.AcquireForceByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcquireForceByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "skipByUserId":
                    Result.SkipByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SkipByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAwait":
                    Result.DeleteAwaitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteAwaitRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAwaitByUserId":
                    Result.DeleteAwaitByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteAwaitByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}