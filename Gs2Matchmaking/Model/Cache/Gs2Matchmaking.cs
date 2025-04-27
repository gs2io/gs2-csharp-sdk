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

namespace Gs2.Gs2Matchmaking.Model.Cache
{
    public static class Gs2Matchmaking
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
                case "describeGatherings":
                    Result.DescribeGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGathering":
                    Result.CreateGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGatheringByUserId":
                    Result.CreateGatheringByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGatheringByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGathering":
                    Result.UpdateGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGatheringByUserId":
                    Result.UpdateGatheringByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGatheringByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "doMatchmakingByPlayer":
                    Result.DoMatchmakingByPlayerResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DoMatchmakingByPlayerRequest.FromJson(requestPayload)
                    );
                    break;
                case "doMatchmaking":
                    Result.DoMatchmakingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DoMatchmakingRequest.FromJson(requestPayload)
                    );
                    break;
                case "doMatchmakingByUserId":
                    Result.DoMatchmakingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DoMatchmakingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "ping":
                    Result.PingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PingRequest.FromJson(requestPayload)
                    );
                    break;
                case "pingByUserId":
                    Result.PingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGathering":
                    Result.GetGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "cancelMatchmaking":
                    Result.CancelMatchmakingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CancelMatchmakingRequest.FromJson(requestPayload)
                    );
                    break;
                case "cancelMatchmakingByUserId":
                    Result.CancelMatchmakingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CancelMatchmakingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "earlyComplete":
                    Result.EarlyCompleteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.EarlyCompleteRequest.FromJson(requestPayload)
                    );
                    break;
                case "earlyCompleteByUserId":
                    Result.EarlyCompleteByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.EarlyCompleteByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGathering":
                    Result.DeleteGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatingModelMasters":
                    Result.DescribeRatingModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRatingModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRatingModelMaster":
                    Result.CreateRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRatingModelMaster":
                    Result.GetRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRatingModelMaster":
                    Result.UpdateRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRatingModelMaster":
                    Result.DeleteRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatingModels":
                    Result.DescribeRatingModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRatingModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRatingModel":
                    Result.GetRatingModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRatingModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentModelMaster":
                    Result.GetCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentModelMaster":
                    Result.PreUpdateCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PreUpdateCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMaster":
                    Result.UpdateCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMasterFromGitHub":
                    Result.UpdateCurrentModelMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentModelMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSeasonModels":
                    Result.DescribeSeasonModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSeasonModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSeasonModel":
                    Result.GetSeasonModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSeasonModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSeasonModelMasters":
                    Result.DescribeSeasonModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSeasonModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSeasonModelMaster":
                    Result.CreateSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSeasonModelMaster":
                    Result.GetSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSeasonModelMaster":
                    Result.UpdateSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSeasonModelMaster":
                    Result.DeleteSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSeasonGatherings":
                    Result.DescribeSeasonGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSeasonGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMatchmakingSeasonGatherings":
                    Result.DescribeMatchmakingSeasonGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMatchmakingSeasonGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "doSeasonMatchmaking":
                    Result.DoSeasonMatchmakingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DoSeasonMatchmakingRequest.FromJson(requestPayload)
                    );
                    break;
                case "doSeasonMatchmakingByUserId":
                    Result.DoSeasonMatchmakingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DoSeasonMatchmakingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSeasonGathering":
                    Result.GetSeasonGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSeasonGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeParticipant":
                    Result.VerifyIncludeParticipantResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyIncludeParticipantRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeParticipantByUserId":
                    Result.VerifyIncludeParticipantByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyIncludeParticipantByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSeasonGathering":
                    Result.DeleteSeasonGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSeasonGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedSeasonGatherings":
                    Result.DescribeJoinedSeasonGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeJoinedSeasonGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedSeasonGatheringsByUserId":
                    Result.DescribeJoinedSeasonGatheringsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeJoinedSeasonGatheringsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedSeasonGathering":
                    Result.GetJoinedSeasonGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetJoinedSeasonGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedSeasonGatheringByUserId":
                    Result.GetJoinedSeasonGatheringByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetJoinedSeasonGatheringByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatings":
                    Result.DescribeRatingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRatingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatingsByUserId":
                    Result.DescribeRatingsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRatingsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRating":
                    Result.GetRatingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRatingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRatingByUserId":
                    Result.GetRatingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRatingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "putResult":
                    Result.PutResultResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PutResultRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRating":
                    Result.DeleteRatingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRatingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBallot":
                    Result.GetBallotResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBallotRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBallotByUserId":
                    Result.GetBallotByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetBallotByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "vote":
                    Result.VoteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VoteRequest.FromJson(requestPayload)
                    );
                    break;
                case "voteMultiple":
                    Result.VoteMultipleResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VoteMultipleRequest.FromJson(requestPayload)
                    );
                    break;
                case "commitVote":
                    Result.CommitVoteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CommitVoteRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}