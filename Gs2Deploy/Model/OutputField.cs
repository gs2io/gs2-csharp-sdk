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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class OutputField : IComparable
	{
        public string Name { set; get; }
        public string FieldName { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type OutputField.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(FieldName, other.FieldName);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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