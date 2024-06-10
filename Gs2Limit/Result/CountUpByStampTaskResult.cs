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
using Gs2.Gs2Limit.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Limit.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CountUpByStampTaskResult : IResult
	{
        public Gs2.Gs2Limit.Model.Counter Item { set; get; } = null!;
        public string NewContextStack { set; get; } = null!;

        public CountUpByStampTaskResult WithItem(Gs2.Gs2Limit.Model.Counter item) {
            this.Item = item;
            return this;
        }

        public CountUpByStampTaskResult WithNewContextStack(string newContextStack) {
            this.NewContextStack = newContextStack;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CountUpByStampTaskResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CountUpByStampTaskResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Limit.Model.Counter.FromJson(data["item"]))
                .WithNewContextStack(!data.Keys.Contains("newContextStack") || data["newContextStack"] == null ? null : data["newContextStack"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["newContextStack"] = NewContextStack,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (NewContextStack != null) {
                writer.WritePropertyName("newContextStack");
                writer.Write(NewContextStack.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}