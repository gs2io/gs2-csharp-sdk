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

namespace Gs2.Core.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ResultMetadata : IComparable
	{
        public string Uncommitted { set; get; } = null!;
        public ResultMetadata WithUncommitted(string uncommitted) {
            this.Uncommitted = uncommitted;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ResultMetadata FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ResultMetadata()
                .WithUncommitted(!data.Keys.Contains("uncommitted") || data["uncommitted"] == null ? null : data["uncommitted"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["uncommitted"] = Uncommitted,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Uncommitted != null) {
                writer.WritePropertyName("uncommitted");
                writer.Write(Uncommitted);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ResultMetadata;
            var diff = 0;
            if (Uncommitted == null && Uncommitted == other.Uncommitted)
            {
                // null and null
            }
            else
            {
                diff += Uncommitted.CompareTo(other.Uncommitted);
            }
            return diff;
        }

        public void Validate() {
            {
            }
        }

        public object Clone() {
            return new ResultMetadata {
                Uncommitted = Uncommitted.Clone() as string,
            };
        }
    }
}