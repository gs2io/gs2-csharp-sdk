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
using Gs2.Gs2Quest.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Quest.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetProgressResult : IResult
	{
        public Gs2.Gs2Quest.Model.Progress Item { set; get; } = null!;
        public Gs2.Gs2Quest.Model.QuestGroupModel QuestGroup { set; get; } = null!;
        public Gs2.Gs2Quest.Model.QuestModel Quest { set; get; } = null!;

        public GetProgressResult WithItem(Gs2.Gs2Quest.Model.Progress item) {
            this.Item = item;
            return this;
        }

        public GetProgressResult WithQuestGroup(Gs2.Gs2Quest.Model.QuestGroupModel questGroup) {
            this.QuestGroup = questGroup;
            return this;
        }

        public GetProgressResult WithQuest(Gs2.Gs2Quest.Model.QuestModel quest) {
            this.Quest = quest;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetProgressResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetProgressResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Quest.Model.Progress.FromJson(data["item"]))
                .WithQuestGroup(!data.Keys.Contains("questGroup") || data["questGroup"] == null ? null : Gs2.Gs2Quest.Model.QuestGroupModel.FromJson(data["questGroup"]))
                .WithQuest(!data.Keys.Contains("quest") || data["quest"] == null ? null : Gs2.Gs2Quest.Model.QuestModel.FromJson(data["quest"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["questGroup"] = QuestGroup?.ToJson(),
                ["quest"] = Quest?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (QuestGroup != null) {
                QuestGroup.WriteJson(writer);
            }
            if (Quest != null) {
                Quest.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}