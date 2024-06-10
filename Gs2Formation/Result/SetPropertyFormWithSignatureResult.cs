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

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SetPropertyFormWithSignatureResult : IResult
	{
        public Gs2.Gs2Formation.Model.PropertyForm Item { set; get; } = null!;
        public Gs2.Gs2Formation.Model.PropertyFormModel ProeprtyFormModel { set; get; } = null!;

        public SetPropertyFormWithSignatureResult WithItem(Gs2.Gs2Formation.Model.PropertyForm item) {
            this.Item = item;
            return this;
        }

        public SetPropertyFormWithSignatureResult WithProeprtyFormModel(Gs2.Gs2Formation.Model.PropertyFormModel proeprtyFormModel) {
            this.ProeprtyFormModel = proeprtyFormModel;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetPropertyFormWithSignatureResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetPropertyFormWithSignatureResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Formation.Model.PropertyForm.FromJson(data["item"]))
                .WithProeprtyFormModel(!data.Keys.Contains("proeprtyFormModel") || data["proeprtyFormModel"] == null ? null : Gs2.Gs2Formation.Model.PropertyFormModel.FromJson(data["proeprtyFormModel"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["proeprtyFormModel"] = ProeprtyFormModel?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (ProeprtyFormModel != null) {
                ProeprtyFormModel.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}