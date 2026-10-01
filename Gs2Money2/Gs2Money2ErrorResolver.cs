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
using System.Linq;
using Gs2.Core.Exception;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public static class Gs2Money2ErrorResolver
	{
		public static Gs2Exception Resolve(string method, Gs2Exception error)
		{
			if (error?.Errors == null) {
				return error;
			}
			Gs2Exception resolved = null;
			switch (method) {
				case "DepositByUserId":
					if (error.Errors.Any(v => v != null && v.code == "wallet.operation.conflict")) {
						resolved = new Exception.ConflictException(error);
					}
					break;
				case "Withdraw":
				case "WithdrawByUserId":
					if (error.Errors.Any(v => v != null && v.code == "wallet.operation.conflict")) {
						resolved = new Exception.ConflictException(error);
					}
					else if (error.Errors.Any(v => v != null && v.code == "wallet.balance.insufficient")) {
						resolved = new Exception.InsufficientException(error);
					}
					break;
				case "VerifyReceipt":
				case "VerifyReceiptByUserId":
					if (error.Errors.Any(v => v != null && v.code == "receipt.payload.invalid")) {
						resolved = new Exception.ReceiptInvalidException(error);
					}
					break;
				case "AllocateSubscriptionStatus":
				case "AllocateSubscriptionStatusByUserId":
					if (error.Errors.Any(v => v != null && v.code == "subscription.transaction.used")) {
						resolved = new Exception.AlreadyUsedException(error);
					}
					break;
				case "TakeoverSubscriptionStatus":
				case "TakeoverSubscriptionStatusByUserId":
					if (error.Errors.Any(v => v != null && v.code == "subscription.transaction.used")) {
						resolved = new Exception.LockPeriodNotElapsedException(error);
					}
					break;
			}
			if (resolved == null) {
				return error;
			}
			resolved.Metadata = error.Metadata;
			return resolved;
		}
	}
}