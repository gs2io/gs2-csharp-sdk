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

namespace Gs2.Gs2Ranking2.Model.Cache
{
    public static class Gs2Ranking2
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
                case "describeGlobalRankingModels":
                    Result.DescribeGlobalRankingModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRankingModel":
                    Result.GetGlobalRankingModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalRankingModelMasters":
                    Result.DescribeGlobalRankingModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGlobalRankingModelMaster":
                    Result.CreateGlobalRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGlobalRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRankingModelMaster":
                    Result.GetGlobalRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGlobalRankingModelMaster":
                    Result.UpdateGlobalRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGlobalRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGlobalRankingModelMaster":
                    Result.DeleteGlobalRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGlobalRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalRankingScores":
                    Result.DescribeGlobalRankingScoresResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingScoresRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalRankingScoresByUserId":
                    Result.DescribeGlobalRankingScoresByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingScoresByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "putGlobalRankingScore":
                    Result.PutGlobalRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutGlobalRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "putGlobalRankingScoreByUserId":
                    Result.PutGlobalRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutGlobalRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRankingScore":
                    Result.GetGlobalRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRankingScoreByUserId":
                    Result.GetGlobalRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGlobalRankingScoreByUserId":
                    Result.DeleteGlobalRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGlobalRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyGlobalRankingScore":
                    Result.VerifyGlobalRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyGlobalRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyGlobalRankingScoreByUserId":
                    Result.VerifyGlobalRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyGlobalRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalRankingReceivedRewards":
                    Result.DescribeGlobalRankingReceivedRewardsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingReceivedRewardsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalRankingReceivedRewardsByUserId":
                    Result.DescribeGlobalRankingReceivedRewardsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingReceivedRewardsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGlobalRankingReceivedReward":
                    Result.CreateGlobalRankingReceivedRewardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGlobalRankingReceivedRewardRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGlobalRankingReceivedRewardByUserId":
                    Result.CreateGlobalRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGlobalRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveGlobalRankingReceivedReward":
                    Result.ReceiveGlobalRankingReceivedRewardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveGlobalRankingReceivedRewardRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveGlobalRankingReceivedRewardByUserId":
                    Result.ReceiveGlobalRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveGlobalRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRankingReceivedReward":
                    Result.GetGlobalRankingReceivedRewardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingReceivedRewardRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRankingReceivedRewardByUserId":
                    Result.GetGlobalRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGlobalRankingReceivedRewardByUserId":
                    Result.DeleteGlobalRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGlobalRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalRankings":
                    Result.DescribeGlobalRankingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGlobalRankingsByUserId":
                    Result.DescribeGlobalRankingsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGlobalRankingsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRanking":
                    Result.GetGlobalRankingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGlobalRankingByUserId":
                    Result.GetGlobalRankingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGlobalRankingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankingModels":
                    Result.DescribeClusterRankingModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRankingModel":
                    Result.GetClusterRankingModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankingModelMasters":
                    Result.DescribeClusterRankingModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createClusterRankingModelMaster":
                    Result.CreateClusterRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateClusterRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRankingModelMaster":
                    Result.GetClusterRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateClusterRankingModelMaster":
                    Result.UpdateClusterRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateClusterRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteClusterRankingModelMaster":
                    Result.DeleteClusterRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteClusterRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankingScores":
                    Result.DescribeClusterRankingScoresResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingScoresRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankingScoresByUserId":
                    Result.DescribeClusterRankingScoresByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingScoresByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "putClusterRankingScore":
                    Result.PutClusterRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutClusterRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "putClusterRankingScoreByUserId":
                    Result.PutClusterRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutClusterRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRankingScore":
                    Result.GetClusterRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRankingScoreByUserId":
                    Result.GetClusterRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteClusterRankingScoreByUserId":
                    Result.DeleteClusterRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteClusterRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyClusterRankingScore":
                    Result.VerifyClusterRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyClusterRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyClusterRankingScoreByUserId":
                    Result.VerifyClusterRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyClusterRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankingReceivedRewards":
                    Result.DescribeClusterRankingReceivedRewardsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingReceivedRewardsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankingReceivedRewardsByUserId":
                    Result.DescribeClusterRankingReceivedRewardsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingReceivedRewardsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createClusterRankingReceivedReward":
                    Result.CreateClusterRankingReceivedRewardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateClusterRankingReceivedRewardRequest.FromJson(requestPayload)
                    );
                    break;
                case "createClusterRankingReceivedRewardByUserId":
                    Result.CreateClusterRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateClusterRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveClusterRankingReceivedReward":
                    Result.ReceiveClusterRankingReceivedRewardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveClusterRankingReceivedRewardRequest.FromJson(requestPayload)
                    );
                    break;
                case "receiveClusterRankingReceivedRewardByUserId":
                    Result.ReceiveClusterRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ReceiveClusterRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRankingReceivedReward":
                    Result.GetClusterRankingReceivedRewardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingReceivedRewardRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRankingReceivedRewardByUserId":
                    Result.GetClusterRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteClusterRankingReceivedRewardByUserId":
                    Result.DeleteClusterRankingReceivedRewardByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteClusterRankingReceivedRewardByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankings":
                    Result.DescribeClusterRankingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeClusterRankingsByUserId":
                    Result.DescribeClusterRankingsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeClusterRankingsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRanking":
                    Result.GetClusterRankingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getClusterRankingByUserId":
                    Result.GetClusterRankingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetClusterRankingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribeRankingModels":
                    Result.DescribeSubscribeRankingModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribeRankingModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeRankingModel":
                    Result.GetSubscribeRankingModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRankingModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribeRankingModelMasters":
                    Result.DescribeSubscribeRankingModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribeRankingModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSubscribeRankingModelMaster":
                    Result.CreateSubscribeRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateSubscribeRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeRankingModelMaster":
                    Result.GetSubscribeRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSubscribeRankingModelMaster":
                    Result.UpdateSubscribeRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateSubscribeRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSubscribeRankingModelMaster":
                    Result.DeleteSubscribeRankingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSubscribeRankingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribes":
                    Result.DescribeSubscribesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribesByUserId":
                    Result.DescribeSubscribesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addSubscribe":
                    Result.AddSubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddSubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "addSubscribeByUserId":
                    Result.AddSubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddSubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribeRankingScores":
                    Result.DescribeSubscribeRankingScoresResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribeRankingScoresRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribeRankingScoresByUserId":
                    Result.DescribeSubscribeRankingScoresByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribeRankingScoresByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "putSubscribeRankingScore":
                    Result.PutSubscribeRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutSubscribeRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "putSubscribeRankingScoreByUserId":
                    Result.PutSubscribeRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutSubscribeRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeRankingScore":
                    Result.GetSubscribeRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeRankingScoreByUserId":
                    Result.GetSubscribeRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSubscribeRankingScoreByUserId":
                    Result.DeleteSubscribeRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSubscribeRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifySubscribeRankingScore":
                    Result.VerifySubscribeRankingScoreResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifySubscribeRankingScoreRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifySubscribeRankingScoreByUserId":
                    Result.VerifySubscribeRankingScoreByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifySubscribeRankingScoreByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribeRankings":
                    Result.DescribeSubscribeRankingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribeRankingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSubscribeRankingsByUserId":
                    Result.DescribeSubscribeRankingsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSubscribeRankingsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeRanking":
                    Result.GetSubscribeRankingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRankingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeRankingByUserId":
                    Result.GetSubscribeRankingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRankingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentRankingMaster":
                    Result.GetCurrentRankingMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentRankingMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRankingMaster":
                    Result.UpdateCurrentRankingMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentRankingMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentRankingMasterFromGitHub":
                    Result.UpdateCurrentRankingMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentRankingMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribe":
                    Result.GetSubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSubscribeByUserId":
                    Result.GetSubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSubscribe":
                    Result.DeleteSubscribeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSubscribeRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSubscribeByUserId":
                    Result.DeleteSubscribeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSubscribeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}