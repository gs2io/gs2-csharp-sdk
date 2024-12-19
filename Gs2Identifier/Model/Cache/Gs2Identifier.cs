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

namespace Gs2.Gs2Identifier.Model.Cache
{
    public static class Gs2Identifier
    {
        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "describeUsers":
                    Result.DescribeUsersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeUsersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createUser":
                    Result.CreateUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateUser":
                    Result.UpdateUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "getUser":
                    Result.GetUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteUser":
                    Result.DeleteUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteUserRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeSecurityPolicies":
                    Result.DescribeSecurityPoliciesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeSecurityPoliciesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeCommonSecurityPolicies":
                    Result.DescribeCommonSecurityPoliciesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeCommonSecurityPoliciesRequest.FromJson(requestPayload)
                    );
                    break;
                case "createSecurityPolicy":
                    Result.CreateSecurityPolicyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateSecurityPolicyRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateSecurityPolicy":
                    Result.UpdateSecurityPolicyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateSecurityPolicyRequest.FromJson(requestPayload)
                    );
                    break;
                case "getSecurityPolicy":
                    Result.GetSecurityPolicyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetSecurityPolicyRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteSecurityPolicy":
                    Result.DeleteSecurityPolicyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteSecurityPolicyRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeIdentifiers":
                    Result.DescribeIdentifiersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeIdentifiersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createIdentifier":
                    Result.CreateIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "getIdentifier":
                    Result.GetIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteIdentifier":
                    Result.DeleteIdentifierResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteIdentifierRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeAttachedGuards":
                    Result.DescribeAttachedGuardsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeAttachedGuardsRequest.FromJson(requestPayload)
                    );
                    break;
                case "attachGuard":
                    Result.AttachGuardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AttachGuardRequest.FromJson(requestPayload)
                    );
                    break;
                case "detachGuard":
                    Result.DetachGuardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DetachGuardRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPassword":
                    Result.CreatePasswordResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreatePasswordRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPassword":
                    Result.GetPasswordResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPasswordRequest.FromJson(requestPayload)
                    );
                    break;
                case "enableMfa":
                    Result.EnableMfaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.EnableMfaRequest.FromJson(requestPayload)
                    );
                    break;
                case "challengeMfa":
                    Result.ChallengeMfaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ChallengeMfaRequest.FromJson(requestPayload)
                    );
                    break;
                case "disableMfa":
                    Result.DisableMfaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DisableMfaRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePassword":
                    Result.DeletePasswordResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePasswordRequest.FromJson(requestPayload)
                    );
                    break;
                case "getHasSecurityPolicy":
                    Result.GetHasSecurityPolicyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetHasSecurityPolicyRequest.FromJson(requestPayload)
                    );
                    break;
                case "attachSecurityPolicy":
                    Result.AttachSecurityPolicyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AttachSecurityPolicyRequest.FromJson(requestPayload)
                    );
                    break;
                case "detachSecurityPolicy":
                    Result.DetachSecurityPolicyResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DetachSecurityPolicyRequest.FromJson(requestPayload)
                    );
                    break;
                case "login":
                    Result.LoginResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.LoginRequest.FromJson(requestPayload)
                    );
                    break;
                case "loginByUser":
                    Result.LoginByUserResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.LoginByUserRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}