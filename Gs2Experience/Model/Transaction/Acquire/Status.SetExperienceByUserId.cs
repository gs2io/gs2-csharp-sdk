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
using Gs2.Gs2Experience.Request;

namespace Gs2.Gs2Experience.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
            SetExperienceByUserIdRequest request
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
            SetExperienceByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not Status clone)
            {
 diff --- end */
/* diff +++ start */
            if (self?.Clone() is not Status clone ||
                request?.ExperienceValue == null) {
/* diff +++ end */
                throw new NullReferenceException();
            }
            clone.ExperienceValue = request.ExperienceValue;
            clone.Revision = 0; /* diff +++ */
            return clone;
/* diff +++ start */
        }

        public static Status SpeculativeExecution(
            this Status self,
            SetExperienceByUserIdRequest request,
            ExperienceModel model
        ) {
            if (self?.RankCapValue == null ||
                request?.ExperienceValue == null || model == null) {
                throw new NullReferenceException();
            }
            return model.RecalculateStatus(
                self,
                request.ExperienceValue.Value,
                self.RankCapValue.Value
            );
/* diff +++ end */
        }

        public static SetExperienceByUserIdRequest Rate(
            this SetExperienceByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.ExperienceValue = (long?) (request.ExperienceValue * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !TryApplyServerRate(
                    request.ExperienceValue ?? 1L,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.ExperienceValue = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class SetExperienceByUserIdRequestExt
    {
        public static SetExperienceByUserIdRequest Rate(
            this SetExperienceByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.ExperienceValue = (long?) ((request.ExperienceValue ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !StatusExt.TryApplyServerRate(
                    request.ExperienceValue ?? 1L,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.ExperienceValue = value;
/* diff +++ end */
            return request;
        }
    }
}