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
using Gs2.Gs2Schedule.Request;
using Gs2.Util.LitJson;

namespace Gs2.Gs2Schedule.Model
{
    public static class StampAction
    {
        public static Gs2Request ToRequest(Gs2.Core.Model.ConsumeAction action) {
            switch (action.Action) {
                case "Gs2Schedule:DeleteTriggerByUserId":
                    return DeleteTriggerByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Schedule:VerifyTriggerByUserId":
                    return VerifyTriggerByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Schedule:VerifyEventByUserId":
                    return VerifyEventByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
            }
            throw new ArgumentException($"unknown action {action.Action}");
        }

        public static Gs2Request ToRequest(Gs2.Core.Model.VerifyAction action) {
            switch (action.Action) {
                case "Gs2Schedule:VerifyTriggerByUserId":
                    return VerifyTriggerByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Schedule:VerifyEventByUserId":
                    return VerifyEventByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
            }
            throw new ArgumentException($"unknown action {action.Action}");
        }

        public static Gs2Request ToRequest(Gs2.Core.Model.AcquireAction action) {
            switch (action.Action) {
                case "Gs2Schedule:TriggerByUserId":
                    return TriggerByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
                case "Gs2Schedule:ExtendTriggerByUserId":
                    return ExtendTriggerByUserIdRequest.FromJson(JsonMapper.ToObject(action.Request));
            }
            throw new ArgumentException($"unknown action {action.Action}");
        }
    }
}