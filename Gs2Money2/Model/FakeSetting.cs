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
	public class FakeSetting : IComparable
	{
        public string AcceptFakeReceipt { set; get; } = null!;
        public FakeSetting WithAcceptFakeReceipt(string acceptFakeReceipt) {
            this.AcceptFakeReceipt = acceptFakeReceipt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static FakeSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new FakeSetting()
                .WithAcceptFakeReceipt(!data.Keys.Contains("acceptFakeReceipt") || data["acceptFakeReceipt"] == null ? null : data["acceptFakeReceipt"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["acceptFakeReceipt"] = AcceptFakeReceipt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AcceptFakeReceipt != null) {
                writer.WritePropertyName("acceptFakeReceipt");
                writer.Write(AcceptFakeReceipt.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as FakeSetting;
            var diff = 0;
            if (AcceptFakeReceipt == null && AcceptFakeReceipt == other.AcceptFakeReceipt)
            {
                // null and null
            }
            else
            {
                diff += AcceptFakeReceipt.CompareTo(other.AcceptFakeReceipt);
            }
            return diff;
        }

        public void Validate() {
            {
                switch (AcceptFakeReceipt) {
                    case "Accept":
                    case "Reject":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("fakeSetting", "money2.fakeSetting.acceptFakeReceipt.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new FakeSetting {
                AcceptFakeReceipt = AcceptFakeReceipt,
            };
        }
    }
}