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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class DrawnPrize : IComparable
	{
        public string PrizeId { set; get; }
        public Gs2.Gs2Lottery.Model.AcquireAction[] AcquireActions { set; get; }
        public DrawnPrize WithPrizeId(string prizeId) {
            this.PrizeId = prizeId;
            return this;
        }
        public DrawnPrize WithAcquireActions(Gs2.Gs2Lottery.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DrawnPrize FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DrawnPrize()
                .WithPrizeId(!data.Keys.Contains("prizeId") || data["prizeId"] == null ? null : data["prizeId"].ToString())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null ? new Gs2.Gs2Lottery.Model.AcquireAction[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Lottery.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["prizeId"] = PrizeId,
                ["acquireActions"] = new JsonData(AcquireActions == null ? new JsonData[]{} :
                        AcquireActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PrizeId != null) {
                writer.WritePropertyName("prizeId");
                writer.Write(PrizeId.ToString());
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DrawnPrize;
            var diff = 0;
            if (PrizeId == null && PrizeId == other.PrizeId)
            {
                // null and null
            }
            else
            {
                diff += PrizeId.CompareTo(other.PrizeId);
            }
            if (AcquireActions == null && AcquireActions == other.AcquireActions)
            {
                // null and null
            }
            else
            {
                diff += AcquireActions.Length - other.AcquireActions.Length;
                for (var i = 0; i < AcquireActions.Length; i++)
                {
                    diff += AcquireActions[i].CompareTo(other.AcquireActions[i]);
                }
            }
            return diff;
        }
    }
}