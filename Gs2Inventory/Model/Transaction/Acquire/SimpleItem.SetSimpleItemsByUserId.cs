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
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class SimpleItemExt
    {
        public static bool IsExecutable(
/* diff --- start
            this SimpleItem self,
 diff --- end */
            this SimpleItem[] self, /* diff +++ */
            SetSimpleItemsByUserIdRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
/* diff +++ start */
                foreach (var v in changed) {
                    v.Validate();
                }
/* diff +++ end */
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

/* diff --- start
        public static SimpleItem SpeculativeExecution(
            this SimpleItem self,
 diff --- end */
/* diff +++ start */
        public static SimpleItem[] SpeculativeExecution(
            this SimpleItem[] self,
/* diff +++ end */
            SetSimpleItemsByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Inventory:SetSimpleItemsByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Inventory:SetSimpleItemsByUserId");
//#endif
            return self.Clone() as SimpleItem;
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as SimpleItem[];
            if (clone == null) {
                throw new NullReferenceException();
            }
            foreach (var v in clone) {
                v.Count += request.Counts.FirstOrDefault(i => i.ItemName == v.ItemName)?.Count ?? 0;
            }
            return clone;
/* diff +++ end */
        }

        public static SetSimpleItemsByUserIdRequest Rate(
            this SetSimpleItemsByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Inventory:SetSimpleItemsByUserId");
        }
    }

    public static partial class SetSimpleItemsByUserIdRequestExt
    {
        public static SetSimpleItemsByUserIdRequest Rate(
            this SetSimpleItemsByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Inventory:SetSimpleItemsByUserId");
        }
    }
}