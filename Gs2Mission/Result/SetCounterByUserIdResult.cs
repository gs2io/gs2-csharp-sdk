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
using Gs2.Gs2Mission.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Mission.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SetCounterByUserIdResult : IResult
	{
        public Gs2.Gs2Mission.Model.Counter Item { set; get; } = null!;
        public Gs2.Gs2Mission.Model.Counter Old { set; get; } = null!;
        public Gs2.Gs2Mission.Model.Complete[] ChangedCompletes { set; get; } = null!;

        public SetCounterByUserIdResult WithItem(Gs2.Gs2Mission.Model.Counter item) {
            this.Item = item;
            return this;
        }

        public SetCounterByUserIdResult WithOld(Gs2.Gs2Mission.Model.Counter old) {
            this.Old = old;
            return this;
        }

        public SetCounterByUserIdResult WithChangedCompletes(Gs2.Gs2Mission.Model.Complete[] changedCompletes) {
            this.ChangedCompletes = changedCompletes;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetCounterByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetCounterByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Mission.Model.Counter.FromJson(data["item"]))
                .WithOld(!data.Keys.Contains("old") || data["old"] == null ? null : Gs2.Gs2Mission.Model.Counter.FromJson(data["old"]))
                .WithChangedCompletes(!data.Keys.Contains("changedCompletes") || data["changedCompletes"] == null || !data["changedCompletes"].IsArray ? null : data["changedCompletes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.Complete.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData changedCompletesJsonData = null;
            if (ChangedCompletes != null && ChangedCompletes.Length > 0)
            {
                changedCompletesJsonData = new JsonData();
                foreach (var changedComplete in ChangedCompletes)
                {
                    changedCompletesJsonData.Add(changedComplete.ToJson());
                }
            }
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["old"] = Old?.ToJson(),
                ["changedCompletes"] = changedCompletesJsonData,
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
            if (ChangedCompletes != null) {
                writer.WritePropertyName("changedCompletes");
                writer.WriteArrayStart();
                foreach (var changedComplete in ChangedCompletes)
                {
                    if (changedComplete != null) {
                        changedComplete.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }
    }
}