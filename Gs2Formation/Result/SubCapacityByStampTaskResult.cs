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
	public class SubCapacityByStampTaskResult : IResult
	{
        public Gs2.Gs2Formation.Model.Mold Item { set; get; }
        public Gs2.Gs2Formation.Model.MoldModel MoldModel { set; get; }
        public string NewContextStack { set; get; }
        public ResultMetadata Metadata { set; get; }

        public SubCapacityByStampTaskResult WithItem(Gs2.Gs2Formation.Model.Mold item) {
            this.Item = item;
            return this;
        }

        public SubCapacityByStampTaskResult WithMoldModel(Gs2.Gs2Formation.Model.MoldModel moldModel) {
            this.MoldModel = moldModel;
            return this;
        }

        public SubCapacityByStampTaskResult WithNewContextStack(string newContextStack) {
            this.NewContextStack = newContextStack;
            return this;
        }

        public SubCapacityByStampTaskResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SubCapacityByStampTaskResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubCapacityByStampTaskResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Formation.Model.Mold.FromJson(data["item"]))
                .WithMoldModel(!data.Keys.Contains("moldModel") || data["moldModel"] == null ? null : Gs2.Gs2Formation.Model.MoldModel.FromJson(data["moldModel"]))
                .WithNewContextStack(!data.Keys.Contains("newContextStack") || data["newContextStack"] == null ? null : data["newContextStack"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["moldModel"] = MoldModel?.ToJson(),
                ["newContextStack"] = NewContextStack,
                ["metadata"] = Metadata?.ToJson(),
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
            if (NewContextStack != null) {
                writer.WritePropertyName("newContextStack");
                writer.Write(NewContextStack.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}