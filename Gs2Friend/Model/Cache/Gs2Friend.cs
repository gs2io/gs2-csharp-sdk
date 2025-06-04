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

namespace Gs2.Gs2Friend.Model.Cache
{
    public static class Gs2Friend
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
                case "getServiceVersion":
                    Result.GetServiceVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetServiceVersionRequest.FromJson(requestPayload)
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
                case "getProfile":
                    Result.GetProfileResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetProfileRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProfileByUserId":
                    Result.GetProfileByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetProfileByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateProfile":
                    Result.UpdateProfileResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateProfileRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateProfileByUserId":
                    Result.UpdateProfileByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateProfileByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteProfileByUserId":
                    Result.DeleteProfileByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteProfileByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFriends":
                    Result.DescribeFriendsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeFriendsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFriendsByUserId":
                    Result.DescribeFriendsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeFriendsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBlackList":
                    Result.DescribeBlackListResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBlackListRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBlackListByUserId":
                    Result.DescribeBlackListByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeBlackListByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "registerBlackList":
                    Result.RegisterBlackListResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RegisterBlackListRequest.FromJson(requestPayload)
                    );
                    break;
                case "registerBlackListByUserId":
                    Result.RegisterBlackListByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RegisterBlackListByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unregisterBlackList":
                    Result.UnregisterBlackListResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnregisterBlackListRequest.FromJson(requestPayload)
                    );
                    break;
                case "unregisterBlackListByUserId":
                    Result.UnregisterBlackListByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnregisterBlackListByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFollows":
                    Result.DescribeFollowsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeFollowsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFollowsByUserId":
                    Result.DescribeFollowsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeFollowsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFollow":
                    Result.GetFollowResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFollowRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFollowByUserId":
                    Result.GetFollowByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFollowByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "follow":
                    Result.FollowResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.FollowRequest.FromJson(requestPayload)
                    );
                    break;
                case "followByUserId":
                    Result.FollowByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.FollowByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "unfollow":
                    Result.UnfollowResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnfollowRequest.FromJson(requestPayload)
                    );
                    break;
                case "unfollowByUserId":
                    Result.UnfollowByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UnfollowByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFriend":
                    Result.GetFriendResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFriendRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFriendByUserId":
                    Result.GetFriendByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFriendByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addFriend":
                    Result.AddFriendResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddFriendRequest.FromJson(requestPayload)
                    );
                    break;
                case "addFriendByUserId":
                    Result.AddFriendByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddFriendByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFriend":
                    Result.DeleteFriendResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteFriendRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFriendByUserId":
                    Result.DeleteFriendByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteFriendByUserIdRequest.FromJson(requestPayload)
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
                case "describeReceiveRequests":
                    Result.DescribeReceiveRequestsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeReceiveRequestsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceiveRequestsByUserId":
                    Result.DescribeReceiveRequestsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeReceiveRequestsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveRequest":
                    Result.GetReceiveRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetReceiveRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "getReceiveRequestByUserId":
                    Result.GetReceiveRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetReceiveRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "acceptRequest":
                    Result.AcceptRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcceptRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "acceptRequestByUserId":
                    Result.AcceptRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcceptRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "rejectRequest":
                    Result.RejectRequestResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RejectRequestRequest.FromJson(requestPayload)
                    );
                    break;
                case "rejectRequestByUserId":
                    Result.RejectRequestByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RejectRequestByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPublicProfile":
                    Result.GetPublicProfileResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPublicProfileRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}