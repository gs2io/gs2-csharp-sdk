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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class UnityAd : IComparable
	{
        public string[] Keys { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UnityAd.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.CompareArray(Keys, other.Keys);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
                Keys = Keys?.Clone() as string[],
            };
        }
    }
}