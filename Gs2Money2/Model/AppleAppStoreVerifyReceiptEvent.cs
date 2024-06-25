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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AppleAppStoreVerifyReceiptEvent : IComparable
	{
        public string Environment { set; get; } = null!;
        public AppleAppStoreVerifyReceiptEvent WithEnvironment(string environment) {
            this.Environment = environment;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AppleAppStoreVerifyReceiptEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AppleAppStoreVerifyReceiptEvent()
                .WithEnvironment(!data.Keys.Contains("environment") || data["environment"] == null ? null : data["environment"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["environment"] = Environment,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Environment != null) {
                writer.WritePropertyName("environment");
                writer.Write(Environment.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AppleAppStoreVerifyReceiptEvent;
            var diff = 0;
            if (Environment == null && Environment == other.Environment)
            {
                // null and null
            }
            else
            {
                diff += Environment.CompareTo(other.Environment);
            }
            return diff;
        }

        public void Validate() {
            {
                switch (Environment) {
                    case "sandbox":
                    case "production":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("appleAppStoreVerifyReceiptEvent", "money2.appleAppStoreVerifyReceiptEvent.environment.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new AppleAppStoreVerifyReceiptEvent {
                Environment = Environment,
            };
        }
    }
}