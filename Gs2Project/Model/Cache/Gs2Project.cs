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

namespace Gs2.Gs2Project.Model.Cache
{
    public static class Gs2Project
    {
        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "createAccount":
                    Result.CreateAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "verify":
                    Result.VerifyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.VerifyRequest.FromJson(requestPayload)
                    );
                    break;
                case "signIn":
                    Result.SignInResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SignInRequest.FromJson(requestPayload)
                    );
                    break;
                case "forget":
                    Result.ForgetResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ForgetRequest.FromJson(requestPayload)
                    );
                    break;
                case "issuePassword":
                    Result.IssuePasswordResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.IssuePasswordRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateAccount":
                    Result.UpdateAccountResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateAccountRequest.FromJson(requestPayload)
                    );
                    break;
                case "enableMfa":
                    Result.EnableMfaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.EnableMfaRequest.FromJson(requestPayload)
                    );
                    break;
                case "challengeMfa":
                    Result.ChallengeMfaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ChallengeMfaRequest.FromJson(requestPayload)
                    );
                    break;
                case "disableMfa":
                    Result.DisableMfaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DisableMfaRequest.FromJson(requestPayload)
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
                case "getServiceVersion":
                    Result.GetServiceVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetServiceVersionRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeProjects":
                    Result.DescribeProjectsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeProjectsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createProject":
                    Result.CreateProjectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateProjectRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProject":
                    Result.GetProjectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetProjectRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProjectToken":
                    Result.GetProjectTokenResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetProjectTokenRequest.FromJson(requestPayload)
                    );
                    break;
                case "getProjectTokenByIdentifier":
                    Result.GetProjectTokenByIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetProjectTokenByIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateProject":
                    Result.UpdateProjectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateProjectRequest.FromJson(requestPayload)
                    );
                    break;
                case "activateRegion":
                    Result.ActivateRegionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ActivateRegionRequest.FromJson(requestPayload)
                    );
                    break;
                case "waitActivateRegion":
                    Result.WaitActivateRegionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WaitActivateRegionRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteProject":
                    Result.DeleteProjectResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteProjectRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBillingMethods":
                    Result.DescribeBillingMethodsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBillingMethodsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createBillingMethod":
                    Result.CreateBillingMethodResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateBillingMethodRequest.FromJson(requestPayload)
                    );
                    break;
                case "getBillingMethod":
                    Result.GetBillingMethodResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetBillingMethodRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateBillingMethod":
                    Result.UpdateBillingMethodResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateBillingMethodRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteBillingMethod":
                    Result.DeleteBillingMethodResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteBillingMethodRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeReceipts":
                    Result.DescribeReceiptsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeReceiptsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeBillings":
                    Result.DescribeBillingsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeBillingsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDumpProgresses":
                    Result.DescribeDumpProgressesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDumpProgressesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDumpProgress":
                    Result.GetDumpProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDumpProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "waitDumpUserData":
                    Result.WaitDumpUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WaitDumpUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "archiveDumpUserData":
                    Result.ArchiveDumpUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ArchiveDumpUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "dumpUserData":
                    Result.DumpUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DumpUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDumpUserData":
                    Result.GetDumpUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDumpUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCleanProgresses":
                    Result.DescribeCleanProgressesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeCleanProgressesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCleanProgress":
                    Result.GetCleanProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCleanProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "waitCleanUserData":
                    Result.WaitCleanUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WaitCleanUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "cleanUserData":
                    Result.CleanUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CleanUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeImportProgresses":
                    Result.DescribeImportProgressesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeImportProgressesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getImportProgress":
                    Result.GetImportProgressResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetImportProgressRequest.FromJson(requestPayload)
                    );
                    break;
                case "waitImportUserData":
                    Result.WaitImportUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.WaitImportUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareImportUserData":
                    Result.PrepareImportUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareImportUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "importUserData":
                    Result.ImportUserDataResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ImportUserDataRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeImportErrorLogs":
                    Result.DescribeImportErrorLogsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeImportErrorLogsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getImportErrorLog":
                    Result.GetImportErrorLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetImportErrorLogRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}