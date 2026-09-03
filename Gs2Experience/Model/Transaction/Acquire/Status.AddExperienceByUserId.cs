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
using Gs2.Gs2Experience.Request;

namespace Gs2.Gs2Experience.Model.Transaction
{
    public static partial class StatusExt
    {
        public static bool IsExecutable(
            this Status self,
/* diff --- start
            AddExperienceByUserIdRequest request
 diff --- end */
/* diff +++ start */
            AddExperienceByUserIdRequest request,
            ExperienceModel model
/* diff +++ end */
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
                self.SpeculativeExecution(request, model); /* diff +++ */
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public static Status SpeculativeExecution(
            this Status self,
/* diff --- start
            AddExperienceByUserIdRequest request
 diff --- end */
/* diff +++ start */
            AddExperienceByUserIdRequest request,
            ExperienceModel model
/* diff +++ end */
        ) {
            if (self?.ExperienceValue == null ||
                self.RankCapValue == null ||
                request?.ExperienceValue == null || model == null) {
                throw new NullReferenceException();
            }
            return model.RecalculateStatus(
                self,
                checked(self.ExperienceValue.Value + request.ExperienceValue.Value),
                self.RankCapValue.Value
            );
        }

        public static AddExperienceByUserIdRequest Rate(
            this AddExperienceByUserIdRequest request,
            double rate
        ) {
            if (request == null || !TryApplyServerRate(
                    request.ExperienceValue ?? 1L,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.ExperienceValue = value;
            return request;
        }
    }

    public static partial class AddExperienceByUserIdRequestExt
    {
        public static AddExperienceByUserIdRequest Rate(
            this AddExperienceByUserIdRequest request,
            BigInteger rate
        ) {
            if (request == null || !StatusExt.TryApplyServerRate(
                    request.ExperienceValue ?? 1L,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.ExperienceValue = value;
            return request;
        }
    }
}
