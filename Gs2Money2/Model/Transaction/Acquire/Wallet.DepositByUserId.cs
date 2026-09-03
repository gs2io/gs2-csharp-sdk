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
using Gs2.Core.Model;
using Gs2.Gs2Money2.Request;

namespace Gs2.Gs2Money2.Model.Transaction
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
            if (self?.Clone() is not Wallet clone ||
                request?.DepositTransactions == null)
            {
                throw new NullReferenceException();
            }
            var depositTransactions = (clone.DepositTransactions ??
                    Array.Empty<DepositTransaction>())
                .Select(transaction => transaction?.Clone() as DepositTransaction)
                .Where(transaction => transaction != null)
                .ToList();
            foreach (var depositTransaction in request.DepositTransactions) {
                if (depositTransaction == null) {
                    continue;
                }
                if (depositTransaction.Price == 0) {
                    var free = depositTransactions.FirstOrDefault(transaction =>
                        transaction.Price == 0
                    );
                    var freeCount = (free?.Count ?? 0) + depositTransaction.Count;
                    depositTransactions = depositTransactions
                        .Where(transaction => transaction.Price != 0)
                        .ToList();
                    if (freeCount > 0) {
                        depositTransactions.Add(
                            new DepositTransaction()
                                .WithPrice(0)
                                .WithCurrency(null)
                                .WithCount(freeCount)
                                .WithDepositedAt(
                                    depositTransaction.DepositedAt ?? free?.DepositedAt
                                )
                        );
                    }
                }
                else {
                    depositTransactions.Add(
                        depositTransaction.Clone() as DepositTransaction
                    );
                }
            }
            clone.DepositTransactions = depositTransactions.ToArray();
            clone.Summary = CalculateSummary(clone.DepositTransactions);
            clone.Revision = 0;
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
            var paid = (clone.DepositTransactions ?? Array.Empty<DepositTransaction>())
                .Where(transaction => transaction?.Price > 0)
                .Select(transaction => transaction.Clone() as DepositTransaction)
                .ToList();
            var free = source.DepositTransactions?
                .FirstOrDefault(transaction => transaction?.Price == 0);
            if ((free?.Count ?? 0) > 0) {
                paid.Add(free.Clone() as DepositTransaction);
            }
            clone.DepositTransactions = paid.ToArray();
            clone.Summary = CalculateSummary(clone.DepositTransactions);
            return clone;
        }

        internal static WalletSummary CalculateSummary(
            DepositTransaction[] depositTransactions
        ) {
            var paid = depositTransactions?
                .Where(transaction => transaction?.Price > 0)
                .Sum(transaction => transaction.Count ?? 0) ?? 0;
            var free = depositTransactions?
                .Where(transaction => transaction?.Price == 0)
                .Sum(transaction => transaction.Count ?? 0) ?? 0;
            return new WalletSummary()
                .WithPaid(paid)
                .WithFree(free)
                .WithTotal(paid + free);
        }

        public static DepositByUserIdRequest Rate(
            this DepositByUserIdRequest request,
            double rate
        ) {
            if (request?.DepositTransactions != null) {
                foreach (var transaction in request.DepositTransactions) {
                    if (transaction != null) {
                        transaction.Count = (int?) (transaction.Count * rate);
                    }
                }
            }
            return request;
        }
    }

    public static partial class DepositByUserIdRequestExt
    {
        public static DepositByUserIdRequest Rate(
            this DepositByUserIdRequest request,
            BigInteger rate
        ) {
            if (request?.DepositTransactions != null) {
                foreach (var transaction in request.DepositTransactions) {
                    if (transaction != null) {
                        transaction.Count = (int?) (
                            (transaction.Count ?? 0) * rate
                        );
                    }
                }
            }
            return request;
        }
    }
}
