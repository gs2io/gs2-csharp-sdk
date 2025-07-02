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
                case "describeGatherings":
                    Result.DescribeGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGathering":
                    Result.CreateGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGatheringByUserId":
                    Result.CreateGatheringByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateGatheringByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGathering":
                    Result.UpdateGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGatheringByUserId":
                    Result.UpdateGatheringByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateGatheringByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "doMatchmakingByPlayer":
                    Result.DoMatchmakingByPlayerResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoMatchmakingByPlayerRequest.FromJson(requestPayload)
                    );
                    break;
                case "doMatchmaking":
                    Result.DoMatchmakingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoMatchmakingRequest.FromJson(requestPayload)
                    );
                    break;
                case "doMatchmakingByUserId":
                    Result.DoMatchmakingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoMatchmakingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "ping":
                    Result.PingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PingRequest.FromJson(requestPayload)
                    );
                    break;
                case "pingByUserId":
                    Result.PingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGathering":
                    Result.GetGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "cancelMatchmaking":
                    Result.CancelMatchmakingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CancelMatchmakingRequest.FromJson(requestPayload)
                    );
                    break;
                case "cancelMatchmakingByUserId":
                    Result.CancelMatchmakingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CancelMatchmakingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "earlyComplete":
                    Result.EarlyCompleteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EarlyCompleteRequest.FromJson(requestPayload)
                    );
                    break;
                case "earlyCompleteByUserId":
                    Result.EarlyCompleteByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EarlyCompleteByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGathering":
                    Result.DeleteGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatingModelMasters":
                    Result.DescribeRatingModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRatingModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRatingModelMaster":
                    Result.CreateRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRatingModelMaster":
                    Result.GetRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRatingModelMaster":
                    Result.UpdateRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRatingModelMaster":
                    Result.DeleteRatingModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteRatingModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatingModels":
                    Result.DescribeRatingModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRatingModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRatingModel":
                    Result.GetRatingModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRatingModelRequest.FromJson(requestPayload)
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
                case "getCurrentModelMaster":
                    Result.GetCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentModelMaster":
                    Result.PreUpdateCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMaster":
                    Result.UpdateCurrentModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentModelMasterFromGitHub":
                    Result.UpdateCurrentModelMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentModelMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSeasonModels":
                    Result.DescribeSeasonModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSeasonModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSeasonModel":
                    Result.GetSeasonModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSeasonModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSeasonModelMasters":
                    Result.DescribeSeasonModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSeasonModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSeasonModelMaster":
                    Result.CreateSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSeasonModelMaster":
                    Result.GetSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSeasonModelMaster":
                    Result.UpdateSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSeasonModelMaster":
                    Result.DeleteSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSeasonGatherings":
                    Result.DescribeSeasonGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSeasonGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMatchmakingSeasonGatherings":
                    Result.DescribeMatchmakingSeasonGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeMatchmakingSeasonGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "doSeasonMatchmaking":
                    Result.DoSeasonMatchmakingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoSeasonMatchmakingRequest.FromJson(requestPayload)
                    );
                    break;
                case "doSeasonMatchmakingByUserId":
                    Result.DoSeasonMatchmakingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoSeasonMatchmakingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSeasonGathering":
                    Result.GetSeasonGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSeasonGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeParticipant":
                    Result.VerifyIncludeParticipantResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyIncludeParticipantRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeParticipantByUserId":
                    Result.VerifyIncludeParticipantByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyIncludeParticipantByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSeasonGathering":
                    Result.DeleteSeasonGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteSeasonGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedSeasonGatherings":
                    Result.DescribeJoinedSeasonGatheringsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeJoinedSeasonGatheringsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedSeasonGatheringsByUserId":
                    Result.DescribeJoinedSeasonGatheringsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeJoinedSeasonGatheringsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedSeasonGathering":
                    Result.GetJoinedSeasonGatheringResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetJoinedSeasonGatheringRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedSeasonGatheringByUserId":
                    Result.GetJoinedSeasonGatheringByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetJoinedSeasonGatheringByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatings":
                    Result.DescribeRatingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRatingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRatingsByUserId":
                    Result.DescribeRatingsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeRatingsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRating":
                    Result.GetRatingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRatingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRatingByUserId":
                    Result.GetRatingByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetRatingByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "putResult":
                    Result.PutResultResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PutResultRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRating":
                    Result.DeleteRatingResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteRatingRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBallot":
                    Result.GetBallotResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBallotRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBallotByUserId":
                    Result.GetBallotByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBallotByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "vote":
                    Result.VoteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VoteRequest.FromJson(requestPayload)
                    );
                    break;
                case "voteMultiple":
                    Result.VoteMultipleResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VoteMultipleRequest.FromJson(requestPayload)
                    );
                    break;
                case "commitVote":
                    Result.CommitVoteResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CommitVoteRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}