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
using Gs2.Gs2Showcase.Request;

namespace Gs2.Gs2Showcase.Model.Transaction
{
/* diff --- start
    public static partial class RandomShowcaseStatusExt
 diff --- end */
    public static partial class RandomDisplayItemExt /* diff +++ */
    {
        public static bool IsExecutable(
/* diff --- start
            this RandomShowcaseStatus self,
 diff --- end */
            this RandomDisplayItem self, /* diff +++ */
            DecrementPurchaseCountByUserIdRequest request
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

/* diff --- start
        public static RandomShowcaseStatus SpeculativeExecution(
            this RandomShowcaseStatus self,
 diff --- end */
/* diff +++ start */
        public static RandomDisplayItem SpeculativeExecution(
            this RandomDisplayItem self,
/* diff +++ end */
            DecrementPurchaseCountByUserIdRequest request
        ) {
/* diff --- start
            if (self.Clone() is not RandomShowcaseStatus clone)
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as RandomDisplayItem;
            if (clone == null)
/* diff +++ end */
            {
                throw new NullReferenceException();
            }
            clone.CurrentPurchaseCount -= request.Count;
            return clone;
        }

        public static DecrementPurchaseCountByUserIdRequest Rate(
            this DecrementPurchaseCountByUserIdRequest request,
            double rate
        ) {
            request.Count = (int?) (request.Count * rate);
            return request;
        }
    }

    public static partial class DecrementPurchaseCountByUserIdRequestExt
    {
        public static DecrementPurchaseCountByUserIdRequest Rate(
            this DecrementPurchaseCountByUserIdRequest request,
            BigInteger rate
        ) {
            request.Count = (int?) ((request.Count ?? 0) * rate);
            return request;
        }
    }
}