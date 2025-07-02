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

namespace Gs2.Gs2SeasonRating.Model.Cache
{
    public static class Gs2SeasonRating
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
                case "describeMatchSessions":
                    Result.DescribeMatchSessionsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeMatchSessionsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createMatchSession":
                    Result.CreateMatchSessionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateMatchSessionRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMatchSession":
                    Result.GetMatchSessionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetMatchSessionRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMatchSession":
                    Result.DeleteMatchSessionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteMatchSessionRequest.FromJson(requestPayload)
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
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentSeasonModelMaster":
                    Result.GetCurrentSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentSeasonModelMaster":
                    Result.PreUpdateCurrentSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentSeasonModelMaster":
                    Result.UpdateCurrentSeasonModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentSeasonModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentSeasonModelMasterFromGitHub":
                    Result.UpdateCurrentSeasonModelMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentSeasonModelMasterFromGitHubRequest.FromJson(requestPayload)
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