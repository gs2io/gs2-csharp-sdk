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
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Gs2.Core.Domain;
using Gs2.Core.Model;

namespace Gs2.Core.Exception
{
    public class TransactionException : Gs2Exception
    {
        private TransactionAccessTokenDomain _transaction;
        private bool _isWorthRetry;

        public TransactionException(
            TransactionAccessTokenDomain transaction,
            Gs2Exception exception
        ): base(exception.errors) {
            this._transaction = transaction;
            this._isWorthRetry = exception is InternalServerErrorException ||
                exception is QuotaLimitExceededException ||
                exception is ServiceUnavailableException ||
                exception is ConflictException ||
                exception is RequestTimeoutException ||
                exception is UnauthorizedException;
        }

        public override bool RecommendRetry => this._isWorthRetry;
        public override bool RecommendAutoRetry => this._isWorthRetry;

        public override int StatusCode => 500;

        public bool IsWorthRetry() {
            return _isWorthRetry;
        }

#if UNITY_2017_1_OR_NEWER
#if !GS2_ENABLE_UNITASK
        public Gs2Future Retry() {
            IEnumerator Impl(Gs2Future self)
            {
                var future = this._transaction.WaitFuture();
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                }
            }
            return new Gs2InlineFuture(Impl);
        }
#else
        public Gs2Future Retry()
        {
            IEnumerator Impl(Gs2Future self)
            {
                yield return UniTask.ToCoroutine(async () =>
                {
                    try
                    {
                        await this._transaction.WaitAsync();
                    }
                    catch (Gs2Exception e)
                    {
                        self.OnError(e);
                    }
                });
            }
            return new Gs2InlineFuture(Impl);
        }
        
        public async Task RetryAsync() {
            await this._transaction.WaitAsync();
        }
#endif
#endif
    }
}