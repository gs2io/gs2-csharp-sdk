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
using Gs2.Core.Model;
using Gs2.Gs2Money2.Request;

namespace Gs2.Gs2Money2.Model.Transaction
{
    public static partial class WalletExt
    {
        public static bool IsExecutable(
            this Wallet self,
            WithdrawByUserIdRequest request
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
            WithdrawByUserIdRequest request
        ) {
            if (request?.WithdrawCount == null) {
                throw new InvalidOperationException("withdraw count is unavailable");
            }
            if (self.Clone() is not Wallet clone)
            {
                throw new NullReferenceException();
            }
            var depositTransactions = (clone.DepositTransactions ??
                    Array.Empty<DepositTransaction>())
                .Select(transaction => transaction?.Clone() as DepositTransaction)
                .Where(transaction => transaction != null)
                .ToList();
            var remaining = request.WithdrawCount.Value;
            if (!(request.PaidOnly ?? false)) {
                var free = depositTransactions.FirstOrDefault(transaction =>
                    transaction.Price == 0
                );
                if (free != null) {
                    var consumed = Math.Min(free.Count ?? 0, remaining);
                    remaining = checked(remaining - consumed);
                    var freeCount = checked((free.Count ?? 0) - consumed);
                    depositTransactions = depositTransactions
                        .Where(transaction => transaction.Price != 0)
                        .ToList();
                    if (freeCount > 0) {
                        free.Count = freeCount;
                        depositTransactions.Add(free);
                    }
                }
            }
            for (var i = 0; i < depositTransactions.Count && remaining > 0; i++) {
                var transaction = depositTransactions[i];
                if (transaction.Price == 0 || (transaction.Count ?? 0) <= 0) {
                    continue;
                }
                var consumed = Math.Min(transaction.Count.Value, remaining);
                var unitPrice = transaction.Price.Value / transaction.Count.Value;
                transaction.Count = checked(transaction.Count.Value - consumed);
                transaction.Price = checked(unitPrice * transaction.Count.Value);
                remaining = checked(remaining - consumed);
            }
            depositTransactions = depositTransactions
                .Where(transaction => (transaction.Count ?? 0) > 0)
                .ToList();
            if (remaining > 0) {
                throw new BadRequestException(new[] {
                    new RequestError("wallet", "fewBalance"),
                });
            }
            clone.DepositTransactions = depositTransactions.ToArray();
            clone.Summary = WalletExt.CalculateSummary(clone.DepositTransactions);
            clone.Revision = 0;
            return clone;
        }

        public static WithdrawByUserIdRequest Rate(
            this WithdrawByUserIdRequest request,
            double rate
        ) {
            request.WithdrawCount = (int?) (request.WithdrawCount * rate);
            return request;
        }
    }

    public static partial class WithdrawByUserIdRequestExt
    {
        public static WithdrawByUserIdRequest Rate(
            this WithdrawByUserIdRequest request,
            BigInteger rate
        ) {
            request.WithdrawCount = (int?) ((request.WithdrawCount ?? 0) * rate);
            return request;
        }
    }
}
