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

namespace Gs2.Gs2Deploy.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class OutputField : IComparable
	{
        public string Name { set; get; } = null!;
        public string FieldName { set; get; } = null!;
        public OutputField WithName(string name) {
            this.Name = name;
            return this;
        }
        public OutputField WithFieldName(string fieldName) {
            this.FieldName = fieldName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static OutputField FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new OutputField()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithFieldName(!data.Keys.Contains("fieldName") || data["fieldName"] == null ? null : data["fieldName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["fieldName"] = FieldName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (FieldName != null) {
                writer.WritePropertyName("fieldName");
                writer.Write(FieldName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as OutputField;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (FieldName == null && FieldName == other.FieldName)
            {
                // null and null
            }
            else
            {
                diff += FieldName.CompareTo(other.FieldName);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("outputField", "deploy.outputField.name.error.tooLong"),
                    });
                }
            }
            {
                if (FieldName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("outputField", "deploy.outputField.fieldName.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new OutputField {
                Name = Name,
                FieldName = FieldName,
            };
        }
    }
}