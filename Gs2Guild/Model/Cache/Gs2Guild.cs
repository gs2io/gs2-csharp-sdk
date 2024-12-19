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

namespace Gs2.Gs2Guild.Model.Cache
{
    public static class Gs2Guild
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
                case "describeGuildModelMasters":
                    Result.DescribeGuildModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGuildModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGuildModelMaster":
                    Result.CreateGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuildModelMaster":
                    Result.GetGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGuildModelMaster":
                    Result.UpdateGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGuildModelMaster":
                    Result.DeleteGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGuildModels":
                    Result.DescribeGuildModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeGuildModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuildModel":
                    Result.GetGuildModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGuildModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "searchGuilds":
                    Result.SearchGuildsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SearchGuildsRequest.FromJson(requestPayload)
                    );
                    break;
                case "searchGuildsByUserId":
                    Result.SearchGuildsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SearchGuildsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGuild":
                    Result.CreateGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGuildByUserId":
                    Result.CreateGuildByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateGuildByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuild":
                    Result.GetGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuildByUserId":
                    Result.GetGuildByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetGuildByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGuild":
                    Result.UpdateGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGuildByGuildName":
                    Result.UpdateGuildByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateGuildByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMember":
                    Result.DeleteMemberResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMemberRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMemberByGuildName":
                    Result.DeleteMemberByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMemberByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMemberRole":
                    Result.UpdateMemberRoleResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateMemberRoleRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMemberRoleByGuildName":
                    Result.UpdateMemberRoleByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateMemberRoleByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchUpdateMemberRole":
                    Result.BatchUpdateMemberRoleResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.BatchUpdateMemberRoleRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchUpdateMemberRoleByGuildName":
                    Result.BatchUpdateMemberRoleByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.BatchUpdateMemberRoleByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGuild":
                    Result.DeleteGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGuildByGuildName":
                    Result.DeleteGuildByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteGuildByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "increaseMaximumCurrentMaximumMemberCountByGuildName":
                    Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaximumCurrentMaximumMemberCount":
                    Result.DecreaseMaximumCurrentMaximumMemberCountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseMaximumCurrentMaximumMemberCountRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaximumCurrentMaximumMemberCountByGuildName":
                    Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCurrentMaximumMemberCount":
                    Result.VerifyCurrentMaximumMemberCountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCurrentMaximumMemberCountRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCurrentMaximumMemberCountByGuildName":
                    Result.VerifyCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeMember":
                    Result.VerifyIncludeMemberResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyIncludeMemberRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeMemberByUserId":
                    Result.VerifyIncludeMemberByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyIncludeMemberByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setMaximumCurrentMaximumMemberCountByGuildName":
                    Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "assume":
                    Result.AssumeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AssumeRequest.FromJson(requestPayload)
                    );
                    break;
                case "assumeByUserId":
                    Result.AssumeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AssumeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedGuilds":
                    Result.DescribeJoinedGuildsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeJoinedGuildsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedGuildsByUserId":
                    Result.DescribeJoinedGuildsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeJoinedGuildsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedGuild":
                    Result.GetJoinedGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetJoinedGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedGuildByUserId":
                    Result.GetJoinedGuildByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetJoinedGuildByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdrawal":
                    Result.WithdrawalResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WithdrawalRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdrawalByUserId":
                    Result.WithdrawalByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WithdrawalByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLastGuildMasterActivity":
                    Result.GetLastGuildMasterActivityResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLastGuildMasterActivityRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLastGuildMasterActivityByGuildName":
                    Result.GetLastGuildMasterActivityByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLastGuildMasterActivityByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "promoteSeniorMember":
                    Result.PromoteSeniorMemberResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PromoteSeniorMemberRequest.FromJson(requestPayload)
                    );
                    break;
                case "promoteSeniorMemberByGuildName":
                    Result.PromoteSeniorMemberByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PromoteSeniorMemberByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentGuildMaster":
                    Result.GetCurrentGuildMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentGuildMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentGuildMaster":
                    Result.UpdateCurrentGuildMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentGuildMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentGuildMasterFromGitHub":
                    Result.UpdateCurrentGuildMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentGuildMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceiveRequests":
                    Result.DescribeReceiveRequestsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeReceiveRequestsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceiveRequestsByGuildName":
                    Result.DescribeReceiveRequestsByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeReceiveRequestsByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveRequest":
                    Result.GetReceiveRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetReceiveRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveRequestByGuildName":
                    Result.GetReceiveRequestByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetReceiveRequestByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "acceptRequest":
                    Result.AcceptRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcceptRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "acceptRequestByGuildName":
                    Result.AcceptRequestByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcceptRequestByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "rejectRequest":
                    Result.RejectRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RejectRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "rejectRequestByGuildName":
                    Result.RejectRequestByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RejectRequestByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSendRequests":
                    Result.DescribeSendRequestsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSendRequestsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSendRequestsByUserId":
                    Result.DescribeSendRequestsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSendRequestsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSendRequest":
                    Result.GetSendRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSendRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSendRequestByUserId":
                    Result.GetSendRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSendRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendRequest":
                    Result.SendRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SendRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendRequestByUserId":
                    Result.SendRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SendRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRequest":
                    Result.DeleteRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRequestByUserId":
                    Result.DeleteRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIgnoreUsers":
                    Result.DescribeIgnoreUsersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeIgnoreUsersRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIgnoreUsersByGuildName":
                    Result.DescribeIgnoreUsersByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeIgnoreUsersByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIgnoreUser":
                    Result.GetIgnoreUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetIgnoreUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIgnoreUserByGuildName":
                    Result.GetIgnoreUserByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetIgnoreUserByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "addIgnoreUser":
                    Result.AddIgnoreUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddIgnoreUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "addIgnoreUserByGuildName":
                    Result.AddIgnoreUserByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddIgnoreUserByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteIgnoreUser":
                    Result.DeleteIgnoreUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteIgnoreUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteIgnoreUserByGuildName":
                    Result.DeleteIgnoreUserByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteIgnoreUserByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}