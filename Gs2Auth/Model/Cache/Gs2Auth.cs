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

namespace Gs2.Gs2Auth.Model.Cache
{
    public static class Gs2Auth
    {
        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "login":
                    Result.LoginResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.LoginRequest.FromJson(requestPayload)
                    );
                    break;
                case "loginBySignature":
                    Result.LoginBySignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.LoginBySignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "federation":
                    Result.FederationResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.FederationRequest.FromJson(requestPayload)
                    );
                    break;
                case "issueTimeOffsetTokenByUserId":
                    Result.IssueTimeOffsetTokenByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.IssueTimeOffsetTokenByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getServiceVersion":
                    Result.GetServiceVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetServiceVersionRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}