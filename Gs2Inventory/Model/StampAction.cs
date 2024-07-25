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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Inventory.Request;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Inventory.Model
{
    public static class StampAction
    {
        public static Gs2Request ToRequest(Gs2.Core.Model.ConsumeAction action) {
            switch (action.Action) {
                case "Gs2Inventory:VerifyInventoryCurrentMaxCapacityByUserId":
                    return VerifyInventoryCurrentMaxCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:ConsumeItemSetByUserId":
                    return ConsumeItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifyItemSetByUserId":
                    return VerifyItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifyReferenceOfByUserId":
                    return VerifyReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:ConsumeSimpleItemsByUserId":
                    return ConsumeSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifySimpleItemByUserId":
                    return VerifySimpleItemByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:ConsumeBigItemByUserId":
                    return ConsumeBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifyBigItemByUserId":
                    return VerifyBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
            }
            throw new ArgumentException($"unknown action {action.Action}");
        }

        public static Gs2Request ToRequest(Gs2.Core.Model.VerifyAction action) {
            switch (action.Action) {
                case "Gs2Inventory:VerifyInventoryCurrentMaxCapacityByUserId":
                    return VerifyInventoryCurrentMaxCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifyItemSetByUserId":
                    return VerifyItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifyReferenceOfByUserId":
                    return VerifyReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifySimpleItemByUserId":
                    return VerifySimpleItemByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:VerifyBigItemByUserId":
                    return VerifyBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
            }
            throw new ArgumentException($"unknown action {action.Action}");
        }

        public static Gs2Request ToRequest(Gs2.Core.Model.AcquireAction action) {
            switch (action.Action) {
                case "Gs2Inventory:AddCapacityByUserId":
                    return AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:SetCapacityByUserId":
                    return SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:AcquireItemSetByUserId":
                    return AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:AcquireItemSetWithGradeByUserId":
                    return AcquireItemSetWithGradeByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:AddReferenceOfByUserId":
                    return AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:DeleteReferenceOfByUserId":
                    return DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:AcquireSimpleItemsByUserId":
                    return AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:SetSimpleItemsByUserId":
                    return SetSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:AcquireBigItemByUserId":
                    return AcquireBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Inventory:SetBigItemByUserId":
                    return SetBigItemByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
            }
            throw new ArgumentException($"unknown action {action.Action}");
        }
    }
}