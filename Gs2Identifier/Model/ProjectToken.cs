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

namespace Gs2.Gs2Identifier.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ProjectToken : IComparable
	{
        public string Token { set; get; } = null!;
        public ProjectToken WithToken(string token) {
            this.Token = token;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ProjectToken FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ProjectToken()
                .WithToken(!data.Keys.Contains("token") || data["token"] == null ? null : data["token"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["token"] = Token,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Token != null) {
                writer.WritePropertyName("token");
                writer.Write(Token.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ProjectToken;
            var diff = 0;
            if (Token == null && Token == other.Token)
            {
                // null and null
            }
            else
            {
                diff += Token.CompareTo(other.Token);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Token.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("projectToken", "identifier.projectToken.token.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ProjectToken {
                Token = Token,
            };
        }
    }
}