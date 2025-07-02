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

namespace Gs2.Gs2Lottery.Model.Cache
{
    public static class Gs2Lottery
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
                case "describeLotteryModelMasters":
                    Result.DescribeLotteryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeLotteryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createLotteryModelMaster":
                    Result.CreateLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLotteryModelMaster":
                    Result.GetLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateLotteryModelMaster":
                    Result.UpdateLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteLotteryModelMaster":
                    Result.DeleteLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePrizeTableMasters":
                    Result.DescribePrizeTableMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePrizeTableMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPrizeTableMaster":
                    Result.CreatePrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreatePrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPrizeTableMaster":
                    Result.GetPrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updatePrizeTableMaster":
                    Result.UpdatePrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdatePrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePrizeTableMaster":
                    Result.DeletePrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeletePrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLotteryModels":
                    Result.DescribeLotteryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeLotteryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLotteryModel":
                    Result.GetLotteryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLotteryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePrizeTables":
                    Result.DescribePrizeTablesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePrizeTablesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPrizeTable":
                    Result.GetPrizeTableResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPrizeTableRequest.FromJson(requestPayload)
                    );
                    break;
                case "drawByUserId":
                    Result.DrawByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DrawByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prediction":
                    Result.PredictionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PredictionRequest.FromJson(requestPayload)
                    );
                    break;
                case "predictionByUserId":
                    Result.PredictionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PredictionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "drawWithRandomSeedByUserId":
                    Result.DrawWithRandomSeedByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DrawWithRandomSeedByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeProbabilities":
                    Result.DescribeProbabilitiesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeProbabilitiesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeProbabilitiesByUserId":
                    Result.DescribeProbabilitiesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeProbabilitiesByUserIdRequest.FromJson(requestPayload)
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
                case "getCurrentLotteryMaster":
                    Result.GetCurrentLotteryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentLotteryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentLotteryMaster":
                    Result.PreUpdateCurrentLotteryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentLotteryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentLotteryMaster":
                    Result.UpdateCurrentLotteryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentLotteryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentLotteryMasterFromGitHub":
                    Result.UpdateCurrentLotteryMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentLotteryMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePrizeLimits":
                    Result.DescribePrizeLimitsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePrizeLimitsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPrizeLimit":
                    Result.GetPrizeLimitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPrizeLimitRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetPrizeLimit":
                    Result.ResetPrizeLimitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ResetPrizeLimitRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBoxes":
                    Result.DescribeBoxesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBoxesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBoxesByUserId":
                    Result.DescribeBoxesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBoxesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBox":
                    Result.GetBoxResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBoxRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBoxByUserId":
                    Result.GetBoxByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBoxByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetBox":
                    Result.ResetBoxResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ResetBoxRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetBoxByUserId":
                    Result.ResetBoxByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ResetBoxByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}