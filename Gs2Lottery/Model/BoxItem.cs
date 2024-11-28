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

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BoxItem : IComparable
	{
        public string PrizeId { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; } = null!;
        public int? Remaining { set; get; } = null!;
        public int? Initial { set; get; } = null!;
        public BoxItem WithPrizeId(string prizeId) {
            this.PrizeId = prizeId;
            return this;
        }
        public BoxItem WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }
        public BoxItem WithRemaining(int? remaining) {
            this.Remaining = remaining;
            return this;
        }
        public BoxItem WithInitial(int? initial) {
            this.Initial = initial;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BoxItem FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BoxItem()
                .WithPrizeId(!data.Keys.Contains("prizeId") || data["prizeId"] == null ? null : data["prizeId"].ToString())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithRemaining(!data.Keys.Contains("remaining") || data["remaining"] == null ? null : (int?)(data["remaining"].ToString().Contains(".") ? (int)double.Parse(data["remaining"].ToString()) : int.Parse(data["remaining"].ToString())))
                .WithInitial(!data.Keys.Contains("initial") || data["initial"] == null ? null : (int?)(data["initial"].ToString().Contains(".") ? (int)double.Parse(data["initial"].ToString()) : int.Parse(data["initial"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["prizeId"] = PrizeId,
                ["acquireActions"] = acquireActionsJsonData,
                ["remaining"] = Remaining,
                ["initial"] = Initial,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PrizeId != null) {
                writer.WritePropertyName("prizeId");
                writer.Write(PrizeId.ToString());
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Remaining != null) {
                writer.WritePropertyName("remaining");
                writer.Write((Remaining.ToString().Contains(".") ? (int)double.Parse(Remaining.ToString()) : int.Parse(Remaining.ToString())));
            }
            if (Initial != null) {
                writer.WritePropertyName("initial");
                writer.Write((Initial.ToString().Contains(".") ? (int)double.Parse(Initial.ToString()) : int.Parse(Initial.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BoxItem;
            var diff = 0;
            if (PrizeId == null && PrizeId == other.PrizeId)
            {
                // null and null
            }
            else
            {
                diff += PrizeId.CompareTo(other.PrizeId);
            }
            if (AcquireActions == null && AcquireActions == other.AcquireActions)
            {
                // null and null
            }
            else
            {
                diff += AcquireActions.Length - other.AcquireActions.Length;
                for (var i = 0; i < AcquireActions.Length; i++)
                {
                    diff += AcquireActions[i].CompareTo(other.AcquireActions[i]);
                }
            }
            if (Remaining == null && Remaining == other.Remaining)
            {
                // null and null
            }
            else
            {
                diff += (int)(Remaining - other.Remaining);
            }
            if (Initial == null && Initial == other.Initial)
            {
                // null and null
            }
            else
            {
                diff += (int)(Initial - other.Initial);
            }
            return diff;
        }

        public void Validate() {
            {
                if (PrizeId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("boxItem", "lottery.boxItem.prizeId.error.tooLong"),
                    });
                }
            }
            {
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("boxItem", "lottery.boxItem.acquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (Remaining < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("boxItem", "lottery.boxItem.remaining.error.invalid"),
                    });
                }
                if (Remaining > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("boxItem", "lottery.boxItem.remaining.error.invalid"),
                    });
                }
            }
            {
                if (Initial < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("boxItem", "lottery.boxItem.initial.error.invalid"),
                    });
                }
                if (Initial > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("boxItem", "lottery.boxItem.initial.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BoxItem {
                PrizeId = PrizeId,
                AcquireActions = AcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
                Remaining = Remaining,
                Initial = Initial,
            };
        }
    }
}