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

namespace Gs2.Gs2Freeze.Model.Cache
{
    public static class Gs2Freeze
    {
        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "describeStages":
                    Result.DescribeStagesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeStagesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStage":
                    Result.GetStageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetStageRequest.FromJson(requestPayload)
                    );
                    break;
                case "promoteStage":
                    Result.PromoteStageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PromoteStageRequest.FromJson(requestPayload)
                    );
                    break;
                case "rollbackStage":
                    Result.RollbackStageResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.RollbackStageRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeOutputs":
                    Result.DescribeOutputsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeOutputsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getOutput":
                    Result.GetOutputResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetOutputRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}