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
                case "describeAccounts":
                    Result.DescribeAccountsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeAccountsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createAccount":
                    Result.CreateAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTimeOffset":
                    Result.UpdateTimeOffsetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateTimeOffsetRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBanned":
                    Result.UpdateBannedResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateBannedRequest.FromJson(requestPayload)
                    );
                    break;
                case "addBan":
                    Result.AddBanResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AddBanRequest.FromJson(requestPayload)
                    );
                    break;
                case "removeBan":
                    Result.RemoveBanResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RemoveBanRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAccount":
                    Result.GetAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteAccount":
                    Result.DeleteAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "authentication":
                    Result.AuthenticationResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AuthenticationRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOvers":
                    Result.DescribeTakeOversResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeTakeOversRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOversByUserId":
                    Result.DescribeTakeOversByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeTakeOversByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOver":
                    Result.CreateTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverByUserId":
                    Result.CreateTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverOpenIdConnect":
                    Result.CreateTakeOverOpenIdConnectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateTakeOverOpenIdConnectRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverOpenIdConnectAndByUserId":
                    Result.CreateTakeOverOpenIdConnectAndByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateTakeOverOpenIdConnectAndByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOver":
                    Result.GetTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOverByUserId":
                    Result.GetTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTakeOver":
                    Result.UpdateTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTakeOverByUserId":
                    Result.UpdateTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOver":
                    Result.DeleteTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOverByUserIdentifier":
                    Result.DeleteTakeOverByUserIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteTakeOverByUserIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOverByUserId":
                    Result.DeleteTakeOverByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteTakeOverByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "doTakeOver":
                    Result.DoTakeOverResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoTakeOverRequest.FromJson(requestPayload)
                    );
                    break;
                case "doTakeOverOpenIdConnect":
                    Result.DoTakeOverOpenIdConnectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DoTakeOverOpenIdConnectRequest.FromJson(requestPayload)
                    );
                    break;
                case "getAuthorizationUrl":
                    Result.GetAuthorizationUrlResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetAuthorizationUrlRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePlatformIds":
                    Result.DescribePlatformIdsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePlatformIdsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePlatformIdsByUserId":
                    Result.DescribePlatformIdsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePlatformIdsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPlatformId":
                    Result.CreatePlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreatePlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPlatformIdByUserId":
                    Result.CreatePlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreatePlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPlatformId":
                    Result.GetPlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPlatformIdByUserId":
                    Result.GetPlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "findPlatformId":
                    Result.FindPlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FindPlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "findPlatformIdByUserId":
                    Result.FindPlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.FindPlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePlatformId":
                    Result.DeletePlatformIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeletePlatformIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePlatformIdByUserIdentifier":
                    Result.DeletePlatformIdByUserIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeletePlatformIdByUserIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePlatformIdByUserId":
                    Result.DeletePlatformIdByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeletePlatformIdByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDataOwnerByUserId":
                    Result.GetDataOwnerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDataOwnerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateDataOwnerByUserId":
                    Result.UpdateDataOwnerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateDataOwnerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteDataOwnerByUserId":
                    Result.DeleteDataOwnerByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteDataOwnerByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOverTypeModels":
                    Result.DescribeTakeOverTypeModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeTakeOverTypeModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOverTypeModel":
                    Result.GetTakeOverTypeModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetTakeOverTypeModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeTakeOverTypeModelMasters":
                    Result.DescribeTakeOverTypeModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeTakeOverTypeModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createTakeOverTypeModelMaster":
                    Result.CreateTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateTakeOverTypeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTakeOverTypeModelMaster":
                    Result.GetTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetTakeOverTypeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateTakeOverTypeModelMaster":
                    Result.UpdateTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateTakeOverTypeModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteTakeOverTypeModelMaster":
                    Result.DeleteTakeOverTypeModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteTakeOverTypeModelMasterRequest.FromJson(requestPayload)
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
            }
        }
    }
}