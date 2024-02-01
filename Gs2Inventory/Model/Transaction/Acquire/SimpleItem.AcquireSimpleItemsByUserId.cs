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
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class SimpleItemExt
    {
        public static bool IsExecutable(
            this SimpleItem[] self,
            AcquireSimpleItemsByUserIdRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
                foreach (var v in changed) {
                    v.Validate();
                }
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static SimpleItem[] SpeculativeExecution(
            this SimpleItem[] self,
            AcquireSimpleItemsByUserIdRequest request
        ) {
            var clone = self.Clone() as SimpleItem[];
            if (clone == null) {
                throw new NullReferenceException();
            }
            foreach (var v in clone) {
                v.Count += request.AcquireCounts.FirstOrDefault(i => i.ItemName == v.ItemName)?.Count ?? 0;
            }
            return clone;
        }

        public static AcquireSimpleItemsByUserIdRequest Rate(
            this AcquireSimpleItemsByUserIdRequest request,
            double rate
        ) {
            foreach (var acquireCount in request.AcquireCounts) {
                acquireCount.Count = (long?) (acquireCount.Count * rate);
            }
            return request;
        }
    }

    public static partial class AcquireSimpleItemsByUserIdRequestExt
    {
        public static AcquireSimpleItemsByUserIdRequest Rate(
            this AcquireSimpleItemsByUserIdRequest request,
            BigInteger rate
        ) {
            foreach (var acquireCount in request.AcquireCounts) {
                acquireCount.Count = (long?) (acquireCount.Count * rate);
            }
            return request;
        }
    }
}