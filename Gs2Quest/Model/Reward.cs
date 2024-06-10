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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Quest.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Reward : IComparable
	{
        public string Action { set; get; } = null!;
        public string Request { set; get; } = null!;
        public string ItemId { set; get; } = null!;
        public int? Value { set; get; } = null!;
        public Reward WithAction(string action) {
            this.Action = action;
            return this;
        }
        public Reward WithRequest(string request) {
            this.Request = request;
            return this;
        }
        public Reward WithItemId(string itemId) {
            this.ItemId = itemId;
            return this;
        }
        public Reward WithValue(int? value) {
            this.Value = value;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Reward FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Reward()
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithRequest(!data.Keys.Contains("request") || data["request"] == null ? null : data["request"].ToString())
                .WithItemId(!data.Keys.Contains("itemId") || data["itemId"] == null ? null : data["itemId"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (int?)(data["value"].ToString().Contains(".") ? (int)double.Parse(data["value"].ToString()) : int.Parse(data["value"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["action"] = Action,
                ["request"] = Request,
                ["itemId"] = ItemId,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (Request != null) {
                writer.WritePropertyName("request");
                writer.Write(Request.ToString());
            }
            if (ItemId != null) {
                writer.WritePropertyName("itemId");
                writer.Write(ItemId.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (int)double.Parse(Value.ToString()) : int.Parse(Value.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Reward;
            var diff = 0;
            if (Action == null && Action == other.Action)
            {
                // null and null
            }
            else
            {
                diff += Action.CompareTo(other.Action);
            }
            if (Request == null && Request == other.Request)
            {
                // null and null
            }
            else
            {
                diff += Request.CompareTo(other.Request);
            }
            if (ItemId == null && ItemId == other.ItemId)
            {
                // null and null
            }
            else
            {
                diff += ItemId.CompareTo(other.ItemId);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Action.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("reward", "quest.reward.action.error.tooLong"),
                    });
                }
            }
            {
                if (Request.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("reward", "quest.reward.request.error.tooLong"),
                    });
                }
            }
            {
                if (ItemId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("reward", "quest.reward.itemId.error.tooLong"),
                    });
                }
            }
            {
                if (Value < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("reward", "quest.reward.value.error.invalid"),
                    });
                }
                if (Value > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("reward", "quest.reward.value.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Reward {
                Action = Action,
                Request = Request,
                ItemId = ItemId,
                Value = Value,
            };
        }
    }
}