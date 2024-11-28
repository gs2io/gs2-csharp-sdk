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
using Gs2.Gs2Inbox.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ReceiveGlobalMessageByUserIdResult : IResult
	{
        public Gs2.Gs2Inbox.Model.Message[] Item { set; get; } = null!;

        public ReceiveGlobalMessageByUserIdResult WithItem(Gs2.Gs2Inbox.Model.Message[] item) {
            this.Item = item;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReceiveGlobalMessageByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReceiveGlobalMessageByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null || !data["item"].IsArray ? null : data["item"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inbox.Model.Message.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData itemJsonData = null;
            if (Item != null && Item.Length > 0)
            {
                itemJsonData = new JsonData();
                foreach (var ite in Item)
                {
                    itemJsonData.Add(ite.ToJson());
                }
            }
            return new JsonData {
                ["item"] = itemJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                writer.WritePropertyName("item");
                writer.WriteArrayStart();
                foreach (var ite in Item)
                {
                    if (ite != null) {
                        ite.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }
    }
}