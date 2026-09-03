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
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
/* diff --- start
using System.Linq;
 diff --- end */
using System.Numerics;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Limit.Request;

namespace Gs2.Gs2Limit.Model.Transaction
{
    public static partial class CounterExt
    {
        public static bool IsExecutable(
            this Counter self,
            CountDownByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
                self.SpeculativeExecution(request); /* diff +++ */
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (System.Exception) { /* diff +++ */
                return false;
            }
        }

        public static Counter SpeculativeExecution(
            this Counter self,
            CountDownByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not Counter clone)
            {
                throw new NullReferenceException();
 diff --- end */
/* diff +++ start */
            if (self?.Count == null || request?.CountDownValue == null) {
                throw new InvalidOperationException("counter is unavailable");
/* diff +++ end */
            }
/* diff --- start
            clone.Count -= request.CountDownValue;
 diff --- end */
/* diff +++ start */
            if (self.Clone() is not Counter clone) {
                throw new InvalidOperationException("counter is unavailable");
            }
            clone.Count = checked(
                self.Count.Value - request.CountDownValue.Value
            );
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        public static CountDownByUserIdRequest Rate(
            this CountDownByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.CountDownValue = (int?) (request.CountDownValue * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !CounterRate.TryApply(
                    request.CountDownValue ?? 1,
                    rate,
                    out var value
                )) {
                return request;
            }
            request.CountDownValue = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class CountDownByUserIdRequestExt
    {
        public static CountDownByUserIdRequest Rate(
            this CountDownByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.CountDownValue = (int?) ((request.CountDownValue ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !CounterRate.TryApply(
                    request.CountDownValue ?? 1,
                    rate,
                    out var value
                )) {
                return request;
            }
            request.CountDownValue = value;
/* diff +++ end */
            return request;
        }
    }
}