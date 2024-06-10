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
using Gs2.Gs2Idle.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Idle.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SetMaximumIdleMinutesByUserIdResult : IResult
	{
        public Gs2.Gs2Idle.Model.Status Item { set; get; } = null!;
        public Gs2.Gs2Idle.Model.Status Old { set; get; } = null!;

        public SetMaximumIdleMinutesByUserIdResult WithItem(Gs2.Gs2Idle.Model.Status item) {
            this.Item = item;
            return this;
        }

        public SetMaximumIdleMinutesByUserIdResult WithOld(Gs2.Gs2Idle.Model.Status old) {
            this.Old = old;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetMaximumIdleMinutesByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetMaximumIdleMinutesByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Idle.Model.Status.FromJson(data["item"]))
                .WithOld(!data.Keys.Contains("old") || data["old"] == null ? null : Gs2.Gs2Idle.Model.Status.FromJson(data["old"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["old"] = Old?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (Old != null) {
                Old.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}