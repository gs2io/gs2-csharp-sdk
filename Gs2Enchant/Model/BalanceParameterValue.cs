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

namespace Gs2.Gs2Enchant.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BalanceParameterValue : IComparable
	{
        public string Name { set; get; } = null!;
        public long? Value { set; get; } = null!;
        public BalanceParameterValue WithName(string name) {
            this.Name = name;
            return this;
        }
        public BalanceParameterValue WithValue(long? value) {
            this.Value = value;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BalanceParameterValue FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BalanceParameterValue()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (long?)(data["value"].ToString().Contains(".") ? (long)double.Parse(data["value"].ToString()) : long.Parse(data["value"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (long)double.Parse(Value.ToString()) : long.Parse(Value.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BalanceParameterValue;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterValue", "enchant.balanceParameterValue.name.error.tooLong"),
                    });
                }
            }
            {
                if (Value < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterValue", "enchant.balanceParameterValue.value.error.invalid"),
                    });
                }
                if (Value > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterValue", "enchant.balanceParameterValue.value.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BalanceParameterValue {
                Name = Name,
                Value = Value,
            };
        }
    }
}