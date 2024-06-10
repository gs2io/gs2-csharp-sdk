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

namespace Gs2.Gs2AdReward.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AppLovinMax : IComparable
	{
        public string AllowAdUnitId { set; get; } = null!;
        public string EventKey { set; get; } = null!;
        public AppLovinMax WithAllowAdUnitId(string allowAdUnitId) {
            this.AllowAdUnitId = allowAdUnitId;
            return this;
        }
        public AppLovinMax WithEventKey(string eventKey) {
            this.EventKey = eventKey;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AppLovinMax FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AppLovinMax()
                .WithAllowAdUnitId(!data.Keys.Contains("allowAdUnitId") || data["allowAdUnitId"] == null ? null : data["allowAdUnitId"].ToString())
                .WithEventKey(!data.Keys.Contains("eventKey") || data["eventKey"] == null ? null : data["eventKey"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["allowAdUnitId"] = AllowAdUnitId,
                ["eventKey"] = EventKey,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AllowAdUnitId != null) {
                writer.WritePropertyName("allowAdUnitId");
                writer.Write(AllowAdUnitId.ToString());
            }
            if (EventKey != null) {
                writer.WritePropertyName("eventKey");
                writer.Write(EventKey.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AppLovinMax;
            var diff = 0;
            if (AllowAdUnitId == null && AllowAdUnitId == other.AllowAdUnitId)
            {
                // null and null
            }
            else
            {
                diff += AllowAdUnitId.CompareTo(other.AllowAdUnitId);
            }
            if (EventKey == null && EventKey == other.EventKey)
            {
                // null and null
            }
            else
            {
                diff += EventKey.CompareTo(other.EventKey);
            }
            return diff;
        }

        public void Validate() {
            {
                if (AllowAdUnitId.Length > 16) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appLovinMax", "adReward.appLovinMax.allowAdUnitId.error.tooLong"),
                    });
                }
            }
            {
                if (EventKey.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("appLovinMax", "adReward.appLovinMax.eventKey.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new AppLovinMax {
                AllowAdUnitId = AllowAdUnitId,
                EventKey = EventKey,
            };
        }
    }
}