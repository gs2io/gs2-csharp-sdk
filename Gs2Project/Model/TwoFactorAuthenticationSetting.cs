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

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class TwoFactorAuthenticationSetting : IComparable
	{
        public string Status { set; get; } = null!;
        public TwoFactorAuthenticationSetting WithStatus(string status) {
            this.Status = status;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TwoFactorAuthenticationSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TwoFactorAuthenticationSetting()
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["status"] = Status,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TwoFactorAuthenticationSetting;
            var diff = 0;
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            return diff;
        }

        public void Validate() {
            {
                switch (Status) {
                    case "Verifying":
                    case "Enable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("twoFactorAuthenticationSetting", "project.twoFactorAuthenticationSetting.status.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new TwoFactorAuthenticationSetting {
                Status = Status,
            };
        }
    }
}