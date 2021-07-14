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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Formation.Result
{
	[Preserve]
	[System.Serializable]
	public class GetMoldResult : IResult
	{
        public Gs2.Gs2Formation.Model.Mold Item { set; get; }
        public Gs2.Gs2Formation.Model.MoldModel MoldModel { set; get; }

        public GetMoldResult WithItem(Gs2.Gs2Formation.Model.Mold item) {
            this.Item = item;
            return this;
        }

        public GetMoldResult WithMoldModel(Gs2.Gs2Formation.Model.MoldModel moldModel) {
            this.MoldModel = moldModel;
            return this;
        }

    	[Preserve]
        public static GetMoldResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetMoldResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Formation.Model.Mold.FromJson(data["item"]))
                .WithMoldModel(!data.Keys.Contains("moldModel") || data["moldModel"] == null ? null : Gs2.Gs2Formation.Model.MoldModel.FromJson(data["moldModel"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["moldModel"] = MoldModel?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (MoldModel != null) {
                MoldModel.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}