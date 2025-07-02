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
                case "describeGuildModelMasters":
                    Result.DescribeGuildModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeGuildModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGuildModelMaster":
                    Result.CreateGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuildModelMaster":
                    Result.GetGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGuildModelMaster":
                    Result.UpdateGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGuildModelMaster":
                    Result.DeleteGuildModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteGuildModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeGuildModels":
                    Result.DescribeGuildModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeGuildModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuildModel":
                    Result.GetGuildModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetGuildModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "searchGuilds":
                    Result.SearchGuildsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SearchGuildsRequest.FromJson(requestPayload)
                    );
                    break;
                case "searchGuildsByUserId":
                    Result.SearchGuildsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SearchGuildsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGuild":
                    Result.CreateGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "createGuildByUserId":
                    Result.CreateGuildByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateGuildByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuild":
                    Result.GetGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "getGuildByUserId":
                    Result.GetGuildByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetGuildByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGuild":
                    Result.UpdateGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateGuildByGuildName":
                    Result.UpdateGuildByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateGuildByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMember":
                    Result.DeleteMemberResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteMemberRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMemberByGuildName":
                    Result.DeleteMemberByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteMemberByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMemberRole":
                    Result.UpdateMemberRoleResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateMemberRoleRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMemberRoleByGuildName":
                    Result.UpdateMemberRoleByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateMemberRoleByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchUpdateMemberRole":
                    Result.BatchUpdateMemberRoleResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.BatchUpdateMemberRoleRequest.FromJson(requestPayload)
                    );
                    break;
                case "batchUpdateMemberRoleByGuildName":
                    Result.BatchUpdateMemberRoleByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.BatchUpdateMemberRoleByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGuild":
                    Result.DeleteGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteGuildByGuildName":
                    Result.DeleteGuildByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteGuildByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "increaseMaximumCurrentMaximumMemberCountByGuildName":
                    Result.IncreaseMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaximumCurrentMaximumMemberCount":
                    Result.DecreaseMaximumCurrentMaximumMemberCountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DecreaseMaximumCurrentMaximumMemberCountRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaximumCurrentMaximumMemberCountByGuildName":
                    Result.DecreaseMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DecreaseMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCurrentMaximumMemberCount":
                    Result.VerifyCurrentMaximumMemberCountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyCurrentMaximumMemberCountRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyCurrentMaximumMemberCountByGuildName":
                    Result.VerifyCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeMember":
                    Result.VerifyIncludeMemberResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyIncludeMemberRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyIncludeMemberByUserId":
                    Result.VerifyIncludeMemberByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyIncludeMemberByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setMaximumCurrentMaximumMemberCountByGuildName":
                    Result.SetMaximumCurrentMaximumMemberCountByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetMaximumCurrentMaximumMemberCountByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "assume":
                    Result.AssumeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AssumeRequest.FromJson(requestPayload)
                    );
                    break;
                case "assumeByUserId":
                    Result.AssumeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AssumeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedGuilds":
                    Result.DescribeJoinedGuildsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeJoinedGuildsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeJoinedGuildsByUserId":
                    Result.DescribeJoinedGuildsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeJoinedGuildsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedGuild":
                    Result.GetJoinedGuildResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetJoinedGuildRequest.FromJson(requestPayload)
                    );
                    break;
                case "getJoinedGuildByUserId":
                    Result.GetJoinedGuildByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetJoinedGuildByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMemberMetadata":
                    Result.UpdateMemberMetadataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateMemberMetadataRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMemberMetadataByUserId":
                    Result.UpdateMemberMetadataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateMemberMetadataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdrawal":
                    Result.WithdrawalResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WithdrawalRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdrawalByUserId":
                    Result.WithdrawalByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WithdrawalByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLastGuildMasterActivity":
                    Result.GetLastGuildMasterActivityResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLastGuildMasterActivityRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLastGuildMasterActivityByGuildName":
                    Result.GetLastGuildMasterActivityByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLastGuildMasterActivityByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "promoteSeniorMember":
                    Result.PromoteSeniorMemberResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PromoteSeniorMemberRequest.FromJson(requestPayload)
                    );
                    break;
                case "promoteSeniorMemberByGuildName":
                    Result.PromoteSeniorMemberByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PromoteSeniorMemberByGuildNameRequest.FromJson(requestPayload)
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
                case "getCurrentGuildMaster":
                    Result.GetCurrentGuildMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentGuildMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentGuildMaster":
                    Result.PreUpdateCurrentGuildMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentGuildMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentGuildMaster":
                    Result.UpdateCurrentGuildMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentGuildMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentGuildMasterFromGitHub":
                    Result.UpdateCurrentGuildMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentGuildMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceiveRequests":
                    Result.DescribeReceiveRequestsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeReceiveRequestsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceiveRequestsByGuildName":
                    Result.DescribeReceiveRequestsByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeReceiveRequestsByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveRequest":
                    Result.GetReceiveRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetReceiveRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveRequestByGuildName":
                    Result.GetReceiveRequestByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetReceiveRequestByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "acceptRequest":
                    Result.AcceptRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcceptRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "acceptRequestByGuildName":
                    Result.AcceptRequestByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcceptRequestByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "rejectRequest":
                    Result.RejectRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RejectRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "rejectRequestByGuildName":
                    Result.RejectRequestByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RejectRequestByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSendRequests":
                    Result.DescribeSendRequestsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSendRequestsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSendRequestsByUserId":
                    Result.DescribeSendRequestsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeSendRequestsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSendRequest":
                    Result.GetSendRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSendRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSendRequestByUserId":
                    Result.GetSendRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetSendRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendRequest":
                    Result.SendRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SendRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendRequestByUserId":
                    Result.SendRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SendRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRequest":
                    Result.DeleteRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRequestByUserId":
                    Result.DeleteRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIgnoreUsers":
                    Result.DescribeIgnoreUsersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeIgnoreUsersRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIgnoreUsersByGuildName":
                    Result.DescribeIgnoreUsersByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeIgnoreUsersByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIgnoreUser":
                    Result.GetIgnoreUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetIgnoreUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIgnoreUserByGuildName":
                    Result.GetIgnoreUserByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetIgnoreUserByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "addIgnoreUser":
                    Result.AddIgnoreUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AddIgnoreUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "addIgnoreUserByGuildName":
                    Result.AddIgnoreUserByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AddIgnoreUserByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteIgnoreUser":
                    Result.DeleteIgnoreUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteIgnoreUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteIgnoreUserByGuildName":
                    Result.DeleteIgnoreUserByGuildNameResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteIgnoreUserByGuildNameRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}