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

namespace Gs2.Gs2Ranking.Model.Cache
{
    public static class Gs2Ranking
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
                case "describeCategoryModels":
                    Result.DescribeCategoryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeCategoryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCategoryModel":
                    Result.GetCategoryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCategoryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCategoryModelMasters":
                    Result.DescribeCategoryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeCategoryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createCategoryModelMaster":
                    Result.CreateCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCategoryModelMaster":
                    Result.GetCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCategoryModelMaster":
                    Result.UpdateCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteCategoryModelMaster":
                    Result.DeleteCategoryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteCategoryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "subscribe":
                    Result.SubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "subscribeByUserId":
                    Result.SubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeScores":
                    Result.DescribeScoresResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeScoresRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeScoresByUserId":
                    Result.DescribeScoresByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeScoresByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getScore":
                    Result.GetScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "getScoreByUserId":
                    Result.GetScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRankings":
                    Result.DescribeRankingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRankingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRankingssByUserId":
                    Result.DescribeRankingssByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRankingssByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeNearRankings":
                    Result.DescribeNearRankingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeNearRankingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRanking":
                    Result.GetRankingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRankingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRankingByUserId":
                    Result.GetRankingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRankingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "putScore":
                    Result.PutScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PutScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "putScoreByUserId":
                    Result.PutScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PutScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "calcRanking":
                    Result.CalcRankingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CalcRankingRequest.FromJson(requestPayload)
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
                case "getCurrentRankingMaster":
                    Result.GetCurrentRankingMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentRankingMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentRankingMaster":
                    Result.PreUpdateCurrentRankingMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentRankingMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRankingMaster":
                    Result.UpdateCurrentRankingMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentRankingMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRankingMasterFromGitHub":
                    Result.UpdateCurrentRankingMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentRankingMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribe":
                    Result.GetSubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeByUserId":
                    Result.GetSubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unsubscribe":
                    Result.UnsubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UnsubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "unsubscribeByUserId":
                    Result.UnsubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UnsubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribesByCategoryName":
                    Result.DescribeSubscribesByCategoryNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSubscribesByCategoryNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribesByCategoryNameAndUserId":
                    Result.DescribeSubscribesByCategoryNameAndUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSubscribesByCategoryNameAndUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}