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
                case "describeLotteryModelMasters":
                    Result.DescribeLotteryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLotteryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createLotteryModelMaster":
                    Result.CreateLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLotteryModelMaster":
                    Result.GetLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateLotteryModelMaster":
                    Result.UpdateLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteLotteryModelMaster":
                    Result.DeleteLotteryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteLotteryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePrizeTableMasters":
                    Result.DescribePrizeTableMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePrizeTableMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPrizeTableMaster":
                    Result.CreatePrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreatePrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPrizeTableMaster":
                    Result.GetPrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updatePrizeTableMaster":
                    Result.UpdatePrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdatePrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePrizeTableMaster":
                    Result.DeletePrizeTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePrizeTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLotteryModels":
                    Result.DescribeLotteryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLotteryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLotteryModel":
                    Result.GetLotteryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLotteryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePrizeTables":
                    Result.DescribePrizeTablesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePrizeTablesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPrizeTable":
                    Result.GetPrizeTableResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPrizeTableRequest.FromJson(requestPayload)
                    );
                    break;
                case "drawByUserId":
                    Result.DrawByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DrawByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prediction":
                    Result.PredictionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PredictionRequest.FromJson(requestPayload)
                    );
                    break;
                case "predictionByUserId":
                    Result.PredictionByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PredictionByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "drawWithRandomSeedByUserId":
                    Result.DrawWithRandomSeedByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DrawWithRandomSeedByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeProbabilities":
                    Result.DescribeProbabilitiesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeProbabilitiesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeProbabilitiesByUserId":
                    Result.DescribeProbabilitiesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeProbabilitiesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentLotteryMaster":
                    Result.GetCurrentLotteryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentLotteryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentLotteryMaster":
                    Result.UpdateCurrentLotteryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentLotteryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentLotteryMasterFromGitHub":
                    Result.UpdateCurrentLotteryMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentLotteryMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePrizeLimits":
                    Result.DescribePrizeLimitsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePrizeLimitsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPrizeLimit":
                    Result.GetPrizeLimitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPrizeLimitRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetPrizeLimit":
                    Result.ResetPrizeLimitResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetPrizeLimitRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBoxes":
                    Result.DescribeBoxesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBoxesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBoxesByUserId":
                    Result.DescribeBoxesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBoxesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBox":
                    Result.GetBoxResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBoxRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBoxByUserId":
                    Result.GetBoxByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBoxByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetBox":
                    Result.ResetBoxResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetBoxRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetBoxByUserId":
                    Result.ResetBoxByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetBoxByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}