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
using Gs2.Gs2Money.Request;

namespace Gs2.Gs2Money.Model.Transaction
{
    public static partial class WalletExt
    {
        public static bool IsExecutable(
            this Wallet self,
            DepositByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (System.Exception) {
                return false;
            }
        }

        public static Wallet SpeculativeExecution(
            this Wallet self,
            DepositByUserIdRequest request
        ) {
            if (request?.Count == null || request.Price == null) {
                throw new NullReferenceException();
            }
/* diff --- start
            if (self.Clone() is not Wallet clone)
 diff --- end */
/* diff +++ start */
            var clone = self.Clone() as Wallet;
            if (clone == null)
/* diff +++ end */
            {
                throw new NullReferenceException();
            }
            var unitPrice = (float)(Math.Ceiling(
                request.Price.Value * 10000f / request.Count.Value
            ) / 10000d);
            var details = (clone.Detail ?? Array.Empty<WalletDetail>())
                .Select(detail => detail?.Clone() as WalletDetail)
                .ToList();
            var target = details.FirstOrDefault(detail =>
                detail?.Price == unitPrice
            );
            if (target == null) {
                target = new WalletDetail()
                    .WithPrice(unitPrice)
                    .WithCount(0);
                details.Add(target);
            }
            checked {
                target.Count += request.Count;
                if (request.Price > 0) {
                    clone.Paid += request.Count;
                }
                else {
                    clone.Free += request.Count;
                }
            }
            clone.Detail = details.ToArray();
/* diff --- start
            clone.Total += request.Count;
 diff --- end */
/* diff +++ start */
            clone.Revision = 0;
/* diff +++ end */
            return clone;
        }

        internal static Wallet SpeculativeSyncFree(
            this Wallet self,
            Wallet source
        ) {
            if (self?.Clone() is not Wallet clone || source == null)
            {
                throw new NullReferenceException();
            }
            var details = (clone.Detail ?? Array.Empty<WalletDetail>())
                .Select(detail => detail?.Clone() as WalletDetail)
                .Where(detail => detail != null)
                .ToList();
            var freeDetail = details.FirstOrDefault(detail =>
                detail.Price == 0
            );
            if (freeDetail == null) {
                freeDetail = new WalletDetail().WithPrice(0);
                details.Add(freeDetail);
            }
            freeDetail.Count = source.Detail?
                .FirstOrDefault(detail => detail?.Price == 0)?.Count
                ?? source.Free
                ?? 0;
            clone.Free = source.Free;
            clone.Detail = details.ToArray();
            return clone;
        }

        public static DepositByUserIdRequest Rate(
            this DepositByUserIdRequest request,
            double rate
        ) {
            request.Count = (int?) (request.Count * rate);
            return request;
        }
    }

    public static partial class DepositByUserIdRequestExt
    {
        public static DepositByUserIdRequest Rate(
            this DepositByUserIdRequest request,
            BigInteger rate
        ) {
            request.Count = (int?) ((request.Count ?? 0) * rate);
            return request;
        }
    }
}
