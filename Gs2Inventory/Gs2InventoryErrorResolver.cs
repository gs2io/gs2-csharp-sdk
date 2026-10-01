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

namespace Gs2.Gs2Inventory
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public static class Gs2InventoryErrorResolver
	{
		public static Gs2Exception Resolve(string method, Gs2Exception error)
		{
			if (error?.Errors == null) {
				return error;
			}
			Gs2Exception resolved = null;
			switch (method) {
				case "AcquireItemSetByUserId":
				case "AcquireItemSetWithGradeByUserId":
				case "AcquireSimpleItemsByUserId":
				case "SetSimpleItemsByUserId":
				case "AcquireBigItemByUserId":
				case "SetBigItemByUserId":
					if (error.Errors.Any(v => v != null && v.code == "itemSet.operation.conflict")) {
						resolved = new Exception.ConflictException(error);
					}
					break;
				case "ConsumeItemSet":
				case "ConsumeItemSetByUserId":
				case "ConsumeSimpleItems":
				case "ConsumeSimpleItemsByUserId":
				case "ConsumeBigItem":
				case "ConsumeBigItemByUserId":
					if (error.Errors.Any(v => v != null && v.code == "itemSet.operation.conflict")) {
						resolved = new Exception.ConflictException(error);
					}
					else if (error.Errors.Any(v => v != null && v.code == "itemSet.count.insufficient")) {
						resolved = new Exception.InsufficientException(error);
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