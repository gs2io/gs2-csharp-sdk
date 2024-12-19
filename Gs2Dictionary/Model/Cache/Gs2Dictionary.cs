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

namespace Gs2.Gs2Dictionary.Model.Cache
{
    public static class Gs2Dictionary
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
                case "describeEntryModels":
                    Result.DescribeEntryModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEntryModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEntryModel":
                    Result.GetEntryModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEntryModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEntryModelMasters":
                    Result.DescribeEntryModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEntryModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createEntryModelMaster":
                    Result.CreateEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEntryModelMaster":
                    Result.GetEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateEntryModelMaster":
                    Result.UpdateEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteEntryModelMaster":
                    Result.DeleteEntryModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteEntryModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEntries":
                    Result.DescribeEntriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEntriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeEntriesByUserId":
                    Result.DescribeEntriesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeEntriesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addEntriesByUserId":
                    Result.AddEntriesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddEntriesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEntry":
                    Result.GetEntryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEntryRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEntryByUserId":
                    Result.GetEntryByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEntryByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEntryWithSignature":
                    Result.GetEntryWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEntryWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getEntryWithSignatureByUserId":
                    Result.GetEntryWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetEntryWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetByUserId":
                    Result.ResetByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyEntry":
                    Result.VerifyEntryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyEntryRequest.FromJson(requestPayload)
                    );
                    break;
                case "verifyEntryByUserId":
                    Result.VerifyEntryByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.VerifyEntryByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteEntries":
                    Result.DeleteEntriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteEntriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteEntriesByUserId":
                    Result.DeleteEntriesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteEntriesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLikes":
                    Result.DescribeLikesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLikesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLikesByUserId":
                    Result.DescribeLikesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeLikesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addLikes":
                    Result.AddLikesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddLikesRequest.FromJson(requestPayload)
                    );
                    break;
                case "addLikesByUserId":
                    Result.AddLikesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddLikesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLike":
                    Result.GetLikeResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLikeRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLikeByUserId":
                    Result.GetLikeByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetLikeByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetLikes":
                    Result.ResetLikesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetLikesRequest.FromJson(requestPayload)
                    );
                    break;
                case "resetLikesByUserId":
                    Result.ResetLikesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ResetLikesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteLikes":
                    Result.DeleteLikesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteLikesRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteLikesByUserId":
                    Result.DeleteLikesByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteLikesByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentEntryMaster":
                    Result.GetCurrentEntryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentEntryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentEntryMaster":
                    Result.UpdateCurrentEntryMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentEntryMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentEntryMasterFromGitHub":
                    Result.UpdateCurrentEntryMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentEntryMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}