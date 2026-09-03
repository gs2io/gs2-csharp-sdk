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
            SetRankCapByUserIdRequest request
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
            SetRankCapByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Experience:SetRankCapByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Experience:SetRankCapByUserId");
//#endif
            return self.Clone() as Status;
 diff --- end */
/* diff +++ start */
            if (self?.Clone() is not Status clone ||
                request?.RankCapValue == null) {
                throw new NullReferenceException();
            }
            clone.RankCapValue = request.RankCapValue;
            clone.Revision = 0;
            return clone;
        }

        public static Status SpeculativeExecution(
            this Status self,
            SetRankCapByUserIdRequest request,
            ExperienceModel model
        ) {
            if (self?.ExperienceValue == null ||
                request?.RankCapValue == null || model == null) {
                throw new NullReferenceException();
            }
            return model.RecalculateStatus(
                self,
                self.ExperienceValue.Value,
                request.RankCapValue.Value
            );
/* diff +++ end */
        }

        public static SetRankCapByUserIdRequest Rate(
            this SetRankCapByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Experience:SetRankCapByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class SetRankCapByUserIdRequestExt
    {
        public static SetRankCapByUserIdRequest Rate(
            this SetRankCapByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Experience:SetRankCapByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}