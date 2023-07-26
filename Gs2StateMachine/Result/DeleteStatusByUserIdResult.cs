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
using Gs2.Gs2StateMachine.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2StateMachine.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DeleteStatusByUserIdResult : IResult
	{
        public Gs2.Gs2StateMachine.Model.Status Item { set; get; }

        public DeleteStatusByUserIdResult WithItem(Gs2.Gs2StateMachine.Model.Status item) {
            this.Item = item;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DeleteStatusByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteStatusByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2StateMachine.Model.Status.FromJson(data["item"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}