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
using Gs2.Gs2JobQueue.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2JobQueue.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PushByUserIdResult : IResult
	{
        public Gs2.Gs2JobQueue.Model.Job[] Items { set; get; }
        public bool? AutoRun { set; get; }

        public PushByUserIdResult WithItems(Gs2.Gs2JobQueue.Model.Job[] items) {
            this.Items = items;
            return this;
        }

        public PushByUserIdResult WithAutoRun(bool? autoRun) {
            this.AutoRun = autoRun;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PushByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PushByUserIdResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null ? new Gs2.Gs2JobQueue.Model.Job[]{} : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2JobQueue.Model.Job.FromJson(v);
                }).ToArray())
                .WithAutoRun(!data.Keys.Contains("autoRun") || data["autoRun"] == null ? null : (bool?)bool.Parse(data["autoRun"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["items"] = new JsonData(Items == null ? new JsonData[]{} :
                        Items.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["autoRun"] = AutoRun,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WriteArrayStart();
            foreach (var item in Items)
            {
                if (item != null) {
                    item.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (AutoRun != null) {
                writer.WritePropertyName("autoRun");
                writer.Write(bool.Parse(AutoRun.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}