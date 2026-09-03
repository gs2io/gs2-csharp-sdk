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
            AddRankCapByUserIdRequest request
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
            AddRankCapByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not Status clone)
            {
 diff --- end */
/* diff +++ start */
            if (self?.RankCapValue == null ||
                request?.RankCapValue == null ||
                self.Clone() is not Status clone) {
/* diff +++ end */
                throw new NullReferenceException();
            }
/* diff --- start
            clone.RankCapValue += request.RankCapValue;
 diff --- end */
/* diff +++ start */
            clone.RankCapValue = checked(
                self.RankCapValue.Value + request.RankCapValue.Value
            );
            clone.Revision = 0;
/* diff +++ end */
            return clone;
/* diff +++ start */
        }

        public static Status SpeculativeExecution(
            this Status self,
            AddRankCapByUserIdRequest request,
            ExperienceModel model
        ) {
            if (self?.ExperienceValue == null ||
                self.RankCapValue == null ||
                request?.RankCapValue == null || model == null) {
                throw new NullReferenceException();
            }
            return model.RecalculateStatus(
                self,
                self.ExperienceValue.Value,
                checked(self.RankCapValue.Value + request.RankCapValue.Value)
            );
/* diff +++ end */
        }

        public static AddRankCapByUserIdRequest Rate(
            this AddRankCapByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            request.RankCapValue = (long?) (request.RankCapValue * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !TryApplyServerRate(
                    request.RankCapValue ?? 1L,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.RankCapValue = value;
/* diff +++ end */
            return request;
        }
    }

    public static partial class AddRankCapByUserIdRequestExt
    {
        public static AddRankCapByUserIdRequest Rate(
            this AddRankCapByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            request.RankCapValue = (long?) ((request.RankCapValue ?? 0) * rate);
 diff --- end */
/* diff +++ start */
            if (request == null || !StatusExt.TryApplyServerRate(
                    request.RankCapValue ?? 1L,
                    rate,
                    out var value
                )) {
                return null;
            }
            request.RankCapValue = value;
/* diff +++ end */
            return request;
        }
    }
}