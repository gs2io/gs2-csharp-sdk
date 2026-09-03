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
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Grade.Request;

namespace Gs2.Gs2Grade.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            SubGradeByUserIdRequest request
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
            catch (Exception) { /* diff +++ */
                return false;
            }
        }

        public static Status SpeculativeExecution(
            this Status self,
            SubGradeByUserIdRequest request
        ) {
            if (self.Clone() is not Status clone)
            {
                throw new NullReferenceException();
            }
/* diff --- start
            clone.GradeValue -= request.GradeValue;
 diff --- end */
/* diff +++ start */
            if (clone.GradeValue == null ||
                request?.GradeValue == null ||
                request.GradeValue < 0) {
                throw new ArgumentOutOfRangeException(nameof(request));
            }
            var changed = checked(
                clone.GradeValue.Value - request.GradeValue.Value
            );
            if (changed < 1) {
                throw new ArgumentOutOfRangeException(nameof(request));
            }
            clone.GradeValue = changed;
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        public static SubGradeByUserIdRequest Rate(
            this SubGradeByUserIdRequest request,
            double rate
        ) {
            request.GradeValue = (long?) (request.GradeValue * rate);
            return request;
        }
    }

    public static partial class SubGradeByUserIdRequestExt
    {
        public static SubGradeByUserIdRequest Rate(
            this SubGradeByUserIdRequest request,
            BigInteger rate
        ) {
            request.GradeValue = (long?) ((request.GradeValue ?? 0) * rate);
            return request;
        }
    }
}