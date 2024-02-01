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
using Gs2.Gs2Money.Request;

namespace Gs2.Gs2Money.Model.Transaction
{
    public static partial class WalletExt
    {
        public static bool IsExecutable(
            this Wallet self,
            WithdrawByUserIdRequest request
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

        public static Wallet SpeculativeExecution(
            this Wallet self,
            WithdrawByUserIdRequest request
        ) {
            var clone = self.Clone() as Wallet;
            if (clone == null)
            {
                throw new NullReferenceException();
            }
            if (request.PaidOnly ?? false) {
                clone.Paid -= request.Count;
                if (clone.Paid < 0) {
                    return clone;
                }
            }
            else {
                if (clone.Free + clone.Paid < request.Count) {
                    if (clone.Free < 0) {
                        return clone;
                    }
                }
                else {
                    clone.Free -= request.Count;
                    if (clone.Free < 0) {
                        clone.Paid += clone.Free;
                        clone.Free = 0;
                    }
                }
            }
            return clone;
        }

        public static WithdrawByUserIdRequest Rate(
            this WithdrawByUserIdRequest request,
            double rate
        ) {
            request.Count = (int?) (request.Count * rate);
            return request;
        }
    }

    public static partial class WithdrawByUserIdRequestExt
    {
        public static WithdrawByUserIdRequest Rate(
            this WithdrawByUserIdRequest request,
            BigInteger rate
        ) {
            request.Count = (int?) ((request.Count ?? 0) * rate);
            return request;
        }
    }
}