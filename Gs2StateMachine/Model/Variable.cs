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

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Variable : IComparable
	{
        public string StateMachineName { set; get; } = null!;
        public string Value { set; get; } = null!;
        public Variable WithStateMachineName(string stateMachineName) {
            this.StateMachineName = stateMachineName;
            return this;
        }
        public Variable WithValue(string value) {
            this.Value = value;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Variable FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Variable()
                .WithStateMachineName(!data.Keys.Contains("stateMachineName") || data["stateMachineName"] == null ? null : data["stateMachineName"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : data["value"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["stateMachineName"] = StateMachineName,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StateMachineName != null) {
                writer.WritePropertyName("stateMachineName");
                writer.Write(StateMachineName.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(Value.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Variable;
            var diff = 0;
            if (StateMachineName == null && StateMachineName == other.StateMachineName)
            {
                // null and null
            }
            else
            {
                diff += StateMachineName.CompareTo(other.StateMachineName);
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
                if (StateMachineName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("variable", "stateMachine.variable.stateMachineName.error.tooLong"),
                    });
                }
            }
            {
                if (Value.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("variable", "stateMachine.variable.value.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new Variable {
                StateMachineName = StateMachineName,
                Value = Value,
            };
        }
    }
}