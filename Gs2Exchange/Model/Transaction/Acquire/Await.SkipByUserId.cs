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

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Exchange.Request;

namespace Gs2.Gs2Exchange.Model.Transaction
{
    public static partial class AwaitExt
    {
        public static bool IsExecutable(
            this Await self,
            SkipByUserIdRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
                changed.Validate();
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static Await SpeculativeExecution(
            this Await self,
            SkipByUserIdRequest request
        ) {
#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Exchange:SkipByUserId");
#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Exchange:SkipByUserId");
#endif
            return self.Clone() as Await;
        }

        public static SkipByUserIdRequest Rate(
            this SkipByUserIdRequest request,
            double rate
        ) {
            request.Minutes = (int?) (request.Minutes * rate);
            return request;
        }
    }

    public static partial class SkipByUserIdRequestExt
    {
        public static SkipByUserIdRequest Rate(
            this SkipByUserIdRequest request,
            BigInteger rate
        ) {
            request.Minutes = (int?) ((request.Minutes ?? 0) * rate);
            return request;
        }
    }
}