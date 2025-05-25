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

namespace Gs2.Gs2Money.Model.Cache
{
    public static class Gs2Money
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
                case "describeWallets":
                    Result.DescribeWalletsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeWalletsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeWalletsByUserId":
                    Result.DescribeWalletsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeWalletsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getWallet":
                    Result.GetWalletResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetWalletRequest.FromJson(requestPayload)
                    );
                    break;
                case "getWalletByUserId":
                    Result.GetWalletByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetWalletByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "depositByUserId":
                    Result.DepositByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DepositByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdraw":
                    Result.WithdrawResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WithdrawRequest.FromJson(requestPayload)
                    );
                    break;
                case "withdrawByUserId":
                    Result.WithdrawByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.WithdrawByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceipts":
                    Result.DescribeReceiptsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeReceiptsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getByUserIdAndTransactionId":
                    Result.GetByUserIdAndTransactionIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetByUserIdAndTransactionIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "recordReceipt":
                    Result.RecordReceiptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RecordReceiptRequest.FromJson(requestPayload)
                    );
                    break;
                case "revertRecordReceipt":
                    Result.RevertRecordReceiptResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RevertRecordReceiptRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}