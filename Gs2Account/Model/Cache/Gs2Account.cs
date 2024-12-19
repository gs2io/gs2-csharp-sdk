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

namespace Gs2.Gs2Account.Model.Cache
{
    public static class Gs2Account
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
                case "describeAccounts":
                    Result.DescribeAccountsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAccountsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createAccount":
                    Result.CreateAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTimeOffset":
                    Result.UpdateTimeOffsetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateTimeOffsetRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBanned":
                    Result.UpdateBannedResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateBannedRequest.FromJson(requestPayload)
                    );
                    break;
                case "addBan":
                    Result.AddBanResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddBanRequest.FromJson(requestPayload)
                    );
                    break;
                case "removeBan":
                    Result.RemoveBanResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RemoveBanRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAccount":
                    Result.GetAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAccount":
                    Result.DeleteAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "authentication":
                    Result.AuthenticationResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AuthenticationRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOvers":
                    Result.DescribeTakeOversResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeTakeOversRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOversByUserId":
                    Result.DescribeTakeOversByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeTakeOversByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOver":
                    Result.CreateTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverByUserId":
                    Result.CreateTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverOpenIdConnect":
                    Result.CreateTakeOverOpenIdConnectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateTakeOverOpenIdConnectRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverOpenIdConnectAndByUserId":
                    Result.CreateTakeOverOpenIdConnectAndByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateTakeOverOpenIdConnectAndByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOver":
                    Result.GetTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOverByUserId":
                    Result.GetTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTakeOver":
                    Result.UpdateTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTakeOverByUserId":
                    Result.UpdateTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOver":
                    Result.DeleteTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOverByUserIdentifier":
                    Result.DeleteTakeOverByUserIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteTakeOverByUserIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOverByUserId":
                    Result.DeleteTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "doTakeOver":
                    Result.DoTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DoTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "doTakeOverOpenIdConnect":
                    Result.DoTakeOverOpenIdConnectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DoTakeOverOpenIdConnectRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAuthorizationUrl":
                    Result.GetAuthorizationUrlResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetAuthorizationUrlRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePlatformIds":
                    Result.DescribePlatformIdsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePlatformIdsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePlatformIdsByUserId":
                    Result.DescribePlatformIdsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePlatformIdsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPlatformId":
                    Result.CreatePlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreatePlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPlatformIdByUserId":
                    Result.CreatePlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreatePlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPlatformId":
                    Result.GetPlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPlatformIdByUserId":
                    Result.GetPlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "findPlatformId":
                    Result.FindPlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.FindPlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "findPlatformIdByUserId":
                    Result.FindPlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.FindPlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePlatformId":
                    Result.DeletePlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePlatformIdByUserIdentifier":
                    Result.DeletePlatformIdByUserIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePlatformIdByUserIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePlatformIdByUserId":
                    Result.DeletePlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDataOwnerByUserId":
                    Result.GetDataOwnerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetDataOwnerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteDataOwnerByUserId":
                    Result.DeleteDataOwnerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteDataOwnerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOverTypeModels":
                    Result.DescribeTakeOverTypeModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeTakeOverTypeModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOverTypeModel":
                    Result.GetTakeOverTypeModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetTakeOverTypeModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOverTypeModelMasters":
                    Result.DescribeTakeOverTypeModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeTakeOverTypeModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverTypeModelMaster":
                    Result.CreateTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateTakeOverTypeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOverTypeModelMaster":
                    Result.GetTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetTakeOverTypeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTakeOverTypeModelMaster":
                    Result.UpdateTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateTakeOverTypeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOverTypeModelMaster":
                    Result.DeleteTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteTakeOverTypeModelMasterRequest.FromJson(requestPayload)
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
            }
        }
    }
}