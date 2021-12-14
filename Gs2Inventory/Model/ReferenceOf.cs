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

namespace Gs2.Gs2Inventory.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ReferenceOf : IComparable
	{
        public string ReferenceOfId { set; get; }
        public string Name { set; get; }

        public ReferenceOf WithReferenceOfId(string referenceOfId) {
            this.ReferenceOfId = referenceOfId;
            return this;
        }

        public ReferenceOf WithName(string name) {
            this.Name = name;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReferenceOf FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReferenceOf()
                .WithReferenceOfId(!data.Keys.Contains("referenceOfId") || data["referenceOfId"] == null ? null : data["referenceOfId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["referenceOfId"] = ReferenceOfId,
                ["name"] = Name,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ReferenceOfId != null) {
                writer.WritePropertyName("referenceOfId");
                writer.Write(ReferenceOfId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ReferenceOf;
            var diff = 0;
            if (ReferenceOfId == null && ReferenceOfId == other.ReferenceOfId)
            {
                // null and null
            }
            else
            {
                diff += ReferenceOfId.CompareTo(other.ReferenceOfId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            return diff;
        }
    }
}