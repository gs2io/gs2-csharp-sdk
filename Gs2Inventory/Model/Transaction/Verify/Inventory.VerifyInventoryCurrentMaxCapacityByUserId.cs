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

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Inventory.Request;

namespace Gs2.Gs2Inventory.Model.Transaction
{
    public static partial class InventoryExt
    {
        public static bool IsExecutable(
            this Inventory self,
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            switch (request.VerifyType) {
                case "less":
                    return self.CurrentInventoryMaxCapacity < request.CurrentInventoryMaxCapacity;
                case "lessEqual":
                    return self.CurrentInventoryMaxCapacity <= request.CurrentInventoryMaxCapacity;
                case "greater":
                    return self.CurrentInventoryMaxCapacity > request.CurrentInventoryMaxCapacity;
                case "greaterEqual":
                    return self.CurrentInventoryMaxCapacity >= request.CurrentInventoryMaxCapacity;
                case "equal":
                    return self.CurrentInventoryMaxCapacity == request.CurrentInventoryMaxCapacity;
                case "notEqual":
                    return self.CurrentInventoryMaxCapacity != request.CurrentInventoryMaxCapacity;
            }
            return false;
        }

        public static Inventory SpeculativeExecution(
            this Inventory self,
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            return self.Clone() as Inventory;
        }

        public static VerifyInventoryCurrentMaxCapacityByUserIdRequest Rate(
            this VerifyInventoryCurrentMaxCapacityByUserIdRequest request,
            double rate
        ) {
            request.CurrentInventoryMaxCapacity = (int?) (request.CurrentInventoryMaxCapacity * rate);
            return request;
        }
    }

    public static partial class VerifyInventoryCurrentMaxCapacityByUserIdRequestExt
    {
        public static VerifyInventoryCurrentMaxCapacityByUserIdRequest Rate(
            this VerifyInventoryCurrentMaxCapacityByUserIdRequest request,
            BigInteger rate
        ) {
            request.CurrentInventoryMaxCapacity = (int?) ((request.CurrentInventoryMaxCapacity ?? 0) * rate);
            return request;
        }
    }
}