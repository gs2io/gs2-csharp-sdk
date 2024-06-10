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

namespace Gs2.Gs2Formation.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Config : IComparable
	{
        public string Key { set; get; } = null!;
        public string Value { set; get; } = null!;
        public Config WithKey(string key) {
            this.Key = key;
            return this;
        }
        public Config WithValue(string value) {
            this.Value = value;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Config FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Config()
                .WithKey(!data.Keys.Contains("key") || data["key"] == null ? null : data["key"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : data["value"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["key"] = Key,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Key != null) {
                writer.WritePropertyName("key");
                writer.Write(Key.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(Value.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Config;
            var diff = 0;
            if (Key == null && Key == other.Key)
            {
                // null and null
            }
            else
            {
                diff += Key.CompareTo(other.Key);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += Value.CompareTo(other.Value);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Key.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("config", "formation.config.key.error.tooLong"),
                    });
                }
            }
            {
                if (Value.Length > 51200) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("config", "formation.config.value.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new Config {
                Key = Key,
                Value = Value,
            };
        }
    }
}