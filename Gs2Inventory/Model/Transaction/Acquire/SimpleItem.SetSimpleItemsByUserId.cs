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
using System.Numerics;
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
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
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
            if (self == null || request?.Counts == null) {
                throw new NullReferenceException();
            }
            var clone = new SimpleItem[self.Length];
            for (var i = 0; i < self.Length; i++) {
                clone[i] = self[i]?.Clone() as SimpleItem;
                var item = clone[i];
                if (item == null) continue;
                var changed = false;
                foreach (var count in request.Counts) {
                    if (count?.ItemName != item.ItemName ||
                        !count.Count.HasValue) continue;
                    item.Count = count.Count;
                    changed = true;
                }
                if (changed) item.Revision = 0;
            }
            return clone;
/* diff +++ end */
        }

        public static SetSimpleItemsByUserIdRequest Rate(
            this SetSimpleItemsByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class SetSimpleItemsByUserIdRequestExt
    {
        public static SetSimpleItemsByUserIdRequest Rate(
            this SetSimpleItemsByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}
