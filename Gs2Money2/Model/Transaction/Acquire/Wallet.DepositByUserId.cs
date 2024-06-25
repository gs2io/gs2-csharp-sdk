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
using Gs2.Gs2Money2.Request;

namespace Gs2.Gs2Money2.Model.Transaction
{
    public static partial class WalletExt
    {
        public static bool IsExecutable(
            this Wallet self,
            DepositByUserIdRequest request
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
            DepositByUserIdRequest request
        ) {
            if (self.Clone() is not Wallet clone)
            {
                throw new NullReferenceException();
            }
            foreach (var depositTransaction in request.DepositTransactions) {
                if (depositTransaction.Price == 0) {
                    clone.Summary.Free += depositTransaction.Count;
                }
                else {
                    clone.Summary.Paid += depositTransaction.Count;
                }
                clone.Summary.Total += depositTransaction.Count;
            }
            return clone;
        }

        public static DepositByUserIdRequest Rate(
            this DepositByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Money2:DepositByUserId");
        }
    }

    public static partial class DepositByUserIdRequestExt
    {
        public static DepositByUserIdRequest Rate(
            this DepositByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Money2:DepositByUserId");
        }
    }
}