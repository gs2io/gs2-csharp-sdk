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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Guild.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Guild.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AcceptRequestResult : IResult
	{
        public Gs2.Gs2Guild.Model.ReceiveMemberRequest Item { set; get; } = null!;
        public Gs2.Gs2Guild.Model.Guild Guild { set; get; } = null!;

        public AcceptRequestResult WithItem(Gs2.Gs2Guild.Model.ReceiveMemberRequest item) {
            this.Item = item;
            return this;
        }

        public AcceptRequestResult WithGuild(Gs2.Gs2Guild.Model.Guild guild) {
            this.Guild = guild;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcceptRequestResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcceptRequestResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Guild.Model.ReceiveMemberRequest.FromJson(data["item"]))
                .WithGuild(!data.Keys.Contains("guild") || data["guild"] == null ? null : Gs2.Gs2Guild.Model.Guild.FromJson(data["guild"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["guild"] = Guild?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (Guild != null) {
                Guild.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}