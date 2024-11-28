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
	public class UnityAd : IComparable
	{
        public string[] Keys { set; get; } = null!;
        public UnityAd WithKeys(string[] keys) {
            this.Keys = keys;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnityAd FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UnityAd()
                .WithKeys(!data.Keys.Contains("keys") || data["keys"] == null || !data["keys"].IsArray ? null : data["keys"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData keysJsonData = null;
            if (Keys != null && Keys.Length > 0)
            {
                keysJsonData = new JsonData();
                foreach (var key in Keys)
                {
                    keysJsonData.Add(key);
                }
            }
            return new JsonData {
                ["keys"] = keysJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Keys != null) {
                writer.WritePropertyName("keys");
                writer.WriteArrayStart();
                foreach (var key in Keys)
                {
                    if (key != null) {
                        writer.Write(key.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnityAd;
            var diff = 0;
            if (Keys == null && Keys == other.Keys)
            {
                // null and null
            }
            else
            {
                diff += Keys.Length - other.Keys.Length;
                for (var i = 0; i < Keys.Length; i++)
                {
                    diff += Keys[i].CompareTo(other.Keys[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (Keys.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unityAd", "adReward.unityAd.keys.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnityAd {
                Keys = Keys.Clone() as string[],
            };
        }
    }
}